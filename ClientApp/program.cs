using Microsoft.Azure.Devices.Client;
using System.Text;
using Newtonsoft.Json;
using System.Management;
using System.Threading.Tasks;
using Microsoft.Azure.Amqp.Framing;
using System.Runtime.CompilerServices;




public class program
{
    public List<string> ramProperties = new List<string> 
{ 
    "FreePhysicalMemory",      // Current free RAM (KB)
    "TotalVisibleMemorySize",  // Total RAM installed (KB)
    "TotalVirtualMemorySize"   // RAM + Page file (KB)
};
    public List<string> batteryProperties = new List<string> 
{ 
    "BatteryStatus", // Code (Charging/Discharging)
    "EstimatedChargeRemaining", // Percentage %
    "EstimatedRunTime",         // Minutes left
    "TimeOnBattery",            // Seconds on battery
    // "DesignCapacity",           // New capacity
    // "FullChargeCapacity",       // Current max capacity
    // "CycleCount",               // Wear count
    // "Manufacturer"              // Vendor name
};
    public List<string> cpuProperties = new List<string> 
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

List<string> diskProperties = new List<string> 
{ 
    "DeviceID",                // e.g., C:
    "FreeSpace",               // Available bytes
    "Size",                    // Total bytes
    "FileSystem"               // e.g., NTFS
};





public const string BATTERY = "Battery";
public const string RAM = "OperatingSystem";

public const string DISK = "LogicalDisk";

public const string CPU = "Processor";


private static DatabaseManager db = new DatabaseManager();
public Double Battery;
    static async Task Main(string[] args)
        {
            
            db.InitializeDatabase();
            Console.WriteLine("Resource Monitor Client starting...");
            program app = new program();

            while(true){
                Console.WriteLine(DateTime.UtcNow);
                app.BatteryStatus();
                app.cpuStatus();
                app.ramStatus();
                app.diskStatus();
                
                
                await Task.Delay(50000);

                // Console.Write("end");

            }

            


            }


    public ManagementObjectCollection findIt( String library, String element)
    {   
        String win32 = "Win32_" + library;
        String str = $"SELECT {element} FROM {win32}";
        ManagementObjectSearcher searcher = 
                new ManagementObjectSearcher(str);
        return searcher.Get();
    }

    public void BatteryStatus(){
        BatteryObject battery = new BatteryObject();
        String[] el =  ["BatteryStatus", "EstimatedChargeRemaining", "EstimatedRunTime", "TimeOnBattery"];
        List<string> result = new List<string>();
        battery.Timestamp = DateTime.UtcNow;
        ManagementObjectCollection dataCollection  = this.findIt(battery.GetWinName() , string.Join(", " , battery.batteryProperties));
        foreach(ManagementObject batt in dataCollection){
            if (batt[el[0]] != null)
            {
                //BatteryStatus
                if(Convert.ToInt16(batt[el[0]]) == 1)
                {
                    battery.Status = "Discharging";
                    result.Add("Discahrging");
                }else if(Convert.ToInt16(batt[el[0]]) == 2)
                {
                    battery.Status = "Charging";
                    result.Add("Charching");
                }
                else
                {
                    battery.Status = "No Info";
                    result.Add("-1");
                }
            }
            else
            {
                result.Add("-1");
            }
            if (batt[el[1]] != null)
            {
                //EstimatedChargeRemaining
                battery.EstimatedChargeRemaining = Convert.ToString(batt[el[1]]);
                result.Add(Convert.ToString(batt[el[1]]));
            }
            else
            {
                battery.EstimatedChargeRemaining = "no Info";
                result.Add("-1");
            }
            if (batt[el[2]] != null)
            {
                //EstimatedRunTime
                battery.EstimatedRunTime =Convert.ToString(batt[el[2]]); 
                result.Add(Convert.ToString(batt[el[2]]));
            }
            else
            {
                battery.EstimatedRunTime = "no Info";
                result.Add("-1");
            }
            if (batt[el[3]] != null)
            {
                //TimeOnBattery
                battery.TimeOnBattery = Convert.ToString(batt[el[3]]); 
                result.Add(Convert.ToString(batt[el[3]]));
            }
            else
            {
                battery.TimeOnBattery = "no Info";
                result.Add("-1");
            }
            

            }
            db.LogBattery(battery);
            Console.WriteLine(string.Join(", " , batteryProperties));
            Console.WriteLine(string.Join(", " , result));
                
    }

