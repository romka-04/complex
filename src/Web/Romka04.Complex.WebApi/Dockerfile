#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Web/Romka04.Complex.WebApi/Romka04.Complex.WebApi.csproj", "src/Web/Romka04.Complex.WebApi/"]
COPY ["src/Web/Romka04.Complex.Models/Romka04.Complex.Models.csproj", "src/Web/Romka04.Complex.Models/"]
COPY ["src/Web/Romka04.Complex.Client/Romka04.Complex.Client.csproj", "src/Web/Romka04.Complex.Client/"]
COPY ["src/Core Libraries/Romka04.Complex.Core/Romka04.Complex.Core.csproj", "src/Core Libraries/Romka04.Complex.Core/"]
RUN dotnet restore "src/Web/Romka04.Complex.WebApi/Romka04.Complex.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Web/Romka04.Complex.WebApi"
RUN dotnet build "Romka04.Complex.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Romka04.Complex.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Romka04.Complex.WebApi.dll"]