using Selenium_Specflow.Hooks;
using Selenium_Specflow.CustomMethods ;
using OpenQA.Selenium;
using System;
using System.Threading;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Selenium_Specflow.Config;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using Ledger_AutomationTesting.ExcelUtilities;
using System.Data;
using System.Data.OleDb;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Linq.Expressions;

namespace Selenium_Specflow.PageObjects
{
    
    class Amazon
    {
        private IWebDriver _driver;
        public Amazon() => _driver = Hook.GetDriver();

        DataTable dt = new DataTable();             
        
        ExcelLib excel = new ExcelLib();

        

        IWebElement searchBox => _driver.FindElement(By.XPath("//input[@id='twotabsearchtextbox']"));
        IWebElement submitBtn => _driver.FindElement(By.XPath("//input[@id='nav-search-submit-button']"));
        IWebElement productTitle => _driver.FindElement(By.XPath("//span[@id='productTitle']"));
        IList<IWebElement> productImgs => _driver.FindElements(By.XPath("//li[@class='a-spacing-small item imageThumbnail a-declarative']"));
        IWebElement popupClose => _driver.FindElement(By.XPath("//button[@data-action='a-popover-close']"));
        IWebElement popupTitle => _driver.FindElement(By.XPath("//div[@id='ivTitle']"));

        IWebElement itemPrice => _driver.FindElement(By.XPath("//span[@id='priceblock_dealprice']"));
        IWebElement deliveryDates => _driver.FindElement(By.XPath("//div[@id='ddmDeliveryMessage']/b"));
        
        //TestDataId	SearchString	ItemName	Color	Size	Price	DeliveryDates
        public void NavigateToHome(string url)
        {            
            
            _driver.Navigate().GoToUrl(url.Replace("'", ""));
            Assert.IsTrue(_driver.Title.ToLower().Contains("amazon"));
        }


        public void Search(string searchString)
        {
            searchBox.EnterText(searchString);
            submitBtn.Click();
            Thread.Sleep(3000);
            Assert.IsTrue(_driver.Title.ToLower().Contains(searchString.ToLower()));   
        }

        public void SelectItem(string itemName)
        {
            IWebElement itemLink =_driver.FindElement(By.XPath("//span[contains(text(), '"+ itemName + "')]"));
            itemLink.Click();
            Thread.Sleep(3000);
            _driver.SwitchTo().Window(_driver.WindowHandles.Last());
            Assert.IsTrue(_driver.Title.ToLower().Contains(itemName.ToLower()));
        }

        public void ChooseColorAndSize(string color, string size)
        {
            //li[@title='Click to select 128GB']
            IWebElement colorLink = _driver.FindElement(By.XPath("//li[@title='Click to select "+ color + "']"));
            IWebElement sizeLink = _driver.FindElement(By.XPath("//li[@title='Click to select " + size + "']"));
            colorLink.Click();
            Thread.Sleep(3000);
            sizeLink.Click();
            Thread.Sleep(3000);
            string price = itemPrice.Text;
            string delvDates = deliveryDates.Text;
            excel.writeToExcel("Amazon", 2, 6, price);
            excel.writeToExcel("Amazon", 2, 7, delvDates);
        }

        public void HoverOverImages()
        {
            int i = 1;
            foreach (IWebElement img in productImgs)
            {
                img.Click();
                Thread.Sleep(2000);
                //IWebElement mainImage = _driver.FindElement(By.XPath("(//img[@class='a-dynamic-image a-stretch-horizontal'])[" + i + "]"));
                //mainImage.Click();
                //ul[@class='a-unordered-list a-nostyle a-horizontal list maintain-height']
                IWebElement mainImage = _driver.FindElement(By.XPath("//div[@id='main-image-container']"));

                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                string title = (string)js.ExecuteScript("return document.title");
                js.ExecuteScript("var mouseEvent = document.createEvent('MouseEvents');mouseEvent.initEvent('mouseover', true, true); arguments[0].dispatchEvent(mouseEvent);", mainImage);

                js.ExecuteScript("arguments[0].scrollIntoView(true);", mainImage);
                mainImage.Click();
                Thread.Sleep(2000);
                if (_driver.FindElements(By.XPath("//button[@data-action='a-popover-close']")).Count>0) {
                    popupClose.Click();
                    Thread.Sleep(2000);
                }


                Thread.Sleep(3000);
               // string popupText = popupTitle.Text;
               // popupClose.Click();
                //Thread.Sleep(2000);
                i = i + 1;
            }



        }


