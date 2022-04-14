# Tournament Tracker  
Tournament Tracker is a desktop application that use WinForm UI.  
This application allow user to create and a manage an elimination-style tournament. This application offer the following:  
1. Set Entree Fee and Prizes. Total Prize must be lower than total collected fees.  
2. Choosing between high-score or low-score win.  
3. Tournament with any number of teams (the app will create byes to complete the tournament).  
4. The team order are randomize using C# System.Random which should provide good enough randomness for a tournament.  
5. Email notification to each player to notify of upcoming and when the tournament ends.
6. Choose between Sql Server or CSV File for Data Storage.

## Build Instruction
1. Clone the code and open with visual studio.  
2. On Solution Explorer, Right-Click on TrackerData Project and Publish the database to MSSQLLocalDB and use window authentication.  
3. On SQL Server Object Explorer, locate and double-click on the published database. After that, right-click on the database and select properties. Copy the connection string.  
4. On Solution Explorer, Open TrackerWinFormUI Project, Open App.config and replace the connection string.  
5. In App.config:  
  1. filePath is for defining the path the save the CSV data (if used). 
  2. highScoreWin is a flag to decide whether system should consider team with higher score as the winner or use lower score instead.   
  3. Sender Email and Sender Name is for email notification system.  
  4. MailSettings section define the email sending method. default setting is using localhost.  
6. In Program.cs, line 'GlobalConfig.InitiallizeConnection(DatabaseType.Sql);' define whether the app will use SQL server or CSV file as database
7. Build and enjoy

## Credits
The original project use .Net Framework 4.5 with WinForm UI up to this [commit](https://github.com/Amirruddin-Razak/TournamentTracker/tree/ea4a84fdc012ecccd09849de6e30b42eccd68792) is made by referring to [IAmTimCorey Project Series](https://www.youtube.com/watch?v=HalXZUHfKLA&list=PLLWMQd6PeGY3t63w-8MMIjIyYS7MsFcCi).
All subsequence improvement afterwards are my personal work.
