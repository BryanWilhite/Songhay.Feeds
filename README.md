# Songhay Feeds

![build badge](https://songhay.visualstudio.com/_apis/public/build/definitions/e6de8b87-c501-478d-8af5-b564cbd966cc/2/badge)

Songhay Feeds are `Activities` around downloading Syndication feeds (in RSS or Atom) and converting them into static JSON files. This file format can be used to bind to display component for a “dashboard” view.

Feeds `Activities` are intended to run in the cloud which, as of this writing, is [in Azure](https://docs.microsoft.com/en-us/azure/app-service/web-sites-create-web-jobs). The only clue one would have to assume that running on Azure is possible is the [conventional](https://docs.microsoft.com/en-us/azure/app-service/web-sites-create-web-jobs#acceptablefiles) `run.ps1` [script](./Songhay.Feeds.Shell/run.ps1).

@[BryanWilhite](https://twitter.com/bryanwilhite)