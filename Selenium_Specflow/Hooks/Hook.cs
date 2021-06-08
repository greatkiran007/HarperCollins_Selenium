using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;
using Microsoft.Vbe.Interop;
using System.Configuration;

namespace Selenium_Specflow.Hooks
{
    [Binding]
    public sealed class Hook
    {
        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;
        //private readonly IObjectContainer _objectContainer;
        private static IWebDriver _driver;
        //private static KlovReporter klov;
        private ScenarioContext _scenarioContext;
        private static FeatureContext _featureContext;
        public static string errImagePath;
        public Hook(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {

            

            SelectBrowser(BrowserType.Chrome);
            //Initialize Extent report before test starts
            var htmlReporter = new ExtentHtmlReporter(System.AppContext.BaseDirectory+"TestReport.html");
            htmlReporter.Configuration().Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            //Attach report to reporter
            extent = new ExtentReports();

            extent.AttachReporter(htmlReporter);
        }

        public Dictionary<string, string> GetConfig(string path)
        {

            

            
            string fileData = "";
            using (StreamReader sr = new StreamReader(path))
            {
                fileData = sr.ReadToEnd().Replace("\r", "");
            }
            Dictionary<string, string> Properties = new Dictionary<string, string>();
            string[] kvp;
            string[] records = fileData.Split("\n".ToCharArray());
            foreach (string record in records)
            {
                kvp = record.Split("=".ToCharArray());
                Properties.Add(kvp[0], kvp[1]);
            }
            return Properties;

        }
            
        [AfterStep]
        public void InsertReportingStep(ScenarioContext scenarioContext)
        {

            if (scenarioContext.TestError == null)
            {
                var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                if (stepType == "Given")
                    scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "When")
                    scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "And")
                    scenario.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text);
            }
            else if (scenarioContext.TestError != null)
            {
                var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                if (stepType == "Given")
                {
                    scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.InnerException);
                    scenario.AddScreenCaptureFromPath(errImagePath);
                }


                else if (stepType == "When")
                { scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.InnerException);
                    scenario.AddScreenCaptureFromPath(errImagePath);
                }

                else if (stepType == "Then")
                {  scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                scenario.AddScreenCaptureFromPath(errImagePath);
                }

                else if (stepType == "And") 
                { 
                scenario.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.InnerException);
                scenario.AddScreenCaptureFromPath(errImagePath);
                }

        }
        scenario.AddScreenCaptureFromPath(errImagePath);

            Thread.Sleep(1000);  

        }

        public static IWebDriver GetDriver()
        {
            return _driver;
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            _driver.Quit();
            //Flush report once test completes
            extent.Flush();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            //Create dynamic feature name
            featureName = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

       
        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            //SelectBrowser(BrowserType.Chrome);
            //Create dynamic scenario name
            scenario = featureName.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);

        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            //SelectBrowser(BrowserType.Chrome);
            //Create dynamic scenario name
            //scenario = featureName.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);

            //_driver.Quit();
            //Flush report once test completes
           // extent.Flush();

        }

        public static string TakeScreenshot(IWebDriver driver)
        {
            string path1 = AppDomain.CurrentDomain.BaseDirectory+ "Screenshot";

            if (Directory.Exists(path1))
            {
                Console.WriteLine("That path exists already.");
            }
            else {
                DirectoryInfo di = Directory.CreateDirectory(path1);
            }
            // Try to create the directory.

            errImagePath = path1 + "error.png";
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(errImagePath, ScreenshotImageFormat.Png);
            return errImagePath;
        }


        private static void SelectBrowser(BrowserType browserType)
        {

            var chrome = ConfigurationManager.AppSettings["Chrome"];
            String BasePath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", "");

            switch (browserType)
            {
                case BrowserType.Chrome:
                    var options = new ChromeOptions();
                    options.AddArgument("no-sandbox");
                    ChromeDriverService service = ChromeDriverService.CreateDefaultService("webdriver.chrome.driver", BasePath+ chrome);
                    _driver = new ChromeDriver(service, options, TimeSpan.FromMinutes(3));
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Int16.Parse(ConfigurationManager.AppSettings["ImplicitWait"]));
                    _driver.Manage().Window.Maximize();
                    
                    break;
                case BrowserType.Firefox:
                    var driverDir = System.IO.Path.GetDirectoryName(ConfigurationManager.AppSettings["GeckoDir"]);
                    FirefoxDriverService ffservice = FirefoxDriverService.CreateDefaultService(driverDir, BasePath+ConfigurationManager.AppSettings["Firefox"]);
                    ffservice.FirefoxBinaryPath = ConfigurationManager.AppSettings["FirefoxBinary"];
                    ffservice.HideCommandPromptWindow = true;

                    ffservice.SuppressInitialDiagnosticInformation = true;
                    ffservice.Start();
                    _driver = new FirefoxDriver(ffservice);
                    
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Int16.Parse(ConfigurationManager.AppSettings["ImplicitWait"]));
                    _driver.Manage().Window.Maximize();

                    break;
                default:
                    break;
            }
        }

        enum BrowserType
        {
            Chrome, Firefox, Edge
        }
    }
}
