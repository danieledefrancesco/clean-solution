ps -a
cat coverlet.log
pidLine=$(ps -a | grep dotnet)
pidLine=$(echo $pidLine)
pid=${pidLine%% *}
echo "pid = $pid"
kill $pid