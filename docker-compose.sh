#!/bin/bash
#current_time=$(date +"%Y%m%d%H%M%S")
git checkout -- .
git pull
git log -n 1
current_time=$(git log -n 1 --pretty=format:%H)
echo ------------------------------------------
echo VERSION=$current_time > .env
echo ------------------------------------------
echo 设置版本:$current_time
echo ------------------------------------------
echo git拉取成功
echo ------------------------------------------
echo 开始执行docker-compose编译
echo ------------------------------------------
docker push 121.40.220.126:81/net_core/gateway:${VERSION}
docker-compose up -d
echo ------------------------------------------
echo 编译成功
echo ------------------------------------------