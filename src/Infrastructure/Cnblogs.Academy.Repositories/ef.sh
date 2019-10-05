#!/bin/bash
dotnet ef --startup-project ../../Presentation/Cnblogs.Academy.WebAPI migrations add $1
