healthCheck=$(curl -u ${SONAR_LOGIN}:${SONAR_PASSWORD} ${SONAR_URI}/api/system/health)
if [[ ${healthCheck} == *"GREEN"* ]]
then
  exit 0
else
  exit 1
fi