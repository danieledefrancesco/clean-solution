sleep 180s
catch=$(curl --include --request POST -u ${SONAR_LOGIN}:${SONAR_PASSWORD} "${SONAR_URI}/api/projects/create?project=${SONAR_PROJECT_KEY}&name=${SONAR_PROJECT_NAME}" 2>&1)