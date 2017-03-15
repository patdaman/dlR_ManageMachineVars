USE [DevOps]
GO
SET IDENTITY_INSERT [shell].[Scripts] ON 

INSERT [shell].[Scripts] ([id], [script_name], [script_text], [create_date], [modify_date], [is_active]) VALUES (100000, N'CopyConfigFiles.ps1', N'### SCRIPT VARIABLES ###
$VerbosePreference = "continue"

#$TF_BUILD_SOURCESDIRECTORY = "c:\tfs"
#$TF_BUILD_SOURCESDIRECTORY = "E:\Builds\8\11\src"

#$tfBuildSrcDir = $TF_BUILD_SOURCESDIRECTORY

$tfBuildSrcDir = $Env:TF_BUILD_SOURCESDIRECTORY
$nameArray = $tfBuildSrcDir.Split("\")

$srcPath =$nameArray[0] + "\PrintableConfig"

#E:\Builds\5\11\src\printableconfig

$destPath = $tfBuildSrcDir
$destPath2 = $tfBuildSrcDir + "\Main"
$destPath3 = $nameArray[0] + "\" + $nameArray[1] +  "\PrintableConfig"

#$srcPath =$nameArray[0]+ "\" + $nameArray[1] + "\PrintableConfig"

###Debug only###########
#$destPath = "D:\Buildtest" + "\Main"


Write-Verbose $nameArray[0]
Write-Verbose $nameArray[1]
Write-Verbose $srcPath
Write-Verbose $destPath
Write-Verbose $destPath2
Write-Verbose $destPath3

$d1 = $destPath + "\PrintableConfig"
$d2 = $destPath2 + "\PrintableConfig"
$d3 = $destPath3 + "\PrintableConfig"

Write-Verbose $d1
Write-Verbose $d2
Write-Verbose $d3

Get-ChildItem -Path $tfBuildSrcDir  -Include *Test*.dll -Recurse -Exclude *.pdb -ErrorAction SilentlyContinue | ?{ $_.fullname -match "\\obj\\?" } | foreach ($_) {remove-item $_.fullname -Force -WhatIf }

Get-ChildItem -Path $tfBuildSrcDir  -Include *Test*.dll -Recurse -Exclude *.pdb -ErrorAction SilentlyContinue | ?{ $_.fullname -match "\\obj\\?" } | foreach ($_) {remove-item $_.fullname -Force }

if(-not (Test-path $d1))
{
    Copy-item -Path $srcPath -Destination $destPath -Recurse -WhatIf -ErrorAction SilentlyContinue
    Copy-item -Path $srcPath -Destination $destPath -Recurse -ErrorAction SilentlyContinue
}
else
{
    Get-ChildItem -Path $destPath  -Include *PrintableConfig* -Recurse -Exclude *.config -ErrorAction SilentlyContinue| ? { $_.fullname -notmatch "\\PrintableInstaller\\?" } | foreach ($_) {remove-item $_.fullname -Recurse -WhatIf }
    #Get-ChildItem -Path $destPath  -Include *PrintableConfig* -Recurse -Exclude *.config  | ? { $_.fullname -notmatch "\\PrintableInstaller\\?" } | foreach ($_) {remove-item $_.fullname -Recurse }

    Copy-item -Path $srcPath -Destination $destPath -Recurse -WhatIf -ErrorAction SilentlyContinue
    #Copy-item -Path $srcPath -Destination $destPath -Recurse
}


if(-not (Test-path $d2))
{
  Copy-item -Path $srcPath -Destination $destPath2 -Recurse -WhatIf -ErrorAction SilentlyContinue
  Copy-item -Path $srcPath -Destination $destPath2 -Recurse -ErrorAction SilentlyContinue
}

else
{
    Get-ChildItem -Path $destPath2  -Include *PrintableConfig* -Recurse -Exclude *.config -ErrorAction SilentlyContinue | ? { $_.fullname -notmatch "\\PrintableInstaller\\?" } | foreach ($_) {remove-item $_.fullname -Recurse -WhatIf }
    #Get-ChildItem -Path $destPath2  -Include *PrintableConfig* -Recurse -Exclude *.config  | ? { $_.fullname -notmatch "\\PrintableInstaller\\?" } | foreach ($_) {remove-item $_.fullname -Recurse }


    Copy-item -Path $srcPath -Destination $destPath2 -Recurse -WhatIf -ErrorAction SilentlyContinue
    #Copy-item -Path $srcPath -Destination $destPath2 -Recurse -WhatIf
}


#if(-not (Test-path $d3))
#{
#  Copy-item -Path $srcPath -Destination $destPath3 -Recurse
#}



############################### Copy dlls for DI #######################
$lib = $destPath2 + "\lib"

#############################ManagerI18n#############################################
$managerI18nTestPathDbg = $destPath2 + "\ManagerI18n\Model.Tests\bin\Debug"
$managerI18nTestPathRel = $destPath2 + "\ManagerI18n\Model.Tests\bin\Release"

if((Test-path $managerI18nTestPathDbg))
{
  Get-ChildItem -Path $lib -Filter "EntityFramework.SqlServer.dll" |  Copy-item -Destination $managerI18nTestPathDbg -WhatIf
  Get-ChildItem -Path $lib -Filter "Pti.EntityFramework.dll" |  Copy-item -Destination $managerI18nTestPathDbg -WhatIf

  ### Do the copy ######################
  Get-ChildItem -Path $lib -Filter "EntityFramework.SqlServer.dll" |  Copy-item -Destination $managerI18nTestPathDbg -Force
  Get-ChildItem -Path $lib -Filter "Pti.EntityFramework.dll" |  Copy-item -Destination $managerI18nTestPathDbg -Force

}

if((Test-path $managerI18nTestPathRel))
{
  Get-ChildItem -Path $lib -Filter "EntityFramework.SqlServer.dll" |  Copy-item -Destination $managerI18nTestPathRel -WhatIf
  Get-ChildItem -Path $lib -Filter "Pti.EntityFramework.dll" |  Copy-item -Destination $managerI18nTestPathRel -WhatIf

  ### Do the copy ######################
  Get-ChildItem -Path $lib -Filter "EntityFramework.SqlServer.dll" |  Copy-item -Destination $managerI18nTestPathRel -Force
  Get-ChildItem -Path $lib -Filter "Pti.EntityFramework.dll" |  Copy-item -Destination $managerI18nTestPathRel -Force

}

##################################Pti.Api.WebApi###########################################
$PtiApiWebApiDbg = $destPath2 + "\Pti\Pti.Api\Pti.Api.WebApi.Tests\bin\Debug"
$PtiApiWebApiRel = $destPath2 + "\Pti\Pti.Api\Pti.Api.WebApi.Tests\bin\Release"

if((Test-path $PtiApiWebApiDbg))
{
  Get-ChildItem -Path $lib -Filter "Printable.UIPublishing.dll" |  Copy-item -Destination $PtiApiWebApiDbg -WhatIf
  Get-ChildItem -Path $lib -Filter "Printable.Context.dll" |  Copy-item -Destination $PtiApiWebApiDbg -WhatIf

  ### Do the copy ######################
  Get-ChildItem -Path $lib -Filter "Printable.UIPublishing.dll" |  Copy-item -Destination $PtiApiWebApiDbg -Force
  Get-ChildItem -Path $lib -Filter "Printable.Context.dll" |  Copy-item -Destination $PtiApiWebApiDbg -Force

}

if((Test-path $PtiApiWebApiRel))
{
  Get-ChildItem -Path $lib -Filter "Printable.UIPublishing.dll" |  Copy-item -Destination $PtiApiWebApiRel -WhatIf
  Get-ChildItem -Path $lib -Filter "Printable.Context.dll" |  Copy-item -Destination $PtiApiWebApiRel -WhatIf

  ### Do the copy ######################
  Get-ChildItem -Path $lib -Filter "Printable.UIPublishing.dll" |  Copy-item -Destination $PtiApiWebApiRel -Force
  Get-ChildItem -Path $lib -Filter "Printable.Context.dll" |  Copy-item -Destination $PtiApiWebApiRel -Force

}', CAST(N'2017-03-14T15:38:21.923' AS DateTime), NULL, 1)
INSERT [shell].[Scripts] ([id], [script_name], [script_text], [create_date], [modify_date], [is_active]) VALUES (100001, N'DelNodeModsDir.ps1', N'
		   Param(
    [Parameter(Mandatory=$True)]
    [string] $delNodeMods
)

# $delNodeMods = "true"

$logfolder = "E:\Logs\DelMods"

$logPath = "$logfolder\Log.txt"

function write-log($msg)
{
    $now = Get-Date -format "MM-dd-yyyy HH:mm "

    if ( -Not (Test-Path "$logfolder"))
    {
     New-Item -Path "$scriptPath$logfolder" -ItemType Directory | out-null
     $now + $msg | Out-File -FilePath $logPath
    }
    else
    {
     $now + $msg | Out-File -FilePath $logPath -append
    }
 
}


$ErrorActionPreference = "SilentlyContinue";

$tfBuildSrcDir = $Env:TF_BUILD_SOURCESDIRECTORY

# $tfBuildSrcDir = "E:\Builds\11\18\src"

$ModulesDir = $tfBuildSrcDir + "\Main\PortalApps\src\PortalApps\node_modules" 


if($delNodeMods -eq "true")
{
    write-log "clean the mods folders needed = $delNodeMods"

    if((Test-Path $ModulesDir))
    {
        write-log "$ModulesDir found and mark for deletion."

        # $cmd = "rd /s /q $ModulesDir" 

		$cmd = "E:\FastCopy\fastcopy /cmd=delete /wipe_del /auto_close /speed=full /no_confirm_del $ModulesDir" 

        cmd /c $cmd | Out-Null
              
        write-log "$ModulesDir deleted."
    }
    else
    {
    write-log "$ModulesDir Directory not found."
    }
}
else
{
    write-log "clean the mods folders not needed = $clean"
}', CAST(N'2017-03-14T15:39:38.347' AS DateTime), NULL, 1)
INSERT [shell].[Scripts] ([id], [script_name], [script_text], [create_date], [modify_date], [is_active]) VALUES (100002, N'PTIpostbuild.ps1', N'[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True)]
    [string]$releaseEnv,
    [string]$branch,
    [string]$buildnum,
    [string]$sqlUpdate,
	[string]$deploy
)

#### Function to output log entries to saved file ###
function write-log($msg)
{
$day=Get-Date -Format "MM-dd-yyyy"
$time=(Get-Date).ToString()
$logPath= "$logFolder\$day BuildLog.txt"
if ( -Not (Test-Path "$logFolder"))
    {
     New-Item -Path "$logFolder" -ItemType Directory | out-null
     $time + " - " + $msg | Out-File -FilePath $logPath
    }
else
    {
     $time + " - " + $msg | Out-File -FilePath $logPath -append
    }
}

### SCRIPT VARIABLES ###
$TF_BUILD_SOURCESDIRECTORY = $Env:TF_BUILD_SOURCESDIRECTORY
#$TF_BUILD_BINARIESDIRECTORY = $ENV:TF_BUILD_BINARIESDIRECTORY
## Set the SMTP Server address
$SMTPSRV = "mta.corp.printable.com"
## Set the Email address to recieve from
$EmailFrom = "notification@app.pti.com"

## Set the Email address to send the email to
$EmailTo = "devops@pti.com"
$EmailToEngrs = "n-build@pti.com"

##Set Message Body
$Body = ""

$msbuildpath = "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe"
$scriptFolder = "E:\SqlScripts"

## Change src folder location based on Branch type.  Main has an additional folder level.
#if($branch -eq "Main")
#{$relsrc =  Join-Path $TF_BUILD_SOURCESDIRECTORY -ChildPath "Main"}
#else
#{$relsrc = $TF_BUILD_SOURCESDIRECTORY}

$relsrc =  Join-Path $TF_BUILD_SOURCESDIRECTORY -ChildPath "Main"
$symbolsDest = "E:\ptiSymbols"

$logFolder = "E:\Logs\BuildLogs"
$sqlProjDir = Join-Path $relsrc -ChildPath "Database"

#copy symbols for debug
robocopy $relsrc $symbolsDest /purge /s *.pdb *.xml

if($sqlUpdate -eq "true") 
{

###Generate SQL scripts
$ptbuild = "`"$msbuildpath`" /t:build pt.sqlproj /flp:logfile=ptbuild.log"

Try {
	$ErrorActionPreference = "Stop"
	Set-Location -Path $sqlProjDir\pt -PassThru
	Invoke-Expression "& $ptbuild"

	if($LASTEXITCODE -ne "0") {
		$message = "SSDT project build failed"
		write-log $message
		send-mailmessage -To $EmailTo -From $EmailFrom -Subject "SSDT FAILURE" -SmtpServer $SMTPSRV -Body $message
		Exit
	} else {
		write-log "SSDT solution was successfully built"
	}
} 
Catch {
 $ErrorMessage = $_.Exception.Message
 write-log $ErrorMessage
}

$message = @" 
Hello Team,

We are starting publishing SQL changes of $buildnum to $releaseEnv.

No services are stopped at this point, but the SQL changes may cause errors when the sites are accessed.

Testing on QA is not recommended during this time. We will send an email when deployment gets completed.

Thanks,
DevOps Team
"@ 


	# Send Pre Publish Email
	send-mailmessage -To $EmailToEngrs -From $EmailFrom -Subject "SQL Publish starting on QA for $buildnum" -SmtpServer $SMTPSRV -Body $message
		
	write-log "SQL Pre-Publish email sent"
		
	#Publish SQL scripts
	$databases = Get-ChildItem $sqlProjDir | Where { $_.PSIsContainer -and $_.Name -ne "ImageServer_INTL" }

	Foreach($database in $databases) {
		$sqlPublishOptions = "/p:SqlPublishProfilePath=E:\PrintableConfig\profile.QA.$database.publish.xml /p:TargetDatabaseName=$database /p:PublishScriptFileName=$database`_publish.sql /flp:logfile=$database`Publish.log"
		$sqlPublish="`"$msbuildpath`" /t:Publish $sqlPublishOptions"
		
		Try {
			$ErrorActionPreference = "Stop"

			Set-Location -Path $sqlProjDir\$database -PassThru
			Invoke-Expression "& $sqlPublish"
			
			if($LASTEXITCODE -ne "0") {
				$message = "SSDT project published failed on $database"
				write-log $message
				send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Failed to publish $database database" -SmtpServer $SMTPSRV -Body $message
				Exit
			} else {
				write-log "Successfuly published to $database database"
			}
		
			If(Test-Path -path "$sqlProjDir\$database\bin\Debug\$database`_publish.sql") {
				Copy-Item $sqlProjDir\$database\bin\Debug\$database`_publish.sql $scriptFolder\$database`_publish.sql -Force
				write-log "Sucessfully copied $database`_publish.sql to $scriptFolder"
			}
		} Catch {
			$ErrorMessage = $_.Exception.Message
			write-log $ErrorMessage
		}
	}
} else {
		write-log "SQL Publish was set to $sqlUpdate and hence no sql projects built or published."
}

### Export Changesets for Build ###
Try {
	$notesdir = Split-Path $TF_BUILD_SOURCESDIRECTORY | Join-Path -ChildPath "bin"
	& $TF_BUILD_SOURCESDIRECTORY\BuildScripts\TfbNotes.exe /textfile:$notesdir/notes.txt
	write-log "Successfully exported changeset notes from TFS"
}
catch {
	$ErrorMessage = $_.Exception.Message
	write-log $ErrorMessage
}

# Send email after transfer completes
$message = @" 
Hello Team,

The binaries for the latest build of $buildnum have been created.

The binaries will now be copied to the $releaseEnv distribution server.

Thanks,
DevOps Team


"@ 
send-mailmessage -To $EmailTo -From $EmailFrom -Subject "New build is about to be copied" -SmtpServer $SMTPSRV -Body $message

### Transfer files to QA distrn server ###
Try {
 & $TF_BUILD_SOURCESDIRECTORY\BuildScripts\TFSrelease.ps1 -releaseEnv $releaseEnv -branch $branch -buildnum $buildnum -srcFolder $relsrc -deploy $deploy
 write-log "Release has successfully completed on $releaseEnv for build $buildnum"
}
catch {
 $ErrorMessage = $_.Exception.Message
 write-log $ErrorMessage
}
', CAST(N'2017-03-14T15:42:06.273' AS DateTime), NULL, 1)
INSERT [shell].[Scripts] ([id], [script_name], [script_text], [create_date], [modify_date], [is_active]) VALUES (100003, N'PTIpostbuildv2.ps1', N'
		   [CmdletBinding()]
Param(
    [Parameter(Mandatory=$True)]
    [string]$releaseEnv,
    [string]$branch,
    [string]$buildnum,
    [string]$sqlUpdate,
	[string]$deploy,
    [string]$delNodeMods
)

#### Function to output log entries to saved file ###
function write-log($msg) {
    $day=Get-Date -Format "MM-dd-yyyy"
    $time=(Get-Date).ToString()
    $logPath= "$logFolder\$day BuildLog.txt"

    if (-Not (Test-Path "$logFolder")) {
        New-Item -Path "$logFolder" -ItemType Directory | out-null
        $time + " - " + $msg | Out-File -FilePath $logPath
    }
    else {
     $time + " - " + $msg | Out-File -FilePath $logPath -append
    }
}

### SCRIPT VARIABLES ###
$TF_BUILD_SOURCESDIRECTORY = $Env:TF_BUILD_SOURCESDIRECTORY
#$TF_BUILD_BINARIESDIRECTORY = $ENV:TF_BUILD_BINARIESDIRECTORY

## Set the SMTP Server address
$SMTPSRV = "mta.corp.printable.com"

## Set the Email address to recieve from
$EmailFrom = "notification@pti.com"

## Set the Email address to send the email to
$EmailTo = "devops@pti.com"

##Set Message Body
$Body = ""

## MSBuild path, currently version 12 
$msbuildpath = "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe"

## sql scripts folder location
$scriptFolder = "E:\SqlScripts"

## Change src folder location based on Branch type.  Main has an additional folder level.
#if($branch -eq "Main")
#{$relsrc =  Join-Path $TF_BUILD_SOURCESDIRECTORY -ChildPath "Main"}
#else
#{$relsrc = $TF_BUILD_SOURCESDIRECTORY}

$relsrc =  Join-Path $TF_BUILD_SOURCESDIRECTORY -ChildPath "Main"

$symbolsDest = "E:\ptiSymbols"

$logFolder = "D:\Logs\BuildLogs"

$sqlProjDir = Join-Path $relsrc -ChildPath "Database"

#copy symbols for debug
robocopy $relsrc $symbolsDest /purge /s *.pdb *.xml

###Generate SQL scripts
$ptbuild = "`"$msbuildpath`" /t:build pt.sqlproj /flp:logfile=ptbuild.log"

Try {
    $ErrorActionPreference = "Stop"
    
    Set-Location -Path $sqlProjDir\pt -PassThru
    Invoke-Expression "& $ptbuild"

    if($LASTEXITCODE -ne "0"){
        $message = "SSDT project build failed"
        write-log $message
        send-mailmessage -To $EmailTo -From $EmailFrom -Subject "SSDT FAILURE" -SmtpServer $SMTPSRV -Body $message
        Exit
    }
    else {
    write-log "SSDT solution was successfully built"
    }
}
Catch {
    $ErrorMessage = $_.Exception.Message
    write-log $ErrorMessage
}


#Publish SQL scripts
$databases = Get-ChildItem $sqlProjDir | Where { $_.PSIsContainer -and $_.Name -ne "ImageServer_INTL" }

Foreach($database in $databases) {
    $sqlPublishOptions = "/p:SqlPublishProfilePath=E:\PrintableConfig\profile.QA.$database.publish.xml /p:TargetDatabaseName=$database /p:PublishScriptFileName=$database`_publish.sql /flp:logfile=$database`Publish.log"
 
    if($sqlUpdate -eq "true") {
    $sqlPublish="`"$msbuildpath`" /t:Publish $sqlPublishOptions"
    }
    else {
    $donotupdatedb = "/p:UpdateDatabase=False"
    $sqlPublish="`"$msbuildpath`" /t:Publish $donotupdatedb $sqlPublishOptions"
    }
    
    Try {
        $ErrorActionPreference = "Stop"

        Set-Location -Path $sqlProjDir\$database -PassThru
        
        Invoke-Expression "& $sqlPublish"
		
        if($LASTEXITCODE -ne "0") {
		 $message = "SSDT project published failed on $database"
		 write-log $message
		 send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Failed to publish $database database" -SmtpServer $SMTPSRV -Body $message
		 Exit
		}
		else {
            write-log "Successfuly published to $database database"
        }
		
		If(Test-Path -path "$scriptFolder\$database`_publish.sql") {
            Copy-Item $sqlProjDir\$database\bin\Debug\$database`_publish.sql $scriptFolder\$database`_publish.sql -Force
    		write-log "Sucessfully copied $database`_publish.sql to $scriptFolder"
		}
	}
    Catch {
        $ErrorMessage = $_.Exception.Message
		write-log $ErrorMessage
    }
}

