NEW_TAG="registry-vpc.cn-hangzhou.aliyuncs.com/cnblogs/$1";
# NEW_TAG="registry.cn-hangzhou.aliyuncs.com/cnblogs/$1";
docker tag $1 $NEW_TAG
docker push $NEW_TAG
docker rmi $NEW_TAG