            public void UploadFile(string filePath)
        {
            //string filepath=createRandomFile(@"C:\test");
            


        }
        private static string GetExistingProcessHandle()
        {
            string mainWindowHandle = null;

            var process = Process.GetProcessesByName("chrome").FirstOrDefault();
            //var processes[] = Process.GetProcessesByName("chrome");
            IEnumerable<Process> oPrs = Process.GetProcessesByName("chrome").OrderBy(pr => pr.StartTime);
            //ProcessThreadCollection threads = process.Threads;

            foreach (Process oPr in oPrs)
            {
                Console.WriteLine(oPr.StartTime);
                Console.WriteLine(oPr.Handle);


                // if item.Name == "FooThread", then ThreadExists = true...
                // so, if !ThreadExists then call FooThreadCaller() and so on.
            }
            //
            Console.WriteLine(oPrs.Last().ProcessName);
            Console.WriteLine(oPrs.Last().StartTime);

            if (process != null)
                mainWindowHandle = oPrs.Last().Handle.ToString("x");
            

            return mainWindowHandle;
        }


        public string createRandomFile(string folderPath)
        {

            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            string fileName = folderPath+ @"\"+ timeStamp + ".txt";

            
            FileInfo fi = new FileInfo(fileName);

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (fi.Exists)
                {
                    fi.Delete();
                }

                // Create a new file     
                using (FileStream fs = fi.Create())
                {
                    Byte[] txt = new UTF8Encoding(true).GetBytes("New file.");
                    fs.Write(txt, 0, txt.Length);
                    Byte[] author = new UTF8Encoding(true).GetBytes("bllah blah");
                    fs.Write(author, 0, author.Length);
                }

                // Write file contents on console.     
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
                return fileName;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
                return fileName;
            }



        }



        /*Utils.WinAppDriver winApp = new Utils.WinAppDriver();
        //winApp.SelectFile(filePath);


        // WinAppDriver code starts here


        string handle = _driver.CurrentWindowHandle;

        Console.WriteLine(_driver.Title);
            AppiumOptions options = new AppiumOptions();
        options.PlatformName = "Windows";
            options.AddAdditionalCapability("PlatformName", "Windows");
            handle = handle.Replace("CDwindow-", "");
            string currHandle = GetExistingProcessHandle();
        options.AddAdditionalCapability("appTopLevelWindow", currHandle);
            //options.AddAdditionalCapability("app", @"C:\Program Files\Google\Chrome\Application\chrome.exe");


            winAppSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"),options);
            // uploading a sample file called “Data.txt” from the D drive.
            // IWebElement openWindowPopupFileNameTextbox = winAppSession.FindElementByXPath("//Edit[@Name='File name:']");
            //openWindowPopupFileNameTextbox.SendKeys(filePath);

            //IWebElement openWindowPopupOpenButton = winAppSession.FindElementByXPath("//Button[@Name='Open'][@AutomationId='1']");
            //openWindowPopupOpenButton.Click();

            winAppSession.SwitchTo().Window(handle);
        Console.WriteLine("This is the page with title: " + winAppSession.Title);

            string xpath_LeftClickButtonOpen = "/Pane[@ClassName='#32769'][@Name='Desktop 1']/Pane[@ClassName='Chrome_WidgetWin_1'][@Name='Recordings - Dropbox - Google Chrome']/Window[@ClassName='#32770'][@Name='Open']/Button[@ClassName='Button'][@Name='Open']";
        IWebElement openWindowPopupOpenButton = winAppSession.FindElementByXPath(xpath_LeftClickButtonOpen);


        openWindowPopupOpenButton.Click();*/

        
    }

    
}
