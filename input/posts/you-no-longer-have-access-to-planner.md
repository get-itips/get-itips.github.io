Title: You no longer have access to <Guid> on Planner
Published: 11/12/2021
Tags: Planner
---

## Symptons

I was called by a customer running into this error, essentially, they create company Azure AD accounts for external users (not using guests accounts) to access some resources.

One of these resources is Microsoft Planner, the users started reporting that they couldn't comment on the assigned tasks of Planner.

The error received (in red) was

>You no longer have access to \<Guid>\. Technical Details

>Correlation Id. \<Correlation Id>

## Investigation

Looking for the error on a web search brings this official **Docs** article:

[Guests can't add comments to assigned Planner tasks](https://docs.microsoft.com/office/troubleshoot/planner/guests-cannot-comment-assigned-tasks)

However, as said, these users were not accessing Planner using Guests account, but company accounts. Anyway, I checked if the account's SMTP address was also present in other account or contact object, as the article says, but it was not.

There is one special ingredient in this scenario, they have **Exchange Online doing hybrid with Exchange On-Premises** and **they don't create Mailboxes for these external resources**.

Digging with developer tools, I could see that the HTTP request that was failing was a call to this Graph API:

https://tasks.office.com/contoso.com/GraphApiV1/PostReplyInGroupConversationThreadAsync

"PostReplyInGroupConversation", so it must has something to do with the e-mail that is sent to the 365 Group when a new comment is made on a task.

I thought "they do not have mailboxes, not even On-Premises". There is this support article [Comment on tasks in Microsoft Planner](https://support.microsoft.com/en-us/office/comment-on-tasks-in-microsoft-planner-fd4aedde-7785-4cd0-96ee-122fbc9140e1) that says

>If your organization is not using Exchange Online for your account, you may not be able to comment on tasks in Planner

## Resolution

I asked to create mailboxes for these users (Exchange Server Mailboxes, but if we run into any issue we will migrate them to Online), we waited for replication, and Voilá! the users could start commenting on Tasks and the email to the group was succesfully sent.

I hope this is useful for anyone looking for this error and not using guest accounts.

I also would like to add that, if you invite a Guest user that also happens to have a 365 Tenant, they will be able to comment in a Task without any issue.