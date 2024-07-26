# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /source
COPY . .
RUN dotnet restore "Financeiro.Auth.csproj"
RUN dotnet publish "Financeiro.Auth.csproj" -c release -o /app --no-restore

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_URLS=http://+:5112;https://+:5113
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=teste123
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV TZ=America/Sao_Paulo

EXPOSE 5112
EXPOSE 5113

ENTRYPOINT ["dotnet", "Financeiro.Auth.dll"]

