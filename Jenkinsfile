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
                    sh 'ls'
                    sh 'docker info'
                }
            }
        }
    }
}