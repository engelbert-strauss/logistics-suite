$Version = "v0.3.0"

$ComposeFile = Get-Content -Path "./docker-compose-template.yml" -Encoding UTF8
$ComposeFile = $ComposeFile.Replace("#{VERSION}#", $Version)
Set-Content -Path "./docker-compose.yml" -Value $ComposeFile -Encoding UTF8
docker-compose.exe build --parallel
