# 📡 DeviceFlow Monitor (In Development)

DeviceFlow Monitor is a real-time, cloud-hosted dashboard for visualizing resource usage from multiple devices.  
Built with **.NET 8**, **Azure SignalR**, and **React/TypeScript**, the project demonstrates how to stream live telemetry—CPU load, memory usage, battery, and online status—directly to a responsive web interface.

The current version uses **simulated telemetry** to show the complete SignalR/WebSocket flow.  
Future updates will include lightweight device agents that send real system metrics to the backend.

## 🚀 Features
- Real-time WebSocket updates (Azure SignalR)
- Multi-device simulated telemetry
- React + TypeScript + Material-UI frontend
- Azure App Service deployment
- GitHub Actions CI/CD pipeline

## 🛠️ Tech Stack
- **Backend:** .NET 8, ASP.NET Core, SignalR
- **Frontend:** React, TypeScript, Material-UI
- **Cloud:** Azure App Services, Azure SignalR Service
- **DevOps:** GitHub Actions
