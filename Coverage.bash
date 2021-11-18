#!/bin/bash

srcPath=$(pwd)
sourcePath="$srcPath"
testPath="$srcPath/testresults"
testFile="$testPath/test_results.xml"
coveragePath="$testPath/coverage"
reportFile="$testPath/coverage.cobertura.xml"
reportPath="$coveragePath/reports"

echo "1 - setting chmod to working directory"
sudo chmod -R 777 "$srcPath"
sudo chmod -R 777 ~/.dotnet/
sudo chmod -R 777 ~/.config/
sudo chmod -R 777 /usr/local/share/dotnet/

echo "2 - login on codeartifact"
dotnet nuget remove source stationkim/stationkim
aws codeartifact login --tool nuget --repository stationkim --domain stationkim --domain-owner 432226259175

echo "3 - Removing test results folder: $testPath"
rm -r "$testPath"

echo "4 - Ensure that global report tool is installed"
dotnet tool install dotnet-reportgenerator-globaltool

echo "5 - Generating tests"
dotnet test "$sourcePath" -c Release --results-directory "$testPath" --logger "trx;LogFileName=$testFile" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput="$coveragePath" /p:Exclude="[*Tests]*%2c[*Data]*"

echo "6 - Generating HTML charts"
~/.dotnet/tools/reportgenerator "-reports:$reportFile" "-targetdir:$reportPath" "-reporttypes:HTMLInline;HTMLChart"

echo "7 - Opening Safari to see results"
open "testresults/coverage/reports/index.html"
