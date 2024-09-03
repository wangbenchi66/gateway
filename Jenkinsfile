pipeline {
    agent any

    environment {
        REGISTRY_URL = '121.40.220.126:81' // Docker 注册表的 URL
        GIT_URL = 'https://gitee.com/wangbenchi66/gateway.git' // Git 仓库的 URL
        GIT_CREDENTIALS_ID = 'a6c7625f-1524-4b80-8251-e0d37d5b4dfb' // Jenkins 中存储的 Git 凭证 ID
        //IMAGE_TAG = "" // Docker 镜像的标签，使用 Git 提交哈希
        DOCKER_IMAGE_NAME = 'gateway' // Docker 镜像的名称
        DOCKER_REPO = 'net_core' // Docker 仓库的名称
        DEPLOYMENT_FILE = 'docker-ssh.yml' // 部署使用的 Docker Compose 文件
    }

    stages {
        stage('Checkout') {
            steps {
                git credentialsId: "${GIT_CREDENTIALS_ID}", url: "${GIT_URL}"
            }
        }
        
        stage('Set Environment Variables') {
            steps {
                script {
                    // 获取 Git 提交短哈希
                    def gitCommit = bat(returnStdout: true, script: 'git rev-parse --short HEAD').trim()

                    // 合并为镜像标签
                    env.IMAGE_TAG = "${env.BRANCH_NAME}-${gitCommit}"
                    //输出镜像标签
                    echo "IMAGE_TAG: ${env.IMAGE_TAG}"
                }
            }
         }
        
        stage('Build Docker Image') {
            steps {
                script {
                    // 使用 Docker 注册表的凭证登录并构建、标记和推送 Docker 镜像
                    bat """
                        docker login -u admin -p wangbenchi123 ${REGISTRY_URL}
                        docker build -t ${DOCKER_IMAGE_NAME}:${env.IMAGE_TAG} .
                        docker tag ${DOCKER_IMAGE_NAME}:${env.IMAGE_TAG} ${REGISTRY_URL}/${DOCKER_REPO}/${DOCKER_IMAGE_NAME}:${env.IMAGE_TAG}
                        docker push ${REGISTRY_URL}/${DOCKER_REPO}/${DOCKER_IMAGE_NAME}:${env.IMAGE_TAG}
                    """
                }
            }
        }

        
        stage('Deploy to ssh') {
            steps {
                script {
                    def deploymentScript = """
                        cd /www/wwwroot/jenkins
                        sed -i 's/\\GIT_COMMIT/${env.IMAGE_TAG}/g' ${DEPLOYMENT_FILE}
                        docker-compose -f ${DEPLOYMENT_FILE} up -d
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
                                        sourceFiles: "${DEPLOYMENT_FILE}"
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