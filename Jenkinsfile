pipeline {
    agent any

    environment {
        REGISTRY_URL = '121.40.220.126:81'
        GIT_URL = 'https://gitee.com/wangbenchi66/gateway.git'
        GIT_CREDENTIALS_ID = 'a6c7625f-1524-4b80-8251-e0d37d5b4dfb'
        //IMAGE_TAG = "${env.CICD_GIT_BRANCH}-${env.CICD_GIT_COMMIT}-${env.CICD_EXECUTION_SEQUENCE}"
        IMAGE_TAG = "${env.GIT_COMMIT}"
    }

    stages {
        stage('Checkout') {
            steps {
                git credentialsId: "${GIT_CREDENTIALS_ID}", url: "${GIT_URL}"
            }
        }
        
        // stage('Set Environment Variables') {
        //     steps {
        //         script {
        //             // 获取 Git 分支名
        //             // def branchName = sh(returnStdout: true, script: 'git rev-parse --abbrev-ref HEAD').trim()

        //             // // 获取 Git 提交短哈希
        //             // def gitCommit = sh(returnStdout: true, script: 'git rev-parse --short HEAD').trim()

        //             // // 合并为镜像标签
        //             // IMAGE_TAG = branchName+'-'+gitCommit
        //         }
        //     }
        // }
        
        stage('Build Docker Image') {
            steps {
                script {
                    // 使用 Docker 注册表的凭证登录并构建、标记和推送 Docker 镜像
                    bat """
                        docker login -u admin -p wangbenchi123 ${REGISTRY_URL}
                        docker build -t gateway:${IMAGE_TAG} .
                        docker tag gateway:${IMAGE_TAG} ${REGISTRY_URL}/net_core/gateway:${IMAGE_TAG}
                        docker push ${REGISTRY_URL}/net_core/gateway:${IMAGE_TAG}
                    """
                }
            }
        }

        
        stage('Deploy to ssh') {
            steps {
                script {
                    def deploymentScript = """
                        cd /www/wwwroot/jenkins
                        sed -i 's/\\GIT_COMMIT/${IMAGE_TAG}/g' docker-ssh.yml
                        docker-compose -f docker-ssh.yml up -d
                    """

                    sshPublisher(
                        publishers: [
                            sshPublisherDesc(
                                configName: 'ssh',
                                transfers: [
                                    sshTransfer(
                                        execCommand: deploymentScript,
                                        execTimeout: 120000,
                                        flatten: false,
                                        makeEmptyDirs: false,
                                        noDefaultExcludes: false,
                                        patternSeparator: '[, ]+',
                                        remoteDirectory: '/www/wwwroot/jenkins', 
                                        sourceFiles: 'docker-ssh.yml'
                                    )
                                ],
                                usePromotionTimestamp: false,
                                useWorkspaceInPromotion: false,
                                verbose: true
                            )
                        ]
                    )
                }
            }
        }
    }
}