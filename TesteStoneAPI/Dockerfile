FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY ["TesteElastic/TesteElastic.csproj", "TesteElastic/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Models/Model.csproj", "Models/"]

RUN dotnet restore "TesteElastic/TesteElastic.csproj"

COPY . .

WORKDIR "/app/TesteElastic"
RUN dotnet build "TesteElastic.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TesteElastic.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
EXPOSE 80

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TesteElastic.dll"]
