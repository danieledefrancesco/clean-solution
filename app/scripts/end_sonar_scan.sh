reportgenerator "-reports:test-results/**/*.opencover.xml" "-targetdir:test-report"
dotnet sonarscanner end /d:sonar.login=${SONAR_LOGIN} /d:sonar.password=${SONAR_PASSWORD}
exitStatus=$?
sh ./scripts/get_sonar_report.sh
echo "Exit status is $exitStatus"
exit $exitStatus