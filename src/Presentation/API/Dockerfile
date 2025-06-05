FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY src/Core/Application/*.csproj src/Core/Application/
COPY src/Core/Domain/*.csproj src/Core/Domain/
COPY src/Infrastructure/Adapter/*.csproj src/Infrastructure/Adapter/
COPY src/Infrastructure/Persistence/*.csproj src/Infrastructure/Persistence/
COPY src/Presentation/API/*.csproj src/Presentation/API/
RUN dotnet restore src/Presentation/API/*.csproj
COPY . .
RUN dotnet publish src/Presentation/API/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 as runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","API.dll" ]
