using System;



public class CpuObject

{
    
    public const string WinName = "Processor";
        public string GetWinName()
    {
        return WinName;
    }

    public List<string> Properties = new List<string> 
{ 
    "LoadPercentage",         // Current usage %
    "CurrentClockSpeed",      // Current speed (MHz)
    "Name",                   // Full model name
    "NumberOfCores",          // Physical cores
    "NumberOfLogicalProcessors", // Threads
    "Architecture",           // x64, x86, ARM64
    "Manufacturer",           // Intel, AMD
    "MaxClockSpeed",          // Max design speed
    "CpuStatus"               // Health status code
};
    public DateTime Timestamp { get; set; }
    public string LoadPercentage { get; set; }
    public string CurrentClockSpeed { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }


}