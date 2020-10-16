FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/VacationRental.Api/VacationRental.Api.csproj", "src/VacationRental.Api/"]
COPY ["src/VacationRental.Core/VacationRental.Core.csproj", "src/VacationRental.Core/"]
COPY ["src/VacationRental.Infrastructure/VacationRental.Infrastructure.csproj", "src/VacationRental.Infrastructure/"]
RUN dotnet restore "src/VacationRental.Api/VacationRental.Api.csproj"
COPY . .
WORKDIR "/src/src/VacationRental.Api"
RUN dotnet build "VacationRental.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VacationRental.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VacationRental.Api.dll"]