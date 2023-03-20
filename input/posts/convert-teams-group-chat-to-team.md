Title: Convert Microsoft Teams group chat to team
Published: 3/20/2023
Tags: Teams
---

# Introduction

Microsoft Teams Group Chat members can grow, sometimes, a conversation that is happening in a Group Chat would be better and deserve its own Microsoft Teams team, let's say
we have a Group Chat with 25 members, what it takes to get the list of members is not straigthforward. So, with a little PowerShell magic, we can "convert" a group chat into a team.

Actually, what the script does is creating a team with the same membership as the group chat, the group chat will stay as is and you will have to redirect the users to the 
new team. It will ask you a couple of things and then run the required commands.

# Requirements
- Latest stable Microsoft Graph PowerShell module
- Latest stable Microsoft Teams module

# The script

You can find it [here](https://github.com/get-itips/MiscScripts/blob/main/Teams/CreateTeamFromGroupChat.ps1) and I expect to update it with new features, contributions are welcomed.
