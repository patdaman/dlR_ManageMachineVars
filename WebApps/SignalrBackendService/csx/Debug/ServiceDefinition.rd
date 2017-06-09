<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SignalrBackendService" generation="1" functional="0" release="0" Id="cd4991f0-98a7-4d99-9b7a-1f3c287f1f42" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SignalrBackendServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="SignalrWebService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/LB:SignalrWebService:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/LB:SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/MapCertificate|SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="SignalrWebService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/MapSignalrWebService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/MapSignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/MapSignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/MapSignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/MapSignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/MapSignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="SignalrWebServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/MapSignalrWebServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:SignalrWebService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapSignalrWebService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapSignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapSignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapSignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapSignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapSignalrWebServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="SignalrWebService" generation="1" functional="0" release="0" software="C:\Users\pdelosreyes\Documents\GitHub\dlR_ManageAppVars\WebApps\SignalrBackendService\csx\Debug\roles\SignalrWebService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SW:SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SignalrWebService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;SignalrWebService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="SignalrWebServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="SignalrWebServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="SignalrWebServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="4328152c-406e-471b-a336-ab2b2ec64e77" ref="Microsoft.RedDog.Contract\ServiceContract\SignalrBackendServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="31cd8ebd-3b0a-47b9-be39-f2583c54ccfc" ref="Microsoft.RedDog.Contract\Interface\SignalrWebService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="b788c428-2ecb-4bde-bac1-29ef64b34402" ref="Microsoft.RedDog.Contract\Interface\SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SignalrBackendService/SignalrBackendServiceGroup/SignalrWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>