param location string = resourceGroup().location

@minLength(5)
@maxLength(50)
param containerRegistryName string = 'containerRegistryCustomerInfo'
param containerRegistryTier string = 'Basic'

resource newContainerRegistry 'Microsoft.ContainerRegistry/registries@2023-01-01-preview' = {
  name: containerRegistryName
  location: location
  sku: {
    name: containerRegistryTier
  }
  properties: {
    adminUserEnabled: true
  }
}

output registryUsername string = newContainerRegistry.name
