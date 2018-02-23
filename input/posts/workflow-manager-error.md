Title: Workflow Manager Re-Registration despite everything seems ok
Published: 11/8/2016
Tags: Sharepoint
---

So,

If you did the exact steps and you receive the "Workflow is connected" message when you click on Workflow Manager Service Application inside Sharepoint Central Admin, but somehow, something is not working quite right, or maybe you receive this message trying to publish in Sharepoint Designer:

![WorkFlow Manager Error](/images/workflowmgr_error.png)

Solution: Re-Register the Workflow Service using Register-SPWorkflowService and the -Force Parameter. There is a good chance everything starts working then.
