using System.Text;
using System.Management;
using System.Collections.ObjectModel;




public class program
{

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
            }

            


            }


    public ManagementObjectCollection findIt(String library, String element)
    {   
        Dictionary<string,string> inf = new Dictionary<string, string>();
        String win32 = "Win32_" + library;
        String str = $"SELECT {element} FROM {win32}";
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(str);
        foreach(String s in element.Split(","))
        {
            inf.Add(s,"no info"); 
        }
        
        return searcher.Get();
    }

    
    
    
    
    
    public void BatteryStatus(){
        BatteryObject battery = new BatteryObject();

        battery.Timestamp = DateTime.UtcNow;

        ManagementObjectCollection dataCollection  = this.findIt(battery.GetWinName() , string.Join(", " , battery.Properties));
        
        foreach(ManagementObject batt in dataCollection){
            if (batt[battery.Properties[0]] != null)
            {
                //BatteryStatus
                if(Convert.ToInt16(batt[battery.Properties[0]]) == 1)
                {
                    battery.Status = "Discharging";
    
                }else if(Convert.ToInt16(batt[battery.Properties[0]]) == 2)
                {
                    battery.Status = "Charging";
                    }
                else
                {
                    battery.Status = "No Info";
                    }
            }
            else
            {
                battery.Status = "-1";
            }
            if (batt[battery.Properties[1]] != null)
            {
                //EstimatedChargeRemaining
                battery.EstimatedChargeRemaining = Convert.ToString(batt[battery.Properties[1]]);
            }
            else
            {
                battery.EstimatedChargeRemaining = "no Info";
            }
            if (batt[battery.Properties[2]] != null)
            {
                //EstimatedRunTime
                battery.EstimatedRunTime =Convert.ToString(batt[battery.Properties[2]]); 
            }
            else
            {
                battery.EstimatedRunTime = "no Info";
            }
            if (batt[battery.Properties[3]] != null)
            {
                //TimeOnBattery
                battery.TimeOnBattery = Convert.ToString(batt[battery.Properties[3]]); 
            }
            else
            {
                battery.TimeOnBattery = "no Info";
            }
            

            }
            db.LogBattery(battery);
            Console.WriteLine(string.Join(", " , battery.Properties));
                
    }

    public void cpuStatus(){


        List<string> result = new List<string>();

        CpuObject cpu = new CpuObject();
        cpu.Timestamp = DateTime.UtcNow;
        ManagementObjectCollection dataCollection  = this.findIt(cpu.GetWinName() , string.Join(", " , cpu.Properties));
        foreach(ManagementObject batt in dataCollection){
            if (batt[cpu.Properties[0]] != null)
            {
                cpu.LoadPercentage = Convert.ToString(batt[cpu.Properties[0]]);
            }
            else
            {
                cpu.LoadPercentage = "-1";
                result.Add("-1");
            }
            if (batt[cpu.Properties[1]] != null)
            {
                cpu.CurrentClockSpeed = Convert.ToString(batt[cpu.Properties[1]]);
            }
            else
            {
                cpu.CurrentClockSpeed = "no Info";
                result.Add("-1");
            }
            if (batt[cpu.Properties[2]] != null)
            {
                //EstimatedRunTime
                cpu.Name =Convert.ToString(batt[cpu.Properties[2]]); 
            }
            else
            {
                cpu.Name = "no Info";
                result.Add("-1");
            }
            if (batt[cpu.Properties[3]] != null)
            {
                //TimeOnBattery
                cpu.Status = Convert.ToString(batt[cpu.Properties[3]]); 
                result.Add(Convert.ToString(batt[cpu.Properties[3]]));
            }
            else
            {
                cpu.Status = "no Info";
                result.Add("-1");
            }
            

            }
            db.LogCpu(cpu);
            Console.WriteLine(string.Join(", " , cpu.Properties));
    }
    public void diskStatus(){

        List<string> result = new List<string>();
        DiskObject disk = new DiskObject();
        disk.Timestamp = DateTime.UtcNow;
        
        ManagementObjectCollection dataCollection  = this.findIt( disk.GetWinName(), string.Join(", " , disk.Properties));
        
        foreach(ManagementObject batt in dataCollection){
            if (batt[disk.Properties[0]] != null)
            {
                //BatteryStatus
                disk.DeviceID = Convert.ToString(batt[disk.Properties[0]]);   
            }
            else
            {
                disk.DeviceID = "-1";
                result.Add("-1");
            }
            if (batt[disk.Properties[1]] != null)
            {
                disk.FreeSpace = Convert.ToString(batt[disk.Properties[1]]);
            }
            else
            {
                disk.FreeSpace = "-1";
                result.Add("-1");
            }
            if (batt[disk.Properties[2]] != null)
            {
                disk.Size = Convert.ToString(batt[disk.Properties[2]]);
            }
            else
            {
                disk.Size = "-1";
                result.Add("-1");
            }
            if (batt[disk.Properties[3]] != null)
            {
                disk.FileSystem = Convert.ToString(batt[disk.Properties[3]]);
            }
            else
            {
                disk.Size = "-1";
                result.Add("-1");
            }
            

            }
            // Console.WriteLine(DateTime.UtcNow);
            db.LogDisk(disk);
            Console.WriteLine(string.Join(", " , disk.Properties));
    }
    public void ramStatus(){

        List<string> result = new List<string>();
        RamObject ram = new RamObject();
        ram.Timestamp = DateTime.UtcNow;
        ManagementObjectCollection dataCollection  = this.findIt(ram.GetWinName() , string.Join(", " , ram.Properties));
        foreach(ManagementObject batt in dataCollection){
            if (batt[ram.Properties[0]] != null)
            {
                ram.FreePhysicalMemory = Convert.ToString(batt[ram.Properties[0]]);
                // result.Add(Convert.ToString(batt[el[0]]));
            }
            else
            {
                ram.FreePhysicalMemory = "-1";
                result.Add("-1");
            }
            if (batt[ram.Properties[1]] != null)
            {
                ram.TotalVirtualMemorySize = Convert.ToString(batt[ram.Properties[1]]);
                // result.Add(Convert.ToString(batt[el[1]]));
            }
            else
            {
                ram.TotalVirtualMemorySize = "-1";
                result.Add("-1");
            }
            if (batt[ram.Properties[2]] != null)
            {
                ram.TotalVisibleMemorySize = Convert.ToString(batt[ram.Properties[2]]);
                // result.Add(Convert.ToString(batt[el[2]]));
            }
            else
            {
                ram.TotalVisibleMemorySize = "-1";
                result.Add("-1");
            }

            }
            db.LogRam(ram);

            // Console.WriteLine(DateTime.UtcNow);
            Console.WriteLine(string.Join(", " , ram.Properties));
            
    }
}