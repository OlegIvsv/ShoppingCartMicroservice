﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
# COPY ./ShoppingCart.sln ./
COPY ["ShoppingCart.Interfaces/*.csproj", "./ShoppingCart.Interfaces/"]
COPY ["ShoppingCart.Domain/*.csproj", "./ShoppingCart.Domain/"]
COPY ["ShoppingCart.Infrastructure/*.csproj", "./ShoppingCart.Infrastructure/"]
COPY ["ShoppingCart.Api/*.csproj", "./ShoppingCart.Api/"]

RUN dotnet restore "./ShoppingCart.Interfaces/"
RUN dotnet restore "./ShoppingCart.Domain/"
RUN dotnet restore "./ShoppingCart.Infrastructure/"
RUN dotnet restore "./ShoppingCart.Api/"

COPY . .

WORKDIR "/src/ShoppingCart.Domain"
RUN dotnet build -c Release -o /app
WORKDIR "/src/ShoppingCart.Infrastructure"
RUN dotnet build -c Release -o /app
WORKDIR "/src/ShoppingCart.Interfaces"
RUN dotnet build -c Release -o /app
WORKDIR "/src/ShoppingCart.Api"
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingCart.Api.dll"]