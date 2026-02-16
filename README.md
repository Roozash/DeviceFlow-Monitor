# 📡 DeviceFlow Monitor (Python Agent)

DeviceFlow Monitor is a lightweight, cross-platform system monitoring agent built with **Python**. It collects real-time system metrics (CPU, Memory, Disk, Network) and transmits them to a cloud backend via **AWS API Gateway**.

## 🚀 Features

*   **Real-time Monitoring**: Captures system performance metrics every few seconds.
*   **Resilient Data Transmission**:
    *   Buffering: Data is stored locally in a **SQLite** database (`device_flow_monitor.db`) if the network is unavailable or the API is unreachable.
    *   Retry Logic: Automatically attempts to resend buffered metrics when the connection is restored.
*   **Cross-Platform**: Runs on Windows, Linux, and macOS (powered by `psutil`).
*   **Cloud Native**: Designed to feed data into a serverless AWS backend.

## 🛠️ Tech Stack

*   **Language**: Python 3.x
*   **Libraries**:
    *   `psutil`: System monitoring
    *   `requests`: HTTP API communication
    *   `sqlite3`: Local data persistence
*   **Cloud Integration**: AWS (API Gateway -> Lambda -> DynamoDB/TimeStream)

## 📦 Setup & Usage

### 1. Prerequisites
*   Python 3.8 or higher installed.

### 2. Installation
Navigate to the `ClientApp` directory and install the required dependencies:

```bash
cd ClientApp
pip install -r requirements.txt
```

### 3. Configuration
Open `ClientApp/app.py` and configure your AWS endpoint:

```python
API_URL = "https://your-api-id.execute-api.region.amazonaws.com/stage/resource"
API_KEY = "your-api-key-here"
```

### 4. Running the Agent
Start the monitor:

```bash
python app.py
```

The agent will begin collecting metrics and attempting to send them to the configured API URL. Check the console output for status logs.
