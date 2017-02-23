param(
    [string] $Wordlist = "ordlista.txt"
)

if (Test-Path $Wordlist) {
    Write-Host "Starting Niancat API with wordlist from $Wordlist"

    & build/Niancat.Api.exe $Wordlist
}
else {
    Write-Warning "Wordlist file $Wordlist does not exist; aborting."
}
