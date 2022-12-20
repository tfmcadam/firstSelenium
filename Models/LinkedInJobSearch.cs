using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace LinkedInJobSearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Set up Chrome driver
            ChromeDriver driver = new ChromeDriver("C:/Program Files/Google/Chrome/Application/chrome.exe");
            

            // Navigate to LinkedIn login page
            driver.Navigate().GoToUrl("https://www.linkedin.com/login");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[5]/header/div/nav/ul/li[3]/a")));


            // // Find the email and password input fields and enter the login credentials
            // IWebElement emailInput = driver.FindElement(By.Id("username"));
            // emailInput.SendKeys("your_email@example.com");
            // IWebElement passwordInput = driver.FindElement(By.Id("password"));
            // passwordInput.SendKeys("your_password");

            // // Find the login button and click it
            // IWebElement loginButton = driver.FindElement(By.XPath("//button[@type='submit']"));
            // loginButton.Click();

            // Navigate to the jobs page
            driver.Navigate().GoToUrl("https://www.linkedin.com/jobs/");

            Thread.Sleep(1500);
            IWebElement search_keywords = driver.FindElement(By.XPath("/html/body/div[4]/header/div/div/div/div[2]/div[2]/div/div/input[1]"));

            search_keywords.Click();
            search_keywords.Clear();
            Thread.Sleep(2000);
            search_keywords.SendKeys("sales" + Keys.Return);
            Thread.Sleep(2000);

            IWebElement remote_location_input = driver.FindElement(By.XPath ("//input[@aria-label='City, state, or zip code']"));
            remote_location_input.Click();
            remote_location_input.Clear();
            Thread.Sleep(2000);
            remote_location_input.SendKeys("remote" + Keys.Return);
            Thread.Sleep(2000);
            // Find easy apply button and press it
            // easy_apply_button.Click();
                                        
            driver.FindElement(By.XPath("//button[@aria-label='Easy Apply filter.']")).Click();
            // Thread.Sleep(2500);
            Thread.Sleep(3500);


            // Find all the job listings on the page
            IList<IWebElement> jobListings = driver.FindElements(By.XPath("//li[contains(@class, 'ember-view')]"));

            // IWebElement easyApplyButton = driver.FindElement(By.XPath("//button[contains(@class, 'jobs-apply-button')]"));
            // easyApplyButton.Click();
            // Loop through each job listing
            foreach (IWebElement jobListing in jobListings)
            {
                // IWebElement clickjob = jobListing.FindElement(By.XPath("//li[@class, 'ember-view']"));
                // clickjob.Click();
                // Check if the job has an easy apply button
                IWebElement single = driver.FindElement(By.XPath("//div[contains(@class, 'jobs-unified-top-card')]"));
                single.Click();

                bool hasEasyApply = driver.FindElements(By.XPath("//button[contains(@class, 'jobs-apply-button')]")).Count > 0;
                if (hasEasyApply)
                {
                    // Click the easy apply button
                    IWebElement easyApplyButton = driver.FindElement(By.XPath("//button[contains(@class, 'jobs-apply-button')]"));
                    easyApplyButton.Click();

                    // Wait for the apply form to load
                    System.Threading.Thread.Sleep(2000);
                    Console.WriteLine("i'm here");
                    driver.FindElement(By.XPath ("//button[contains(@class, 'artdeco-button--primary')]")).Click();
                    Thread.Sleep(2500);


                    driver.FindElement(By.XPath ("//button[contains(@class, 'artdeco-button--1')]")).Click();
                    // Find the submit application button and click it

                    Thread.Sleep(2500);
                    driver.FindElement(By.XPath("//button[contains(@class, 'artdeco-button--primary')]")).Click();

                    IWebElement submitButton = driver.FindElement(By.XPath("//button[@type='submit']"));
                    submitButton.Click();
                    continue;
                }
            }

            // Close the driver
            // driver.Quit();
        }
    }
}