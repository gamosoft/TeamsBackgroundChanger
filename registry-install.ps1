# Set variables to indicate value and key to set
$RegistryPath = 'Registry::HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Kind.Picture\shell\assign-teams-background'
$CommandPath  = 'Registry::HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Kind.Picture\shell\assign-teams-background\command'
$Label        = 'Assign as Teams Background'
$IconKey      = 'Icon'
############################################
# Old Teams => C:\Users\[username]\AppData\Roaming\Microsoft\Teams\Backgrounds\Uploads
# $UploadsPath  = '$env:APPDATA\Microsoft\Teams\Backgrounds\Uploads'
# $FileName     = 'background-changer'
$IconValue    = '%USERPROFILE%\AppData\Local\Microsoft\Teams\app.ico'
############################################
# New Teams => C:\Users\[username]\AppData\Local\Packages\MSTeams_8wekyb3d8bbwe\LocalCache\Microsoft\MSTeams\Backgrounds\Uploads
$UploadsPath  = '$env:LOCALAPPDATA\Packages\MSTeams_8wekyb3d8bbwe\LocalCache\Microsoft\MSTeams\Backgrounds\Uploads'
$FileName     = '4d27205e-f3de-4f2d-becf-ed940bb5b979'
# $IconValue    = 'C:\Program Files\WindowsApps\MSTeams_23202.1507.2283.7280_x64__8wekyb3d8bbwe\Images\TeamsForWorkNewBadgeLogo.scale-200.png'
############################################
$CommandValue = 'PowerShell.exe -NoLogo -NoProfile -WindowStyle Hidden -Command "Copy-Item ''%1'' ' + $UploadsPath + '\' + $FileName + '.png; Copy-Item ''%1'' '+ $UploadsPath +'\' + $FileName + '_thumb.png;"'

# Create the keys if they don't exist
If (-NOT (Test-Path $RegistryPath)) {
    New-Item -Path $RegistryPath -Force | Out-Null
}
If (-NOT (Test-Path $CommandPath)) {
    New-Item -Path $CommandPath -Force | Out-Null
}

# Now set/update the values
Set-ItemProperty -Path $RegistryPath -Name '(Default)' -Value $Label -Force
Set-ItemProperty -Path $RegistryPath -Name $IconKey -Value $IconValue -Force
Set-ItemProperty -Path $CommandPath -Name '(Default)' -Value $CommandValue -Force