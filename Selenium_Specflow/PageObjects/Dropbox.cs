using Selenium_Specflow.Hooks;
using OpenQA.Selenium;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Selenium_Specflow.Config;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using Ledger_AutomationTesting.ExcelUtilities;
using System.Data;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;


//using Selenium_Specflow.Steps;
namespace Selenium_Specflow.PageObjects
{
    
    class Dropbox
    {
        private IWebDriver _driver;
        public Dropbox() => _driver = Hook.GetDriver();
        DataTable dt = new DataTable();
        
        ExcelLib excel = new ExcelLib();
        
        IWebElement emailField => _driver.FindElement(By.XPath("//input[@name='login_email']"));
        IWebElement passwordField => _driver.FindElement(By.XPath("//input[@name='login_password']"));
        IWebElement submitBtn => _driver.FindElement(By.XPath("//button[@type='submit']"));
        IWebElement overviewText => _driver.FindElement(By.XPath("//span[contains(text(),'Overview')]"));

        IWebElement uploadBtn => _driver.FindElement(By.XPath("//button[@aria-label='Upload, applies to Recordings']"));

        IWebElement uploadFolder => _driver.FindElement(By.XPath("//div[contains(text(), 'Folder')]"));

        IWebElement uploadFile => _driver.FindElement(By.XPath("//div[contains(text(), 'Files')]"));
        

        IWebElement confMsg => _driver.FindElement(By.XPath("//span[@class='dig-Snackbar-message ']"));
        IWebElement accntMenu => _driver.FindElement(By.XPath("//div[@aria-label='Account menu']"));
        IWebElement signOut => _driver.FindElement(By.XPath("//div[contains(text(), 'Sign out')]"));



        public void Signout() {
            accntMenu.Click();
            signOut.Click();

        }


        public void NavigateToHome(string url)
        {            
            
            _driver.Navigate().GoToUrl(url);
            Assert.IsTrue(_driver.Title.ToLower().Contains("dropbox"));
        }


        public void Login(string email, string password)
        {           
            emailField.EnterText(email);
            passwordField.EnterText(password);
            submitBtn.Click();
            Thread.Sleep(10000);
            Assert.IsTrue(_driver.Title.ToLower().Contains("files"));          
            
        }

        public void NavigateToFolder(string folderPath)
        {

            IWebElement folderLink =_driver.FindElement(By.XPath("//a[@href= '/home/" + folderPath + "']"));
            folderLink.Click();
            Thread.Sleep(5000);
            Assert.IsTrue(_driver.Title.ToLower().Contains(folderPath.ToLower()));

        }

        public void UploadFile(string filePath)
        {
            //string filepath=createRandomFile(@"C:\test");
            uploadBtn.Click();
            uploadFile.Click();
            Thread.Sleep(3000);

            SendKeys.SendWait(filePath);
            SendKeys.SendWait("{Enter}");
            Thread.Sleep(5000);
            Assert.IsTrue(confMsg.Text.ToLower().Contains("uploaded"));

            //div[@aria-label='Account menu']
            //div[contains(text(), 'Sign out')]

            Signout();
        }

        public void UploadFolder(string folderPath)
        {
            
            uploadBtn.Click();
            uploadFolder.Click();
            Thread.Sleep(3000);

            SendKeys.SendWait(folderPath);
            SendKeys.SendWait("{Enter}");
            SendKeys.SendWait("{Enter}");
            Thread.Sleep(5000);
            Console.WriteLine(_driver.WindowHandles.Count);

            //SwitchTo Alert is not working here, hence using SendKeys here
            //AcceptAlert(_driver);
            //_driver.SwitchTo().DefaultContent();
            
            SendKeys.SendWait("{Tab}");
            Thread.Sleep(2000);
            SendKeys.SendWait("{Enter}");
            Thread.Sleep(5000);
            Assert.IsTrue(confMsg.Text.ToLower().Contains("uploaded"));
            Signout();
        }

