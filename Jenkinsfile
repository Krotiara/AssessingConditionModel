pipeline {
    agent any

    stages {
        stage('Source'){
            steps{
                 checkout([$class: 'GitSCM', branches: [[name: '*/main']], doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], userRemoteConfigs: [[credentialsId: 'b50741b3-08a2-4e79-8078-584180c33c5e', url: 'git@github.com:Krotiara/AssessingConditionModel.git']]])
            }
        }
        stage('Restore packages') {
            steps {
              bat "\"${tool 'MSBuild'}\" /t:Restore AssessingConditionModel.sln"  
            }
        }
        stage('Clean') {
            steps {
                bat "\"${tool 'MSBuild'}\" AssessingConditionModel.sln /nologo /nr:false /p:platform=\"Any CPU\" /p:configuration=\"debug\" /t:clean"
            }
        }
        stage('Build') {
            steps {
               bat "\"${tool 'MSBuild'}\" AssessingConditionModel.sln /p:DeployOnBuild=true /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:SkipInvalidConfigurations=true /t:build /p:Configuration=Debug /p:Platform=\"Any CPU\" /p:DeleteExistingFiles=True /p:publishUrl=c:\\inetpub\\wwwroot"
            }
        }
        stage('Run Unit Tests') {
            steps {
                bat "\"C:/Program Files/dotnet/dotnet.exe\" test \"AssessingConditionModel.sln\""
            }
        }
        stage('Deploy Local') {
            steps {
                bat 'docker info'
                bat 'docker version'
                bat 'docker compose version'
                bat 'docker compose up -d --no-color --wait'
                bat 'docker compose ps'
            }
        }
        stage('Run Integration Tests') {
            steps {
                echo 'Run Integration Tests....'
            }
        }
        stage('Deploy Production') {
            steps {
                echo 'Deploy Production....'
            }
        }
        stage('Run Post Deployment Tests') {
            steps {
                echo 'Run Post Deployment Tests....'
            }
        }
    }
    post{
        always {
          sh "docker-compose down || true"
        }
        success {
          bitbucketStatusNotify buildState: "SUCCESSFUL"
        }
        failure {
          bitbucketStatusNotify buildState: "FAILED"
        }
    }
}
