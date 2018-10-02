# Songhay Feeds

[![Build Status](https://songhay.visualstudio.com/SonghaySystem/_apis/build/status/songhay-system-job-feeds-yaml-build)](https://songhay.visualstudio.com/SonghaySystem/_build/latest?definitionId=9)

Songhay Feeds are `Activities` around downloading Syndication feeds (in RSS or Atom) and converting them into static JSON files. This file format can be used to bind to display component for a “dashboard” view.

Feeds `Activities` are intended to run in the cloud which, as of this writing, is [in Azure](https://docs.microsoft.com/en-us/azure/app-service/web-sites-create-web-jobs). The only clue one would have to assume that running on Azure is possible is the [conventional](https://docs.microsoft.com/en-us/azure/app-service/web-sites-create-web-jobs#acceptablefiles) `run.ps1` [script](./Songhay.Feeds.Shell/run.ps1).

@[BryanWilhite](https://twitter.com/bryanwilhite)