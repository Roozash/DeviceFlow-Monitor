using System;



public class DiskObject
{
    
    public const string WinName = "LogicalDisk";
    
        public string GetWinName()
    {
        return WinName;
    }

public List<string> Properties = new List<string> 
{ 
    "DeviceID",                // e.g., C:
    "FreeSpace",               // Available bytes
    "Size",                    // Total bytes
    "FileSystem"               // e.g., NTFS
};


    public DateTime Timestamp { get; set; }
    public string DeviceID { get; set; }
    public string FreeSpace { get; set; }
    public string Size { get; set; }
    public string FileSystem { get; set; }


}