#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Api/VacationRental.Api/VacationRental.Api.csproj", "src/Api/VacationRental.Api/"]
COPY ["src/Infrastructure/VacationRental.Infrastructure.Data/VacationRental.Infrastructure.Data.csproj", "src/Infrastructure/VacationRental.Infrastructure.Data/"]
COPY ["src/Core/VacationRental.Core.Interfaces/VacationRental.Core.Interfaces.csproj", "src/Core/VacationRental.Core.Interfaces/"]
COPY ["src/Core/VacationRental.Core.Entities/VacationRental.Core.Entities.csproj", "src/Core/VacationRental.Core.Entities/"]
COPY ["src/Core/VacationRental.Core.Dtos/VacationRental.Core.Dtos.csproj", "src/Core/VacationRental.Core.Dtos/"]
COPY ["src/Shared/VacationRental.Common/VacationRental.Common.csproj", "src/Shared/VacationRental.Common/"]
COPY ["src/Core/VacationRental.Core.Services/VacationRental.Core.Services.csproj", "src/Core/VacationRental.Core.Services/"]
RUN dotnet restore "src/Api/VacationRental.Api/VacationRental.Api.csproj"
COPY . .
WORKDIR "/src/src/Api/VacationRental.Api"
RUN dotnet build "VacationRental.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VacationRental.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VacationRental.Api.dll"]