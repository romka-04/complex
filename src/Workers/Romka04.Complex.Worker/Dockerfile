#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Workers/Romka04.Complex.Worker/Romka04.Complex.Worker.csproj", "src/Workers/Romka04.Complex.Worker/"]
COPY ["src/Core Libraries/Romka04.Complex.Core/Romka04.Complex.Core.csproj", "src/Core Libraries/Romka04.Complex.Core/"]
RUN dotnet restore "src/Workers/Romka04.Complex.Worker/Romka04.Complex.Worker.csproj"
COPY . .
WORKDIR "/src/src/Workers/Romka04.Complex.Worker"
RUN dotnet build "Romka04.Complex.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Romka04.Complex.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Romka04.Complex.Worker.dll"]