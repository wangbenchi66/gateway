pipeline {
    agent {
        node {
        label 'maven'
        }
    }

    stages {
        stage('Build Docker Image') {
            steps {
                container('maven') {
                    sh 'docker build -f Dockerfile -t gateway-$BRANCH_NAME-$BUILD_NUMBER .'
                }
            }
        }
    }
}