begin_sonar_scan:
	dotnet sonarscanner begin \
		/k:${SONAR_PROJECT_KEY} \
		/d:sonar.cs.vstest.reportsPaths=**/*.trx \
		/d:sonar.cs.opencover.reportsPaths=**/*.opencover.xml \
		/d:sonar.login=${SONAR_LOGIN} \
		/d:sonar.password=${SONAR_PASSWORD} \
		/d:sonar.host.url=${SONAR_URI} \
		/d:sonar.qualitygate.wait=true
	dotnet build

end_coverlet:
	kill $(shell ps -ax | grep dotnet | grep -v defunct | grep -v coverlet | grep -v grep | awk '{ print $$1 }')

end_sonar_scan:
	reportgenerator "-reports:test-results/**/*.opencover.xml" "-targetdir:test-report"
	dotnet sonarscanner end /d:sonar.login=${SONAR_LOGIN} /d:sonar.password=${SONAR_PASSWORD}

run_unit_tests:
	dotnet test --filter FullyQualifiedName!~.Specs --settings settings/coverlet.runsettings --logger trx --results-directory "test-results"
	reportgenerator "-reports:test-results/**/*.opencover.xml" "-targetdir:test-report"

MIFGRATION_NAME?=DEFAULT_MIGRATION
add_migration:
	dotnet ef migrations add $(MIGRATION_NAME) --project ./src/AspNetCore.Examples.ProductService.Infrastructure/AspNetCore.Examples.ProductService.Infrastructure.csproj

update_database:
	dotnet ef database update --project ./src/AspNetCore.Examples.ProductService.Infrastructure/AspNetCore.Examples.ProductService.Infrastructure.csproj

run_functional_tests:
	curl --connect-timeout 5 \
                 --max-time 10 \
                 --retry 10	 \
                 --retry-delay 30 \
                 --retry-max-time 300 \
                 --retry-all-errors \
                 ${SUT_BASE_URL}/healthcheck
	dotnet test ${SPECS_PROJECT_PATH}/${SPECS_PROJECT_NAME}

generate_functional_tests_report:
	livingdoc test-assembly ${SPECS_PROJECT_PATH}/bin/Debug/${NET_VERSION}/${SPECS_DLL_NAME} -t ${SPECS_PROJECT_PATH}/bin/Debug/${NET_VERSION}/TestExecution.json --output behavioral-test-results

wait_app_is_started:
	curl --connect-timeout 5 \
        --max-time 10 \
        --retry 10	 \
        --retry-delay 30 \
        --retry-max-time 300 \
        --retry-all-errors \
        'http://localhost/healthcheck'

create_queue:
	az storage queue create -n $(QUEUE_NAME) --connection-string "${QUEUE_STORAGE_CONNECTION_STRING}"