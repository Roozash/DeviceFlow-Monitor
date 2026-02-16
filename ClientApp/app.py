# pip install psutil
import psutil
import db   # import db.py (DatabaseManager)
import time
import requests
from datetime import datetime 

API_URL = "https://yec9vfpyyk.execute-api.eu-north-1.amazonaws.com/Device_monitor_API"
API_KEY = "<I have no idea what to put here now so just this for now>"
TIMEOUT_SEC = 10

def get_memory_metrics():
    vm = psutil.virtual_memory()
    # vm.total, vm.available, vm.used are in bytes
    return {
        "ram_total_mb": round(vm.total / (1024 * 1024), 2),
        "ram_used_mb": round(vm.used / (1024 * 1024), 2),
        "ram_available_mb": round(vm.available / (1024 * 1024), 2),
        "ram_percent": vm.percent,
    }

def get_cpu_metrics():
    v1 = psutil.cpu_percent(interval=1)
    v2 = psutil.cpu_times()
    v3 = psutil.cpu_freq()
    v4 = psutil.cpu_count()
    v5 = psutil.cpu_stats()
    # vm.total, vm.available, vm.used are in bytes
    return {
        "cpu_percent": v1,
        "cpu_times": v2,
        "cpu_freq": v3,
        # "cpu_count": v4,
        # "cpu_stats": v5,
    }

def get_disk_metrics():
    v1 = psutil.disk_usage('/')
    v2 = psutil.disk_partitions()
    v3 = psutil.disk_io_counters()
    # vm.total, vm.available, vm.used are in bytes
    return {
        "disk_usage": v1,
        # "disk_partitions": v2,
        # "disk_io_counters": v3,
        
    }
    
def get_network_metrics():
    v1 = psutil.net_io_counters()
    v2 = psutil.net_if_addrs()
    v3 = psutil.net_connections()
    v4 = psutil.net_if_stats()
    # vm.total, vm.available, vm.used are in bytes
    return {
        "net_counters": v1,
        # "net_if_addrs": v2,
        # "net_connections": v3,
        # "net_if_stats": v4,
    }

def get_sensors_metrics():
    # Some problem with this function , 
    # it seems it is in the library but it is not working
    v1 = psutil.sensors_temperatures()
    v2 = psutil.sensors_fans()
    return {
        "sensors_temperatures": v1,
        "sensors_fans": v2,
    }


def get_battery_metrics():
    v1 = psutil.sensors_battery()
    # vm.total, vm.available, vm.used are in bytes  
    return {
        "battery": v1,
    }


def get_metrics_json():
    # Collect all metrics
    metrics = {
        "memory": get_memory_metrics(),
        "cpu": get_cpu_metrics(),
        "disk": get_disk_metrics(),
        "network": get_network_metrics(),
        # "sensors": get_sensors_metrics() # Uncomment if supported
    }

    # Serialize to JSON
    payload_json = json.dumps(metrics)
    
    ts_ms = int(time.time() * 1000)
    with open("sample.json", "w", encoding="utf-8") as f:
        f.write(payload_json)
    f.close()
    return ts_ms, payload_json
    
def post_sample(sample: str , seq: int, ts_ms: int) -> bool:
    try:
        r = requests.post(
            API_URL,
            data=sample,
            params={
                "seq": seq,
                "ts_ms": ts_ms,
            },
            headers={"X-API-Key": API_KEY},
            timeout=TIMEOUT_SEC,
        )
        if r.status_code != 200:
            return False

        data = r.json()
        return data.get("ok") is True and data.get("errors", 0) == 0
    except requests.RequestException:
        return False
    except ValueError:
        # response wasn't JSON
        return False




import json



def main():
    # Instantiate the class
    db_mgr = db.DatabaseManager("device_flow_monitor.db")
    db_mgr.init_db()
    seq = 0
    while True:
        seq += 1
        try:
            ts_ms, payload_json = get_metrics_json()
        except Exception as e:
            print(seq, "Error getting metrics:", e)
            return

        if post_sample(payload_json, seq, ts_ms):
            print(seq, "Posted sample successfully")
        else:
            print(seq, "Failed to post sample")
            print(seq, "Saving metrics at", ts_ms)
            try:
                db_mgr.save_metrics("device_001", ts_ms, seq, payload_json)
                print(seq, "Metrics saved successfully.")
            except Exception as e:
                print(seq, "Error saving metrics:", e)
            
        # Save to DB

        time.sleep(5)



if __name__ == "__main__":
    main()
