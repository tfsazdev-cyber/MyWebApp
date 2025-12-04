# Azure DevOps Pipeline Files Overview

This document describes all the CI/CD pipeline files and documentation created for your .NET Docker application.

## 📄 Pipeline Files

### 1. `azure-pipelines.yml` (Main Pipeline)
**Purpose**: Complete CI/CD pipeline with all deployment options

**Includes**:
- ✅ Build and test .NET application
- ✅ Build Docker image
- ✅ Push to Azure Container Registry
- ✅ Deploy to Azure App Service (Service Connection)
- ✅ Deploy using Azure DevOps Agent

**Use this when**: You want a comprehensive pipeline with all deployment methods

---

### 2. `azure-pipelines-service-connection.yml`
**Purpose**: Simplified pipeline focused on Azure service connection deployment

**Includes**:
- ✅ Build Docker image
- ✅ Push to ACR
- ✅ Deploy to Azure App Service
- ✅ Deployment verification

**Use this when**: You only deploy to Azure App Service and don't need agent-based deployment

---

### 3. `azure-pipelines-agent-deployment.yml`
**Purpose**: Pipeline focused on self-hosted agent deployment

**Includes**:
- ✅ Build Docker image
- ✅ Push to ACR
- ✅ Pull and deploy on agent machine
- ✅ Container health checks

**Use this when**: You deploy to on-premises servers or VMs with self-hosted agents

---

## 📚 Documentation Files

### 4. `AZURE_DEVOPS_SETUP.md`
**Purpose**: Comprehensive setup guide with detailed instructions

**Contains**:
- Prerequisites
- Step-by-step service connection setup
- Agent installation instructions
- Pipeline configuration guide
- Troubleshooting section

**Use this for**: Detailed setup instructions and troubleshooting

---

### 5. `QUICK_START.md`
**Purpose**: Quick reference guide for fast setup

**Contains**:
- Quick setup steps (5-15 minutes)
- Common variables table
- Verification commands

**Use this for**: Fast setup when you're familiar with Azure DevOps

---

## 🛠️ Supporting Files

### 6. `.dockerignore`
**Purpose**: Optimize Docker build by excluding unnecessary files

**Excludes**:
- Build artifacts (bin/, obj/)
- IDE files (.vs/, .vscode/)
- Git files
- Documentation files
- Temporary files

**Benefits**: Faster builds, smaller build context, more secure builds

---

## 🚀 Getting Started

### Option 1: Complete Pipeline (Recommended for First Time)
1. Follow `QUICK_START.md` for fast setup
2. Use `azure-pipelines.yml` as your pipeline file
3. Configure all variables as shown in the guide

### Option 2: Service Connection Only
1. Set up ACR and Azure service connections
2. Use `azure-pipelines-service-connection.yml`
3. Update variables for your environment

### Option 3: Agent Deployment Only
1. Set up self-hosted agent
2. Use `azure-pipelines-agent-deployment.yml`
3. Configure agent pool name in variables

## 📋 Required Service Connections

Before running any pipeline, you need:

1. **Azure Container Registry Service Connection**
   - Type: Docker Registry → Azure Container Registry
   - Used for: Pushing Docker images

2. **Azure Service Connection** (for service connection deployment)
   - Type: Azure Resource Manager
   - Used for: Deploying to Azure App Service

## 🔧 Required Variables

All pipelines need these variables updated (see `QUICK_START.md` for examples):

- `dockerRegistryServiceConnection`: ACR service connection name
- `containerRegistry`: ACR URL (e.g., `myregistry.azurecr.io`)
- `imageRepository`: Container image name
- `azureSubscription`: Azure service connection name (for App Service deployment)
- `appServiceName`: Azure App Service name
- `resourceGroup`: Azure Resource Group name
- `agentPool`: Agent pool name (for agent-based deployment)

## ✅ Next Steps

1. ✅ Choose your pipeline file
2. ✅ Set up service connections (see `AZURE_DEVOPS_SETUP.md`)
3. ✅ Update variables in pipeline YAML
4. ✅ Create pipeline in Azure DevOps
5. ✅ Run and verify deployment

## 📞 Need Help?

- Check `AZURE_DEVOPS_SETUP.md` for detailed instructions
- Review `QUICK_START.md` for quick reference
- Check Azure DevOps pipeline logs for errors
- Verify service connection permissions

---

**Happy Deploying! 🚀**

