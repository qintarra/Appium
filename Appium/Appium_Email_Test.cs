using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace appium_email_test
{
    public class Tests
    {
        private AppiumDriver<AndroidElement> _driver;
        private static readonly string appium = "http://localhost:4723/wd/hub";

        [SetUp]
        public void Setup()
        {
            var driverOption = new AppiumOptions();
            driverOption.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            driverOption.AddAdditionalCapability(MobileCapabilityType.DeviceName, "Pixel_3a_API_33_x86_64"); //your device name
            driverOption.AddAdditionalCapability(MobileCapabilityType.AutomationName, "UiAutomator2");

            _driver = new AndroidDriver<AndroidElement>(new Uri(appium), driverOption);
        }


        private void Swipe()
        {
            TouchAction swipeUp = new TouchAction(_driver);
            swipeUp.Press(550, 1500)
                .Wait(0).MoveTo(550, 500)
                .Release()
                .Perform();
        }

        [Test]
        public void GmailMessageSend()
        {
            Swipe();

            _driver.FindElementByAccessibilityId("Gmail").Click();
            _driver.FindElementById("com.google.android.gm:id/welcome_tour_got_it").Click();
            _driver.FindElementById("com.google.android.gm:id/action_done").Click();
            _driver.FindElementByAccessibilityId("Close").Click();
            _driver.FindElementById("com.google.android.gm:id/compose_button").Click();         
            _driver.FindElementByXPath("//android.widget.EditText[@text=\"\"]\r\n").SendKeys("CHANGE_ME@gmail.com"); // email of the recipient
            _driver.FindElementById("com.google.android.gm:id/peoplekit_listview_flattened_row").Click();
            _driver.FindElementById("com.google.android.gm:id/subject").SendKeys("appium_hw");
            _driver.HideKeyboard();
            _driver.FindElementByAccessibilityId("Send").Click();

            WebDriverWait forSent = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
            forSent.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//android.widget.TextView[@text=\"Sent\"]\r\n")));

            _driver.FindElementByAccessibilityId("Open navigation drawer").Click();
            _driver.FindElementByXPath("//android.widget.TextView[@text=\"Sent\"]\r\n").Click();
            _driver.FindElementByXPath("//android.widget.TextView[@text=\"appium_hw\"]\r\n");
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
                _driver.Quit();
        }
    }
}