param serviceCode string

param region string = 'japaneast'

resource appServicePlan 'Microsoft.Web/serverfarms@2021-01-15' = {
    name: 'plan-${serviceCode}-consumption'
    location: region
    sku: {
        tier: 'Dynamic'
        name: 'Y1'
    }
    kind: ''
    properties: {
    }
}

output id string = appServicePlan.id
