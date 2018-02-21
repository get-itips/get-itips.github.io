Title: Call failed to establish due to a media connectivity failure when both endpoints are...
Published: 7/21/2017
Tags: Skype4B
---
internal
external
or one endpoint is internal and the other is federated or remote...

any situation will do.

This was happening to me on an On-Premise deployment of Skype For Business (Front end and Edge Server) calling to Unified Messaging contact on Exchange Online.

ICE Candidate negotiation was happening but eventually no remote-candidates were negotiated.

ms-client-diagnostics: 22; reason="Call failed to establish due to a media connectivity failure when both endpoints are internal";UserType="Callee";MediaType="audio";MediaChanBlob="NetworkErr=no error,ErrTime=0,RTPSeq=0,SeqDelta=0,RTPTime=0,RTCPTime=0,TransptRecvErr=0x0,RecvErrTime=0,TransptSendErr=0x0,SendErrTime=0,InterfacesStall=0x0,InterfacesConnCheck=0x0,MediaTimeout=0,RtcpByeSent=0,RtcpByeRcvd=0,BlobVer=1";

So this must be firewall-related, this is almost always firewall-related, it seems first that the NAT was maybe responding with another external IP from de ISP Pool, but that was ok, the rules were correct, we checked and re-rechecked, and after so many hours of troubleshooting with Snooper and Wireshark, we started to look more deeply on the firewall and tried weird things.

The firewall, a Fortigate one, had a NAT button enabled on the rule, you would say, that is obvious that must be activated... but we tried and deactivated that and... Voilá! we saw the face of god at that moment, it worked, the remote-candidates were negotiated at that moment  (so always be sure that this line is present if you want to know that it is working ok)

a=remote-candidates:1 65.55.127.80 54860 2 65.55.127.80 54721

and we lived happily ever after.