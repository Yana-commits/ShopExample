FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . /src
WORKDIR /src/Order/Order.Host
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Order.Host.dll"]