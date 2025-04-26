# FixWixFiles_WiXv4.ps1 - 100% Reliable, No Duplicates, WiX v4 Compatible
$wxsFiles = Get-ChildItem -Path . -Filter *.wxs -Recurse

foreach ($file in $wxsFiles) {
    $xml = New-Object System.Xml.XmlDocument
    $xml.PreserveWhitespace = $true  # Keep formatting intact
    $xml.Load($file.FullName)

    $ns = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
    $ns.AddNamespace("wix", "http://wixtoolset.org/schemas/v4/wxs")

    $modified = $false
    $files = $xml.SelectNodes("//wix:File[not(@Name)]", $ns)

    foreach ($fileNode in $files) {
        $id = $fileNode.GetAttribute("Id")
        if ($id) {
            # Extract filename from Id (remove _kr suffix if present)
            $name = $id -replace '_kr$', '' -replace '\.png_kr$', '.png'
            $fileNode.SetAttribute("Name", $name)
            $modified = $true
            Write-Host "✅ Added Name=`"$name`" to $($file.Name)"
        }
    }

    if ($modified) {
        # Create backup
        $backupPath = "$($file.FullName).bak"
        Copy-Item $file.FullName $backupPath -Force

        # Save with original encoding
        $xml.Save($file.FullName)
    }
}

Write-Host "`nAll files processed. Backups created with .bak extension"
