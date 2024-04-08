FROM mcr.microsoft.com/dotnet/aspnet:5.0

# Instalar o SDK .NET
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        wget \
        apt-transport-https \
        software-properties-common \
    && wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        dotnet-sdk-5.0 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY . .
EXPOSE 44311
# Configurar o certificado SSL e confiar nele
RUN dotnet dev-certs https --trust
ENV ASPNETCORE_URLS=https://+:44311;https://+:80;
ENV ASPNETCORE_ENVIRONMENT=Development
# Define o comando de entrada para iniciar a aplicação
ENTRYPOINT ["dotnet", "LibTads.Web.Host.dll"]
