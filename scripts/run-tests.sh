#!/bin/bash

set -e

projects=( \
    "src/NModbus.Tests/NModbus.Tests.csproj", \
    "src/NModbus.BasicServer.Tests/NModbus.BasicServer.Tests.csproj", \
    "src/NModbus.Transport.IP.Tests/NModbus.Transport.IP.Tests.csproj")

versions=("net8.0", "net9.0", "net10.0")

for project in $projects; do
    for version in $versions; do 
        dotnet run --project $project --framework $version
    done
done

