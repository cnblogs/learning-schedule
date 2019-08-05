GIT_COMMIT=$(git rev-parse --short HEAD)
docker build --build-arg GIT_COMMIT=$GIT_COMMIT -t academy-api:$1 -f src/Presentation/Cnblogs.Academy.WebAPI/Dockerfile ./src/Presentation/Cnblogs.Academy.WebAPI
docker build --build-arg GIT_COMMIT=$GIT_COMMIT -t academy-spa:$1 -f src/Presentation/Cnblogs.Academy.SPA/Dockerfile ./src/Presentation/Cnblogs.Academy.SPA
docker build --build-arg GIT_COMMIT=$GIT_COMMIT -t academy-web:$1 -f src/Presentation/Cnblogs.Academy.Web/Dockerfile ./src/Presentation/Cnblogs.Academy.Web
