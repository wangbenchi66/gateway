#!/bin/bash
###
 # @Author: wangbenchi66 69945864@qq.com
 # @Date: 2024-02-05 16:00:17
 # @LastEditors: wangbenchi66 69945864@qq.com
 # @LastEditTime: 2024-06-15 14:14:21
 # @FilePath: \gateway\docker-compose.sh
 # @Description: 这是默认设置,请设置`customMade`, 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
### 
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
docker login -u admin 121.40.220.126:81 -password=wangbenchi123
docker push 121.40.220.126:81/net_core/gateway:${VERSION}
docker-compose up -d
echo ------------------------------------------
echo 编译成功!
echo ------------------------------------------