# Set Log File
#  Set App.config
#  Load DevOps.dll

#################################################################
$domain = "printable"
$username = "pdelosreyes"
$password = "Patman7474!"
$machineName = "hqdev07.dev.corp.printable.com"
#################################################################
Start-Transcript -Path ".\..\..\Logs\Log.txt";
$configPath = ".\App.config";
#[System.AppDomain]::CurrentDomain.SetData("APP_CONFIG_FILE", $configPath)
Import-Module .\DevOps.dll -Force;
$pauseText = 'Write-Host "Press any key to continue ..."';
$exitText = 'Write-Host "Press any key to exit ..."';
#############################################################################################################

# Commands:
Get-NetworkAdapter;

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -recycle -machineName $machineName -username $username -password $password -domain $domain;

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Get-SiteInformation -machineName $machineName -username $username -password $password -domain $domain;

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -keepAlive 'False' -machineName $machineName -username $username -password $password -domain $domain;

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -keepAlive 'True' -machineName $machineName -username $username -password $password -domain $domain;

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -stop -machineName $machineName -username $username -password $password -domain $domain;

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -start -machineName $machineName -username $username -password $password -domain $domain;

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -recycle -machineName $machineName -username $username -password $password -domain $domain;

$exitText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");