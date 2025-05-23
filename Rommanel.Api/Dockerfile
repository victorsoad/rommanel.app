# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo da API para o container
COPY . . 

# Restaura as dependências e publica
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia o resultado do build
COPY --from=build /app/out ./

# Expõe a porta que a aplicação irá rodar
EXPOSE 5000

# Configura o entrypoint
ENTRYPOINT ["dotnet", "Rommanel.API.dll"]
