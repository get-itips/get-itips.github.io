Title: The Microsoft Exchange Unified Messaging Call Router service rejected the call for the following reason
Published: 5/11/2022
Tags: ExchangeServer
---

In the process of adding an Exchange Server 2019 to a current Exchange organization with Exchange Server 2013, Auto Attendant/UM stopped working, investigating event viewer logs
in Exchange Server 2013 server, I found this event entry (Event Id 1647)

```powershell
The Microsoft Exchange Unified Messaging Call Router service rejected the call for the following reason: 15647;source="ex2013.contoso.net";reason="Access to Active Directory error occured." Microsoft.Exchange.UM.UMCore.CallRejectedException: The Mailbox server with the FQDN ex2019.contoso.net couldn't be located in Active Directory.
   at Microsoft.Exchange.UM.UMCore.EnterpriseRedirectionTarget.GetRoutingInformation(String serverFqdn, Boolean isSecuredCall, String& routingFqdn, Int32& routingPort)
   at Microsoft.Exchange.UM.UMCore.EnterpriseRedirectionTarget.GetBackEndBrickRedirectionTarget(ADUser user, IRoutingContext context)
   at Microsoft.Exchange.UM.UMCore.EnterpriseRedirectionTarget.GetForNonUserSpecificCall(OrganizationId orgId, IRoutingContext context)
   at Microsoft.Exchange.UM.UMCore.AutoAttendantCallHandler.HandleCall(CafeRoutingContext context)
   at Microsoft.Exchange.UM.UMCore.RouterCallHandler.InternalHandle(ICallHandler[] handlers, CafeRoutingContext context)
   at Microsoft.Exchange.UM.UcmaPlatform.UcmaCallRouterPlatform.TryHandleIncomingCall(CallReceivedEventArgs`1 args, Exception& error, Boolean& isDiagnosticCall)
   at Microsoft.Exchange.UM.UcmaPlatform.UcmaUtils.<>c__DisplayClassf.<ProcessPlatformRequestAndReportErrors>b__d()
```

The error might be misleading as it implies some Active Directory connection issue, but it was mentioning the newly incorporated Exchange Server 2019 and I thought, why?

I then remember I migrated System Mailboxes to Exchange 2019 box, especially, these two, that have UM-related capabilities:

```powershell
Name                  : SystemMailbox{bb558c35-97f1-4cb9-8ff7-d53741dc928c}
PersistedCapabilities : {OrganizationCapabilityUMGrammarReady, OrganizationCapabilityPstProvider,
                        OrganizationCapabilityMessageTracking, OrganizationCapabilityMailRouting,
                        OrganizationCapabilityClientExtensions, OrganizationCapabilityGMGen,
                        OrganizationCapabilityOABGen, OrganizationCapabilityUMGrammar}

Name                  : SystemMailbox{e0dc1c29-89c3-4034-b678-e6c29d823ed9}
PersistedCapabilities : {OrganizationCapabilityUMDataStorage}
```

Boom! that might be it... so I moved them back to Exchange 2013 mailbox database and AutoAttendant/UM started to work again.
