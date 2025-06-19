Write-Host "`e[?25l" -NoNewline
Write-Host ""

Write-Host "Running tests..." -ForegroundColor Yellow
Write-Host ""

$output = dotnet test | Out-String

if ($output -match "Failed:\s*(\d+), Passed:\s*(\d+), Skipped:\s*(\d+), Total:\s*(\d+)") {
    $failed, $passed, $skipped, $total = $matches[1], $matches[2], $matches[3], $matches[4]
    $totalColor = if ([int]$failed -eq 0) { 'Green' } else { 'Red' }
    
    Write-Host ("{0,-8} {1,5}" -f 'Passed', $passed) -ForegroundColor Green
    Write-Host ("{0,-8} {1,5}" -f 'Failed', $failed) -ForegroundColor Red
    Write-Host ("{0,-8} {1,5}" -f 'Skipped', $skipped) -ForegroundColor Yellow
    Write-Host ("{0,-8} {1,5}" -f 'Total', $total) -ForegroundColor $totalColor
} else {
    Write-Host "`e[1A`e[2K" -NoNewline
    Write-Host "Test Results Missing!" -ForegroundColor Red
}

Write-Host "`e[?25h" -NoNewline
Write-Host ""