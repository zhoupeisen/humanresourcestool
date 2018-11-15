# human-resources-tool
a c# project for a hypothetical HR tool
*** README ONLY WRITTEN ONLY WITH/FOR WINDOWS OS USE ***
This is a simple human resources application I designed. The intentsion is emulate a command
line application for a hypothetical human resource employee who can update, edit, or 
remove information pertaining to an employee within a database.

The following items are recommended to be obtained and used for this console application:

Microsoft SQL Server 2017 Express Edition,
Microsoft Visual Studio Community 2017,
Microsoft SQL Server Managment Studio 2017.

Once all of these are obtained and installed, now to go grab https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorks-oltp-install-script.zip
unzip this to this location: c:\Samples. Then use SSMS to open a particular file called: 'AdventureWorks-oltp-install-script.sql'. 
This is a script that installs the adventureworks database. After opening this file(instawdb.sql), prior to executing it
you must enable SQLCMD Mode within the |query menu|. After this has been done, you can execute the file
to install the AdventureWorks database.

Next is the task of opening a new console project within visual studio community and replacing the default
code of the automatically generated Program.cs with the code I have provided. Upon doing all of
these tasks, you can then build and compile my code within visual studio to then test/run the
application. 

Thanks.

Joseph Taylor Perkinson
