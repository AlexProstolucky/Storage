using System.Data;
using System.Data.SQLite;
internal class Program
{
    private static async Task Main(string[] args)
    {
        string ConnectionString = "Data Source=Storage.db;Version=3;";

        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.OpenAsync();

            using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS ProductTypes (TypeId INTEGER PRIMARY KEY, TypeName TEXT NOT NULL);", connection))
            {
                await command.ExecuteNonQueryAsync();
            }

            using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Providers (ProviderId INTEGER PRIMARY KEY, ProviderName TEXT NOT NULL);", connection))
            {
                await command.ExecuteNonQueryAsync();
            }

            using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Storage (ProductId INTEGER PRIMARY KEY, ProductName TEXT NOT NULL, ProductType INTEGER, ProviderId INTEGER, Quantity INTEGER, Cost DECIMAL(10,2), SupplyDate DATE, FOREIGN KEY (ProductType) REFERENCES ProductTypes(TypeId), FOREIGN KEY (ProviderId) REFERENCES Providers(ProviderId));", connection))
            {
                await command.ExecuteNonQueryAsync();
            }

            Console.WriteLine("DATA BASE CREATED");
        }
        using (var connection = new SQLiteConnection(ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                Console.WriteLine("Сonnection is successful\n");

                Console.WriteLine("ALL INFOMATIN:\n\n");
                string code = "SELECT * FROM Storage";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("ProductId | ProductName | ProductType | ProviderId | Quantity | Cost | SupplyDate");
                        Console.WriteLine("---------------------------------------------------------------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["ProductId"]} | {reader["ProductName"]} | {reader["ProductType"]} | {reader["ProviderId"]} | {reader["Quantity"]} | {reader["Cost"]} | {reader["SupplyDate"]}");
                        }
                    }
                }
                Console.WriteLine("\n\nProductTypes:");
                code = "SELECT * FROM ProductTypes";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("TypeId | TypeName");
                        Console.WriteLine("-----------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["TypeId"]} | {reader["TypeName"]}");
                        }
                    }
                }

                Console.WriteLine("\n\nProviders:");
                code = "SELECT * FROM Providers";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("ProviderId | ProviderName");
                        Console.WriteLine("-----------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["ProviderId"]} | {reader["ProviderName"]}");
                        }
                    }
                }
                Console.WriteLine("\n");
                code = "SELECT MAX(Quantity) AS _Quantity FROM Storage";
                using (var command = new SQLiteCommand(code, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        Console.WriteLine($"\nMaximum quantity of goods: {result}");
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                }

                Console.WriteLine("\n");
                code = "SELECT MIN(Quantity) AS _Quantity FROM Storage";
                using (var command = new SQLiteCommand(code, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        Console.WriteLine($"\nMinimum quantity of goods: {result}");
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                }

                Console.WriteLine("\n");
                code = "SELECT MIN(Cost) AS _Cost FROM Storage";
                using (var command = new SQLiteCommand(code, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        Console.WriteLine($"\nMinimum cost of goods: {result}");
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                }

                Console.WriteLine("\n");
                code = "SELECT MAX(Cost) AS _Cost FROM Storage";
                using (var command = new SQLiteCommand(code, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        Console.WriteLine($"\nMaximum cost of goods: {result}");
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                }

                Console.WriteLine("\nGoods of SupplierA");
                code = "SELECT Storage.*, ProductTypes.*\r\nFROM Storage\r\nJOIN ProductTypes ON Storage.ProductType = ProductTypes.TypeId\r\nWHERE Storage.ProviderId = (\r\n    SELECT ProviderId FROM Providers WHERE ProviderName = 'SupplierA'\r\n);\r\n";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("ProductId | ProductName | ProductType | ProviderId | Quantity | Cost | SupplyDate");
                        Console.WriteLine("---------------------------------------------------------------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["ProductId"]} | {reader["ProductName"]} | {reader["ProductType"]} | {reader["ProviderId"]} | {reader["Quantity"]} | {reader["Cost"]} | {reader["SupplyDate"]}");
                        }
                    }
                }

                Console.WriteLine("\nGoods of type(Category1)");
                code = "SELECT Storage.*, ProductTypes.*\r\nFROM Storage\r\nJOIN ProductTypes ON Storage.ProductType = ProductTypes.TypeId\r\nWHERE ProductTypes.TypeName = 'Category1';\r\n";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("ProductId | ProductName | ProductType | ProviderId | Quantity | Cost | SupplyDate");
                        Console.WriteLine("---------------------------------------------------------------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["ProductId"]} | {reader["ProductName"]} | {reader["ProductType"]} | {reader["ProviderId"]} | {reader["Quantity"]} | {reader["Cost"]} | {reader["SupplyDate"]}");
                        }
                    }
                }

                Console.WriteLine("\nThe oldes good");
                code = "SELECT Storage.*, ProductTypes.*\r\nFROM Storage\r\nJOIN ProductTypes ON Storage.ProductType = ProductTypes.TypeId\r\nORDER BY Storage.SupplyDate ASC\r\nLIMIT 1;\r\n";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("ProductId | ProductName | ProductType | ProviderId | Quantity | Cost | SupplyDate");
                        Console.WriteLine("---------------------------------------------------------------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["ProductId"]} | {reader["ProductName"]} | {reader["ProductType"]} | {reader["ProviderId"]} | {reader["Quantity"]} | {reader["Cost"]} | {reader["SupplyDate"]}");
                        }
                    }
                }

                Console.WriteLine("\nAverage quantity of every type");
                code = "SELECT ProductTypes.TypeName, AVG(Storage.Quantity) AS AverageQuantity\r\nFROM Storage\r\nJOIN ProductTypes ON Storage.ProductType = ProductTypes.TypeId\r\nGROUP BY ProductTypes.TypeName;\r\n";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("TypeName | AverageQuantity");
                        Console.WriteLine("--------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["TypeName"]} | {reader["AverageQuantity"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await connection.CloseAsync();
                Console.WriteLine("\nConnection is failed");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                    Console.WriteLine("\nConnection is disconnected");
                }
            }
        }
    }
}