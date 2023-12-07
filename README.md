# Gateway
网关项目，所有项目通过网关去调用

# docker部署时需要在同一网络内方可访问
## Docker
创建指定网络
``` 
docker network create gateway
```
打包镜像
```
docker build -t gateway .
```
创建容器
```
docker run --network=gateway -d -p 5000:80 --name gateway gateway:latest
```

api：http://api/api

## dotnet
api：http://localhost:7000