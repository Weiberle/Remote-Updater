# Remote-Updater

![alt text](https://github.com/Weiberle/Remote-Updater/blob/master/ReadMeImages/RemoteUpdater_Overview.jpg?raw=true)

The Remote-Updater is a developer tool to update files on a remote Windows PC (real or virtual). In some cases, the PC where the developer is working on, isn't neccesarily the PC where the software is tested by the developer. To deploy the new build versions of your DLLs, you normaly use postbuild steps, you write copy-scripts or in the worse case you do it all by hand. Especially copying new files to another PC makes this boring, repeating steps a little bit more complicated and anoying. Wouldn't it be nice to have a tool that makes this steps easyer and more flexible. A tool that visulizes, if the copy actions were successful or where you can pause some files from beeing copied with one click. A tool where you can add your own code, that is executed before or after the copy action is done e.g. to stop and start a Windows Service. This is where the Remote-Updater comes in and helps you to save your time.

## How to build Remote-Updater
1. Download the repoistory
2. Open the solution in Visual Studio 2022
3. Restore referenced NuGet Packages
4. Build the Solution, make sure you can build .NET 6 applicaitons

## How to use Remote-Updater
Remote-Updater consists of two applications a sender and a receiver, one application for each PC. First at all start the sender at the source PC and the receiver on the target PC. Make sure that the source PC can reach the target PC via LAN. The receiver is listening for a connection established by the sender. An adjustment for the distinct port that's set in the settings might be necessary. 

### Remote Update Sender

### Remote Update Receiver

## Remote-Updater Sender Settings

## Remote-Updater Receiver Settings
