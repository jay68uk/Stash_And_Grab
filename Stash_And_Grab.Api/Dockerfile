#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Stash_And_Grab/Stash_And_Grab.Api/Stash_And_Grab.Api.csproj", "Stash_And_Grab/Stash_And_Grab.Api/"]
COPY ["Stash_And_Grab/Stash_And_Grab.Application/Stash_And_Grab.Application.csproj", "Stash_And_Grab/Stash_And_Grab.Application/"]
RUN dotnet restore "Stash_And_Grab/Stash_And_Grab.Api/Stash_And_Grab.Api.csproj"
COPY . .
WORKDIR "/src/Stash_And_Grab/Stash_And_Grab.Api"
RUN dotnet build "Stash_And_Grab.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stash_And_Grab.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stash_And_Grab.Api.dll"]