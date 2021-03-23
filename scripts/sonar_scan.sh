dotnet sonarscanner begin \
  /k:${SONAR_PROJECT_KEY} \
  /d:sonar.cs.vstest.reportsPaths=**/*.trx \
  /d:sonar.cs.opencover.reportsPaths=**/*.opencover.xml \
  /d:sonar.login=${SONAR_LOGIN} \
  /d:sonar.password=${SONAR_PASSWORD} \
  /d:sonar.host.url=${SONAR_URI} \
  /d:sonar.qualitygate.wait=true
dotnet build
sh ./scripts/test_runner.sh unit
dotnet sonarscanner end /d:sonar.login=${SONAR_LOGIN} /d:sonar.password=${SONAR_PASSWORD}
exitStatus=$?
sh ./scripts/get_sonar_report.sh
echo "Exit status is $exitStatus"
exit $exitStatus

