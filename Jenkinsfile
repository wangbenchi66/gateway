pipeline {
    agent any

    environment {
        REGISTRY_URL = '121.40.220.126:81' // Docker 注册表的 URL
        GIT_URL = 'https://gitee.com/wangbenchi66/gateway.git' // Git 仓库的 URL
        GIT_CREDENTIALS_ID = 'a6c7625f-1524-4b80-8251-e0d37d5b4dfb' // Jenkins 中存储的 Git 凭证 ID
        IMAGE_TAG = "${env.GIT_COMMIT}"// 镜像标签
        GIT_PREVIOUS_COMMIT = "${env.GIT_PREVIOUS_COMMIT}"// 上一次提交的镜像标签
        DOCKER_IMAGE_NAME = 'gateway' // Docker 镜像的名称
        DOCKER_REPO = 'net_core' // Docker 仓库的名称
        DEPLOYMENT_FILE = 'docker-ssh.yml' // 部署使用的 Docker Compose 文件
        DEPLOYMENT_SERVICE = " "//docker-compose发布选项(如果有则只发布指定服务)
        PROJECT_NAME = 'gateway' // docker-compose项目名称
    }

    stages {
        stage('Set Environment Variables') {
            steps {
                script {
                    // // 获取 Git 分支名称
                    // def branchName = bat(returnStdout: true, script: 'git rev-parse --abbrev-ref HEAD').trim()
                    // // 获取 Git 提交短哈希
                    // def gitCommit = bat(returnStdout: true, script: 'git rev-parse --short HEAD').trim()

                    // // 合并为镜像标签
                    // IMAGE_TAG = "${branchName}-${gitCommit}"

                    // 输出镜像标签
                    echo "IMAGE_TAG: ${IMAGE_TAG}"
                    echo "GIT_PREVIOUS_COMMIT: ${GIT_PREVIOUS_COMMIT}"
                }
            }
         }
        
        stage('Build Docker Image') {
            steps {
                script {
                    // 使用 Docker 注册表的凭证登录并构建、标记和推送 Docker 镜像
                    sh """
                        docker login -u admin -p wangbenchi123 ${REGISTRY_URL}
                        docker build -t ${DOCKER_IMAGE_NAME}:${IMAGE_TAG} .
                    """
                }
            }
        }
    }
}