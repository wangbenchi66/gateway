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
        }
      }
    }

    stage('k8s') {
      agent none
      steps {
        kubernetesDeploy(enableConfigSubstitution: true, deleteResource: false, kubeconfigId: 'k8s', configs: 'deployment-substituted.yaml', dockerCredentials: [])
      }
    }

  }
}