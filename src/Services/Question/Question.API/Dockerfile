#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 6002

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Question/Question.API/Question.API.csproj", "Services/Question/Question.API/"]
COPY ["Services/Question/Question.Infrastructure/Question.Infrastructure.csproj", "Services/Question/Question.Infrastructure/"]
COPY ["Services/Question/Question.Domain/Question.Domain.csproj", "Services/Question/Question.Domain/"]
RUN dotnet restore "Services/Question/Question.API/Question.API.csproj"
COPY . .
WORKDIR "/src/Services/Question/Question.API"
RUN dotnet build "Question.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Question.API.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Question.API.dll"]
