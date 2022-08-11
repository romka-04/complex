#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["test/Core Libraries/Romka04.Complex.Core.Tests/Romka04.Complex.Core.Tests.csproj", "test/Core Libraries/Romka04.Complex.Core.Tests/"]
COPY ["src/Core Libraries/Romka04.Complex.Core/Romka04.Complex.Core.csproj", "src/Core Libraries/Romka04.Complex.Core/"]
RUN dotnet restore "test/Core Libraries/Romka04.Complex.Core.Tests/Romka04.Complex.Core.Tests.csproj"
COPY . .
WORKDIR "/src/test/Core Libraries/Romka04.Complex.Core.Tests"
RUN dotnet build "Romka04.Complex.Core.Tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Romka04.Complex.Core.Tests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "Romka04.Complex.Core.Tests.dll"]