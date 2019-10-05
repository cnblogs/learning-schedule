#!/bin/bash
dotnet ef --startup-project ../../Presentation/Cnblogs.Academy.WebAPI database update $1
