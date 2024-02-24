param serviceCode string

param region string = 'japaneast'

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
    name: 'appist-${serviceCode}'
    location: region
    kind: 'other'
    properties: {
        Application_Type: 'web'
        Flow_Type: 'Bluefield'
        Request_Source: 'rest'
        DisableIpMasking: true
    }
}

output connectionString string = applicationInsights.properties.ConnectionString
output name string = applicationInsights.name
