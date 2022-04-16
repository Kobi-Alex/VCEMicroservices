#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 6012

ENV ASPNETCORE_URLS=https://+:443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Exam/Exam.API/Exam.API.csproj", "Services/Exam/Exam.API/"]
COPY ["Services/Exam/Exam.Infrastructure/Exam.Infrastructure.csproj", "Services/Exam/Exam.Infrastructure/"]
COPY ["Services/Exam/Exam.Domain/Exam.Domain.csproj", "Services/Exam/Exam.Domain/"]
RUN dotnet restore "Services/Exam/Exam.API/Exam.API.csproj"
COPY . .
WORKDIR "/src/Services/Exam/Exam.API"
RUN dotnet build "Exam.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exam.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exam.API.dll"]
