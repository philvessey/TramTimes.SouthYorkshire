param(
    [string]$filter,
    [int]$run
)

function Clear-Lines {
    param([int]$count)

    for ($i = 0; $i -lt $count; $i++) {
        Write-Host "`e[1A`e[2K" -NoNewline
    }
}

function Write-Lines {
    param([int]$count)

    for ($i = 0; $i -lt $count; $i++) {
        Write-Host ""
    }
}

Write-Lines -count 1
Write-Host "Checking nuget ..."

if (-not (Test-Path "nuget.config")) {
    Write-Lines -count 1
    Write-Host "NuGet Configuration Missing!" -ForegroundColor Red
    Write-Host "Please ensure you have a valid nuget.config file in the repository root." -ForegroundColor Yellow
    Write-Lines -count 1

    exit 1
}

Write-Host "Checking telerik ..."

if (-not (Test-Path "telerik-license.txt")) {
    Write-Lines -count 1
    Write-Host "Telerik License Missing!" -ForegroundColor Red
    Write-Host "Please ensure you have a valid telerik-license.txt file in the repository root." -ForegroundColor Yellow
    Write-Lines -count 1

    exit 1
}

Write-Host "Checking secrets ..."

$requiredSecrets = @(
    "Parameters:transxchange-username",
    "Parameters:transxchange-userpass"
)

$foundSecrets = dotnet user-secrets list --project TramTimes.Aspire.Host 2>&1 | Out-String

$missingSecrets = @()
foreach ($secret in $requiredSecrets) {
    if ($foundSecrets -notmatch [regex]::Escape($secret)) {
        $missingSecrets += $secret
    }
}

if ($missingSecrets.Count -gt 0) {
    Write-Lines -count 1
    Write-Host "User Secrets Missing!" -ForegroundColor Red

    foreach ($secret in $missingSecrets) {
        Write-Host "dotnet user-secrets set `"$secret`" `"your-value`" --project TramTimes.Aspire.Host" -ForegroundColor Yellow
    }

    Write-Lines -count 1

    exit 1
}

Clear-Lines -count 3
Write-Host "Building solution ..."

$output = dotnet build 2>&1 | Out-String

if ($output -match "net\d+\.\d+") {
    $framework = $matches[0]

    Write-Host "Building playwright ..."
    & "TramTimes.Web.Tests/bin/Debug/$framework/playwright.ps1" install 2>&1 | Out-Null
}

Clear-Lines -count 2
Write-Host "Running tests ..."
Write-Lines -count 1

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
    Write-Lines -count 1
} else {
    Write-Host "Test Results Missing!" -ForegroundColor Red
    Write-Lines -count 1
}

if ($failLines.Count -gt 0) {
    Write-Host "[FAILED]" -ForegroundColor Red

    $failLines | ForEach-Object {
        $cleaned = ($_ -replace 'TramTimes\.Web\.Tests\.Pages\.', '' -replace '\[.*?\]', '' -replace '\s{2,}', '').Trim()
        $cleaned = $cleaned -replace '\(.*?, (run: \d+)\)', ' ($1)'

        Write-Host $cleaned
    }

    Write-Lines -count 1
}

if ($skipLines.Count -gt 0) {
    Write-Host "[SKIPPED]" -ForegroundColor Yellow

    $skipLines | ForEach-Object {
        $cleaned = ($_ -replace 'TramTimes\.Web\.Tests\.Pages\.', '' -replace '\[.*?\]', '' -replace '\s{2,}', '').Trim()
        $cleaned = $cleaned -replace '\(.*?, (run: \d+)\)', ' ($1)'

        Write-Host $cleaned
    }

    Write-Lines -count 1
}

exit 0