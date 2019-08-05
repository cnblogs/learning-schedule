export APP_NAME="academy"
export IMAGE_VERSION=${1:-latest}
export NAS="nas"
export REPLICAS=2
docker stack deploy -c scripts/docker/docker-stack-compose.yml --with-registry-auth $APP_NAME
