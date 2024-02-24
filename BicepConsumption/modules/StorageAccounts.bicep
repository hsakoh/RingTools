param serviceCode string

param region string = 'japaneast'

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
    name: 'stga${toLower(replace(serviceCode, '-', ''))}'
    location: region
    sku: {
        name: 'Standard_LRS'
    }
    kind: 'StorageV2'
    properties: {
        supportsHttpsTrafficOnly: true
        minimumTlsVersion: 'TLS1_2'
        accessTier: 'Hot'
        encryption: {
            services: {
                blob: {
                    enabled: true
                }
                file: {
                    enabled: true
                }
            }
            keySource: 'Microsoft.Storage'
        }
        networkAcls: {
            bypass: 'AzureServices'
            defaultAction: 'Allow'
            virtualNetworkRules: []
            ipRules: []
        }
    }
    resource blobServices 'blobServices' = {
        name: 'default'
        properties: {
            cors: {
                corsRules: []
            }
            deleteRetentionPolicy: {
                enabled: true
                days: 30
            }
        }
        resource containers 'containers' = [for value in [
            'snapshots'
            'timelapses'
        ]: {
            name: value
        }]
    }
}

output id string = storageAccount.id
output name string = storageAccount.name
