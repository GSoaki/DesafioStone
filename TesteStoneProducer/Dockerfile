FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY ["DataProducer/DataProducer.csproj", "DataProducer/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Models/Model.csproj", "Models/"]

RUN dotnet restore "DataProducer/DataProducer.csproj"

COPY . .

WORKDIR "/app/DataProducer"
RUN dotnet build "DataProducer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataProducer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
EXPOSE 80

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DataProducer.dll"]
