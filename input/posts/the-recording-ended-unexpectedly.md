Title: The recording ended unexpectedly
Published: 8/11/2021
Tags: Teams
---

Recently, Microsoft announced through a [Message Center notification](https://admin.microsoft.com/#/MessageCenter/:/messages/MC222640) that every customer will automatically have meeting recordings saved to OneDrive and SharePoint.

Once you stop the recording or end a meeting that is being recorded, the recording should be uploaded to your OneDrive For Business\SharePoint document library depending on the type of meeting.

The recording will be uploaded as the user that initiated the recording, so, if for example, Megan Bowen started the recording, that is the name we will see in the SharePoint document library named "Recordings" as the creator/modifier of the file.

If for any reason, this upload fails, an error message will appear in red letters saying "The recording ended unexpectedly" and a link to download it will be available stating it "expires in 20 day(s)"

![Teams Meetings Recording Error](/images/tmr_error.png)

Besides it would be very strange, I forced that error removing the permissions to save to this Recordings folder to the user initiating the recording.
According to the [documentation](https://docs.microsoft.com/MicrosoftTeams/tmr-meeting-recording-change), when this happens, the recording is uploaded to Azure Media Services:

>If a Teams meeting recording fails to successfully upload to OneDrive/SharePoint, the recording will instead be temporarily saved to Azure Media Services (AMS). Once stored in AMS, no retry attempts are made to automatically upload the recording to OneDrive/SharePoint or Stream.

Take note of that, no retry attempts will be made to automatically upload the recording again, so upon fixing the issue that prevented the uploading, you should download the recording and upload it manually either to the your OneDrive For Business\SharePoint document library.


