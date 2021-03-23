PROJ_DIR:=$(shell dirname $(realpath $(firstword $(MAKEFILE_LIST))))

install:
	mvn -f $(PROJ_DIR) --batch-mode install dependency:resolve > /dev/null

test:
	mvn -f $(PROJ_DIR) test -Dkarate.options="--tags ~@ignore"