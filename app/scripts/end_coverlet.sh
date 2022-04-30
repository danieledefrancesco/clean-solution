pidLine=$(ps -ax | grep dotnet)
pidLine=$(echo $pidLine)
pid=${pidLine%% *}
echo "pid = $pid"
kill $pid