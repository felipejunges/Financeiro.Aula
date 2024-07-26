# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /source
COPY . .
RUN dotnet restore "Financeiro.Aula.Api/Financeiro.Aula.Api.csproj"
RUN dotnet publish "Financeiro.Aula.Api/Financeiro.Aula.Api.csproj" -c release -o /app --no-restore

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_URLS=http://+:5000;https://+:5001
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=teste123
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV TZ=America/Sao_Paulo

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "Financeiro.Aula.Api.dll"]
