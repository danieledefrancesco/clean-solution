sonar-report \
  --sonarurl=${SONAR_URI} \
  --project=${SONAR_PROJECT_KEY} \
  --sonarcomponent=${SONAR_PROJECT_KEY} \
  --application=${SONAR_PROJECT_KEY} \
  --sinceleakperiod="false" \
  --sonarusername=${SONAR_LOGIN} \
  --sonarpassword=${SONAR_PASSWORD} \
  --allbugs="true" \
  --noSecurityHotspot="true" > ./sonar-report/sonar-report.html