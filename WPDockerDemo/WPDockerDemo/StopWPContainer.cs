/*
 * Created by Ranorex
 * User: cbreit
 * Date: 06.06.2017
 * Time: 14:59
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

using System.Net;
using Docker.DotNet;
using Docker.DotNet.X509;
using System.Security.Cryptography.X509Certificates;


namespace WPDockerDemo
{
    /// <summary>
    /// Description of StopWPContainer.
    /// </summary>
    [TestModule("F00563F5-5D62-4A56-8F3A-D68B6251C2C3", ModuleType.UserCode, 1)]
    public class StopWPContainer : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public StopWPContainer()
        {
            // Do not delete - a parameterless constructor is required!
        }
        string _dockerURL = "";
        [TestVariable("df8bbec9-c999-47a0-8db2-ecfa2efa2a01")]
        public string dockerURL
        {
          get { return _dockerURL; }
          set { _dockerURL = value; }
        }

        string _certFile = "";
        [TestVariable("2b0f4605-a1e7-41a6-ab86-ed4ae6f1d381")]
        public string certFile
        {
          get { return _certFile; }
          set { _certFile = value; }
        }
        
        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void ITestModule.Run()
        {
          ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
          
          DockerClient client = null;
          if(certFile.Length >0)
          {
            var credentials = new CertificateCredentials(new X509Certificate2(certFile));
            client = new DockerClientConfiguration(new Uri(dockerURL), credentials).CreateClient();
          }
          else
            client = new DockerClientConfiguration(new Uri(dockerURL)).CreateClient();
        	
        	var id = TestSuite.CurrentTestContainer.Parameters["containerID"];
        	
          var stopParams = new Docker.DotNet.Models.ContainerStopParameters() { WaitBeforeKillSeconds = 10 };
          CancellationToken token = new CancellationToken();
          var stopTask = client.Containers.StopContainerAsync(id, stopParams, token);
          stopTask.Wait();

          var remParams = new Docker.DotNet.Models.ContainerRemoveParameters();
          var deleteTask = client.Containers.RemoveContainerAsync(id, remParams);
          deleteTask.Wait(new TimeSpan(0,0,15));
        }
    }
}
