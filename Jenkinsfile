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
                    sh 'docker build -f Dockerfile -t $REGISTRY/$DOCKERHUB_NAMESPACE/$APP_NAME:SNAPSHOT-$BRANCH_NAME-$BUILD_NUMBER .'
                }
            }
        }
    }
}