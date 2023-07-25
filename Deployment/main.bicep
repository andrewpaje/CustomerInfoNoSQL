// Setting target scope
targetScope = 'subscription'

// Set Paramaters
param resourceGroupName string = 'resourceGroupCustomerInfo'
param location string = 'eastus'
param envName string = 'containerEnvCustomerInfo'
// param applicationName string = 'customer-info'

// param containerImage string
// param containerPort int = 80

// @secure()
// param registryPassword string

// Creating resource group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: location
}

// Deploying CosmosDB
module cosmosDB './cosmosDB.bicep' = {
  name: 'cosmosDB'
  scope: resourceGroup
  params: {
    location: location
  }
}

// Deploying Log Analytics Workspace
module law './logAnalyticsWorkspace.bicep' = {
  name: 'logAnalyticsWorkspace'
  scope: resourceGroup
  params: {
    location: location
    name: 'law-${envName}'
  }
}

// Deploying Container Registry
module containerRegistry './containerRegistry.bicep' = {
  name: 'containerRegistry'
  scope: resourceGroup
  params: {
    location: location
  }
}

// Deploying Container Apps Environment
module containerAppEnvironment './containerAppsEnv.bicep' = {
  name: 'containerAppEnvironment'
  scope: resourceGroup
  params: {
    name: envName
    location: location
    logAnalyticsWorkspaceClientId: law.outputs.clientId
    logAnalyticsWorkspaceClientSecret: law.outputs.clientSecret
  }
}

// // Deploying Container Apps
// module containerApp './containerApps.bicep' = {
//   name: 'containerApp'
//   scope: resourceGroup
//   params: {
//     name: applicationName
//     location: location
//     containerAppEnvironmentId: containerAppEnvironment.outputs.id
//     containerImage: containerImage
//     containerPort: containerPort
//     envVars: [
//       {
//         name: 'ASPNETCORE_ENVIRONMENT'
//         value: 'Production'
//       }
//     ]
//     useExternalIngress: true
//     registry: containerRegistry.outputs.registryUsername
//     registryUsername: containerRegistry.outputs.registryUsername
//     registryPassword: registryPassword
//   }
// }
// output fqdn string = containerApp.outputs.fqdn
