$versions = Get-Content $args[0]

ForEach ($version in $versions) {
	$app = Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -match "My_App_Prefix.*"+$version }
	echo "Removing: $app"
	$app.Uninstall()
}
