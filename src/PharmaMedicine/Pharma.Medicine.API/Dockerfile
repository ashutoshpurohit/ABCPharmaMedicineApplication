#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/PharmaMedicine/Pharma.Medicine.API/Pharma.Medicine.API.csproj", "src/PharmaMedicine/Pharma.Medicine.API/"]
COPY ["src/PharmaMedicine/Pharma.Medicine.Application/Pharma.Medicine.Application.csproj", "src/PharmaMedicine/Pharma.Medicine.Application/"]
COPY ["src/PharmaMedicine/Pharma.Medicine.InfraStructure/Pharma.Medicine.InfraStructure.csproj", "src/PharmaMedicine/Pharma.Medicine.InfraStructure/"]
COPY ["src/PharmaMedicine/Pharma.Medicine.CoreEntity/Pharma.Medicine.CoreEntity.csproj", "src/PharmaMedicine/Pharma.Medicine.CoreEntity/"]
RUN dotnet restore "src/PharmaMedicine/Pharma.Medicine.API/Pharma.Medicine.API.csproj"
COPY . .
WORKDIR "/src/src/PharmaMedicine/Pharma.Medicine.API"
RUN dotnet build "Pharma.Medicine.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pharma.Medicine.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pharma.Medicine.API.dll"]