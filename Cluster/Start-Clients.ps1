#
# Power Shell Script zum Starten der Cluster Fenster
#

while ($true)
{
    "Start Cluster Nodes: s,t:seed nodes, m:monitor f:frontend b,c,d:backend"
    $input = Read-Host "Choose -> "
    switch($input)
    {
        's'
        {
            Start-Process -FilePath PowerShell -ArgumentList @("-Command", "& SeedNode/bin/Debug/SeedNode.exe -p 4053")
        } 
        't'
        {
            Start-Process -FilePath PowerShell -ArgumentList @("-Command", "& SeedNode/bin/Debug/SeedNode.exe -p 4054")
        }

        'm'
        {
            Start-Process -FilePath PowerShell -ArgumentList @("-Command", "& Monitor/bin/Debug/Monitor.exe")
        }

       'f'
        {
            Start-Process -FilePath PowerShell -ArgumentList @("-Command", "& Frontend/bin/Debug/Frontend.exe")
        }

        'b'
        {
            Start-Process -FilePath PowerShell -ArgumentList @("-Command", "& Backend/bin/Debug/Backend.exe -p 4056")
        }
        'c'
        {
            Start-Process -FilePath PowerShell -ArgumentList @("-Command", "& Backend/bin/Debug/Backend.exe -p 4057")
        }
        'd'
        {
            Start-Process -FilePath PowerShell -ArgumentList @("-Command", "& Backend/bin/Debug/Backend.exe -p 4058")
        }
    }
}
