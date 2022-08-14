# Remote-Updater

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/RemoteUpdater_Overview.jpg?raw=true)

The Remote-Updater is a developer tool to update files on a remote Windows PC (real or virtual). In some cases, the PC where the developer is working on, isn't neccesarily the PC where the software is tested by the developer. To deploy the new build versions of your DLLs, you normaly use postbuild steps, you write copy-scripts or in the worse case you do it all by hand. Especially copying new files to another PC makes this boring, repeating steps a little bit more complicated and anoying. Wouldn't it be nice to have a tool that makes this steps easyer and more flexible. A tool that visulizes, if the copy actions were successful or where you can pause some files from beeing copied with one click. A tool where you can add your own code, that is executed before or after the copy action is done e.g. to stop and start a Windows Service. This is where the Remote-Updater comes in and helps you to save your time.

## How to build Remote-Updater
1. Download the repoistory
2. Open the solution in Visual Studio 2022
3. Restore referenced NuGet Packages
4. Build the Solution, make sure you can build .NET 6 applicaitons

## How to use Remote-Updater
Remote-Updater consists of two applications a sender and a receiver, one application for each PC. First at all, start the sender at the source PC and the receiver on the target PC. Make sure that the source PC can reach the target PC via Network. The sender connects to the receiver via SignalR, IP-Address and port must be configured.

### Step 1
First at all choose the files you want to send to the receiver. This can be done via Drag & Drop (folders or files) or with a file dialog (click on the upper right button with the folder icon). New files are visualized by a gray status indicator. 

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Sender/Sender_Files_01.png?raw=true)

### Step 2
When you press the send button the file information is transmitted to the receiver. Go to the target PC where the receiver is running. Now the new files should be listed within the receiver (orange status indicator). 

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Receiver/Receiver_Files_01.png?raw=true)

### Step 3
Now you can select the targetfolder(s) where the files should be copied to. Choosing the target folder can be done via the folder buttons on the right or via context menue, a folder selection Dialog will open. You can choose the target folder just for a single file, selected files or for all files (status indicator will change to grey). 

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Receiver/Receiver_Files_02.png?raw=true)

### Step 4
Now go back to the sender and press the send button again. This time the receiver receives the files and copies them the the configured directories. Successfull updated files will get a green status indicator. 

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Receiver/Receiver_Files_03.png?raw=true)

### Errors
Files that could not be updated will get a red status indicator. The not updateable file might be in use or are write protected. The update process can be triggered by hand every time you want to update the files. There is also an auto-update option which is triggered when file changes are detected (see settings for details).

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Receiver/Receiver_Files_04.png?raw=true)

## Remote-Updater Sender Settings
![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Sender/Sender_Settings_01.png?raw=true)
- Remote-Updater Receiver IP: IP Address of the receiver application. 
- Remote-Updater Receiver Port: Port of the receiver application.
- Bring to front on error: If it's activated the sender application pop-up when an update error occours. Use it if you want to be notified.
- Auto update: If it's activated the sender will track filechanges and trigger the update mechanism.
- Auto update delay: It's a delay after a dectected file change. This avoids triggering the update mechanism for each file that is changed wenn several changeds are made in a row e.g. triggering a build.

## Remote-Updater Receiver Settings 
![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Receiver/Receiver_Settings_01.png?raw=true)
- Ip Address: The IP Address is automatically detected and can't be changed. Use this IP Address in the sender configuration.
- Port: This is the port where the sender tries to connect to the receiver. Use this port in the sender configuration. Make sure the port is open and not blocked by a friewall.
- Bring to front on error: If it's activated the receiver application pop-up when an update error occours. Use it if you want to be notified.
- Connection Timeout: Choose a propper file-transfer-timeout. Especially transfering big files might take too long and the connection will break.

## Remote-Update Receiver Copy Actions
TODO

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Receiver/Receiver_Actions_01.png?raw=true)
![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/Receiver/Receiver_Actions_02.png?raw=true)

## Troubleshooting
TODO
