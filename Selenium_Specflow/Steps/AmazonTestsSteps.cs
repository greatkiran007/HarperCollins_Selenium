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
    public class AmazonTestsSteps
    {

        IWebDriver _driver;
        public AmazonTestsSteps() => _driver = Hook.GetDriver();

        Amazon Amazon = new Amazon();
        
        ExcelLib excel = new ExcelLib();


        [Given(@"I navigate to URL (.*)")]
        public void GivenILaunchDropboxURL(string p0)
        {
            Amazon.NavigateToHome(p0);

        }


        [Given(@"I enter search word in Search field")]
        public void GivenIEnterSearchWordInSearchField()
        {
            string searchStr = excel.getValueFromExcel("Amazon", 1, 2);
            Amazon.Search(searchStr);
        }
        
        [Then(@"I select the Item")]
        public void ThenISelectTheItem()
        {
            string ItemName = excel.getValueFromExcel("Amazon", 1, 3);
            Amazon.SelectItem(ItemName);
        }
        
        [Then(@"I select item color and size")]
        public void ThenISelectItemColorAndSize()
        {
            string Color = excel.getValueFromExcel("Amazon", 1, 4);
            string Size = excel.getValueFromExcel("Amazon", 1, 5);
            Amazon.ChooseColorAndSize(Color, Size);
        }
        
        [Then(@"I hover over the images")]
        public void ThenIHoverOverTheImages()
        {
            Amazon.HoverOverImages();
        }
    }
}
