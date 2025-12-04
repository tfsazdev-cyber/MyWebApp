# Quick Start Guide - Azure DevOps CI/CD Setup

## 🚀 Quick Setup Steps

### 1. Create Service Connections (5 minutes)

#### ACR Service Connection:
1. Azure DevOps → **Project Settings** → **Service connections** → **New service connection**
2. Select **Docker Registry** → **Azure Container Registry**
3. Choose subscription and ACR, name it (e.g., `MyWebApp-ACR`)
4. **Save**

#### Azure Service Connection:
1. **Service connections** → **New service connection** → **Azure Resource Manager**
2. Choose **Service principal (automatic)**
3. Select subscription and resource group, name it (e.g., `MyWebApp-Azure`)
4. **Save**

### 2. Update Pipeline Variables (2 minutes)

Edit `azure-pipelines.yml` and replace these values:

```yaml
dockerRegistryServiceConnection: 'MyWebApp-ACR'  # Your ACR service connection name
containerRegistry: 'myregistry.azurecr.io'        # Your ACR URL
azureSubscription: 'MyWebApp-Azure'               # Your Azure service connection
appServiceName: 'mywebapp-prod'                   # Your App Service name
resourceGroup: 'my-resource-group'                # Your resource group
agentPool: 'Default'                              # Your agent pool (or 'Default')
```

### 3. Create Pipeline in Azure DevOps (2 minutes)

1. **Pipelines** → **New pipeline**
2. Select your repository (Azure Repos/GitHub/etc.)
3. Choose **Existing Azure Pipelines YAML file**
4. Select `azure-pipelines.yml`
5. **Run** → Approve if prompted

### 4. For Agent-Based Deployment (Optional - 15 minutes)

1. **Organization Settings** → **Agent pools** → **Add pool**
   - Name: `MyServerAgents`
   - Type: Self-hosted

2. On your target server:
   ```bash
   # Install Docker
   curl -fsSL https://get.docker.com -o get-docker.sh
   sudo sh get-docker.sh
   
   # Download and configure Azure DevOps agent
   mkdir myagent && cd myagent
   wget https://vstsagentpackage.azureedge.net/agent/2.220.0/vsts-agent-linux-x64-2.220.0.tar.gz
   tar zxvf vsts-agent-linux-x64-2.220.0.tar.gz
   ./config.sh
   ```

3. Update pipeline variable:
   ```yaml
   agentPool: 'MyServerAgents'
   ```

## 📁 Pipeline Files

- **`azure-pipelines.yml`** - Complete CI/CD pipeline (all stages)
- **`azure-pipelines-service-connection.yml`** - Service connection deployment only
- **`azure-pipelines-agent-deployment.yml`** - Agent-based deployment only

## ✅ Verify Deployment

### Service Connection Deployment:
- Check Azure Portal → App Service → **Deployment Center**
- View logs in Azure DevOps pipeline run

### Agent Deployment:
```bash
docker ps | grep mywebapp
docker logs mywebapp
curl http://localhost:8080
```

## 🔧 Common Variables to Update

| Variable | Description | Example |
|----------|-------------|---------|
| `dockerRegistryServiceConnection` | ACR service connection name | `MyWebApp-ACR` |
| `containerRegistry` | ACR URL | `myregistry.azurecr.io` |
| `imageRepository` | Container image name | `mywebapp` |
| `azureSubscription` | Azure service connection | `MyWebApp-Azure` |
| `appServiceName` | App Service name | `mywebapp-prod` |
| `resourceGroup` | Azure resource group | `my-resource-group` |
| `agentPool` | Agent pool name | `Default` or `MyServerAgents` |

## 📚 Full Documentation

See [AZURE_DEVOPS_SETUP.md](AZURE_DEVOPS_SETUP.md) for detailed setup instructions.