### Export Changesets for Build ###
Try
{
	$notesdir = Split-Path $TF_BUILD_SOURCESDIRECTORY | Join-Path -ChildPath "bin"
	& $TF_BUILD_SOURCESDIRECTORY\BuildScripts\TfbNotes.exe /textfile:$notesdir/notes.txt
	write-log "Successfully exported changeset notes from TFS"
}
catch
{
	$ErrorMessage = $_.Exception.Message
	write-log $ErrorMessage
}

# Send email after transfer completes
$message = @" 
Hello Team,

The binaries for the latest build of $buildnum have been created.

The binaries will now be copied to the $releaseEnv distribution server.

Thanks,
DevOps Team


"@

## sending out the pre build start email  
send-mailmessage -To $EmailTo -From $EmailFrom -Subject "New build is about to be copied" -SmtpServer $SMTPSRV -Body $message

## if node modules files need to be deleted
if($delNodeMods -eq "true")
{
   $ModulesDir = $relsrc + "\PortalApps\src\PortalApps\node_modules" 

    write-log "cleaning the nodes modules folders needed = $delNodeMods"

    if((Test-Path $ModulesDir))
    {
        write-log "$ModulesDir found and mark for deletion."

        $_cmd = "rd /s /q $ModulesDir" 

        cmd.exe /c $_cmd | Out-Null
              
        write-log "$ModulesDir deleted."
    }
    else
    {
    write-log "$ModulesDir Directory not found."
    }
}
else
{
    write-log "clean the mods folders not needed = $delNodeMods"
}

