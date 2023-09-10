#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Build.targets", "."]
COPY ["nuget.config", "."]
COPY ["src/Service/OFood.Shop.Api/OFood.Shop.Api.csproj", "src/Service/OFood.Shop.Api/"]
COPY ["src/Common/CasheProvider/CacheHelper/CacheHelper.csproj", "src/Common/CasheProvider/CacheHelper/"]
COPY ["src/Common/CasheProvider/Caching.Abstractions/Caching.Abstractions.csproj", "src/Common/CasheProvider/Caching.Abstractions/"]
COPY ["src/Common/CasheProvider/Caching.InMemory/Caching.InMemory.csproj", "src/Common/CasheProvider/Caching.InMemory/"]
COPY ["src/Common/CasheProvider/Serializer/Serializer.csproj", "src/Common/CasheProvider/Serializer/"]
COPY ["src/Common/CasheProvider/Caching.Redis/Caching.Redis.csproj", "src/Common/CasheProvider/Caching.Redis/"]
COPY ["src/Common/CasheProvider/Redis.StackExchange/Redis.StackExchange.csproj", "src/Common/CasheProvider/Redis.StackExchange/"]
COPY ["src/Common/CasheProvider/RedisConnectionHelper/RedisConnectionHelper.csproj", "src/Common/CasheProvider/RedisConnectionHelper/"]
COPY ["src/Common/CasheProvider/ObjectPoolHelper/ObjectPoolHelper.csproj", "src/Common/CasheProvider/ObjectPoolHelper/"]
COPY ["src/Common/Common.EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/Common/Common.EventBus/EventBusRabbitMQ/"]
COPY ["src/Common/Common.EventBus/EventBus/EventBus.csproj", "src/Common/Common.EventBus/EventBus/"]
COPY ["src/Common/Common.EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "src/Common/Common.EventBus/IntegrationEventLogEF/"]
COPY ["src/Common/OFood.Shop.Common/OFood.Shop.Common.csproj", "src/Common/OFood.Shop.Common/"]
COPY ["src/Config/OFood.Shop.Config/OFood.Shop.Config.csproj", "src/Config/OFood.Shop.Config/"]
COPY ["src/Framework/Core/Framework.Core.Logging/Framework.Core.Logging.csproj", "src/Framework/Core/Framework.Core.Logging/"]
COPY ["src/Framework/Logging/Framework.Logging/Framework.Logging.csproj", "src/Framework/Logging/Framework.Logging/"]
COPY ["src/Framework/Security/Framework.Security/Framework.Security.csproj", "src/Framework/Security/Framework.Security/"]
COPY ["src/Framework/Core/Framework.Core.Security/Framework.Core.Security.csproj", "src/Framework/Core/Framework.Core.Security/"]
COPY ["src/Framework/Core/Framework.Core/Framework.Core.csproj", "src/Framework/Core/Framework.Core/"]
COPY ["src/Infrastructure/OFood.Shop.Infrastructure/OFood.Shop.Infrastructure.csproj", "src/Infrastructure/OFood.Shop.Infrastructure/"]
COPY ["src/Domain/OFood.Shop.Domain/OFood.Shop.Domain.csproj", "src/Domain/OFood.Shop.Domain/"]
COPY ["src/Framework/Core/Framework.Core.Spatial/Framework.Core.Spatial.csproj", "src/Framework/Core/Framework.Core.Spatial/"]
COPY ["src/Service/OFood.Shop.Facade/OFood.Shop.Facade.csproj", "src/Service/OFood.Shop.Facade/"]
COPY ["src/Application/OFood.Shop.Application.Contract/OFood.Shop.Application.Contract.csproj", "src/Application/OFood.Shop.Application.Contract/"]
COPY ["src/Application/OFood.Shop.Application/OFood.Shop.Application.csproj", "src/Application/OFood.Shop.Application/"]
COPY ["src/Query/OFood.Shop.Query/OFood.Shop.Query.csproj", "src/Query/OFood.Shop.Query/"]
COPY ["src/Framework/Core/Framework.Core.Query/Framework.Core.Query.csproj", "src/Framework/Core/Framework.Core.Query/"]
COPY ["src/ThirdPartyService/MapService/MapService.csproj", "src/ThirdPartyService/MapService/"]
COPY ["src/Framework/Logging/Framework.Logging.Sentry/Framework.Logging.Sentry.csproj", "src/Framework/Logging/Framework.Logging.Sentry/"]
COPY ["src/Framework/Web/Framework.Web.Api/Framework.Web.Api.csproj", "src/Framework/Web/Framework.Web.Api/"]
COPY ["src/Framework/Core/Framework.Core.Domain/Framework.Core.Domain.csproj", "src/Framework/Core/Framework.Core.Domain/"]
COPY ["src/Framework/Core/Framework.Core.ExceptionHandling/Framework.Core.ExceptionHandling.csproj", "src/Framework/Core/Framework.Core.ExceptionHandling/"]
RUN dotnet restore "src/Service/OFood.Shop.Api/OFood.Shop.Api.csproj"
COPY . .
WORKDIR "/src/src/Service/OFood.Shop.Api"
RUN dotnet build "OFood.Shop.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OFood.Shop.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OFood.Shop.Api.dll"]