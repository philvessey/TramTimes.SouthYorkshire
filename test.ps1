param(
    [string]$filter,
    [int]$run
)

Write-Host ""
Write-Host "Building solution ..."

$output = dotnet build 2>&1 | Out-String

if ($output -match "net\d+\.\d+") {
    $framework = $matches[0]
    
    Write-Host "Installing playwright for $framework ..."
    $null = & "TramTimes.Web.Tests/bin/Debug/$framework/playwright.ps1" install 2>&1
}

Write-Host "Running tests ..."
Write-Host ""

$command = "dotnet test --settings test.runsettings"

if ($filter -or $run -gt 0) {
    $parts = @()
    
    if ($filter) {
        $parts += "DisplayName~$filter"
    }
    
    if ($run -gt 0) {
        $parts += "DisplayName~run: $run"
    }
    
    $command += " --filter `"$($parts -join '&')`""
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
    
    $failLines | ForEach-Object { 
        $cleaned = ($_ -replace 'TramTimes\.Web\.Tests\.Pages\.', '' -replace '\[.*?\]', '' -replace '\s{2,}', '').Trim()
        $cleaned = $cleaned -replace '\(.*?, (run: \d+)\)', ' ($1)'
        
        Write-Host $cleaned
    }
    
    Write-Host ""
}

if ($skipLines.Count -gt 0) {
    Write-Host "[SKIPPED]" -ForegroundColor Yellow
    
    $skipLines | ForEach-Object { 
        $cleaned = ($_ -replace 'TramTimes\.Web\.Tests\.Pages\.', '' -replace '\[.*?\]', '' -replace '\s{2,}', '').Trim()
        $cleaned = $cleaned -replace '\(.*?, (run: \d+)\)', ' ($1)'
        
        Write-Host $cleaned
    }
    
    Write-Host ""
}