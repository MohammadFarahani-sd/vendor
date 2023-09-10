FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app

FROM base AS final
WORKDIR /app
COPY src/Service/OFood.Shop.Api/publish/ ./

ENV TZ=Europe/London
RUN ln -snf /usr/share/zoneinfo/Europe/London /etc/localtime

ENTRYPOINT ["dotnet", "OFood.Shop.Api.dll" ]