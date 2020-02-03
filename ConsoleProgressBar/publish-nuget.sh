#!/bin/bash -e

read -r VERSION < ./version
CONFIGURATION=Release
PROJECTDIR=`pwd`
#FEED=https://private.com/nuget/
NAME=AaronLuna.ConsoleProgressBar

echo $VERSION
dotnet restore --source https://api.nuget.org/v3/index.json
dotnet build -c $CONFIGURATION
dotnet pack -p:PackageVersion=$VERSION --no-build --configuration $CONFIGURATION --output $PROJECTDIR/bin/$CONFIGURATION 

# nuget push -ApiKey $NUGET_API_KEY $PROJECTDIR/bin/$CONFIGURATION/$NAME.$VERSION.nupkg  -Source $FEED 



