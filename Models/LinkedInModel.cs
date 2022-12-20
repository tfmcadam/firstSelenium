using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using firstSelenium.Models;
namespace LinkedInBot
{
    public class Linkedin
    {
        private IWebDriver driver;
        public Linkedin()
        {
            try
            {
                driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), new ChromeOptions());
                driver.Navigate().GoToUrl("https://www.linkedin.com/login?trk=guest_homepage-basic_nav-header-signin");
                Console.WriteLine("Trying to log in to LinkedIn.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Warning: ChromeDriver " + e.ToString());
            }

            try
            {
                driver.FindElement(By.Id("username")).SendKeys(Config.Email);
                System.Threading.Thread.Sleep(5000);
                driver.FindElement(By.Id("password")).SendKeys(Config.Password);
                System.Threading.Thread.Sleep(5000);
                driver.FindElement(By.XPath('//*[@id="organic-div"]/form/div[3]/button')).Click();
                System.Threading.Thread.Sleep(12000);
            }
            catch
            {
                Console.WriteLine("Could not log in to LinkedIn.");
            }
        }

        public void GenerateUrls()
        {
            if (!Directory.Exists("data"))
            {
                Directory.CreateDirectory("data");
            }

            try
            {
                using (StreamWriter file = new StreamWriter("data/urlData.txt", false, System.Text.Encoding.UTF8))
                {
                    string[] linkedinJobLinks = Utils.LinkedinUrlGenerate().GenerateUrlLinks();
                    foreach (string url in linkedinJobLinks)
                    {
                        file.WriteLine(url);
                    }
                }
                Console.WriteLine("Urls are created successfully, now the bot will visit those urls.");
            }
            catch
            {
                Console.WriteLine("Could not generate url, make sure you have a /data folder and modified the config.py file for your preferences.");
            }
        }

        public void LinkJobApply()
        {
            GenerateUrls();
            int countApplied = 0;
            int countJobs = 0;

            string[] urlData = Utils.GetUrlDataFile();

            foreach (string url in urlData)
            {
                driver.Navigate().GoToUrl(url);

                string totalJobs = driver.FindElement(By.XPath("//small")).Text;
                int totalPages = Utils.JobsToPages(totalJobs);

                string[] urlWords = Utils.UrlToKeywords(url);
                string lineToWrite = "\nCategory: " + urlWords[0] + ", Location: " + urlWords[1] + ", Applying " + totalJobs + " jobs.";
                DisplayWriteResults(lineToWrite);

                for (int page = 0; page < totalPages; page++)
                {
                    int currentPageJobs = Constants
                    url = url + "&start=" + currentPageJobs.ToString();
                    driver.Url = url;
                    System.Threading.Thread.Sleep((int)(random.NextDouble() * Constants.BotSpeed * 1000));

                    IReadOnlyCollection<IWebElement> offersPerPage = driver.FindElements(By.XPath("//li[@data-occludable-job-id]"));

                    List<int> offerIds = new List<int>();
                    foreach (IWebElement offer in offersPerPage)
                    {
                        string offerId = offer.GetAttribute("data-occludable-job-id");
                        offerIds.Add(int.Parse(offerId.Split(":")[^1]));
                    }

                    foreach (int jobID in offerIds)
                    {
                        string offerPage = "https://www.linkedin.com/jobs/view/" + jobID.ToString();
                        driver.Url = offerPage;
                        System.Threading.Thread.Sleep((int)(random.NextDouble() * Constants.BotSpeed * 1000));

                        countJobs++;

                        string jobProperties = GetJobProperties(countJobs);
                        if (jobProperties.Contains("blacklisted"))
                        {
                            lineToWrite = jobProperties + " | " + "* 🤬 Blacklisted Job, skipped!: " + offerPage;
                            DisplayWriteResults(lineToWrite);
                        }
                        else
                        {
                            IWebElement button = EasyApplyButton();
                            if (button != null)
                            {
                                button.Click();
                                System.Threading.Thread.Sleep((int)(random.NextDouble() * Constants.BotSpeed * 1000));
                                countApplied++;
                                try
                                {
                                    IWebElement applyButton = driver.FindElement(By.XPath("//button[contains(.,'Apply')]"));
                                    applyButton.Click();
                                    System.Threading.Thread.Sleep((int)(random.NextDouble() * Constants.BotSpeed * 1000));
                                    Console.WriteLine(countApplied + " | " + "* 🤑 Job applied successfully: " + offerPage);
                                }
                                catch
                                {
                                    Console.WriteLine(countApplied + " | " + "* 🤑 Job already applied: " + offerPage);
                                }
                            }
                            else
                            {
                                Console.WriteLine(countApplied + " | " + "* 🙁 No Apply button: " + offerPage);
                            }
                        }
                    }
                }

                private string GetJobProperties(int countJobs)
                {
                    string lineToWrite = "";
                    try
                    {
                        string jobTitle = driver.FindElement(By.XPath("//h1[@class='jobs-top-card__job-title t-24 t-black t-normal']")).Text;
                        lineToWrite = countJobs + " | " + "Job Title: " + jobTitle;

                        string company = driver.FindElement(By.XPath("//a[@class='jobs-top-card__company-url']")).Text;
                        lineToWrite += ", Company: " + company;

                        string location = driver.FindElement(By.XPath("//span[@class='jobs-top-card__bullet']")).Text;
                        lineToWrite += ", Location: " + location;

                        IReadOnlyCollection<IWebElement> jobTags = driver.FindElements(By.XPath("//li[@class='job-criteria__item jobs-box__list-item']"));
                        string tags = "";
                        foreach (IWebElement tag in jobTags)
                        {
                            tags += ", " + tag.Text;
                        }
                        lineToWrite += ", Tags: " + tags;

                        IReadOnlyCollection<IWebElement> blacklistedWords = driver.FindElements(By.XPath("//li[@class='job-criteria__item jobs-box__list-item' and contains(.,'blacklist')]"));
                        if (blacklistedWords.Count > 0)
                        {
                            lineToWrite += ", Blacklisted Words: ";
                            foreach (IWebElement word in blacklistedWords)
                            {
                                lineToWrite += word.Text + ", ";
                            }
                        }
                    }
                    catch
                    {
                        lineToWrite = countJobs + " | " + "Error occured in getting job properties";
                    }
                    return lineToWrite;
                }

                private IWebElement EasyApplyButton()
                {
                    try
                    {
                        return driver.FindElement(By.XPath("//button[contains(.,'Easy Apply')]"));
                    }
                    catch
                    {
                        return null;
                    }
                }

                private void DisplayWriteResults(string lineToWrite)
                {
                    Console.WriteLine(lineToWrite);
                }

                private void QuitDriver()
                {
                    driver.Quit();
                }
            }
        }
    