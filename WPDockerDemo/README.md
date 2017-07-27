# WordPress Docker Demo

This Ranorex project automates the bootstrap process of WordPress. However, this has to be done only once for a WordPress instance and can thus not easily be done twice on one instance or even in parallel.

The automation carries out the following steps:

- Spawn a docker container, which contains a Wordpress instance
- Open a Browser through a Selenium endpoint
- Automate the bootstrap process
- Validate the bootstraping result
- Close the session on the endpoint
- Stop and remove the Wordpress container

_Note:_ a MySQL docker instance needs to be running, as the containers are linking to this instance to store their data. The DBs created during the bootstrap processes are not purged automatically. The DB thus whould be reset at certain times, but should be fine for playing around.

## Parallel execution

The "SeleniumRunner.exe" gets started by batch-file and simply starts multiple Ranorex test instances in a loop by using the [Process.Start Method](https://msdn.microsoft.com/en-us/library/system.diagnostics.process.start(v=vs.110).aspx "MSDN doc"). To run the automation on multiple endpoins, Ranorex endpoints are created dynamically in code in the "CreateEnpoint" user code module according to the content of the "Browsers.csv" file _(Sidenote: A more convenient way to start multiple Selenium tests will follow shortly)_.
