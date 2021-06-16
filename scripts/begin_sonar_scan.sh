dotnet sonarscanner begin \
  /k:${SONAR_PROJECT_KEY} \
  /d:sonar.cs.vstest.reportsPaths=**/*.trx \
  /d:sonar.cs.opencover.reportsPaths=**/*.opencover.xml \
  /d:sonar.login=${SONAR_LOGIN} \
  /d:sonar.password=${SONAR_PASSWORD} \
  /d:sonar.host.url=${SONAR_URI} \
  /d:sonar.qualitygate.wait=true
dotnet build