# CustomerInfoNoSQL
Customer info stored into Azure Cosmos DB NoSQL using .NET C# Stack

## Prerequisites

- Microsoft Azure Account (https://portal.azure.com/)
- Azure CLI (https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- IDE of choice:
    - Visual Studio Code (https://code.visualstudio.com/)
    - Visual Studio (https://visualstudio.microsoft.com/)
- .NET 7.0 (https://learn.microsoft.com/en-us/dotnet/core/install/)

## Azure Login

Login to your by default to your current Azure Cloud session

```bash
az login
```

## Deploy Infrastructure in Azure Cloud via Azure CLI and Bicep templates

```bash
az deployment sub create --location <location-of-choice> --template-file Deployment/main.bicep
```

## Login to created Azure Container Registry

```bash
az acr login --name containerregistrycustomerinfo
```

## Build Image and Test Locally

```bash
docker build -t customer-info-nosql -f Deployment/Dockerfile ./
docker run -p 8080:80 --env-file .env --name customer-info-nosql customer-info-nosql
```

### Note on .env file contents
```
COSMOS_ENDPOINT=https://cosmosdbcustomerinfo.documents.azure.com:443/
COSMOS_KEY=<Primary Read/Write Key from CosmosDB Keys>
```

## Tag Image and Push to ACR

```bash
docker tag customer-info-nosql containerregistrycustomerinfo.azurecr.io/application/customer_info
docker push containerregistrycustomerinfo.azurecr.io/application/customer_info
```

## Expose Image via Container Apps and Function App

```bash
BICEP FILE is a work in progress!
```
