param(
    [string]$filter
)

Write-Host ""
Write-Host "Running tests..."
Write-Host ""

$command = "dotnet test --settings test.runsettings"

if ($filter) {
    $command += " --filter `"$filter`""
}

$output = Invoke-Expression "$command 2>&1" | Out-String
$allLines = [System.Text.RegularExpressions.Regex]::Split($output, "\r?\n")
$failLines = $allLines | Where-Object { $_ -match '\[FAIL\]' }
$skipLines = $allLines | Where-Object { $_ -match '\[SKIP\]' }
$otherLines = $allLines | Where-Object { $_ -notmatch '\[FAIL\]' -and $_ -notmatch '\[SKIP\]' }

$output = $otherLines -join "`n"

if ($output -match "Failed:\s*(\d+), Passed:\s*(\d+), Skipped:\s*(\d+), Total:\s*(\d+)") {
    $failed, $passed, $skipped, $total = $matches[1], $matches[2], $matches[3], $matches[4]
    
    Write-Host ("{0,-8} {1,5}" -f 'Passed', $passed) -ForegroundColor Green
    Write-Host ("{0,-8} {1,5}" -f 'Failed', $failed) -ForegroundColor Red
    Write-Host ("{0,-8} {1,5}" -f 'Skipped', $skipped) -ForegroundColor Yellow
    Write-Host ("{0,-8} {1,5}" -f 'Total', $total)
    Write-Host ""
} else {
    Write-Host "Test Results Missing!"
    Write-Host ""
}

if ($failLines.Count -gt 0) {
    Write-Host "[FAILED]" -ForegroundColor Red
    $failLines | ForEach-Object { Write-Host ($_ -replace 'TramTimes\.Web\.Tests\.Pages\.', '') }
    Write-Host ""
}

if ($skipLines.Count -gt 0) {
    Write-Host "[SKIPPED]" -ForegroundColor Yellow
    $skipLines | ForEach-Object { Write-Host ($_ -replace 'TramTimes\.Web\.Tests\.Pages\.', '') }
    Write-Host ""
}