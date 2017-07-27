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
    /// Description of SpawnWPContainer.
    /// </summary>
    [TestModule("87F6E316-7E15-478B-AEAA-30E3EEE9E26A", ModuleType.UserCode, 1)]
    public class SpawnWPContainer : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SpawnWPContainer()
        {
            // Do not delete - a parameterless constructor is required!
        }

        string _dockerURL = "";
        [TestVariable("df8bbec9-c999-47a0-8db2-ecfa2efa2a0f")]
        public string dockerURL
        {
          get { return _dockerURL; }
          set { _dockerURL = value; }
        }

        string _certFile = "";
        [TestVariable("2b0f4605-a1e7-41a6-ab86-ed4ae6f1d382")]
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
          // docker run --name wp1 -p 5556:80 --link mysql:mysql 
          // -e WORDPRESS_DB_PASSWORD=pwd -e WORDPRESS_DB_NAME=wp1 -d wordpress

          ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
          
          DockerClient client = null;
          if(certFile.Length >0)
          {
            var credentials = new CertificateCredentials(new X509Certificate2(certFile));
            client = new DockerClientConfiguration(new Uri(dockerURL), credentials).CreateClient();
          }
          else
            client = new DockerClientConfiguration(new Uri(dockerURL)).CreateClient();


   
          Random r = new Random();
            int portNr = r.Next(1500, 8000);
            TestSuite.CurrentTestContainer.Parameters["URL"] = TestSuite.CurrentTestContainer.Parameters["URL"] + ":" +  portNr.ToString();
            
            //Report.Info(portNr.ToString());
            //Report.Info(TestSuite.CurrentTestContainer.Parameters["URL"]);
            
            Docker.DotNet.Models.HostConfig hostConf = new Docker.DotNet.Models.HostConfig();
            hostConf.Links = new List<string>();
            hostConf.Links.Add("mysql");

            var portBindings = new Dictionary<string, IList<Docker.DotNet.Models.PortBinding>> {
                        {
                            80.ToString(), new List<Docker.DotNet.Models.PortBinding> {
                                new Docker.DotNet.Models.PortBinding { HostPort = portNr.ToString(),
                                                                       HostIP = "0.0.0.0"}
                            }
                        }
                    };
            var exposedPorts = new Dictionary<string, object>() {
                    { 80.ToString(), new { HostPort = portNr.ToString() } }
                };

            hostConf.PortBindings = portBindings;

            Docker.DotNet.Models.CreateContainerParameters createContainerParams = new Docker.DotNet.Models.CreateContainerParameters();
            createContainerParams.HostConfig = hostConf;
            createContainerParams.Image = "a8f5a76ca5cd";

            createContainerParams.Env = new List<string>();
            createContainerParams.Env.Add("WORDPRESS_DB_PASSWORD=pwd");
            createContainerParams.Env.Add(String.Format("WORDPRESS_DB_NAME=wp{0}", portNr));


            createContainerParams.ExposedPorts = exposedPorts;

            var createTask = client.Containers.CreateContainerAsync(createContainerParams);
            createTask.Wait();
            var container = createTask.Result;
            
            TestSuite.CurrentTestContainer.Parameters["containerID"] = container.ID;

            var incpectTask = client.Containers.InspectContainerAsync(container.ID);
            incpectTask.Wait();
            var inspect = incpectTask.Result;

            var startParams = new Docker.DotNet.Models.ContainerStartParameters();
            var startTast = client.Containers.StartContainerAsync(container.ID, startParams);
            startTast.Wait();

            



        }
    }
}
