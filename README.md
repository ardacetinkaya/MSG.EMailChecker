# MSG.EMailChecker

[Microsoft Graph](https://developer.microsoft.com/en-us/graph) is a great platform for connecting you to Office 365 account and empower your business with your rich development features. Microsoft Graph has a great API set to do unique operations within a secure way. In this repo. I just tried to demostrate how to listen your e-mail box for messages. According to your business you may want to do some integration jobs or some other things.

Mostly enterprises have some scheduled jobs or long periodic jobs that are listening some inboxes and do some fancy stuff. For example; 

* Generating customer requirments from an e-mail content
* Open a service ticket
* Reply with auto-reply templates according to a content
* Parse message content to get some specific texts
* ...
* ..

And many more... Nowadays most business are moving into Office365, so these kind of automations should be merged as Office365. In this repo., I just demostrated to read e-mail with Microsoft Graph API. You may do the same things with REST APIs of Microsoft Graph. 


When you are doing some stuff with Microsoft Graph, you should register your app. at https://apps.dev.microsoft.com/ . This step is just required to generate appliation id to connect your Office365 account in a secure way. Also from this page you have to set application permissions such as **Mail.Read**, **User.Read**, **Mail.Send**...etc.

To use Microsoft Graph API in a good way, you need some packages;

* [Microsoft.Graph](https://www.nuget.org/packages/Microsoft.Graph)
* [Microsoft.Identity](https://www.nuget.org/packages/Microsoft.Graph)

The login operation for these kind of Microsoft Graph API applications use Microsoft's secure login UI. This familiar login operation provides secure way to connect your Office365 accout.


<img src="https://github.com/ardacetinkaya/MSG.EMailChecker/blob/master/Login.PNG" width="600">


**IMPORTANT**:Due to some platform limitations with .NET Core, *PublicClientApplication* methods are not working as expected. If you are planing to do with .NET Core, you may not use Microsoft.Identity. 

When you successfully login, the e-mail checker starts to check your e-mails for some spesific period.

<img src="https://github.com/ardacetinkaya/MSG.EMailChecker/blob/master/PoC.PNG" width="600">

Some silly info: I just listened MSG's Nightmare while I'm coding. So I changed the name as MSG... By the way, the song is amazing. :) 
https://www.youtube.com/watch?v=UAU9pbpH458



