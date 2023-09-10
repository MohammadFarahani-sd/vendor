to build for packaged use msbuild property /p:PACK=1

example:
dotnet restore -c Release /p:PACK=1
dotnet build -c Release /p:PACK=1
dotnet pack -c Release /p:PACK=1