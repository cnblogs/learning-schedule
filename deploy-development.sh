export APP_NAME="academy"
export IMAGE_VERSION="beta"
export NAS="nas-dev"
export REPLICAS=1
docker stack deploy -c scripts/docker/docker-stack-compose.yml --with-registry-auth $APP_NAME

