ls -fi *.dll -r | % { $_.fullname } | % { [reflection.assemblyname]::GetAssemblyName("$_") | fl }
