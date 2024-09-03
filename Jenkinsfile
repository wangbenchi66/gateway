pipeline {
    agent any

    environment {
        // 获取当前的 Git 提交哈希
        GIT_COMMIT = "${env.GIT_COMMIT}"
    }

    stages {
        stage('Checkout') {
            steps {
                // 从 Gitee 仓库检出代码
                git credentialsId: 'a6c7625f-1524-4b80-8251-e0d37d5b4dfb', url: 'https://gitee.com/wangbenchi66/gateway.git'
                // 打印当前的 Git 提交哈希
                sh 'echo ${GIT_COMMIT}'
            }
        }
        stage('Prepare Environment') {
            steps {
                script {
                    // 将当前的 Git 提交哈希写入 .env 文件
                    writeFile file: '.env', text: "GIT_COMMIT=${env.GIT_COMMIT}"
                }
                // 启动 Docker 容器
                sh 'docker-compose up -d'
            }
        }

        stage('Clean Up Old Images') {
            steps {
                // 删除旧的 Docker 镜像
                sh 'docker rmi gateway:${GIT_PREVIOUS_COMMIT}'
            }
        }

        stage('Docker Login') {
            steps {
                // 登录到 Docker 仓库
                sh 'docker login -u admin 121.40.220.126:81 --password=wangbenchi123'
            }
        }

        stage('Build Docker Image') {
            steps {
                // 构建新的 Docker 镜像
                sh 'docker build -t gateway:${GIT_COMMIT} .'
            }
        }

        stage('Tag Docker Image') {
            steps {
                // 为 Docker 镜像打标签
                sh 'docker tag gateway:${GIT_COMMIT} 121.40.220.126:81/net_core/gateway:${GIT_COMMIT}'
            }
        }

        stage('Push Docker Image') {
            steps {
                // 推送 Docker 镜像到仓库
                sh 'docker push 121.40.220.126:81/net_core/gateway:${GIT_COMMIT}'
            }
        }

        stage('Deploy') {
            steps {
                // 部署应用
                sshPublisher(publishers: [sshPublisherDesc(configName: '121.40.220.126', transfers: [sshTransfer(
                    cleanRemote: false, 
                    excludes: '', 
                    execCommand: '''
                        cd /www/wwwroot/jenkins
                        sed -i 's/\\GIT_COMMIT/${GIT_COMMIT}/g' docker-ssh.yml
                        docker-compose -f docker-ssh.yml up -d
                    ''', 
                    execTimeout: 120000, 
                    flatten: false, 
                    makeEmptyDirs: false, 
                    noDefaultExcludes: false, 
                    patternSeparator: '[, ]+', 
                    remoteDirectory: '/www/wwwroot/jenkins', 
                    remoteDirectorySDF: false, 
                    removePrefix: '', 
                    sourceFiles: 'docker-ssh.yml'
                )], usePromotionTimestamp: false, useWorkspaceInPromotion: false, verbose: false)])
            }
        }
    }
}