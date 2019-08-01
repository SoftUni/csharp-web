FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MyFirstMvcApp/MyFirstMvcApp.csproj", "MyFirstMvcApp/"]
RUN dotnet restore "MyFirstMvcApp/MyFirstMvcApp.csproj"
COPY . .
WORKDIR "/src/MyFirstMvcApp"
RUN dotnet build "MyFirstMvcApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MyFirstMvcApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyFirstMvcApp.dll"]