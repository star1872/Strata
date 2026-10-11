# START-HERE.bat: the official 64-bit Python 3.12 installer, from mirrors in mainland China first and python.org last.
# From China python.org is often unusable: on 2026-10-07 (a rented cloud PC, Windows 10) winget's download from it
# stalled at 11 MB for over ten minutes, while both mirrors below gave the same 25.7 MB file in 1-3 s.
# A download is kept only when it carries the Python Software Foundation's valid Authenticode signature.
param([Parameter(Mandatory = $true)][string]$Out)
$ProgressPreference = 'SilentlyContinue'
[Net.ServicePointManager]::SecurityProtocol = 'Tls12'
$name = 'python-3.12.10-amd64.exe'
$urls = @("https://registry.npmmirror.com/-/binary/python/3.12.10/$name",
          "https://mirrors.huaweicloud.com/python/3.12.10/$name",
          "https://www.python.org/ftp/python/3.12.10/$name")
foreach ($u in $urls) {
    Remove-Item $Out -ErrorAction SilentlyContinue
    Write-Host "  $u"
    try { Invoke-WebRequest -UseBasicParsing -TimeoutSec 120 $u -OutFile $Out }
    catch { Write-Host "    failed: $($_.Exception.Message)"; continue }
    $sig = Get-AuthenticodeSignature $Out
    if ($sig.Status -eq 'Valid' -and $sig.SignerCertificate.Subject -like '*O=Python Software Foundation*') { exit 0 }
    Write-Host "    not a signed Python installer, skipped"
}
Remove-Item $Out -ErrorAction SilentlyContinue
exit 1
