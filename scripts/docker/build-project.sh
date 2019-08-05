docker run --rm -v=$(pwd)/:/project \
    -v=$HOME/.nuget/packages:/root/.nuget/packages \
    -v=$HOME/.local/share/NuGet/v3-cache:/root/.local/share/NuGet/v3-cache \
    -w=/project/src/Presentation/Cnblogs.Academy.WebAPI microsoft/dotnet:2.2-sdk \
    bash -c 'echo "Starting dotnet restore..." && dotnet restore -v n && dotnet publish -c Release -o ./bin/publish'

docker run --rm -v=$(pwd)/src/Presentation/Cnblogs.Academy.SPA:/project \
    -v=$HOME/.npm/:/root/.npm \
    -w=/project node \
    bash -c 'echo "Starting node build..." && npm install && npm run build:ssr'
