mongoCmd="db.$3.count({\"_id\" : \"$4\"})"
docker exec $1 mongo mongodb://localhost:27017/$2 --eval "$mongoCmd" | tail -1