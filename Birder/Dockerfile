#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Birder/Birder.csproj", "Birder/"]
RUN dotnet restore "Birder/Birder.csproj"
COPY . .
WORKDIR "/src/Birder"
RUN dotnet build "Birder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Birder.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Birder.dll"]