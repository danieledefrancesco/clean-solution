echo "Starting create sonar project"
/bin/bash /scripts/wait_sonar_is_green.sh
catch=$(curl --include --request POST -u ${SONAR_LOGIN}:${SONAR_PASSWORD} "${SONAR_URI}/api/projects/create?project=${SONAR_PROJECT_KEY}&name=${SONAR_PROJECT_NAME}" 2>&1)