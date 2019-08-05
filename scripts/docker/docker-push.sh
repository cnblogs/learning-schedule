#!/bin/bash
set -e
./scripts/docker/build-project.sh
./scripts/docker/build-image.sh $1
./scripts/docker/push-image.sh academy-web:$1
./scripts/docker/push-image.sh academy-api:$1
./scripts/docker/push-image.sh academy-spa:$1
trash ./src/Presentation/Cnblogs.Academy.Web/publish
