using System;
using System.Threading;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium;
namespace Utils
{

    public class WinAppDriver
    {
        //Appium Driver URL it works like a windows Service on your PC  
        private const string appiumDriverURI = "http://127.0.0.1:4723";
        //Application Key of your UWA   
        //U can use any .Exe file as well for open a windows Application  
        private const string calApp = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";

        protected static WindowsDriver<WindowsElement> winAppSession;

        public void SelectFile(string filePath)
        {
            if (winAppSession == null)
            {

                //DesiredCapabilities appCapabilities = new DesiredCapabilities();
                AppiumOptions options = new AppiumOptions();
                options.PlatformName = "Windows";                
                options.AddAdditionalCapability("PlatformName", "Windows");                
                options.AddAdditionalCapability("app", @"C:\Program Files\Google\Chrome\Application\chrome.exe");

                //new Uri("http://127.0.0.1:4723"),
                winAppSession = new WindowsDriver<WindowsElement>( options);
                winAppSession.FindElementByName("Open").Click();
                
                Console.WriteLine("sssssssssssssssssssssssssssss");
            }
        }
    }
}