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
        stage('Test') {
            steps {
                bat "\"C:/Program Files/dotnet/dotnet.exe\" test \"AssessingConditionModel.sln\""
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploying....'
            }
        }
    }
}
