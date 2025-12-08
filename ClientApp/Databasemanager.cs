using Microsoft.Data.Sqlite;
using System;
using System.Threading.Tasks.Dataflow;
public class DatabaseManager
    {
        private string _connectionString = "Data Source=DeviceMonitor.db";

        // This runs once to make sure the "cabinet" exists
        public void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                Console.WriteLine("db created ");
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS BatteryLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Timestamp DATETIME,
                        ChargePercentage TEXT,
                        Status TEXT
                    );
                    CREATE TABLE IF NOT EXISTS CpuLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Timestamp DATETIME,
                        LoadPercentage TEXT,
                        Status TEXT
                    );
                    CREATE TABLE IF NOT EXISTS RamLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Timestamp DATETIME,
                        FreePhysicalMemory TEXT,
                        TotalVirtualMemorySize TEXT
                    );
                    CREATE TABLE IF NOT EXISTS DiskLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Timestamp DATETIME,
                        FreeSpace TEXT,
                        DeviceID TEXT
                    );";
                command.ExecuteNonQuery();
            }
        }

        // Specific method to save Battery data
        public void LogBattery(BatteryObject obj)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO BatteryLog (Timestamp, ChargePercentage, Status) VALUES (@time, @charge, @status)";
                command.Parameters.AddWithValue("@time", obj.Timestamp);
                command.Parameters.AddWithValue("@charge", obj.EstimatedChargeRemaining);
                command.Parameters.AddWithValue("@status", obj.Status);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Rows inserted: {rowsAffected} into {connection.DataSource}");
            }
        }
        public void LogCpu(CpuObject obj)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO CpuLog (Timestamp, LoadPercentage, Status) VALUES (@time, @load, @status)";
                command.Parameters.AddWithValue("@time", obj.Timestamp);
                command.Parameters.AddWithValue("@load", obj.LoadPercentage);
                command.Parameters.AddWithValue("@status", obj.Status);
                command.ExecuteNonQuery();
            }
        }
        public void LogRam(RamObject obj)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO RamLog (Timestamp, FreePhysicalMemory, TotalVirtualMemorySize) VALUES (@time, @phyMemory, @vMemory)";
                command.Parameters.AddWithValue("@time", obj.Timestamp);
                command.Parameters.AddWithValue("@phyMemory", obj.FreePhysicalMemory);
                command.Parameters.AddWithValue("@vMemory", obj.TotalVirtualMemorySize);
                command.ExecuteNonQuery();
            }
        }
        public void LogDisk(DiskObject obj)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO DiskLog (Timestamp, FreeSpace, DeviceID) VALUES (@time, @space, @deviceID)";
                command.Parameters.AddWithValue("@time", obj.Timestamp);
                command.Parameters.AddWithValue("@space", obj.FreeSpace);
                command.Parameters.AddWithValue("@deviceID", obj.DeviceID);
                command.ExecuteNonQuery();
            }
        }
    }

