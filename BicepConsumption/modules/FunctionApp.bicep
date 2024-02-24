param serviceCode string

param region string = 'japaneast'

param applicationInsightsConnectionString string
param appServicePlanId string
param storageAccountName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' existing = {
    name: storageAccountName
}

resource funcApp 'Microsoft.Web/sites@2021-01-15' = {
    name: 'func-${serviceCode}-consumption'
    location: region
    tags: {
        'hidden-related:${appServicePlanId}': 'empty'
    }
    kind: 'functionapp'
    properties: {
        siteConfig: {
            appSettings: [
                {
                    name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
                    value: applicationInsightsConnectionString
                }
                {
                    name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
                    value: '~2'
                }
                {
                    name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
                    value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value}'
                }
                {
                    name: 'WEBSITE_CONTENTSHARE'
                    value: 'func-${serviceCode}'
                }
                {
                    name: 'AzureWebJobsStorage'
                    value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value}'
                }
                {
                    name: 'FUNCTIONS_EXTENSION_VERSION'
                    value: '~4'
                }
                {
                    name: 'FUNCTIONS_WORKER_RUNTIME'
                    value: 'dotnet'
                }
                {
                    name: 'WEBSITE_RUN_FROM_PACKAGE'
                    value: '1'
                }
                {
                    name: 'StorageConnectionString'
                    value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value}'
                }
                {
                    name: 'TimeZoneOffset'
                    value: '09:00:00'
                }
                {
                    name: 'OffsetDays'
                    value: '0'
                }
                {
                    name: 'AzureWebJobs.LocalTriggerFunction.Disabled'
                    value: 'false'
                }
                {
                    name: 'AzureWebJobs.MakeDailyTimelapseFunction.Disabled'
                    value: 'false'
                }
                {
                    name: 'AzureWebJobs.RefreshFunction.Disabled'
                    value: 'true'
                }
                {
                    name: 'AzureWebJobs.SinginFunction.Disabled'
                    value: 'true'
                }
                {
                    name: 'AzureWebJobs.SnapshotCollectorFunction.Disabled'
                    value: 'false'
                }
            ]
            cors:{
                allowedOrigins:[
                    'https://portal.azure.com'
                ]
            }
            connectionStrings: []
            alwaysOn: false
            ftpsState: 'Disabled'
            remoteDebuggingEnabled: false
            webSocketsEnabled: false
            ipSecurityRestrictions: [
                {
                    ipAddress: 'Any'
                    action: 'Allow'
                    priority: 1
                    name: 'Allow all'
                    description: 'Allow all access'
                }
            ]
            scmIpSecurityRestrictionsUseMain: true
            use32BitWorkerProcess: true
            netFrameworkVersion: 'v6.0'
        }
        serverFarmId: appServicePlanId
        clientAffinityEnabled: false
        httpsOnly: true
    }
}
output name string = funcApp.name
