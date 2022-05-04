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
	pidLine=$(ps -ax | grep dotnet)
	pidLine=$(echo $pidLine)
	pid=${pidLine%% *}
	echo "pid = $pid"
	kill $pid

end_sonar_scan:
	reportgenerator "-reports:test-results/**/*.opencover.xml" "-targetdir:test-report"
	dotnet sonarscanner end /d:sonar.login=${SONAR_LOGIN} /d:sonar.password=${SONAR_PASSWORD}
	exitStatus=$?
	sonar-report \
    		--sonarurl=${SONAR_URI} \
    		--project=${SONAR_PROJECT_KEY} \
    		--sonarcomponent=${SONAR_PROJECT_KEY} \
    		--application=${SONAR_PROJECT_KEY} \
    		--sinceleakperiod="false" \
    		--sonarusername=${SONAR_LOGIN} \
    		--sonarpassword=${SONAR_PASSWORD} \
    		--allbugs="true" \
    		--noSecurityHotspot="true" > ./sonar-report/sonar-report.html
	echo "Exit status is $exitStatus"
	exit $exitStatus

run_unit_tests:
	dotnet test --settings settings/coverlet.runsettings --logger trx --results-directory "test-results"

start_with_coverlet_watch:
	coverlet out/AspNetCore.Examples.ProductService.dll \
		--target dotnet \
		--targetargs "out/AspNetCore.Examples.ProductService.dll --urls=http://0.0.0.0:80/" \
		--format opencover \
		--include-test-assembly \
		--verbosity:detailed \
		--output test-results/behavioral.opencover.xml > coverlet.log

sonar_scan:
	dotnet sonarscanner begin \
		/k:${SONAR_PROJECT_KEY} \
		/d:sonar.cs.vstest.reportsPaths=**/*.trx \
		/d:sonar.cs.opencover.reportsPaths=**/*.opencover.xml \
		/d:sonar.login=${SONAR_LOGIN} \
		/d:sonar.password=${SONAR_PASSWORD} \
		/d:sonar.host.url=${SONAR_URI} \
		/d:sonar.qualitygate.wait=true
	dotnet build
	dotnet test --settings settings/coverlet.runsettings --logger trx --results-directory "test-results"
	reportgenerator "-reports:test-results/**/*.opencover.xml" "-targetdir:test-report"
	dotnet sonarscanner end /d:sonar.login=${SONAR_LOGIN} /d:sonar.password=${SONAR_PASSWORD}
	exitStatus=$?
	sonar-report \
		--sonarurl=${SONAR_URI} \
		--project=${SONAR_PROJECT_KEY} \
		--sonarcomponent=${SONAR_PROJECT_KEY} \
		--application=${SONAR_PROJECT_KEY} \
		--sinceleakperiod="false" \
		--sonarusername=${SONAR_LOGIN} \
		--sonarpassword=${SONAR_PASSWORD} \
		--allbugs="true" \
		--noSecurityHotspot="true" > ./sonar-report/sonar-report.html
	echo "Exit status is $exitStatus"
	exit $exitStatus