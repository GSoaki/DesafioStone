FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY ["DataConsumer/DataConsumer.csproj", "DataConsumer/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Models/Model.csproj", "Models/"]

RUN dotnet restore "DataConsumer/DataConsumer.csproj"

COPY . .

WORKDIR "/app/DataConsumer"
RUN dotnet build "DataConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataConsumer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
EXPOSE 80

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DataConsumer.dll"]
