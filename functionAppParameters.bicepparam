using 'functionApp.bicep'

param subscriptionId = 'f27b34f1-c540-48d5-a9ed-66ad00c1b0c0'

param name = 'attendance-backend-dev'

param location = 'North Europe'

param ftpsState = 'FtpsOnly'

param storageAccountName = 'devopsrgad2d'

param sku = 'Dynamic'

param skuCode = 'Y1'

param workerSize = '0'

param workerSizeId = '0'

param numberOfWorkers = '1'

param use32BitWorkerProcess = false

param linuxFxVersion = 'DOTNET-ISOLATED|9.0'

param repoUrl = 'https://github.com/dcs-group-2/attendance-app-backend'

param branch = 'main'

param oidcUserIdentityName = 'attendance-backe-id-9017'

param hostingPlanName = 'ASP-devopsrg-be84'

param serverFarmResourceGroup = 'devops-rg'

param alwaysOn = false
