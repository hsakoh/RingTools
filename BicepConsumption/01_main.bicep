param serviceCode string

param region string = 'japaneast'

module storageAccounts './modules/StorageAccounts.bicep' = {
    name: '${deployment().name}-storageAccounts'
    params: {
        serviceCode: serviceCode
        region: region
    }
}

module applicationInsights './modules/ApplicationInsights.bicep' = {
    name: '${deployment().name}-applicationInsights'
    params: {
        serviceCode: serviceCode
        region: region
    }
}

module appServicePlan './modules/AppServicePlan.bicep' = {
    name: '${deployment().name}-appServicePlan'
    params: {
        serviceCode: serviceCode
        region: region
    }
}

module FunctionApp './modules/FunctionApp.bicep' = {
    name: '${deployment().name}-FunctionApp'
    params: {
        serviceCode: serviceCode
        region: region
        applicationInsightsConnectionString: applicationInsights.outputs.connectionString
        appServicePlanId: appServicePlan.outputs.id
        storageAccountName: storageAccounts.outputs.name
    }
}
