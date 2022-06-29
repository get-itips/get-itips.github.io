Title: Cmdlets run by the Hybrid Configuration Wizard
Published: 6/29/2022
Tags: ExchangeServer
---

Based on several runs of the Hybrid Configuration Wizard from different deployments, these appear to be the cmdlets ran either against On-Premises (Exchange Server) and against the Tenant (Exchange Online).
I'll update these if I found any missing cmdlets, and I also would like to split them between Minimal and Full hybrid options.

# Classic or Modern Full

## Get- cmdlets

| Cmdlet | Where | Parameters |
|--------|-------|-------|
|Get-ExchangeServer|Exchange Server||
|Get-MailboxDatabase|Exchange Server|-IncludePreExchange2013: $true|
|Get-OrganizationConfig|Exchange Server|       |
|Get-HybridConfiguration|Exchange Server|       |
|Get-AcceptedDomain|Exchange Server|       |
|Get-FederatedOrganizationIdentifier|Exchange Server|-IncludeExtendedDomainInfo: $false|
|Get-FederationTrust|Exchange Server||
|Get-WebServicesVirtualDirectory|Exchange Server|-ADPropertiesOnly: $true|
|Get-RemoteDomain|Exchange Server||
|Get-OrganizationConfig|Exchange Online||
|Get-OnPremisesOrganization|Exchange Online||
|Get-AcceptedDomain|Exchange Online||
|Get-MigrationEndpoint|Exchange Online||
|Get-ExchangeCertificate|Exchange Server|-Server <ExchangeHost>|
|Get-RemoteDomain|Exchange Server||
|Get-EmailAddressPolicy|Exchange Server||
|Get-OrganizationRelationship|Exchange Server||
|Get-OrganizationConfig|Exchange Server||
|Get-OrganizationRelationship|Exchange Online||
|Get-OwaVirtualDirectory|Exchange Server|-ADPropertiesOnly: $true|
|Get-AvailabilityAddressSpace|Exchange Server||
|Get-SendConnector|Exchange Server||
|Get-ReceiveConnector|Exchange Server|-Server <ExchangeHost>|
|Get-FederationInformation*1|Exchange Server||
|Get-FederationTrust*1|Exchange Server|-Identity 'Microsoft Federation Gateway'|
|Get-FederationTrust*1|Exchange Online|-Identity MicrosoftOnline|
|Get-OutboundConnector|Exchange Online||
|Get-InboundConnector|Exchange Online||
|Get-IntraOrganizationConfiguration|Exchange Server||
|Get-IntraOrganizationConfiguration|Exchange Online|-OrganizationGuid '<guid>'|
|Get-IntraOrganizationConnector|Exchange Online||
|Get-IntraOrganizationConnector|Exchange Server||
|Get-AuthConfig|Exchange Server||
|Get-ActiveSyncVirtualDirectory|Exchange Server|-ADPropertiesOnly: $true|
|Get-PartnerApplication|Exchange Server|-Identity 'Exchange Online'|

## New-, Set- & Update- cmdlets

