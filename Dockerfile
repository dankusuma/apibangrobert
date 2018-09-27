FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Login.Services/*.csproj ./Login.Services/
RUN dotnet restore

# copy everything else and build app
COPY Login.Services/. ./Login.Services/
WORKDIR /app/Login.Services
RUN dotnet publish -c Release -o out


FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/Login.Services/out ./
ENTRYPOINT ["dotnet", "Login.Services.dll"]