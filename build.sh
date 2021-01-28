#!/bin/bash

#Temp fix
docker run --rm -it -v /home/jody/EmpTS:/EmpTS mcr.microsoft.com/dotnet/sdk:5.0-focal dotnet build /EmpTS/
