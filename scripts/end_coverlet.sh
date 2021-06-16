psOutput=$(ps -aux)
echo "psOutput = $psOutput"
pidLine=$(ps -aux | grep dotnet)
pidLine=$(echo $pidLine)
pid=${pidLine%% *}
echo "pid = $pid"
kill $pid