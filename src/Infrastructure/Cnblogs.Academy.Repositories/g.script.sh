#!/bin/bash
now=$(date '+%Y%m%d%H%M')
dotnet ef --startup-project ../../Presentation/Cnblogs.Academy.WebAPI migrations script $1 -o ../../../scripts/sql/$now.sql
