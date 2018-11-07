$job =
@{
    Name = "job-feeds";
    SubscriptionName = "Songhay System";
    WebSiteName = "songhay-system";
    ZipFile = "$env:AGENT_RELEASEDIRECTORY\$env:BUILD_DEFINITIONNAME\drop\Songhay.Feeds.Shell.zip";
}

Get-AzureSubscription -SubscriptionName $job.SubscriptionName
Select-AzureSubscription -SubscriptionName $job.SubscriptionName

New-AzureWebsiteJob `
    -Name $job.WebSiteName `
    -JobName $job.Name `
    -JobType Triggered `
    -JobFile $job.ZipFile
