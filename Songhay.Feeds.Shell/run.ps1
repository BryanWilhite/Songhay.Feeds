trap 
{ 
    Write-Output $_ 
    exit 1 
}

Set-Location $PSScriptRoot

& dotnet Songhay.Feeds.Shell.dll DownloadFeedsActivity
& dotnet Songhay.Feeds.Shell.dll StoreFeedsActivity
