/*
 * Created by Ranorex
 * User: cbreit
 * Date: 29.06.2017
 * Time: 16:23
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

namespace WPDockerDemo
{
    /// <summary>
    /// Description of CreateEndpoint.
    /// </summary>
    [TestModule("07B2F576-FE38-4E07-82BA-FB6DDF635CC6", ModuleType.UserCode, 1)]
    public class CreateEndpoint : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CreateEndpoint()
        {
            // Do not delete - a parameterless constructor is required!
        }

        string _epUrl = "";
        [TestVariable("15619a1b-66b2-495e-9060-d0ebd9604627")]
        public string epUrl
        {
          get { return _epUrl; }
          set { _epUrl = value; }
        }

        string _conf = "";
        [TestVariable("310125dd-d115-41ea-b386-e5595de16747")]
        public string conf
        {
          get { return _conf; }
          set { _conf = value; }
        }
        
        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void ITestModule.Run()
        {
          if(conf.Length >0)
          {
            // Create endpoint management factory
            var fac = new RemoteEndpointFactory();
            var ep = fac.CreateTransientWebDriverEndpoint(new WebDriverEndpointInfo("tempEp", epUrl));
            
            var values = conf.Split(',');
            string epConf = String.Format(@"{{""browserName"": ""{0}"", ""Version"": ""{1}"", ""video"":true}}", values[0].Trim(), values[2].Trim());
            
            Report.Info(epConf);
            
            var cfg = WebDriverConfiguration.FromJson(epConf);
            cfg.Name = String.Format("tmpConf_{0}_{1}_{2}", values[0].Trim(), values[1].Trim(),values[2].Trim());
            cfg.Description = "";
            
            ep.ActivateConfiguration(cfg);
            
            ep.ConnectAsync()
                .ContinueWith(_ => ep.MakeCurrentHostAsync())
                .Wait();
          }
          else
          {
            Report.Info("Not creating endpoint -- no config provided!");
          }
        }
    }
}
