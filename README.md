# HOW To run tests

#  ! Run terminal in UmsTestEnv folder !

# 1) Temporary PATH
$env:Path += "[your folder structure]\UmsTestEnv\Tools\allure\bin"

# 2) Check that allure is working
allure --version

# 3) Clear test deploy
dotnet clean .\TestCases\TestCases.csproj

# 4) If you wish to remove previous test results use this command
Remove-Item -Recurse -Force .\TestCases\allure-results\* -ErrorAction SilentlyContinue

# 5) RUN TESTS
dotnet test .\TestCases\TestCases.csproj

# 6) Generate report
allure generate ".\TestCases\allure-results" --clean -o ".\TestCases\allure-report"

# 7) Open report
allure open .\TestCases\allure-report -p 6060