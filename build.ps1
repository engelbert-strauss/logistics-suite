[CmdletBinding()]
Param(
    [Parameter(Position = 0, Mandatory = $false, ValueFromRemainingArguments = $true)]
    [string[]]$ScriptArgs
)

$CakeVersion = "0.33.0"

If (Get-Command "dotnet-cake.exe" -ErrorAction SilentlyContinue) {
    # Execute Cake
    dotnet-cake $ScriptArgs
} Else {
     # Create the tools folder.
     $Tools = Join-Path $PSScriptRoot "tools"
     if (!(Test-Path $Tools)) {
         New-Item -Path $Tools -ItemType Directory | Out-Null
     }

     # Install Cake
     &dotnet "tool" "install" "Cake.Tool" "--version=$CakeVersion" "--tool-path=$Tools"

     # Execute Cake
     &"$Tools\dotnet-cake" $ScriptArgs
}

# Return the exit code from Cake.
Exit $LASTEXITCODE;
