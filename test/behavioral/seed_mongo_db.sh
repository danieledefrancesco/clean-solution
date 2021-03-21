entity=$(cat ./seeds/$4 | tr -d '\r')
mongoCmd="db.$3.insert($entity)"
docker exec $1 mongo mongodb://localhost:27017/$2 --eval "$mongoCmd"