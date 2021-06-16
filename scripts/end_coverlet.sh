psOutput=$(ps -a)
echo "psOutput = $psOutput"
coverletLog=$(cat coverlet.log)
echo "coverletLog = $coverletLog"
pidLine=$(ps -a | grep dotnet)
pidLine=$(echo $pidLine)
pid=${pidLine%% *}
echo "pid = $pid"
kill $pid