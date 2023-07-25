param name string
param location string
param logAnalyticsWorkspaceClientId string
param logAnalyticsWorkspaceClientSecret string

resource env 'Microsoft.App/managedEnvironments@2022-01-01-preview' = {
  name: name
  location: location
  properties: {
    type: 'managed'
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspaceClientId
        sharedKey: logAnalyticsWorkspaceClientSecret
      }
    }
  }
}
output id string = env.id
