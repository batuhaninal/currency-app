FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY src/Presentation/Client/*.csproj src/Presentation/Client/
RUN dotnet restore src/Presentation/Client/*.csproj
COPY . .
RUN dotnet publish src/Presentation/Client/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 as runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","Client.dll" ]