| Cmdlet | Where | Parameters (Example) |
|--------|-------|-------|
|Update-EmailAddressPolicy|Exchange Server|-Identity 'Default Policy' -UpdateSecondaryAddressesOnly: $true|
|New/Set-HybridConfiguration|Exchange Server|-ClientAccessServers $null -ExternalIPAddresses $null -Domains 'contoso.com','fabrikam.com' -OnPremisesSmartHost 'mail.contoso.com' -TLSCertificateName '<I>CN=WorldSSL DV CA, O=Sàrl, C=LU\<S>CN=mail.contoso.com' -SendingTransportServers EXC01 -ReceivingTransportServers EXC01 -EdgeTransportServers $null -Features FreeBusy,MoveMailbox,Mailtips,MessageTracking,OwaRedirection,OnlineArchive,SecureMail,Photos|
|Set-EmailAddressPolicy|Exchange Server|-Identity 'Default Policy' -ForceUpgrade: $true -EnabledEmailAddressTemplates 'SMTP:@contoso.com','X400:c=US;a= ;p=Contoso;o=Contoso;','smtp:%m@contoso.mail.onmicrosoft.com'|
|New/Set-OrganizationRelationship|Exchange Server|-FreeBusyAccessEnabled: $true -FreeBusyAccessLevel LimitedDetails -MailTipsAccessEnabled: $true -MailTipsAccessLevel All -DeliveryReportEnabled: $true -PhotosEnabled: $true -TargetOwaURL 'https://outlook.office.com/mail' -Identity 'On-premises to O365 - 6fc3a33e-3ca2-4528-8e3c-75e5d4e9db76'|
|New/Set-OrganizationRelationship|Exchange Online|-FreeBusyAccessEnabled: $true -FreeBusyAccessLevel LimitedDetails -TargetSharingEpr $null -MailTipsAccessEnabled: $true -MailTipsAccessLevel All -DeliveryReportEnabled: $true -PhotosEnabled: $true -TargetOwaURL 'https://mail.contoso.com/owa' -Identity 'O365 to On-premises - 6fc3a33e-3ca2-4528-8e3c-75e5d4e9db76'|
|New/Set-SendConnector|Exchange Server|-Name 'Outbound to Office 365 - 6fc3a33e-3ca2-4528-8e3c-75e5d4e9db76' -AddressSpaces 'smtp:contoso.mail.onmicrosoft.com;1' -DNSRoutingEnabled: $true -ErrorPolicies Default -Fqdn 'mail.contoso.com' -RequireTLS: $true -IgnoreSTARTTLS: $false -SourceTransportServers EXC01 -SmartHosts $null -TLSAuthLevel DomainValidation -DomainSecureEnabled: $false -TLSDomain 'mail.protection.outlook.com' -CloudServicesMailEnabled: $true -TLSCertificateName '<I>CN=Go mommy Secure Certificate Authority - G2, OU=http://certs.gomommy.com/repository/, O="Gomommy.com, Inc.", L=Scottsdale, S=Arizona, C=US\<S>CN=mail.contoso.com' -Identity 'Outbound to Office 365 - 6fc3a33e-3ca2-4528-8e3c-75e5d4e9db76'|
|Set-ReceiveConnector|Exchange Server|-AuthMechanism 'Tls, Integrated, BasicAuth, BasicAuthRequireTLS, ExchangeServer' -Bindings '[::]:25','0.0.0.0:25' -Fqdn 'EXC01.contoso.com' -PermissionGroups 'AnonymousUsers, ExchangeServers, ExchangeLegacyServers' -RemoteIPRanges '::-ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff','0.0.0.0-255.255.255.255' -RequireTLS: $false -TLSDomainCapabilities 'mail.protection.outlook.com:AcceptCloudServicesMail' -TLSCertificateName '<I>CN=Go mommy Secure Certificate Authority - G2, OU=http://certs.gomommy.com/repository/, O="Gomommy.com, Inc.", L=Scottsdale, S=Arizona, C=US<\S>CN=mail.contoso.com' -TransportRole FrontendTransport -Identity 'EXC01\Default Frontend EXC01'|
|New/Set-OutboundConnector|Exchange Online|-Name 'Outbound to 6fc3a33e-3ca2-4528-8e3c-75e5d4e9db76' -RecipientDomains 'contoso.com','fabrikam.com' -SmartHosts 'mail.contoso.com' -ConnectorSource HybridWizard -ConnectorType OnPremises -TLSSettings DomainValidation -TLSDomain 'mail.contoso.com' -CloudServicesMailEnabled: $true -RouteAllMessagesViaOnPremises: $false -UseMxRecord: $false -IsTransportRuleScoped: $false -Identity 'Outbound to 6fc3a33e-3ca2-4528-8e3c-75e5d4e9db76'|
|New/Set-InboundConnector|Exchange Online| -Name 'Inbound from 6fc3a33e-3ca2-4528-8e3c-75e5d4e9db76' -CloudServicesMailEnabled: $true -ConnectorSource HybridWizard -ConnectorType OnPremises -RequireTLS: $true -SenderDomains '*' -SenderIPAddresses $null -RestrictDomainsToIPAddresses: $false -TLSSenderCertificateName '*.contoso.com' -AssociatedAcceptedDomains $null|
|Set-OnPremisesOrganization|Exchange Online|-HybridDomains 'contoso.com','fabrikam.com' -InboundConnector 'Inbound from 6fc3a33e-3ca2-4528-8e3c-75e5d4e9db76' -OutboundConnector 'Outbound to 6fc3a33e-3ca2-4528-8e3c-75e5d4e9db76' -OrganizationRelationship 'O365 to On-premises - 6fc3a33e-3ca2-4528-8e3c-75e5d4e9db76'|
|New/Set-PartnerApplication|Exchange Server|-Identity 'Exchange Online' -Enabled: $true|
|New-RemoteDomain|Exchange Server| -Name 'Hybrid Domain - contoso.mail.onmicrosoft.com'|
|New-AcceptedDomain|Exchange Server|-DomainName 'contoso.mail.onmicrosoft.com' -Name 'contoso.mail.onmicrosoft.com'|
|New-IntraOrganizationConnector|Exchange Server|-Name 'HybridIOC - <guid>' -DiscoveryEndpoint 'https://autodiscover-s.outlook.com/autodiscover/autodiscover.svc' -TargetAddressDomains 'contoso.mail.onmicrosoft.com' -Enabled: $true|
|New-IntraOrganizationConnector|Exchange Online|-Name 'HybridIOC - <guid>' -DiscoveryEndpoint 'https://mail.contoso.com/autodiscover/autodiscover.svc' -TargetAddressDomains 'contoso.com','fabrikam.com' -Enabled: $true|
|New-AuthServer|Exchange Server|-Name 'ACS - <guid>' -AuthMetadataUrl 'https://accounts.accesscontrol.windows.net/22e190d2-28f3-4a09-898f-f7bb4c477af7/metadata/json/1' -DomainName 'contoso.com','contoso.mail.onmicrosoft.com'|
|New-AuthServer|Exchange Server|-Name 'EvoSts - <guid>' -AuthMetadataUrl 'https://login.windows.net/contoso.onmicrosoft.com/federationmetadata/2007-06/federationmetadata.xml' -Type AzureAD|
|New-FederationTrust*1|Exchange Server|-Name 'Microsoft Federation Gateway' -Thumbprint <thumbprint>|
|Set-FederatedOrganizationIdentifier|Exchange Server|-AccountNamespace 'contoso.com' -DelegationFederationTrust 'Microsoft Federation Gateway' -Enabled: $true -DefaultDomain $null|
|Set-FederatedOrganizationIdentifier|Exchange Online|-DefaultDomain 'contoso.mail.onmicrosoft.com' -Enabled: $true|

*1 Apparently only if Exchange 2010 is present in the Exchange Organization
