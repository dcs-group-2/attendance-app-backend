@description('The subscription ID')
param subscriptionId string

@description('The location for all resources')
param location string = resourceGroup().location

@description('Name of the Storage Account for Function App runtime (must be globally unique)')
param storageAccountName string = 'attendancebackendstore'

@description('Function App parameters object (supplied via functionAppParameters.bicepparam)')
param functionAppParams object

// -----------------------------------------------------
// Module: Deploy Storage Account
// -----------------------------------------------------
module storageAccount 'storageAccount.bicep' = {
  name: 'storageAccountDeployment'
  params: {
    location: location
    storagePrefix: 'devops-'
  }
}

// -----------------------------------------------------
// Module: Deploy Function App
// -----------------------------------------------------
module functionApp 'functionApp.bicep' = {
  name: 'functionAppDeployment'
  // Merge common parameters with the Function App–specific ones from your parameter file.
  params: union(
    {
      subscriptionId: subscriptionId
      location: location
      storageAccountName: storageAccountName
    },
    functionAppParams
  )
  dependsOn: [
    storageAccount
  ]
}
