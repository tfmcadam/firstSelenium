using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using firstSelenium.Models;

namespace firstSelenium.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // public class AutomatedUITests : IDisposable
    // {
    //     private readonly IWebDriver _driver;
    //     public AutomatedUITests() => _driver = new ChromeDriver();
    //     public void Dispose()
    //     {
    //         ChromeDriver driver = new ChromeDriver("C:/Program Files/Google/Chrome/Application/chrome.exe");
    //         driver.Navigate().GoToUrl("https://www.Linkedin.com/login");
    //         driver.FindElement(By.Name("session_key.SendKeys"));
    //         Thread.Sleep(20000);
    //         driver.Quit();
    //         _driver.Quit();
    //         _driver.Dispose();
    //     }
    // }

    public IActionResult Index()
    {
        return View();
    }

    public ActionResult ApplyToJobs()
    {
        LinkedInJobSearch.Program.Main(null);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
