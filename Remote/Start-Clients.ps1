#
# Power Shell Script zum Starten der 4 Client-Fenster
#

for ($port = 8081; $port -le 8084; $port++) {
	Start-Process -FilePath PowerShell -ArgumentList @("-Command", "& DeployActors/bin/Release/DeployActors.exe -p $port")
}
