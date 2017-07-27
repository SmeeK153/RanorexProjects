@echo off
SeleniumRunner.exe WPDockerDemo.exe /ep:rxGridhub /confFile=BrowserMatrix.csv /pa:epUrl="http://rxGridhub:4444/wd/hub"  /tc:InternalTest %*