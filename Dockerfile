FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY backend/VeterinarySystem.Api/VeterinarySystem.Api.csproj backend/VeterinarySystem.Api/
COPY backend/VeterinarySystem.Application/VeterinarySystem.Application.csproj backend/VeterinarySystem.Application/
COPY backend/VeterinarySystem.Domain/VeterinarySystem.Domain.csproj backend/VeterinarySystem.Domain/
COPY backend/VeterinarySystem.Infrastructure/VeterinarySystem.Infrastructure.csproj backend/VeterinarySystem.Infrastructure/

RUN dotnet restore backend/VeterinarySystem.Api/VeterinarySystem.Api.csproj

COPY . .

RUN dotnet publish backend/VeterinarySystem.Api/VeterinarySystem.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "VeterinarySystem.Api.dll"]