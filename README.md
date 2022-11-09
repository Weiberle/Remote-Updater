# Remote-Updater

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/RemoteUpdater_Overview.jpg?raw=true)

The Remote-Updater is a developer tool to update files on a remote Windows PC (real or virtual). 

In some cases, you as a developer want to test your new build files (e.g. DLLs) on a remote PC, not on the PC you are is wokring on. To deploy the new build versions of your files, you normaly use postbuild steps, you write copy-scripts or in the worse case you do it all by hand. At least the last case is quite anoying and happens more often than it should be ;)

Wouldn't it be nice to have a tool that makes this copy/update steps much easyer and more flexible. A tool ...
- where you can easily see if the copy actions were successful or not. 
- where you can pause some files from beeing copied/updated with one click. 
- where you can add your own code (.NET), that is executed before or after the copy action is done (e.g. to stop and start a Windows Service) 

This is where the Remote-Updater comes in and helps you to save your expensive time.

## How to build Remote-Updater
1. Download the repoistory
2. Open the solution in Visual Studio 2022
3. Restore referenced NuGet Packages
4. Build the Solution, make sure you can build .NET 6 applicaitons

## How to use Remote-Updater
Remote-Updater consists of two applications a **sender** and a **receiver**, one application for each PC. First at all, start the **sender** at the source PC and the **receiver** on the target PC. Make sure that the source PC can reach the target PC via Network. The **sender** reaches to the **receiver** via SignalR, IP-Address and port must be configured, see settings description for details.

### Step 1 - Select files
**Sender:** First at all choose the files you want to send to the receiver. This can be done via Drag & Drop (folders or files) or with a file dialog (click on the upper right button with the folder+ icon). Added files will be listed. Next to the filepath there is a gray status indicator on the left, the filesize is shown on the right. 

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/sr_01.jpg?raw=true)

### Step 2 - Sending file-information to the receiver
**Sender:** Pressing the send button establishes a connection to the receiver. After the connection was established successfully, the file-information will be send to the receiver. A progressbar at the bottom visualizes this activity.

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages//sr_02.jpg?raw=true)

After the file-information was transfered, the files are listed on the receiver also. The status indicator changes to orange, this means the file-information was transfered but the files were not copied/updated because no targetfolder was selected yet.

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages//sr_03.jpg?raw=true)

### Step 3 - Select targetfolder(s)
**Receiver:** For each file a targetfolder must be configured. Choosing the target folder can be done individually for each file via the folder button next to the file name on the right. A folder selection Dialog will open for choosig the targetfolder. I you want to select the same targetfolder for every file or for selected files, choose the folderbuttons on top of the list. All this can be done via context menue also. The choosed targetfolder is displayed right next to the filename. Files with a valid targetfolder will get a greay status indicator again.

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages//sr_04.jpg?raw=true)

### Step 4 - Coping files to the targetfolder(s)
**Sender:** Pressing the send button again will now trigger the coping/updateing process. This time the files are copied to the configured targetfolders. Existing files will be replaced. There is also an auto-update mechanism which is triggered when sourcefile changes are detected (see settings for details). 

If you want to pause a file from beeing copy/update process deselect it by unchecking the checkbox. If you want to remove files from the copy/update list use the thrashbinbuttons.

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages//sr_05.jpg?raw=true)

Successfully copied/updated files will get a green status indicator.

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages//sr_06.jpg?raw=true)

Files that could not be copied/updated will get a red status indicator. The targetfile(s) might be write proteced or in use by an application.

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages//sr_07.jpg?raw=true)

## Remote-Updater Sender Settings
![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Sender/Sender_Settings_01.png?raw=true)
- Remote-Updater Receiver IP: IP Address of the receiver PC. See receiver settings to get the right IP Address. 
- Remote-Updater Receiver Port: Port of the receiver application.
- Bring to front on error: If it's activated the sender application pops-up when an update error occours. Use it if you want to be notified automatically.
- Auto update: If it's activated the sender will track sourcefile changes and triggers the update mechanism.
- Auto update delay: It's a delay after a dectected sourcefile change. This avoids triggering the update mechanism for each file that is changed when several changes are made in a row e.g. after triggering a build.

## Remote-Updater Receiver Settings 
![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Receiver/Receiver_Settings_01.png?raw=true)
- Ip Address: The IP Address is automatically detected and can't be changed. Use this IP Address in the sender configuration.
- Port: This is the port where the sender tries to reach the receiver. Use this port in the sender configuration. Make sure the port is open and not blocked by a friewall.
- Bring to front on error: If it's activated the receiver application pops-up when an update error occours. Use it if you want to be notified automatically.
- Connection Timeout: Choose a propper file-transfer-timeout. Especially transfering big files might take too long and the connection will break.

## Remote-Update Receiver Copy Actions
The receiver gives you the possibility to execute custom actions before or after a copy/update process is/was triggered. This could be useful e.g. if you want to stop the application that is using your files because they cannot replaced while in use. You can write these actions in C# .NET just by implementing a very simple precopy-action or postcopy-action interface. In the simplest case you just have to return an `ActionName` and implement the `Execute` method.

```csharp
namespace RemoteUpdater.Contracts.Interfaces
{
    public interface ICopyAction
    {
        string ActionName { get; }

        bool HasSettings { get; }

        string Description { get; }

        CopyActionSettings LoadSettings();

        void SaveSettings(CopyActionSettings settings);

        void Init(string settingsFolderPath);

        bool Execute();
    }
}
```
After building your actions, place the DLLs and all depending files in a custom folder below the `Plugins` folder of the receiver application. The Receiver will find your actions via reflection. 

Within the Actions Tab you can see all copy-actions. The order can be changed via Drag & Drop. You can test the actions by pressing the playbutton. If the action was executed successfully the button turns green, if the action fails the button turns red. Settings can be changed after pressing the settingsbutton. Unchecking the checkboxes you an exclude the actions from beeing executed. 

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Receiver/Receiver_Actions_01.png?raw=true)

#### Action-Setting example:

This is the settings view of the Demo.Plugin precopy-action. With the setting you can decide if the precopy-actions fails or succeeds when beeing executed. 

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Receiver/Receiver_Actions_02.png?raw=true)

#### Sample Code:

There is a **Demo.Plugin** and a **StartStopWindowsService.Plugin** project within the solution. 

```csharp
namespace Demo.Plugin
{
    public class PreCopyAction : IPreCopyAction
    {
        private CopyActionSettings _copyActionSettings = new CopyActionSettings { new CopyActionSetting("Thow Exception", "true") };

        public string ActionName => "Demo PreCopy-Action";

        public bool HasSettings => true;

        public string Description => "Example PreCopy-Action";

        public bool Execute()
        {
            if (_copyActionSettings.First().SettingValue.ToLower().Trim() == "true")
            {
                throw new Exception("Execution of action failed.");
            }
            return true;
        }

        public void Init(string settingsFolderPath)
        {
        }

        public CopyActionSettings LoadSettings()
        {
            return _copyActionSettings;
        }

        public void SaveSettings(CopyActionSettings settings)
        {
            _copyActionSettings = settings;
        }
    }
}
```

