tries=5
timeout=1m
exitLoop=0
i=1

while [ $i -le $tries ] && [ $exitLoop -eq 0 ]
do
  healthCheck=$(curl -u ${SONAR_LOGIN}:${SONAR_PASSWORD} ${SONAR_URI}/api/system/health)
  echo "Try #${i}: ${healthCheck}"
  if [[ ${healthCheck} == *"GREEN"* ]]
  then
    exitLoop=1
  else
    sleep $timeout
    $(( i++ ))
  fi

done

if [ $exitLoop -eq 1 ]
then
  echo "Sonar is ready"
  exit 0
else
  echo "Sonar is still not ready after ${tries} retries"
  exit 1
fi
    