#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DemoMef.CLI/DemoMef.CLI.csproj", "DemoMef.CLI/"]
COPY ["DemoMef.Core/DemoMef.Core.csproj", "DemoMef.Core/"]
COPY ["DemoMef.Plugin.Doc/DemoMef.Plugin.Doc.csproj", "DemoMef.Plugin.Doc/"]
COPY ["DemoMef.Plugin.Pdf/DemoMef.Plugin.Pdf.csproj", "DemoMef.Plugin.Pdf/"]
COPY ["DemoMef.Plugin.Rtf/DemoMef.Plugin.Rtf.csproj", "DemoMef.Plugin.Rtf/"]
RUN dotnet restore "DemoMef.CLI/DemoMef.CLI.csproj"
COPY . .
WORKDIR "/src/DemoMef.CLI"
RUN dotnet build "DemoMef.CLI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoMef.CLI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DemoMef.CLI.dll"]