## sending out the pre build start email  
send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Nodes Modules folders deleted" -SmtpServer $SMTPSRV -Body "Nodes Modules folders found at $ModulesDir and deleted."


### Transfer files to QA distrn server ###
Try {
 & $TF_BUILD_SOURCESDIRECTORY\BuildScripts\TFSrelease.ps1 -releaseEnv $releaseEnv -branch $branch -buildnum $buildnum -srcFolder $relsrc -deploy $deploy
 write-log "Release has successfully completed on $releaseEnv for build $buildnum"
}
catch {
 $ErrorMessage = $_.Exception.Message
 write-log $ErrorMessage
}
', CAST(N'2017-03-14T15:42:38.833' AS DateTime), NULL, 1)
INSERT [shell].[Scripts] ([id], [script_name], [script_text], [create_date], [modify_date], [is_active]) VALUES (100004, N'QAdeploy.ps1', N'
		   #### Function to output log entries to saved file ###
function write-log($msg)
{
$day=Get-Date -Format "MM-dd-yyyy"
$time=(Get-Date).ToString()
$logPath= "$scriptPath$logfolder\$day Log.txt"
if ( -Not (Test-Path "$scriptPath$logfolder"))
    {
     New-Item -Path "$scriptPath$logfolder" -ItemType Directory | out-null
     $time + " - " + $msg | Out-File -FilePath $logPath
    }
else
    {
     $time + " - " + $msg | Out-File -FilePath $logPath -append
    }
}

#### Function to monitor processes running on server ###
function process-monitor ($process)
{

$ProcessActive = Get-Process $process -ErrorAction SilentlyContinue
while($ProcessActive -ne $null)
{
 start-sleep -s 15
 $ProcessActive = Get-Process $process -ErrorAction SilentlyContinue
}

}

### Creates a .bat file from an array of Robocopy commands then executes the .bat file.  Once .bat is created, the array is recycled for the next use ###
function makeRunBat ($batFile, $roboArray)
{
 Try
    {
    $ErrorActionPreference = "Stop"
    Out-File -FilePath $batFile -InputObject $roboArray -encoding ASCII -Force
    write-log "$batFile batch file has been created"
    $global:roboArray = @() #Global is used to change variable in script.
    cmd.exe /c $batFile
    write-log "$batFile file has been executed"
    }
 Catch
    {
    write-log $_.Exception.Message
    }
}

### SCRIPT VARIABLES ###

# Set the SMTP Server address
$SMTPSRV = "mta.dc.pti.com"
# Set the Email address to recieve from
$EmailFrom = "notification@app.pti.com"
# Set the Email address to send the email to
$EmailTo = "n-build@pti.com"

$scriptPath = "D:\_ptiScripts\"
$buildenv="QA"
$logfolder="Release_Logs"

##Import CSV list of Services in QA and filter for each array
$qaServers = Import-Csv -Path "$scriptPath\qaServers.csv"
$services = $qaServers | where {$_.service -ne "none"}
$webservers = $qaServers | where {$_.webserver -eq "Y"} | select computer -Unique
$mappings = $qaServers.computer | select -Unique
$cachePaths = $qaServers | where {$_.cache -ne "none"}

### ROBOCOPY VARIABLES ###

$files="/XF 500-100new.asp global.asa p1_ServerConfigConstants.asp reportsSetLogInfo.asp inc_ISMessageBuilder.asp pm_ServerConfigConstants.asp pm_const.asp pf-Constants-Inc.asp NotFound.jpg NotFound.htm GenericError.htm *.csproj *.cs *.vspscc *.build *.vsscc *.vssscc *.vbproj *.config *.pdb *.sln"
$dir="/XD obj debugcache cache SharedComponents webhelp aspnet_client properties temp UnitTests Notification BuildScripts"
$params="/r:3 /W:1 /s /NP /NJH"
$webbatFile = "$scriptPath\QArobocopyWeb.bat"
$appbatFile = "$scriptPath\QArobocopyApp.bat"
$libbatFile = "$scriptPath\QArobocopyLib.bat"
$roboArray = @()

### QA user and password ###
$User = "las\root"
$pass = "sQtkepc3m"
$securePass = ConvertTo-SecureString -String $pass -AsPlainText -Force
$Cred=New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $User, $securePass


###############################################
#                                             #
#            This is where it starts          #
#            doing stuff                      #
#                                             #
#                                             #
###############################################



### This will parse the txt file and extract the build number to be used in the email
Get-ChildItem $scriptPath -Name *.txt |`
ForEach {
  $nameArray = $_.Split(".")
  $buildnum = $nameArray[0]+ "." +$nameArray[1]+ "." +$nameArray[2]
  $buildrev = $nameArray[3]
}

$msgfile ="$scriptPath$buildnum.$buildrev.txt"
write-log "======================================================================"
write-log "Found changesets for $buildnum.$buildrev"


# Send Pre Deployment Email
$message = @" 
Hello Team,

We are starting deployment of build $buildrev of $buildnum to $buildenv.

There will be brief outages on the QA sites. Please pause testing on QA as it is not recommended during this time. We will send an email when deployment gets completed.

Please see the attached file for the changesets & worksets in this build.

Thanks,
DevOps Team


"@ 
send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Deployment starting on $buildenv - build $buildrev of $buildnum" -SmtpServer $SMTPSRV -Body $message -Attachments $msgfile
write-log "Pre-deployment email sent"

## Pause script to allow users to exit servers
Start-Sleep -s 90


## Test connectivity to each QAserver and map UNC path if server is unavailable
Foreach($mapping in $mappings)
{
$path = "\\" + $mapping + "\d$"
if((Test-Path -path $path))
    {
     write-log "$path is reachable"
    }

 Else
 {
    try
    {
    $ErrorActionPreference = "Stop"
    $command = "net use $path $Pass /user:$User"
    cmd /c $command
    write-log "$path has been mapped"
    }
    Catch
    {
    write-log $_.Exception.Message
    }
  }
}

## Stop Windows services
foreach($service in $services)
{
try{
    $ErrorActionPreference = "Stop"
    Invoke-Command -ComputerName $service.computer -Credential $Cred -ScriptBlock {param($serviceName) get-service $serviceName | Stop-Service} -ArgumentList $service.service
    $msg = $service.service + " has been stopped on " + $service.computer
    write-log $msg
    }
catch
    {
    write-log $_.Exception.Message
    }
}

## Pause script to allow services to finish stopping
Start-Sleep -s 30

## IIS STOP for all web servers
foreach ($webserver in $webservers)
{
   Try
    {
   $ErrorActionPreference = "Stop"
   Invoke-Command -ComputerName $webserver.computer -Credential $Cred -ScriptBlock {iisreset /noforce /stop}
   $msg = "IIS has been successfully stopped on " + $webserver.computer
   write-log $msg
    }
    Catch
    {
    write-log $_.Exception.Message
    }
}

### Webserver Cache Removal ###
foreach($cache in $cachePaths)
{
  $path = "\\" + $cache.computer + "\" + $cache.cache
  if((Test-Path -path $path))
  {
  Try
    {
    $ErrorActionPreference = "Stop"
    get-childitem -Path $path | Where { $_.PSIsContainer } | remove-item -Recurse
    write-log "$path has been purged"
    }
  Catch
    {
    write-log $_.Exception.Message
    }
  }
  else
  {
  write-log "$path does not exist"
  }  
}
               

### Determine source location based on Branch type ###
if($nameArray[2] -eq "0")
    {
       $src = "D:\DistrnQA\Main"
       $destFolder = "QA"
    
    }
else
    {
       $src = "D:\DistrnQA\Patch"
       $destFolder = "QA"
    }    


##LVQMGR Servers
$mgrServers = $qaServers.computer | select -Unique
Foreach($mgrServer in $mgrServers -match "mgr")
{
$roboArray += "start robocopy $src\Printable\Manager \\$mgrServer\d$\$destFolder\Printable\Manager $params /PURGE $files  $dir"
$roboArray += "start robocopy $src\Printable\ManagerBridge \\$mgrServer\d$\$destFolder\Printable\ManagerBridge $params /PURGE $files  $dir"
$roboArray += "start robocopy $src\Printable\Services\Manager \\$mgrServer\d$\$destFolder\Printable\Services\Manager $params /PURGE $files  $dir"
$roboArray += "start robocopy $src\MgrServerCodeBase \\$mgrServer\d$\$destFolder\MgrServer\MgrServerCodeBase $params $files $dir"
$roboArray += "start robocopy $src\Printable\ManagerI18N \\$mgrServer\d$\$destFolder\Printable\ManagerI18N $params /purge $files $dir"
}

##LVQAPI01
$roboArray += "start robocopy $src\Pti\Api \\lvqapi01.las.printable.com\d$\$destFolder\Pti\webapi\ $params /PURGE $files $dir"
$roboArray += "start robocopy $src\Pti\jdapi \\lvqapi01.las.printable.com\d$\$destFolder\Pti\jdapi\ $params /PURGE $files $dir"

## LVQPA01
$roboArray += "start robocopy $src\Pti\PortalApps \\lvqpa01.las.printable.com\d$\pti\ $params /PURGE $files $dir"

## LVQFPW01
$roboArray += "start robocopy $src\Pti\FusionProWebPrototype \\lvqfpw01.las.printable.com\d$\pti\FusionProWebPrototype $params /PURGE $files $dir"

##LVQGLB02
$roboArray += "start robocopy $src\Printable\GlobalApps \\lvqglb02.las.printable.com\d$\US\Printable\GlobalApps $params /PURGE $files $dir"
$roboArray += "start robocopy $src\Printable\GlobalApps \\lvqglb02.las.printable.com\d$\USMCC\Printable\GlobalApps $params /PURGE $files $dir"

##LVQPFE03
$roboArray += "start robocopy $src\PFBuild\ImageServer \\lvqpfe03.las.printable.com\D$\US\ImageServer $params /PURGE $files $dir"
$roboArray += "start robocopy $src\PFBuild\TableDrivenWS \\lvqpfe03.las.printable.com\D$\US\TableDrivenWS $params /PURGE $files $dir"

## LVQSTR Servers
$strServers = $qaServers.computer | select -Unique
Foreach($strServer in $strServers -match "str")
{
$roboArray += "start robocopy $src\Printable\PrintOne \\$strServer\d$\USMCC\Printable\PrintOne $params /PURGE $files $dir"
# $roboArray += "start robocopy $src\urlrewrite \\$strServer\D$\urlrewrite $params /purge"
}

##LVQSVC03
# $roboArray += "start robocopy $src\Printable\WebServices\TRANS\0.9 \\lvqsvc03.las.printable.com\D$\$destFolder\printable\WebServices\TRANS\0.9 $params /PURGE $files $dir util Behaviors requesthandlers"
$roboArray += "start robocopy $src\Printable\WebServices\TRANS\1.0 \\lvqsvc03.las.printable.com\D$\$destFolder\printable\WebServices\TRANS\1.0 $params /PURGE $files $dir util Behaviors requesthandlers"
$roboArray += "start robocopy $src\Printable\InternalWebServices \\lvqsvc03.las.printable.com\D$\$destFolder\printable\InternalWebServices $params /PURGE $files $dir"
$roboArray += "start robocopy $src\Pti\oAuthOwinClient \\lvqsvc03.las.printable.com\D$\$destFolder\Pti\oAuthOwinClient $params /PURGE $files $dir"

##Create batch file from array of commands and execute
makeRunBat $webbatFile $roboArray


## checks if any robocopy processes are still running before starting new ones
process-monitor Robocopy
write-log "Web servers have been updated"

###Application Servers###
##LVQDMN01
$roboArray += "start robocopy $src\Printable\CatalogManagement \\LVQdmn01.las.printable.com\d$\us\Printable\CatalogManagement $params /PURGE $files $dir"
$roboArray += "start robocopy $src\Printable\Notification\Templates \\LVQdmn01.las.printable.com\d$\us\Printable\Notification\Templates $params /PURGE $files $dir"
$roboArray += "start robocopy $src\Printable\NotificationService \\LVQdmn01.las.printable.com\d$\us\Printable\NotificationService $params /purge $files $dir"

##LVQINT02
$roboArray += "start robocopy $src\Gateway\i10n \\lvqint02.las.printable.com\d$\$destFolder\i10n $params $files $dir"
$roboArray += "start robocopy $src\Gateway\i10n \\lvqint02.las.printable.com\d$\$destFolder\i10nTest $params $files $dir"
$roboArray += "start robocopy $src\Gateway\i10nClient \\lvqint02.las.printable.com\d$\$destFolder\i10nClient $params $files $dir"

$roboArray += "start robocopy $src\GatewayServices\TransactionDeliveryService \\lvqint02.las.printable.com\d$\$destFolder\GatewayServices\TransactionDeliveryService $params $files $dir"
$roboArray += "start robocopy $src\GatewayServices\BackEndTransactionRequestor \\lvqint02.las.printable.com\d$\$destFolder\GatewayServices\BackEndTransactionRequestor $params $files $dir"

##LVQINT03
$roboArray += "start robocopy $src\GatewayServices\TransactionDeliveryService \\lvqint03.las.printable.com\d$\$destFolder\GatewayServices\TransactionDeliveryService $params $files $dir"
$roboArray += "start robocopy $src\GatewayServices\BackEndTransactionRequestor \\lvqint03.las.printable.com\d$\$destFolder\GatewayServices\BackEndTransactionRequestor $params $files $dir"

##LVQPFD01
$roboArray += "start robocopy $src\PFBuild\Daemon \\lvqpfd01.las.printable.com\D$\US\Daemon $params /PURGE $files $dir" 
$roboArray += "start robocopy $src\Printable\Notification\Templates \\lvqpfd01.las.printable.com\d$\Daemon\Notification\Templates $params /PURGE" 

##Create batch file from array of commands and execute
makeRunBat $appbatFile $roboArray


## checks if any robocopy processes are still running before starting new ones
process-monitor Robocopy
write-log "App servers have been updated"

### DLLs ###
$roboArray += "start robocopy $src\lib \\lvqmgr03.las.printable.com\d$\$destFolder\Printable\Manager\bin CybsSecurity.dll $params"
$roboArray += "start robocopy $src\lib \\lvqmgr03.las.printable.com\d$\$destFolder\Printable\ManagerBridge\ManagerBridge\bin CybsSecurity.dll $params"
$roboArray += "start robocopy $src\lib \\lvqmgr03.las.printable.com\d$\$destFolder\Printable\ManagerI18N\bin Newtonsoft.Json.dll $params"
$roboArray += "start robocopy $src\lib \\lvqmgr04.las.printable.com\d$\$destFolder\Printable\ManagerI18N\bin Newtonsoft.Json.dll $params"
$roboArray += "start robocopy $src\lib \\lvqmgr04.las.printable.com\d$\$destFolder\Printable\Manager\bin CybsSecurity.dll $params"
$roboArray += "start robocopy $src\lib \\lvqmgr04.las.printable.com\d$\$destFolder\Printable\ManagerBridge\ManagerBridge\bin CybsSecurity.dll $params"
$roboArray += "start robocopy $src\lib \\lvqint02.las.printable.com\d$\$destFolder\i10nClient\bin CybsSecurity.dll $params"
$roboArray += "start robocopy $src\lib \\lvqsvc03.las.printable.com\D$\$destFolder\Printable\InternalWebServices\bin CybsSecurity.dll $params"
$roboArray += "start robocopy $src\lib \\lvqstr04.las.printable.com\d$\USMCC\Printable\PrintOne\printone\bin CybsSecurity.dll $params"
$roboArray += "start robocopy $src\lib \\lvqstr05.las.printable.com\d$\USMCC\Printable\PrintOne\printone\bin CybsSecurity.dll $params"

##Create batch file from array of commands and execute
makeRunBat $libbatFile $roboArray


## checks if any robocopy processes are still running before starting new ones
process-monitor Robocopy
write-log "Lib files have been updated"

##IIS Start for all web servers
foreach ($webserver in $webservers)
{
Try
   {
   $ErrorActionPreference = "Stop"
   Invoke-Command -ComputerName $webserver.computer -Credential $Cred -ScriptBlock {iisreset /noforce /start}
   $msg = "IIS has been successfully started on " + $webserver.computer
   write-log $msg
   }
Catch
    {
    write-log $_.Exception.Message
    }
}

##Services START for windows services
foreach($service in $services)
{
try
    {
    $ErrorActionPreference = "Stop"
    Invoke-Command -ComputerName $service.computer -Credential $Cred -ScriptBlock {param($serviceName) get-service $serviceName | Start-Service} -ArgumentList $service.service
    $msg = $service.service + " has been started on " + $service.computer
    write-log $msg
    }
catch
    {
    write-log $_.Exception.Message
    }
}


##archive changelog after script completion
$changelog = Get-ChildItem -Path $ScriptPath -Filter "*.txt"
$changeArray = $changelog.Name.Split(".")
$build = $changeArray[2]

if( $build -eq "0")
{
$folder = $changeArray[0] + "." + $changeArray[1] + ".0"
	if ((Test-Path "$scriptPath\$folder"))
		{
		Move-Item -path $msgfile -destination "$scriptPath\$folder"
		write-log "Moved release notes to archive" 
		}
	else
		{
		mkdir "$scriptPath\$folder"
		Move-Item -path $msgfile -destination $scriptPath\$folder
		write-log "Moved release notes to archive" 
		}
}
else
{
$folder = $changeArray[0] + "." + $changeArray[1] + ".x"
	if ((Test-Path "$scriptPath\$folder"))
		{
		Move-Item -path $msgfile -destination "$scriptPath\$folder"
		write-log "Moved release notes to archive" 
		}
	else
		{
		mkdir "$scriptPath\$folder"
		$oldfolder = $changeArray[0] + "." + $changeArray[1] + ".0"
		Move-Item -path "$scriptPath\$oldfolder\*.txt" -Destination "$scriptPath\$folder"
		Move-Item -path $msgfile -destination $scriptPath\$folder
		Remove-Item -Path "$scriptPath\$oldfolder" | Where { $_.PSIsContainer }
		write-log "Moved release notes to archive" 
		}
}


## update version table with the build and release number
$version = "$buildnum.$buildrev"

$sql = @"
    if not exists (select 1 from [dbo].[Version] where [version] = ''$version'' ) 
            INSERT INTO [dbo].[Version] ([version], [created], [modified]) VALUES (''$version'', getdate(), getdate());
"@

$Connection = New-Object System.Data.SQLClient.SQLConnection
$Connection.ConnectionString = "server=''lvqdb01.las.printable.com'';database=''pt'';User ID=pti_app; Password=pti1234"

$Connection.Open()
$Command = New-Object System.Data.SQLClient.SQLCommand
$Command.Connection = $Connection

try 
{
    $Command.CommandText = $sql
    $Command.ExecuteReader()
    $Connection.Close()
} 
catch 
{
    write-log $_.Exception.Message
}

# Send Post Deployment Email
$message = @" 
Hello Team,

We have completed deployment of build $buildrev of $buildnum to $buildenv.
You can resume testing now.

Thanks,
DevOps Team


"@ 
send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Deployment completed on $buildenv - build $buildrev of $buildnum" -SmtpServer $SMTPSRV -Body $message 
write-log "Post deployment email sent"   

', CAST(N'2017-03-14T15:44:06.153' AS DateTime), NULL, 1)
INSERT [shell].[Scripts] ([id], [script_name], [script_text], [create_date], [modify_date], [is_active]) VALUES (100005, N'TFSrelease.ps1', N'
		   [CmdletBinding()]
Param(
    [Parameter(Mandatory=$True)]
    [string]$releaseEnv,
	[string]$branch,
    [string]$buildnum,
    [string]$srcFolder,
	[string]$deploy
)

#### Function to output log entries to saved file ###
function write-log($msg)
{
$day=Get-Date -Format "MM-dd-yyyy"
$time=(Get-Date).ToString()
$logPath= "$tempPath$logfolder\$day Log.txt"
if ( -Not (Test-Path "$tempPath$logfolder"))
    {
     New-Item -Path "$tempPath$logfolder" -ItemType Directory | out-null
     $time + " - " + $msg | Out-File -FilePath $logPath
    }
else
    {
     $time + " - " + $msg | Out-File -FilePath $logPath -append
    }
}



### SCRIPT VARIABLES ###

# Set the SMTP Server address
$SMTPSRV = "mta.corp.printable.com"
# Set the Email address to recieve from
$EmailFrom = "notification@app.pti.com"
# Set the Email address to send the email to
$EmailTo = "devops@pti.com"
#Set Message Body
$Body = ""

$files = "/XF *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.config *.sln *.pdb global.asa pm_ServerConfigConstants.asp pf-Constants-Inc.asp"
$dir = "/XD obj debugcache cache SharedComponents properties webhelp aspnet_client UnitTests temp BuildScripts"
$params = "/NP /NJH /r:3 /w:1 /s /purge" # /z taken out to speed up copy over

$roboarray = @()
$tempPath = "E:\ptiTools\tfs_$releaseEnv\"
$tempFile = $releaseEnv + "robocopy.bat"
$logfolder = "Release_Logs"
$buildScripts = Split-Path $srcFolder | Join-Path -ChildPath "BuildScripts\"

#### Set variables based on where release is going ####

if($releaseEnv -eq "QA"){

	$server = "lvdst01.dc.pti.com"
	$distrnDest = "\\$server\d$\DistrnQA\$branch"
	$changelogdest = "\\$server\d$\_ptiScripts"
	$deployScript = $buildScripts + $releaseEnv + "deploy.ps1"
	$User = "dc\root"
	$Pass = ConvertTo-SecureString -String "ok2m40tK!" -AsPlainText -Force
	$Cred=New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $User, $Pass
}
elseif($releaseEnv -eq "PROD"){

	$server = "sdapp01.dc.pti.com"
	$distrnDest = "\\$server\d$\_Distrbn\build"
	$User = "dc\root"
	$Pass = ConvertTo-SecureString -String "ok2m40tK!" -AsPlainText -Force
	$Cred=New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $User, $Pass

}
elseif($releaseEnv -eq "UK"){

	$server = "euadm01.cl.pti.com"
	$distrnDest = "\\$server\f$\euAzure"
	$User = "cl\euroot"
	$Pass = ConvertTo-SecureString -String "L&Erbl93srf" -AsPlainText -Force
	$Cred=New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $User, $Pass

}

### map remote distribution point ###
Try
{
	$ErrorActionPreference = "Stop"
	New-PSDrive -Name target -PSProvider FileSystem -Root $distrnDest -Credential $Cred
	write-log "target has been mapped to the UNC Path in $releaseEnv"
}
Catch
{
	$ErrorMessage = $_.Exception.Message
	send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Failed to mount UNC path" -SmtpServer $SMTPSRV -Body $ErrorMessage
	write-log $ErrorMessage
}


### parse buildnum to process logic for changenotes
$buildnumArr = $buildnum.Split(".")

if($buildnumArr[2] -ne "0")  ## X.X.0 will always be from the Main branch. We only need to check if the build is not Main.  All other changesets will go into a X.X.X folder
{
   $chngFolder = $buildnumArr[0] + "." + $buildnumArr[1] + ".x"  
}
else
{
   $chngFolder = $buildnum  
}

if($releaseEnv -eq "QA"){
	#test if previous builds have been done
	if(Test-Path "$changelogdest\$chngFolder\$buildnum.*")
	{
		#get the last revision of the current build
		$changelog = Get-ChildItem -Path $changelogdest\$chngFolder -Filter "$buildnum.*" | Sort CreationTime -Descending | Select -First 1 
		$changeArray = $changelog.Name.Split(".")
		$lastrev = [int]$changeArray[3]

	}
	else
	{

		$lastrev = 0

	}

	#Create .txt file for changelog of new revision  
	$newrev = 1 + $lastrev
	#if($branch -eq "Main")
	#{
	#	$bldAgntPath = Split-Path $srcFolder | Split-Path | Join-Path -ChildPath "bin"
	#}
	#else
	#{
	#	$bldAgntPath = Split-Path $srcFolder | Join-Path -ChildPath "bin"
	#}

    $bldAgntPath = Split-Path $srcFolder | Split-Path | Join-Path -ChildPath "bin"

	Get-Content "$bldAgntPath\Notes.txt" | Out-File "$changelogdest\$buildnum.$newrev.txt" -encoding ASCII
	write-log "Created new changelog file on QA Distribution Server"

}

if(($releaseEnv -eq "QA") -or ($releaseEnv -eq "PROD")){
## dlls for the lib folder
$roboarray += "start robocopy $srcFolder\lib $distrnDest\lib CybsSecurity.dll"

## Dashboard Services
$roboarray += "start robocopy $srcFolder\Services\Dashboard $distrnDest\Printable\Services\Dashboard $params $files $dir"

## WEB API
$roboarray += "start robocopy $srcFolder\Pti\Pti.Api\Pti.Api.WebApi $distrnDest\Pti\Api $params $files $dir"

## JobDirect API
$roboarray += "start robocopy $srcFolder\PdfConversion\PdfConversion.WebApp $distrnDest\Pti\jdapi $params $files $dir"

## Portal Apps
$roboarray += "start robocopy $srcFolder\builds\PortalApps\Release $distrnDest\Pti\PortalApps $params"

## FusionPro Web Prototype
$roboarray += "start robocopy $srcFolder\FusionProWebPrototype\src\FusionProWebPrototype\bin\Release\netcoreapp1.0\win7-x64\publish $distrnDest\Pti\FusionProWebPrototype $params"

## Store Deamon Services
$roboarray += "start robocopy $srcFolder\Daemon\CatalogManagement\CatalogManagement\bin\Debug $distrnDest\Printable\CatalogManagement $params $files $dir"
$roboarray += "start robocopy $srcFolder\Notification\Notification\Templates $distrnDest\Printable\Notification\Templates $params $files $dir"
$roboarray += "start robocopy $srcFolder\Daemon\NotificationService\NotificationService\bin\Debug $distrnDest\Printable\NotificationService $params $files $dir''"

## GLOBAL APPS
$roboarray += "start robocopy $srcFolder\GlobalApps $distrnDest\Printable\GlobalApps $params $files $dir"

## Gateway / Integration
$roboarray += "start robocopy $srcFolder\BGateway\BGateway\Gateway.Net\i10n $distrnDest\Gateway\i10n $params $files $dir" 
$roboarray += "start robocopy $srcFolder\BGateway\BGateway\Gateway.Net\i10nClient $distrnDest\Gateway\i10nClient $params $files $dir"
$roboarray += "start robocopy $srcFolder\BGateway\BGateway\Gateway.Net\services\PrintGatewayServices\TransactionDeliveryService\bin\Debug $distrnDest\GatewayServices\TransactionDeliveryService $params  $files  $dir"
$roboarray += "start robocopy $srcFolder\BGateway\BGateway\Gateway.Net\services\PrintGatewayServices\BackEndTransactionRequestor\bin\Debug $distrnDest\GatewayServices\BackEndTransactionRequestor $params  $files  $dir"

## Manager Apps
$roboarray += "start robocopy $srcFolder\ManagerI18n\Manager.WebApp $distrnDest\Printable\ManagerI18N $params $files $dir"
$roboarray += "start robocopy $srcFolder\ASPClassic\Manager $distrnDest\MgrServerCodeBase $params $files $dir"
$roboarray += "start robocopy $srcFolder\Manager $distrnDest\Printable\Manager $params $files $dir" 
$roboarray += "start robocopy $srcFolder\ManagerBridge $distrnDest\Printable\ManagerBridge $params $files $dir RadControls"  
$roboarray += "start robocopy $srcFolder\Services\Manager $distrnDest\Printable\Services\Manager $params $files $dir"

## FusionPro Daemons
$roboarray += "start robocopy $srcFolder\ImageServer\Daemon\DownSampler\DownSampler\bin\Debug $distrnDest\PFBuild\Daemon\DownSampler $params $files" 
$roboarray += "start robocopy $srcFolder\ImageServer\Daemon\VdpOutputFiles\VdpOutputFiles\bin\Debug $distrnDest\PFBuild\Daemon\VdpOutputFiles $params $files"
$roboarray += "start robocopy $srcFolder\lib $distrnDest\PFBuild\Daemon\VdpOutputFiles Printable.ExternalAPIs.dll $params" 
$roboarray += "start robocopy $srcFolder\ImageServer\Daemon\VdpPreviewFiles\VdpPreviewFiles\bin\Debug $distrnDest\PFBuild\Daemon\VdpPreviewFiles $params $files" 
$roboarray += "start robocopy $srcFolder\Daemon\CopyProduct\CopyProduct\bin\Debug $distrnDest\PFBuild\Daemon\CopyProduct $params $files" 
$roboarray += "start robocopy $srcFolder\Daemon\OutputFiles\OutputFiles\bin\Debug $distrnDest\PFBuild\Daemon\OutputFiles $params $files" 
$roboarray += "start robocopy $srcFolder\Daemon\PressFiles\PressFiles\bin\Debug $distrnDest\PFBuild\Daemon\PressFiles $params $files" 
$roboarray += "start robocopy $srcFolder\Daemon\Scheduler\Scheduler\bin\Debug $distrnDest\PFBuild\Daemon\Scheduler $params $files" 
$roboarray += "start robocopy $srcFolder\Daemon\Compositions\Compositions\bin\Debug $distrnDest\PFBuild\Compositions $params $files" 
#$roboarray += "start robocopy $srcFolder\ImageServer\WebApp\WebApp\bin $distrnDest\PFBuild\ImageServer\Bin CreateImages.bat log4net.dll Printable.Config.dll Printable.Threading.dll WatchProc.exe $params"

## FusionPro Webapps
$roboarray += "start robocopy $srcFolder\ImageServer\WebApp\WebApp $distrnDest\PFBuild\ImageServer $params $files $dir"
$roboarray += "start robocopy $srcFolder\IntelligentForms\WebServices $distrnDest\PFBuild\TableDrivenWS $params $files $dir"
$roboarray += "start robocopy $srcFolder\lib $distrnDest\PFBuild\ImageServer\bin Microsoft.Web.SessionState.SqlInMemory.dll $params"

## Portal
$roboarray += "start robocopy $srcFolder\PrintOne\printone $distrnDest\Printable\PrintOne\printone $params $files $dir RadControls"

## Web Services
$roboarray += "start robocopy $srcFolder\WebServices\WebApp\WebServices\WebServices $distrnDest\printable\WebServices\TRANS\1.0 $params $files $dir util Behaviors requesthandlers"
$roboarray += "start robocopy $srcFolder\WebServices\WebApp\InternalWebServices\InternalWebServices $distrnDest\printable\InternalWebServices $params $files $dir"  
$roboarray += "start robocopy $srcFolder\Pti\Pti.OAuth\oAuthOwinClient $distrnDest\Pti\oAuthOwinClient $params $files $dir"
}

elseif($releaseEnv -eq "UK"){
## API
$roboarray += "start robocopy $srcFolder\Pti\Pti.Api\Pti.Api.WebApi $distrnDest\api\US\pti\webapi /MT /NP /NJH /r:3 /w:1 /z /S /PURGE /XF *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.config *.token *.sln /XD obj debugcache cache SharedComponents"

## DMN

$roboarray += "start robocopy $srcFolder\Daemon\CatalogManagement\CatalogManagement\bin\Debug $distrnDest\dmn\us\Printable\CatalogManagement /PURGE /NJH /NP /MT:16 /NS /S /r:3 /w:1 /z /XF *.cs *.csproj *.vspscc *.build *.sln *.vsscc *.vssscc /XD obj debugcache cache SharedComponents"
$roboarray += "start robocopy $srcFolder\Daemon\NotificationService\NotificationService\bin\Debug $distrnDest\dmn\us\Printable\NotificationService /s /r:3 /w:1 /z /PURGE /NJH /NP /MT:16 /NS /XF *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.pdb *.config *.token /XD obj debugcache cache SharedComponents"
$roboarray += "start robocopy $srcFolder\Notification\Notification\Templates $distrnDest\dmn\us\Printable\Notification\Templates /s /r:3 /w:1 /z /PURGE /NJH /NP /MT:16 /NS /XF *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.config *.token /XD obj debugcache cache SharedComponents"

## GLB
$roboarray += "start robocopy $srcFolder\GlobalApps $distrnDest\glb\us\Printable\GlobalApps /NJH /NS /NP /MT /PURGE /S /r:3 /w:1 /z /XF *.sln *.slnCache *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.config *.token /XD obj debugcache cache SharedComponents"

## INT
$roboarray += "start robocopy $srcFolder\BGateway\BGateway\Gateway.Net\i10n $distrnDest\int\US\bGateway\i10n /PURGE /NJH /NP /MT /NS /s /z /r:5 /w:1 /XF *.config *.token *.pdb *.sln *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc /XD obj debugcache cache SharedComponents"
$roboarray += "start robocopy $srcFolder\BGateway\BGateway\Gateway.Net\i10n $distrnDest\int\US\bGateway\i10nTest /PURGE /NJH /NP /MT /NS /s /z /r:5 /w:1 /XF *.config *.token *.pdb *.sln *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc /XD obj debugcache cache SharedComponents"

$roboarray += "start robocopy $srcFolder\BGateway\BGateway\Gateway.Net\i10nClient $distrnDest\int\US\bGateway\i10nClient /PURGE /NJH /NP /MT /NS /s /z /r:5 /w:1 /XF *.config *.token *.pdb *.sln *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc /XD obj debugcache cache SharedComponents"
$roboarray += "start robocopy $srcFolder\lib $distrnDest\int\US\bGateway\i10nClient\bin CybsSecurity.dll /r:3 /w:1 /z /NJH /NP /MT /NS"

$roboarray += "start robocopy $srcFolder\BGateway\BGateway\Gateway.Net\services\PrintGatewayServices\TransactionDeliveryService\bin\Debug $distrnDest\int\US\bGateway\Services\TransactionDeliveryService /PURGE /NJH /NP /MT /NS /s /z /r:5 /w:1 /XF *.config *.token *.pdb *.sln *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc /XD obj debugcache cache SharedComponents"
$roboarray += "start robocopy $srcFolder\BGateway\BGateway\Gateway.Net\services\PrintGatewayServices\BackEndTransactionRequestor\bin\Debug $distrnDest\int\US\bGateway\Services\BackEndTransactionRequestor /NJH /NP /MT /NS /PURGE /s /z /r:5 /w:1 /XF *.config *.token *.pdb *.sln *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc /XD obj debugcache cache SharedComponents"

## JDAPI
$roboarray += "start robocopy $srcFolder\PdfConversion\PdfConversion.WebApp $distrnDest\jdapi\US\pti\jdapi /MT /NP /NJH /r:3 /w:1 /z /S /PURGE /XF *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.config *.token *.sln /XD obj debugcache cache SharedComponents"

## MGR
$roboarray += "start robocopy $srcFolder\ManagerI18n\Manager.WebApp $distrnDest\mgr\US\Printable\ManagerI18N /NJH /NS /NP /MT /R:3 /W:1 /Z /S /PURGE /XF *.config *.token *.pdb *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc /XD obj properties debugcache cache SharedComponents" 
$roboarray += "start robocopy $srcFolder\Services\Manager $distrnDest\mgr\US\Printable\Services\Manager /NJH /NP /MT /NS /r:3 /w:1 /z /S /PURGE /XF *.config *.token *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.pdb /XD obj debugcache cache SharedComponents"
$roboarray += "start robocopy $srcFolder\ManagerI18n\Manager.WebApp\bin $distrnDest\mgr\ServicesSystemDLLs Pti.Manager.WebApp.dll /r:3 /w:1 /z /NJH /NP /MT /NS"

## PFD
$roboarray += "start robocopy $srcFolder\Daemon\CopyProduct\CopyProduct\bin\Debug $distrnDest\pf3a\pfdCode\US\Daemon\CopyProduct /NJH /NP  /NS /z /r:3 /w:1 /XF *.config *.token *.pdb *.sln *.pdb *.cs /PURGE"
$roboarray += "start robocopy $srcFolder\Daemon\Compositions\Compositions\bin\Debug $distrnDest\pf3a\pfiCode\pti\Compositions /NJH /NP  /NS /z /s /r:3 /w:1 /XF *.config *.token *.pdb *.sln /PURGE"
$roboarray += "start robocopy $srcFolder\Daemon\OutputFiles\OutputFiles\bin\Debug $distrnDest\pf3a\pfdCode\US\Daemon\OutputFiles /NJH /NP  /NS /z /s /r:3 /w:1 /XF *.config *.token *.pdb *.sln /PURGE"
$roboarray += "start robocopy $srcFolder\Daemon\PressFiles\PressFiles\bin\Debug $distrnDest\pf3a\pfdCode\US\Daemon\PressFiles /NJH /NP  /NS /z /s /r:3 /w:1 /XF *.config *.token *.pdb *.sln /PURGE"
$roboarray += "start robocopy $srcFolder\Daemon\Scheduler\Scheduler\bin\Debug $distrnDest\pf3a\pfdCode\US\Daemon\Scheduler /NJH /NP  /NS /z /s /r:3 /w:1 /XF *.config *.token *.pdb *.sln /PURGE"

$roboarray += "start robocopy $srcFolder\ImageServer\Daemon\DownSampler\DownSampler\bin\Debug $distrnDest\pf3a\pfdCode\US\Daemon\DownSampler /NJH /NP  /NS /z /r:3 /w:1 /XF *.config *.token *.pdb *.sln *.pdb *.cs /PURGE"
$roboarray += "start robocopy $srcFolder\ImageServer\Daemon\VdpOutputFiles\VdpOutputFiles\bin\Debug $distrnDest\pf3a\pfdCode\US\Daemon\VdpOutputFiles /NP  /NJH /NS /z /r:3 /w:1 /XF *.config *.token *.pdb *.sln *.pdb *.cs /PURGE"
$roboarray += "start robocopy $srcFolder\ImageServer\Daemon\VdpPreviewFiles\VdpPreviewFiles\bin\Debug $distrnDest\pf3a\pfdCode\US\Daemon\VdpPreviewFiles /NP  /NJH /NS /z /r:3 /w:1 /XF *.config *.token *.pdb *.sln *.pdb *.cs /PURGE"
$roboarray += "start robocopy $srcFolder\Notification\Notification\Templates $distrnDest\pf3a\pfdCode\US\Daemon\Notification\Templates /s /r:3 /w:1 /z /PURGE /NJH /NP /MT /NS /XF *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.config *.token /XD obj debugcache cache SharedComponents"

## PFE
$roboarray += "start robocopy $srcFolder\ImageServer\WebApp\WebApp $distrnDest\pf3a\pfeCode\US\ImageServer /NJH /NP /NS /r:3 /w:1 /s /z /PURGE /XF DIFTester.aspx SimpleFileTester.aspx *.config *.token *.pdb *.cs *.csproj *.vspscc *.build /XD obj debugcache cache SharedComponents"
$roboarray += "start robocopy $srcFolder\lib $distrnDest\pf3a\pfeCode\US\ImageServer\bin Microsoft.Web.SessionState.SqlInMemory.dll /NJH /NP /NS /r:3 /w:1 /s /z"
$roboarray += "start robocopy $srcFolder\IntelligentForms\WebServices $distrnDest\pf3a\pfeCode\US\TableDrivenWS /NJH /NP /NS /r:3 /w:1 /s /z /PURGE /XF *.sln *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.config *.token *.pdb *.sln /XD obj debugcache cache SharedComponents"

## STR
$roboarray += "start robocopy $srcFolder\printone\printone $distrnDest\str\us\Printable\PrintOne\printone /NP /NJH /NS /r:3 /w:1 /S /PURGE /XF *.sln *.slnCache *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.config *.token /XD RadControls obj debugcache cache SharedComponents" 

## SVC
$roboarray += "start robocopy $srcFolder\WebServices\WebApp\WebServices\WebServices $distrnDest\svc\us\Printable\WebServices\TRANS\1.0 /r:3 /w:1 /z /S /purge /NP /MT /NJH /NS /XD obj debugcache cache SharedComponents /XF *.config *.token *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.pdb"
$roboarray += "start robocopy $srcFolder\WebServices\WebApp\InternalWebServices\InternalWebServices $distrnDest\svc\us\Printable\InternalWebServices /r:3 /w:1 /z /S /NJH /NP /MT /NS /PURGE /XD unittests obj debugcache cache SharedComponents /XF *.config *.token *.cs *.csproj *.vspscc *.build *.vsscc *.vssscc *.pdb"
$roboarray += "start robocopy $srcFolder\lib $distrnDest\svc\us\Printable\InternalWebServices\bin CybsSecurity.dll /r:3 /w:1 /z /NJH /NP /MT /NS"
$roboarray += "start robocopy $srcFolder\Notification\Notification\Templates $distrnDest\svc\us\Printable\Notification\Templates /s /r:3 /w:1 /z /NJH /NP /MT /NS"

}


##Create batch file from array of commands
$tempbat = join-path -path $tempPath -childpath $tempFile
Try
{
$ErrorActionPreference = "Stop"
Out-File -FilePath $tempbat -InputObject $roboarray -encoding ASCII -Force
write-log "Batch file has been created"
}
Catch
{
$ErrorMessage = $_.Exception.Message
send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Failed to create batch file" -SmtpServer $SMTPSRV -Body $ErrorMessage
write-log $ErrorMessage
}


##Execute newly created batch file
Try
{
$ErrorActionPreference = "Stop"
cmd.exe /c $tempbat
write-log "Batch file has been executed"
}
Catch
{
$ErrorMessage = $_.Exception.Message
send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Failed to execute batch file" -SmtpServer $SMTPSRV -Body $ErrorMessage
write-log $ErrorMessage
}


## checks if any robocopy processes are still running
$ProcessActive = Get-Process Robocopy -ErrorAction SilentlyContinue
while($ProcessActive -ne $null)
{
 start-sleep -s 10
 $ProcessActive = Get-Process Robocopy -ErrorAction SilentlyContinue
}

# unmount network share
Try
{
$ErrorActionPreference = "Stop"
Remove-PSDrive -Name target
write-log "target has been unmapped"
}
Catch
{
$ErrorMessage = $_.Exception.Message
send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Failed to umount UNC path" -SmtpServer $SMTPSRV -Body $ErrorMessage
write-log $ErrorMessage
}


# Send email after transfer completes
$message = @" 
Hello Team,

The binaries for build $buildnum.$newrev have been pushed to the $releaseEnv distribution server.

If you set the deploy parameter to "True" in the build definition then the deployment will begin momentarily.

If you did not set the parameter, then you will need to manually run the deploy the following script:

$changelogdest\$releaseEnv`deploy.ps1

Thanks,
DevOps Team


"@ 
send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Transfer to $releaseEnv of build $buildnum.$newrev" -SmtpServer $SMTPSRV -Body $message
write-log "status email sent to CM team"


if($deploy -eq "true")
{
Try
{
$ErrorActionPreference = "Stop"
$session = New-PSSession -Name Deploy -ComputerName $server -Credential $Cred -Authentication Credssp
invoke-command -Session $session -filepath $deployScript
Get-PSSession -Name Deploy | Remove-PSSession
write-log "Deployment script has been run on $server"
}
Catch
{
$ErrorMessage = $_.Exception.Message
send-mailmessage -To $EmailTo -From $EmailFrom -Subject "Failed to start deployment script on $server" -SmtpServer $SMTPSRV -Body $ErrorMessage
write-log $ErrorMessage
}
}', CAST(N'2017-03-14T15:45:21.663' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [shell].[Scripts] OFF
