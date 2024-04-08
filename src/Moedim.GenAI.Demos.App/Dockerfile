#------------------------------------------------------------------------------------------------------------------------
#
# USAGE:        This Dockerfile builds the sample for hosting application inside of Kubernetes Cluster.
#
# NOTE:         Please run commands from root of the solution.
#
# BUILD:        docker build --rm -f "Moedim.GenAI.Demos.App/Dockerfile" -t app:Moedim.GenAI.Demos.App .
#
#
# NOTE:        See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#------------------------------------------------------------------------------------------------------------------------

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Moedim.GenAI.Demos.App/Moedim.GenAI.Demos.App.csproj", "Moedim.GenAI.Demos.App/"]
RUN dotnet restore "Moedim.GenAI.Demos.App/Moedim.GenAI.Demos.App.csproj"
COPY . .
WORKDIR "/src/Moedim.GenAI.Demos.App"
RUN dotnet build "Moedim.GenAI.Demos.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Moedim.GenAI.Demos.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Moedim.GenAI.Demos.App.dll"]
