<#
    Setează IP static 192.168.1.10 pe placa de rețea a serverului (computerul "PC").
    Se rulează O SINGURĂ DATĂ, pe server, în PowerShell deschis ca Administrator.

    Starea găsită la verificare (2026-09-17):
        Ethernet 3 / Realtek PCIe GbE Family Controller #2 / MAC 9C-6B-00-D9-CF-24
        192.168.1.15/24 prin DHCP, gateway 192.168.1.1, DNS 192.168.1.1

    Revenire la DHCP, dacă ceva merge prost:
        Set-NetIPInterface -InterfaceAlias 'Ethernet 3' -Dhcp Enabled
        Set-DnsClientServerAddress -InterfaceAlias 'Ethernet 3' -ResetServerAddresses
        Restart-NetAdapter -InterfaceAlias 'Ethernet 3'
#>

$ErrorActionPreference = 'Stop'

$Mac        = '9C-6B-00-D9-CF-24'
$IpNou      = '192.168.1.10'
$Masca      = 24
$Gateway    = '192.168.1.1'
$Dns        = '192.168.1.1'

# --- 1. trebuie rulat ca Administrator ---
$principal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator))
{
    Write-Host "Deschide PowerShell ca Administrator si ruleaza din nou." -ForegroundColor Red
    exit 1
}

# --- 2. identifică placa după adresa MAC, nu după nume ---
$adaptor = Get-NetAdapter | Where-Object { $_.MacAddress -eq $Mac -and $_.Status -eq 'Up' }
if (-not $adaptor)
{
    Write-Host "Nu am gasit placa de retea cu MAC $Mac activa. Plăcile active:" -ForegroundColor Red
    Get-NetAdapter | Where-Object Status -eq 'Up' | Format-Table Name, MacAddress, LinkSpeed
    exit 1
}
$idx = $adaptor.ifIndex
Write-Host "Placa: $($adaptor.Name) (index $idx, MAC $Mac)" -ForegroundColor Cyan

# --- 3. verifică dacă adresa noua e deja ocupată de alt dispozitiv ---
$ipActual = (Get-NetIPAddress -InterfaceIndex $idx -AddressFamily IPv4).IPAddress
if ($ipActual -ne $IpNou -and (Test-Connection -ComputerName $IpNou -Count 2 -Quiet))
{
    Write-Host "$IpNou raspunde la ping — e folosit de alt dispozitiv. Opresc." -ForegroundColor Red
    exit 1
}

# --- 4. aplică configurația statică ---
Write-Host "Setez $IpNou/$Masca, gateway $Gateway, DNS $Dns ..." -ForegroundColor Cyan
Set-NetIPInterface -InterfaceIndex $idx -Dhcp Disabled
Get-NetIPAddress -InterfaceIndex $idx -AddressFamily IPv4 | Remove-NetIPAddress -Confirm:$false
Get-NetRoute -InterfaceIndex $idx -DestinationPrefix '0.0.0.0/0' -ErrorAction SilentlyContinue |
    Remove-NetRoute -Confirm:$false
New-NetIPAddress -InterfaceIndex $idx -IPAddress $IpNou -PrefixLength $Masca -DefaultGateway $Gateway | Out-Null
Set-DnsClientServerAddress -InterfaceIndex $idx -ServerAddresses $Dns
Clear-DnsClientCache

# --- 5. verificare ---
Start-Sleep -Seconds 3
Write-Host "`n--- Rezultat ---" -ForegroundColor Green
Get-NetIPAddress -InterfaceIndex $idx -AddressFamily IPv4 |
    Select-Object IPAddress, PrefixLength, PrefixOrigin, SuffixOrigin | Format-List
Write-Host "Gateway: $((Get-NetRoute -InterfaceIndex $idx -DestinationPrefix '0.0.0.0/0').NextHop)"
Write-Host "DNS:     $((Get-DnsClientServerAddress -InterfaceIndex $idx -AddressFamily IPv4).ServerAddresses)"

Write-Host "`n--- Test conectivitate ---" -ForegroundColor Green
Write-Host "Gateway  : $(if (Test-Connection $Gateway -Count 2 -Quiet) { 'OK' } else { 'EȘUAT' })"
Write-Host "Internet : $(if (Test-Connection 8.8.8.8 -Count 2 -Quiet) { 'OK' } else { 'EȘUAT' })"
Write-Host "DNS      : $(if (Resolve-DnsName github.com -ErrorAction SilentlyContinue) { 'OK' } else { 'EȘUAT' })"

Write-Host "`nDupa asta, in panoul routerului (192.168.1.1) fa o REZERVARE DHCP" -ForegroundColor Yellow
Write-Host "pentru MAC $Mac -> $IpNou, sau scoate $IpNou din intervalul DHCP," -ForegroundColor Yellow
Write-Host "altfel routerul poate da aceeasi adresa altui dispozitiv (conflict de IP)." -ForegroundColor Yellow
