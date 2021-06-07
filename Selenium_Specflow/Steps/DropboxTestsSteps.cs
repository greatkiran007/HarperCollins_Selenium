using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Selenium_Specflow.Hooks;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using Ledger_AutomationTesting.ExcelUtilities;
using Selenium_Specflow.PageObjects;


namespace Selenium_Specflow.Features
{
    [Binding]
    public class DropboxTestsSteps
    {

        public int items = 0;
        IWebDriver _driver;

        Dropbox dropbox = new Dropbox();
        public DropboxTestsSteps() => _driver = Hook.GetDriver();
        ExcelLib excel = new ExcelLib();

        [Given(@"I Launch Dropbox URL (.*)")]
        public void GivenILaunchDropboxURL(string p0)
        {
            dropbox.NavigateToHome(p0);
            
        }
        
        [Given(@"I Login to Dropbox with (.*) and (.*)")]
        public void GivenILoginToDropboxWithAnd(string p0, string p1)
        {
            dropbox.Login(p0, p1);
        }
        
        [Then(@"I navigate to Folder '(.*)'")]
        public void ThenINavigateToFolder(string p0)
        {
            dropbox.NavigateToFolder(p0);
        }


        [Then(@"upload the file '(.*)'")]
        public void ThenUploadTheFile(string p0)
        {
            dropbox.UploadFile(p0);
        }

        [Then(@"upload the folder '(.*)'")]
        public void ThenUploadTheFolder(string p0)
        {
            dropbox.UploadFolder(p0);
        }

        [Then(@"Drag the file '(.*)' and Drop into folder '(.*)'")]
        public void ThenDragTheFileAndDropIntoFolder(string p0, string p1)
        {
            dropbox.DragNDropFile(p0, p1);
        }




    }
}
