Set-Location $PSScriptRoot

& dotnet Songhay.Feeds.Shell.dll DownloadFeedsActivity
& dotnet Songhay.Feeds.Shell.dll StoreFeedsActivity
