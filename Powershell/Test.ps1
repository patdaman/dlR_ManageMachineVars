# Set Log File
#  Set App.config
#  Load DevOps.dll
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
Set-Website -recycle -machineName 'hqdev07.dev.corp.printable.com' -username '"pdelosreyes"' -password 'Patman7474!' -domain 'printable';
#Get-SiteInformation -machineName 'hqdev07.dev.corp.printable.com' -username '"pdelosreyes"' -password 'Patman7474!' -domain 'printable';

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -keepAlive 'False' -machineName 'hqdev07.dev.corp.printable.com' -username '"pdelosreyes"' -password 'Patman7474!' -domain 'printable';

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -keepAlive 'True' -machineName 'hqdev07.dev.corp.printable.com' -username '"pdelosreyes"' -password 'Patman7474!' -domain 'printable';

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -stop -machineName 'hqdev07.dev.corp.printable.com' -username '"pdelosreyes"' -password 'Patman7474!' -domain 'printable';

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -start -machineName 'hqdev07.dev.corp.printable.com' -username '"pdelosreyes"' -password 'Patman7474!' -domain 'printable';

$pauseText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");

Set-Website -recycle -machineName 'hqdev07.dev.corp.printable.com' -username '"pdelosreyes"' -password 'Patman7474!' -domain 'printable';

$exitText;
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown");