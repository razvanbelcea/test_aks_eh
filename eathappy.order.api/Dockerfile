#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["eathappy-order-service/eathappy.order.api/eathappy.order.api.csproj", "eathappy-order-service/eathappy.order.api/"]
COPY ["eathappy-order-service/eathappy.order.data/eathappy.order.data.csproj", "eathappy-order-service/eathappy.order.data/"]
COPY ["eathappy-order-service/eathappy.order.domain/eathappy.order.domain.csproj", "eathappy-order-service/eathappy.order.domain/"]
COPY ["eathappy-order-service/eathappy.order.business/eathappy.order.business.csproj", "eathappy-order-service/eathappy.order.business/"]
RUN dotnet restore "eathappy-order-service/eathappy.order.api/eathappy.order.api.csproj"
COPY . .
WORKDIR "/src/eathappy-order-service/eathappy.order.api"
RUN dotnet build "eathappy.order.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eathappy.order.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eathappy.order.api.dll"]