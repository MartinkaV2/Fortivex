param(
  [int]$Count = 10,
  [int]$DelayMs = 500
)

$uri = 'http://localhost:5105/api/accounts/login'
$body = @{ UserName = 'xyz123'; PasswordHash = 'teszt123' }
$logFile = Join-Path -Path (Get-Location) -ChildPath ("login-stress-$(Get-Date -Format yyyyMMdd-HHmmss).log")

Write-Output "Running $Count login attempts against $uri"
Add-Content -Path $logFile -Value "Login stress test started: $(Get-Date)"

for ($i = 1; $i -le $Count; $i++) {
  $timestamp = Get-Date -Format o
  try {
  $resp = Invoke-RestMethod -Method Post -Uri $uri -Body ($body | ConvertTo-Json) -ContentType 'application/json' -TimeoutSec 10
  $respText = $resp | ConvertTo-Json -Compress
  $line = "[$i] $timestamp OK Response: $respText"
    Write-Output $line
    Add-Content -Path $logFile -Value $line
  } catch {
    $errMsg = $_.Exception.Message
    $line = "[$i] $timestamp ERROR: $errMsg"
    Write-Output $line
    Add-Content -Path $logFile -Value $line
  }
  Start-Sleep -Milliseconds $DelayMs
}

Write-Output "Test complete. Log: $logFile"
Add-Content -Path $logFile -Value "Login stress test ended: $(Get-Date)"
