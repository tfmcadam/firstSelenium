using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium.Firefox;

namespace LinkedInBot
{
public class Constants
{
    public int JobsPerPage = 25;
    public int fast = 2;
    public int medium = 3;
    public int slow = 5;
    int botSpeed = 5;
}
    public class Utils
    {
        private static Random random = new Random();



        public static void PrRed(string prt)
        {
            Console.WriteLine($"\033[91m{prt}\033[00m");
        }

        public static void PrGreen(string prt)
        {
            Console.WriteLine($"\033[92m{prt}\033[00m");
        }
        public static void PrYellow(string prt)
        {
            Console.WriteLine($"\033[93m{prt}\033[00m");
        }

        public static string[] GetUrlDataFile()
        {
            string[] urlData = { };
            try
            {
                string[] lines = System.IO.File.ReadAllLines("data/urlData.txt");
                urlData = lines.ToArray();
            }
            catch (System.IO.FileNotFoundException)
            {
                string text = "FileNotFound:urlData.txt file is not found. Please run ./data folder exists and check config.py values of yours. Then run the bot again";
                PrRed(text);
            }
            return urlData;
        }

        public static int JobsToPages(string numOfJobs)
        {
            int numberOfPages = 1;

            if (numOfJobs.Contains(' '))
            {
                int spaceIndex = numOfJobs.IndexOf(' ');
                string totalJobs = numOfJobs[0..spaceIndex];
                int totalJobsInt = int.Parse(totalJobs.Replace(",", ""));
                numberOfPages = (int)Math.Ceiling(totalJobsInt / 25);
                if (numberOfPages > 40)
                {
                    numberOfPages = 40;
                }
            }
            else
            {
                numberOfPages = int.Parse(numOfJobs);
            }

            return numberOfPages;
        }

        public static string[] UrlToKeywords(string url)
        {
            string keywordUrl = url.Substring(url.IndexOf("keywords=") + 9);
            string keyword = keywordUrl[0..keywordUrl.IndexOf("&")];
            string locationUrl = url.Substring(url.IndexOf("location=") + 9);
            string location = locationUrl[0..locationUrl.IndexOf("&")];
            return new string[] { keyword, location };
        }

        public static void WriteResults(string text)
        {
            string timeStr = DateTime.Now.ToString("yyyyMMdd");
            string fileName = $"Applied Jobs DATA - {timeStr}.txt";
            try
            {
                string[] lines = System.IO.File.ReadAllLines($"data/{fileName}");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter($"data/{fileName}", false, System.Text.Encoding.UTF8))
                {
                    file.WriteLine("---- Applied Jobs Data ---- created at: " + timeStr);
                    file.WriteLine("---- Number | Job Title | Company | Location | Work Place | Posted Date | Applications | Result ");
                    foreach (string line in lines.Where(line => !line.Contains("----")))
                    {
                        file.WriteLine(line);
                    }
                    file.WriteLine(text);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter($"data/{fileName}", false, System.Text.Encoding.UTF8))
                {
                    file.WriteLine("---- Applied Jobs Data ---- created at: " + timeStr);
                    file.WriteLine("---- Number | Job Title | Company | Location | Work Place | Posted Date | Applications | Result ");
                    file.WriteLine(text);
                }
            }
        }

        public static void PrintInfoMes(string bot)
        {
            PrYellow($"ℹ️ {bot} bot is starting to work.");
        }

        public static void PrintSuccessMes(string bot)
        {
            PrGreen($"✅ {bot} bot has finished working.");
        }

        public static void PrintFailMes(string bot)
        {
            PrRed($"❌ {bot} bot has failed to work.");
        }

        public static double RandomUniform(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }
    }
}