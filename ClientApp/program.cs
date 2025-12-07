using Microsoft.Azure.Devices.Client;
using System.Text;
using Newtonsoft.Json;
using System.Management;
using System.Threading.Tasks;
using Microsoft.Azure.Amqp.Framing;



public class program
{

public const string BATTERY = "Battery";
public Double Battery;
    static async Task Main(string[] args)
        {
            
            Console.WriteLine("Resource Monitor Client starting...");
            program app = new program();

            while(true){
                String percent = app.BatteryStatus();
                Console.Write(percent + "\n");
                await Task.Delay(5000);
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

    public String BatteryStatus(){
        String elements =  "BatteryStatus, EstimatedChargeRemaining, EstimatedRunTime, TimeOnBattery";
        String[] el =  ["BatteryStatus", "EstimatedChargeRemaining", "EstimatedRunTime", "TimeOnBattery"];
        List<string> result = new List<string>();
        
        ManagementObjectCollection dataCollection  = this.findIt(BATTERY , string.Join(", " , el));
        // ManagementObjectCollection dataCollection  = this.findIt(BATTERY , "EstimatedChargeRemaining");
        double batteryPercent = 0;
        foreach(ManagementObject batt in dataCollection){
            if (batt[el[0]] != null)
            {
                //BatteryStatus
                
                if(Convert.ToInt16(batt[el[0]]) == 1)
                {
                result.Add("Discahrging");
                }else if(Convert.ToInt16(batt[el[0]]) == 2)
                {
                result.Add("Charching");
                }
                else
                {
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
                result.Add(Convert.ToString(batt[el[1]]));
            }
            else
            {
                result.Add("-1");
            }
            if (batt[el[2]] != null)
            {
                //EstimatedRunTime
                result.Add(Convert.ToString(batt[el[2]]));
            }
            else
            {
                result.Add("-1");
            }
            if (batt[el[3]] != null)
            {
                //TimeOnBattery
                result.Add(Convert.ToString(batt[el[3]]));
            }
            else
            {
                result.Add("-1");
            }
            

            }
            return string.Join(", " , result);
    }



}