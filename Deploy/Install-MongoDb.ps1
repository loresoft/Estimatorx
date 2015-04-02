#Make sure 7za is installed
choco install 7zip.commandline

# Create mongodb and data directory
md c:\mongo\data

# Go to mongodb dir
Push-Location c:\mongo

# Download zipped mongodb binaries to mongodbdir
Invoke-WebRequest https://fastdl.mongodb.org/win32/mongodb-win32-x86_64-2008plus-3.0.1.zip -OutFile mongodb.zip

# Extract mongodb zip
cmd /c 7za e mongodb.zip

# Install mongodb as a windows service
cmd /c c:\mongo\mongod.exe --logpath=c:\mongo\log --dbpath=c:\mongo\data\ --smallfiles --install

# Sleep as a hack to fix an issue where the service sometimes does not finish installing quickly enough
Start-Sleep -Seconds 5

# Start mongodb service
net start mongodb

# Return to last location, to run the build
Pop-Location

Write-Host
Write-Host "monogdb installation complete"