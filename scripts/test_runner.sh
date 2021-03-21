if test "$#" -ne 1; then
    echo "Usage: sh ./test_runner.sh [unit|behavioral]"
    exit 1
fi
if [ $1 != "unit" ] && [ $1 != "behavioral" ]
then
    echo "The only supported test types are unit and behavioral"
    exit 1
fi

dotnet restore
dotnet build
baseTestFolder="test/$1"
projectsPattern="$baseTestFolder/*/*.csproj"
for proj in $projectsPattern; do
    dotnet test $proj  --settings settings/coverlet.runsettings --logger trx --results-directory "test-results"
done
reportgenerator "-reports:test-results/**/*.opencover.xml" "-targetdir:test-report"