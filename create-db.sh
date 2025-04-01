#!/usr/bin/env bash

dotnet tool install --global dotnet-ef
export PATH="$PATH:/home/jare/.dotnet/tools"
dotnet ef migrations add InitialCreate
dotnet ef database update
