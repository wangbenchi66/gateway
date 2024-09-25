pipeline {
  agent {
    node {
      label 'maven'
    }

  }
  stages {
    stage('docker build') {
      agent none
      steps {
        container('maven') {
          sh 'env'
          sh 'docker build -t $JOB_BASE_NAME:$BUILD_ID .'
          sh 'ls -l .'
          sh 'envsubst < deployment.yaml > deployment-substituted.yaml'
          sh 'cat deployment-substituted.yaml'
          sh 'kubectl apply -f deployment-substituted.yaml'
        }
      }
    }
  }
}