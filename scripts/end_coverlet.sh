pidLine=$(ps -a | grep dotnet)
pidLine=$(echo $pidLine)
pid=${pidLine%% *}
kill $pid