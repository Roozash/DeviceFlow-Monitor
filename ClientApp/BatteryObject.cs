using System;



public class BatteryObject
{
    
    public const string WinName = "Battery";

    public string GetWinName()
    {
        return WinName;
    }
    
    public List<string> batteryProperties = new List<string> 
{ 
    "EstimatedChargeRemaining", // Percentage %
    "BatteryStatus",            // Code (Charging/Discharging)
    "EstimatedRunTime",         // Minutes left
    "TimeOnBattery",            // Seconds on battery
    // "DesignCapacity",           // New capacity
    // "FullChargeCapacity",       // Current max capacity
    // "CycleCount",               // Wear count
    // "Manufacturer"              // Vendor name
};

    public DateTime Timestamp { get; set; }
    public string EstimatedChargeRemaining { get; set; }
    public string EstimatedRunTime { get; set; }
    public string TimeOnBattery { get; set; }
    public string Status { get; set; }


}