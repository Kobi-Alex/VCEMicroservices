#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /src
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Report/Report.API/Report.API.csproj", "Services/Report/Report.API/"]
COPY ["Services/Report/Report.Domain/Report.Domain.csproj", "Services/Report/Report.Domain/"]
COPY ["Services/Report/Report.Infrastructure/Report.Infrastructure.csproj", "Services/Report/Report.Infrastructure/"]
RUN dotnet restore "Services/Report/Report.API/Report.API.csproj"
COPY . .
WORKDIR "/src/Services/Report/Report.API"
RUN dotnet build "Report.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Report.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /src
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Report.API.dll"]