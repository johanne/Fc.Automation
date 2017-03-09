# Fc.Automation
For the exercise of FC.

Known Issues:
	There's a problem with the mailsending module, i'm not sure if it's the smtp.
	If the connection is extremely laggy, try increasing the wait in the configuration file (in ms)
	Pre-orders - Failure to accommodate an order results in the app to close.


Prerequisites:

	1. System running windows 7+;
	2. .NET 4.6.2
	3. Chrome Installation
	4. For the batch file/ scheduling, need to have a "C:\" directory

To run this program, you'll need to compile the program from
within Visual Studio 2015.

Steps:
 1. Set Fc.Auto.Exercise.csproj as the start up project.
 2. Build the Solution
 3. Run Fc.Auto.Exercise.exe

Scheduling:
  - You'll need to create a task to automate this run via Windows Task Scheduler.


Alternatively, you can download FoodPanda.zip.
	1. Extract the contents of FoodPanda.zip
	2. Double click on FoodPanda.bat
	3. Files should be copied and a task will be create in Task Scheduler, set to run daily at 11:45 am.


Configuration:
 - Change relevant values in Fc.Auto.Exercise.exe.config
 
The following comprises the driver configurations:

    add key="driver" value="Chrome" - There are currently no other options here
    add key="waitTime" value="3000" - This is the time the code will wait for the UI

The next lines are for configuring the target area - these should exist in the web:

    add key="city" value="Taguig City"
    add key="street" value="7th Avenue, Taguig City, NCR, Philippines"
    add key="filter" value="Deals"
    add key="sortOption" value="FastestDelivery"

The next lines are for the email configuration

    add key="senderName" value="automation"
    add key="senderAddress" value="fc_jd_auto@yopmail.com"
    add key="senderPassword" value=""
    add key="receiverName" value="automation"
    add key="receiverAddress" value="fc_jd_auto@yopmail.com"
    add key="host" value="smtp.yopmail.com"
    add key="port" value="25"

Lastly, the following line is the configuration for the csv file:

    add key ="outputLocation" value=""

If there is no setting in the output location, it will be placed in the users temp directory -> %Temp%;
