#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Applicant/Applicant.API/Applicant.API.csproj", "Services/Applicant/Applicant.API/"]
COPY ["Services/Applicant/Applicant.Domain/Applicant.Domain.csproj", "Services/Applicant/Applicant.Domain/"]
COPY ["Services/Applicant/Applicant.Infrasructure/Applicant.Infrasructure.csproj", "Services/Applicant/Applicant.Infrasructure/"]
RUN dotnet restore "Services/Applicant/Applicant.API/Applicant.API.csproj"
COPY . .
WORKDIR "/src/Services/Applicant/Applicant.API"
RUN dotnet build "Applicant.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Applicant.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Applicant.API.dll"]