Set-Location $PSScriptRoot

$p = Start-Process dotnet -ArgumentList "Songhay.Feeds.Shell.dll DownloadFeedsActivity" -NoNewWindow -PassThru -Wait

if($p.ExitCode -ne 0) { exit $p.ExitCode }

$p = Start-Process dotnet -ArgumentList "Songhay.Feeds.Shell.dll StoreFeedsActivity" -NoNewWindow -PassThru -Wait

exit $p.ExitCode