    public void cpuStatus(){
        String[] el =   ["LoadPercentage",         // Current usage %
                        "CurrentClockSpeed",      // Current speed (MHz)
                        "CpuStatus",
                        "Name",                   // Full model name
                        "NumberOfCores",          // Physical cores
                        "NumberOfLogicalProcessors", // Threads
                        "Architecture",           // x64, x86, ARM64
                        "Manufacturer",           // Intel, AMD
                        "MaxClockSpeed",          // Max design speed
                        ];

        List<string> result = new List<string>();

        CpuObject cpu = new CpuObject();
        cpu.Timestamp = DateTime.UtcNow;
        ManagementObjectCollection dataCollection  = this.findIt(cpu.GetWinName() , string.Join(", " , cpu.Properties));
        foreach(ManagementObject batt in dataCollection){
            if (batt[el[0]] != null)
            {
                cpu.LoadPercentage = Convert.ToString(batt[el[0]]);
            }
            else
            {
                cpu.LoadPercentage = "-1";
                result.Add("-1");
            }
            if (batt[el[1]] != null)
            {
                cpu.CurrentClockSpeed = Convert.ToString(batt[el[1]]);
                result.Add(Convert.ToString(batt[el[1]]));
            }
            else
            {
                cpu.CurrentClockSpeed = "no Info";
                result.Add("-1");
            }
            if (batt[el[2]] != null)
            {
                //EstimatedRunTime
                cpu.Name =Convert.ToString(batt[el[2]]); 
                result.Add(Convert.ToString(batt[el[2]]));
            }
            else
            {
                cpu.Name = "no Info";
                result.Add("-1");
            }
            if (batt[el[3]] != null)
            {
                //TimeOnBattery
                cpu.Status = Convert.ToString(batt[el[3]]); 
                result.Add(Convert.ToString(batt[el[3]]));
            }
            else
            {
                cpu.Status = "no Info";
                result.Add("-1");
            }
            

            }
            db.LogCpu(cpu);
            Console.WriteLine(string.Join(", " , cpuProperties));
            Console.WriteLine(string.Join(", " , result));
    }
    public void diskStatus(){
        String[] el =  [
    "DeviceID",                // e.g., C:
    "FreeSpace",               // Available bytes
    "Size",                    // Total bytes
    "FileSystem" ];
        List<string> result = new List<string>();
        DiskObject disk = new DiskObject();
        disk.Timestamp = DateTime.UtcNow;
        
        ManagementObjectCollection dataCollection  = this.findIt( disk.GetWinName(), string.Join(", " , disk.Properties));
        
        foreach(ManagementObject batt in dataCollection){
            if (batt[el[0]] != null)
            {
                //BatteryStatus
                disk.DeviceID = Convert.ToString(batt[disk.Properties[0]]);   
                result.Add(Convert.ToString(batt[el[0]]));
            }
            else
            {
                disk.DeviceID = "-1";
                result.Add("-1");
            }
            if (batt[el[1]] != null)
            {
                disk.FreeSpace = Convert.ToString(batt[disk.Properties[1]]);
                result.Add(Convert.ToString(batt[el[1]]));
            }
            else
            {
                disk.FreeSpace = "-1";
                result.Add("-1");
            }
            if (batt[el[2]] != null)
            {
                disk.Size = Convert.ToString(batt[disk.Properties[2]]);
                result.Add(Convert.ToString(batt[el[2]]));
            }
            else
            {
                disk.Size = "-1";
                result.Add("-1");
            }
            if (batt[el[3]] != null)
            {
                disk.FileSystem = Convert.ToString(batt[disk.Properties[3]]);
                result.Add(Convert.ToString(batt[el[3]]));
            }
            else
            {
                disk.Size = "-1";
                result.Add("-1");
            }
            

            }
            // Console.WriteLine(DateTime.UtcNow);
            db.LogDisk(disk);
            Console.WriteLine(string.Join(", " , diskProperties));
            Console.WriteLine(string.Join(", " , result));
    }
    public void ramStatus(){
        String[] el =  [
                        "FreePhysicalMemory",      // Current free RAM (KB)
                        "TotalVisibleMemorySize",  // Total RAM installed (KB)
                        "TotalVirtualMemorySize" ];
        List<string> result = new List<string>();
        RamObject ram = new RamObject();
        ram.Timestamp = DateTime.UtcNow;
        ManagementObjectCollection dataCollection  = this.findIt(ram.GetWinName() , string.Join(", " , ram.Properties));
        foreach(ManagementObject batt in dataCollection){
            if (batt[el[0]] != null)
            {
                ram.FreePhysicalMemory = Convert.ToString(batt[ram.Properties[0]]);
                result.Add(Convert.ToString(batt[el[0]]));
            }
            else
            {
                ram.FreePhysicalMemory = "-1";
                result.Add("-1");
            }
            if (batt[el[1]] != null)
            {
                ram.TotalVirtualMemorySize = Convert.ToString(batt[ram.Properties[1]]);
                result.Add(Convert.ToString(batt[el[1]]));
            }
            else
            {
                ram.TotalVirtualMemorySize = "-1";
                result.Add("-1");
            }
            if (batt[el[2]] != null)
            {
                ram.TotalVisibleMemorySize = Convert.ToString(batt[ram.Properties[2]]);
                result.Add(Convert.ToString(batt[el[2]]));
            }
            else
            {
                ram.TotalVisibleMemorySize = "-1";
                result.Add("-1");
            }

            }
            db.LogRam(ram);
            // Console.WriteLine(DateTime.UtcNow);
            Console.WriteLine(string.Join(", " , ramProperties));
            Console.WriteLine(string.Join(", " , result));
    }
}