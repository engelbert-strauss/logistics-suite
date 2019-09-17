[CmdletBinding()]
Param(
    [Parameter(Position = 0, Mandatory = $false, ValueFromRemainingArguments = $true)]
    [string[]]$ScriptArgs
)

If (Get-Command "dotnet-cake.exe" -ErrorAction SilentlyContinue) {
    # Execute Cake
    dotnet-cake $ScriptArgs
} Else {
    Write-Host "Warning: You need to install the dotnet tool ""Cake.Tool"" gobally."
}

# Return the exit code from Cake.
Exit $LASTEXITCODE;
