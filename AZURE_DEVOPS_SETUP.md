# Azure DevOps CI/CD Setup Guide

This guide will help you set up Continuous Integration (CI) and Continuous Deployment (CD) for your .NET Docker application using Azure DevOps.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Service Connections Setup](#service-connections-setup)
3. [Azure DevOps Agent Setup](#azure-devops-agent-setup)
4. [Pipeline Configuration](#pipeline-configuration)
5. [Deployment Options](#deployment-options)

## Prerequisites

- Azure DevOps account and organization
- Azure subscription
- Docker image built and working locally
- Azure Container Registry (ACR) or Docker Hub account
- Azure App Service (for service connection deployment)

## Service Connections Setup

### 1. Azure Container Registry (ACR) Service Connection

1. **Create Azure Container Registry (if not exists)**
   - Go to Azure Portal → Create a resource → Container Registry
   - Choose a unique name (e.g., `mywebappregistry`)
   - Select your resource group and region
   - Enable Admin user or use Service Principal (recommended)

2. **Create Service Connection in Azure DevOps**
   - Navigate to your Azure DevOps project
   - Go to **Project Settings** → **Service connections** → **New service connection**
   - Select **Docker Registry** → **Azure Container Registry**
   - Choose your subscription and container registry
   - Give it a name: `YOUR_ACR_SERVICE_CONNECTION_NAME` (update in pipeline)
   - Click **Save**

### 2. Azure Service Connection (for App Service deployment)

1. **Create Azure App Service (if not exists)**
   - Go to Azure Portal → Create a resource → Web App
   - Configure:
     - **Publish**: Docker Container
     - **Operating System**: Linux
     - **Container Settings**: Azure Container Registry
   - Note down the App Service name

2. **Create Service Connection in Azure DevOps**
   - Go to **Project Settings** → **Service connections** → **New service connection**
   - Select **Azure Resource Manager**
   - Choose **Service principal (automatic)**
   - Select your subscription and resource group
   - Give it a name: `YOUR_AZURE_SERVICE_CONNECTION_NAME` (update in pipeline)
   - Click **Save**

## Azure DevOps Agent Setup

### Option 1: Self-Hosted Agent (for on-premises/server deployment)

1. **Create Agent Pool**
   - Go to **Organization Settings** → **Agent pools** → **Add pool**
   - Name it (e.g., `MyServerAgents`)
   - Type: **Self-hosted**

2. **Install Agent on Target Machine**
   ```bash
   # Download agent (Linux example)
   cd ~
   mkdir myagent && cd myagent
   wget https://vstsagentpackage.azureedge.net/agent/2.220.0/vsts-agent-linux-x64-2.220.0.tar.gz
   tar zxvf vsts-agent-linux-x64-2.220.0.tar.gz
   
   # Configure agent
   ./config.sh
   ```
   
   During configuration:
   - Enter your Azure DevOps organization URL
   - Enter PAT (Personal Access Token) with Agent Pool permissions
   - Select the agent pool you created
   - Choose agent name
   - Select work folder
   - Run as service: Yes (recommended)

3. **Install Docker on Agent Machine**
   ```bash
   # For Ubuntu/Debian
   curl -fsSL https://get.docker.com -o get-docker.sh
   sudo sh get-docker.sh
   sudo usermod -aG docker $USER
   ```

4. **Start Agent Service**
   ```bash
   sudo ./svc.sh install
   sudo ./svc.sh start
   ```

### Option 2: Microsoft-Hosted Agents (for cloud deployments)

- No setup required! Just use `vmImage: 'ubuntu-latest'` in your pipeline
- Docker is pre-installed on Microsoft-hosted agents

## Pipeline Configuration

### Step 1: Update Pipeline Variables

Edit `azure-pipelines.yml` and update these variables:

```yaml
variables:
  dockerRegistryServiceConnection: 'YOUR_ACR_SERVICE_CONNECTION_NAME'  # Replace with your service connection name
  containerRegistry: 'YOUR_ACR_NAME.azurecr.io'  # Replace with your ACR name
  azureSubscription: 'YOUR_AZURE_SERVICE_CONNECTION_NAME'  # Replace with your Azure service connection
  appServiceName: 'YOUR_APP_SERVICE_NAME'  # Replace with your App Service name
  resourceGroup: 'YOUR_RESOURCE_GROUP_NAME'  # Replace with your resource group
  agentPool: 'YOUR_AGENT_POOL_NAME'  # Replace with your agent pool name (for agent deployment)
```

### Step 2: Create Pipeline in Azure DevOps

1. **Create New Pipeline**
   - Go to **Pipelines** → **New pipeline** → **Azure Repos Git** (or your source control)
   - Select your repository
   - Choose **Existing Azure Pipelines YAML file**
   - Select `azure-pipelines.yml` from the root directory
   - Click **Run**

2. **Configure Pipeline Permissions**
   - Grant necessary permissions to service connections
   - Approve pipeline if it's the first run

### Step 3: Environment Setup (for approval gates)

1. **Create Environment**
   - Go to **Pipelines** → **Environments** → **Create environment**
   - Name it `Production` (or match your pipeline environment name)
   - Add approval checks if needed

## Deployment Options

### Option 1: Service Connection Deployment (Recommended for Azure)

**File**: `azure-pipelines-service-connection.yml`

- Deploys directly to Azure App Service
- Uses Azure service connections for authentication
- No manual intervention required
- Best for production Azure deployments

**Use when:**
- Deploying to Azure App Service
- You want managed Azure services
- You need Azure-specific features (slots, scaling, etc.)

### Option 2: Agent-Based Deployment

**File**: `azure-pipelines-agent-deployment.yml`

- Deploys to a server/VM running an Azure DevOps agent
- Full control over the deployment environment
- Can deploy to on-premises servers
- Requires agent installation and maintenance

**Use when:**
- Deploying to on-premises servers
- Deploying to VMs with specific configurations
- You need custom deployment scripts
- You want to control the deployment infrastructure

### Option 3: Combined Pipeline

**File**: `azure-pipelines.yml`

- Includes both deployment methods
- Can deploy to multiple environments
- More comprehensive CI/CD workflow

## Pipeline Stages Overview

### Stage 1: Build
- Restores NuGet packages
- Builds .NET application
- Runs unit tests
- Publishes artifacts

### Stage 2: BuildDocker
- Builds Docker image from Dockerfile
- Tags image with build ID and 'latest'
- Pushes to Azure Container Registry

### Stage 3: DeployToAzure
- Deploys Docker container to Azure App Service
- Uses Azure service connection
- Configures container settings automatically

### Stage 4: DeployWithAgent
- Pulls Docker image on agent machine
- Stops existing container
- Runs new container with updated image
- Verifies deployment

## Troubleshooting

### Service Connection Issues

1. **Authentication Failed**
   - Check service connection permissions
   - Verify service principal has correct roles
   - Regenerate service connection if needed

2. **Container Registry Access Denied**
   - Verify ACR admin user is enabled (if using admin credentials)
   - Check service principal permissions in ACR
   - Verify image repository name matches

### Agent Deployment Issues

1. **Agent Not Found**
   - Verify agent pool name is correct
   - Check agent is online in Agent pools
   - Restart agent service if needed

2. **Docker Not Available**
   - Ensure Docker is installed on agent machine
   - Check agent user has Docker permissions
   - Verify Docker service is running

3. **Port Already in Use**
   - Change `hostPort` variable in pipeline
   - Stop existing container manually
   - Check for other services using the port

### General Issues

1. **Pipeline Fails on Build**
   - Check .NET SDK version matches project
   - Verify all dependencies are available
   - Review build logs for specific errors

2. **Container Won't Start**
   - Check container logs: `docker logs mywebapp`
   - Verify environment variables
   - Ensure port mappings are correct

## Next Steps

1. ✅ Update all variable placeholders in pipeline YAML
2. ✅ Create required service connections
3. ✅ Set up agent pool (if using agent deployment)
4. ✅ Run pipeline and verify deployment
5. ✅ Configure branch policies and approvals
6. ✅ Set up monitoring and alerts

## Additional Resources

- [Azure DevOps Pipelines Documentation](https://docs.microsoft.com/en-us/azure/devops/pipelines/)
- [Azure Container Registry Documentation](https://docs.microsoft.com/en-us/azure/container-registry/)
- [Azure App Service Documentation](https://docs.microsoft.com/en-us/azure/app-service/)
- [Self-Hosted Agents Guide](https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/agents)

## Support

For issues specific to your deployment, check:
- Azure DevOps pipeline logs
- Azure App Service logs (Application Insights)
- Container logs (`docker logs`)
- Agent machine logs

