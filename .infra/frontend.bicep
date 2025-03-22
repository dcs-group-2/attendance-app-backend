@description('Location for all resources.')
param location string = resourceGroup().location

@description('The name of the function app that you wish to create.')
param appName string

@description('Create a static web app')
module swa 'br/public:avm/res/web/static-site:0.3.0' = {
  name: 'client'
  scope: resourceGroup()
  params: {
    name: '${appName}-stapp'
    location: location
    sku: 'Standard'
  }
}
