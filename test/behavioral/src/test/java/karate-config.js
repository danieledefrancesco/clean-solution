function fn() {
    var config = {
        baseUrl: java.lang.System.getenv('SUT_BASE_URL')
    };
    karate.log("Karate config is: ", JSON.stringify(config));

    return config;
}

function execCommand(command)
{
    karate.log("Executing command "+command);
    var ris = karate.exec(command);
    karate.log("Result is " + ris);
    return ris;
}

function execDockerCommand(containerName, command) {
    var cmd = "docker exec " + containerName + " " + command;
    return execCommand(cmd);  
}

function execMongoDbCommand(database, mongoDbCommand) {
    var mongoDbContainerName = java.lang.System.getenv('MONGODB_CONTAINER_NAME');
    var command = "mongo mongodb://localhost:27017/" + database + " --eval " + mongoDbCommand + "";
    return execDockerCommand(mongoDbContainerName, command);
}

function clearProductDatabase() {
    execMongoDbCommand("product-service", "db.product.remove({})");
}

function seed(db, table, entityPath)
{
    var mongoDbContainerName = java.lang.System.getenv('MONGODB_CONTAINER_NAME');
    execCommand("/bin/sh ./seed_mongo_db.sh " + mongoDbContainerName + " " + db + " " + table + " " + entityPath);
}

function getCountById(db, table, id)
{
    var mongoDbContainerName = java.lang.System.getenv('MONGODB_CONTAINER_NAME');
    return execCommand("/bin/sh ./get_mongo_db_count_by_id.sh " + mongoDbContainerName + " " + db + " " + table + " " + id).trim();
}

function addProduct(product) {
    seed("product-service", "product", product);
}

function getDbProductCount(productId){
    return getCountById("product-service", "product", productId);
}
