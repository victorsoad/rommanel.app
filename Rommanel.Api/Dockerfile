# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo da API para o container
COPY . . 

# Restaura as depend�ncias e publica
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia o resultado do build
COPY --from=build /app/out ./

# Exp�e a porta que a aplica��o ir� rodar
EXPOSE 5000

# Configura o entrypoint
ENTRYPOINT ["dotnet", "Rommanel.API.dll"]
