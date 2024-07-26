# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /source
COPY . .
RUN dotnet restore "Financeiro.Boleto.Api/Financeiro.Boleto.Api.csproj"
RUN dotnet publish "Financeiro.Boleto.Api/Financeiro.Boleto.Api.csproj" -c release -o /app --no-restore

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_URLS=http://+:5050;https://+:5051
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=teste123
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV TZ=America/Sao_Paulo

EXPOSE 5050
EXPOSE 5051

ENTRYPOINT ["dotnet", "Financeiro.Boleto.Api.dll"]

