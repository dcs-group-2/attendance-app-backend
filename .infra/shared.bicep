@description('Location for all resources.')
param location string = resourceGroup().location

@description('Tennant ID for the Azure subscription')
param tenantId string

@description('The name of the application you wish to create.')
param appName string

// Key vault
resource keyvault 'Microsoft.KeyVault/vaults@2021-11-01-preview' = {
  name: '${appName}-kv'
  location: location
  properties: {
    enableSoftDelete: true
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enableRbacAuthorization: true
    sku: {
      name: 'standard'
      family: 'A'
    }
    tenantId: tenantId
  }
}

// Log workspace
resource workspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: '${appName}-log'
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 120
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
  }
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${appName}-appi'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Bluefield'
    Request_Source: 'rest'
    WorkspaceResourceId: workspace.id
  }
}
