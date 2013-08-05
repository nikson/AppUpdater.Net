
Application Version Updater
----------------------------


Add `UpdateHelper` file and setup your default parameter ex: _server_, _username_, _password_ etc.  

Check is Update Available: 
```
var Satus = UpdateHelper.CheckUpdate();
```


Run the Updater as a seperate process: 
```
UpdateHelper.RunVersionUpdater()
```
