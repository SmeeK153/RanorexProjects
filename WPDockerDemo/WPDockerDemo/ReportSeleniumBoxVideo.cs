/*
 * Created by Ranorex
 * User: cbreit
 * Date: 04.07.2017
 * Time: 14:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using System.Linq;
using OpenQA.Selenium;

namespace WPDockerDemo
{
    /// <summary>
    /// Description of ReportSeleniumBoxInfo.
    /// </summary>
    [TestModule("2AC383F2-A0D0-4F7F-8E3E-5D4EF8767C07", ModuleType.UserCode, 1)]
    public class ReportSeleniumBoxVideo : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ReportSeleniumBoxVideo()
        {
            // Do not delete - a parameterless constructor is required!
        }

        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void ITestModule.Run()
        {
          var webDriverEndpoint = Host.Local.TryGetAsWebDriverEndpoint();
          if(webDriverEndpoint == null){
              Report.Warn("This test is not running on a web driver host");
              return;
          }
          var existingDriver = (OpenQA.Selenium.Remote.RemoteWebDriver)webDriverEndpoint.WebDrivers.First();
          
          string videoView = String.Format(@"<a href=""{0}"">{1}</a>", "https://789b1ea7eca7.element34.net/videos/" + existingDriver.SessionId.ToString() + ".mp4", "Automation video");
          Report.LogHtml(ReportLevel.Info, "Info", videoView); 
        }
    }
}