        public void AcceptAlert(IWebDriver dr) {

            WebDriverWait wait = new WebDriverWait(dr, TimeSpan.FromSeconds(30));            
            wait.Until(ExpectedConditions.AlertIsPresent()).Accept();
        }
        public void DismissAlert(IWebDriver dr)
        {
            WebDriverWait wait = new WebDriverWait(dr, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.AlertIsPresent()).Dismiss();
        }

        public void DragNDropFile(string fileName, string folderName)
        {
            IWebElement fileLink = _driver.FindElement(By.XPath("//span[contains(text(), '" + fileName + "')]"));
            //IWebElement fileLink = _driver.FindElement(By.PartialLinkText(fileName));
            //IWebElement fileLink = _driver.FindElement(By.XPath("//span[@class='dig-Checkbox']"));
            
            IWebElement folderLink = _driver.FindElement(By.XPath("//span[contains(text(), '"+ folderName + "')]"));




            //1. using JS Script executor -not working
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            string title = (string)js.ExecuteScript("return document.title");
            js.ExecuteScript("function createEvent(typeOfEvent) {\n" + "var event =document.createEvent(\"CustomEvent\");\n"
                    + "event.initCustomEvent(typeOfEvent,true, true, null);\n" + "event.dataTransfer = {\n" + "data: {},\n"
                    + "setData: function (key, value) {\n" + "this.data[key] = value;\n" + "},\n"
                    + "getData: function (key) {\n" + "return this.data[key];\n" + "}\n" + "};\n" + "return event;\n"
                    + "}\n" + "\n" + "function dispatchEvent(element, event,transferData) {\n"
                    + "if (transferData !== undefined) {\n" + "event.dataTransfer = transferData;\n" + "}\n"
                    + "if (element.dispatchEvent) {\n" + "element.dispatchEvent(event);\n"
                    + "} else if (element.fireEvent) {\n" + "element.fireEvent(\"on\" + event.type, event);\n" + "}\n"
                    + "}\n" + "\n" + "function simulateHTML5DragAndDrop(element, destination) {\n"
                    + "var dragStartEvent =createEvent('dragstart');\n" + "dispatchEvent(element, dragStartEvent);\n"
                    + "var dropEvent = createEvent('drop');\n"
                    + "dispatchEvent(destination, dropEvent,dragStartEvent.dataTransfer);\n"
                    + "var dragEndEvent = createEvent('dragend');\n"
                    + "dispatchEvent(element, dragEndEvent,dropEvent.dataTransfer);\n" + "}\n" + "\n"
                    + "var source = arguments[0];\n" + "var destination = arguments[1];\n"
                    + "simulateHTML5DragAndDrop(source,destination);", fileLink, folderLink);


            //2. Usng Actions - try1 - not working
            var builder = new Actions(_driver);
            var dragAndDrop = builder.ClickAndHold(fileLink)
                                        .MoveToElement(folderLink)                                        
                                        .Release(folderLink)
                                        .Build();
            dragAndDrop.Perform();

            //3. Usng Actions - try2 -not working
            var dragAndDrop1 = builder.ClickAndHold(fileLink).MoveToElement(folderLink).Release(fileLink).Build();
            dragAndDrop1.Perform();

            //3. Usng DragAndDrop method- not working
            DragAndDropItem(fileLink, folderLink);

            Thread.Sleep(5000);
            Assert.IsTrue(confMsg.Text.ToLower().Contains("moved"));


            //4. using MouseSimulator- didnt work
            int xFrom = fileLink.Location.X;
            int yFrom = fileLink.Location.Y;
            int xTo = folderLink.Location.X;
            int yTo = folderLink.Location.Y;

            /*MouseSimulator.X = xFrom;
            MouseSimulator.Y = yFrom;
            MouseSimulator.MouseDown(MouseKeyboardLibrary.MouseButton.Left);
            MouseSimulator.X = xTo;
            MouseSimulator.Y = yTo;
            MouseSimulator.MouseUp(MouseKeyboardLibrary.MouseButton.Left);*/


            Signout();
        }

        public void DragAndDropItem(IWebElement from, IWebElement to)
        {
            Actions action = new Actions(_driver);
            action.DragAndDrop(from, to).Build().Perform();
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



    }
}
