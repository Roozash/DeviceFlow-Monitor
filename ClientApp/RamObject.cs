using System;



public class RamObject

{
    
    public const string WinName = "OperatingSystem";
    
        public string GetWinName()
    {
        return WinName;
    }

     public List<string> Properties = new List<string> 
{ 
    "FreePhysicalMemory",      // Current free RAM (KB)
    "TotalVisibleMemorySize",  // Total RAM installed (KB)
    "TotalVirtualMemorySize"   // RAM + Page file (KB)
};

    public DateTime Timestamp { get; set; }
    public string FreePhysicalMemory { get; set; }
    public string TotalVirtualMemorySize { get; set; }
    public string TotalVisibleMemorySize { get; set; }
    public string Status { get; set; }


}