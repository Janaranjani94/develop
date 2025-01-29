using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using webapisample.Models;

namespace webapisample.Data
{
    public class KeytestRepository
    {
        public async Task<string> GetDBConnectionfromAzure()
        {
            string secretvalue = "";
            try
            {

                // Replace with your Key Vault URL
                string keyVaultUrl = "https://demoazuresecret.vault.azure.net/";


               // string keyVaultUrl = "https://<YourKeyVaultName>.vault.azure.net/";
                string clientId = "acb1b3a2-cd2d-4cc9-ac45-93efff1a334e";
                string clientSecret = "8Ng8Q~Fqmpx7fnS2owoS17ZXHPL3aDwynE-yQaa9";
                string tenantId = "b26e8969-ffd9-48cb-95bc-318edbd6cd3e";
                //string secretName = "<YourSecretName>";



                // Replace with the name of the secret
                string secretName = "azuresqldbconnection";
               

                try
                {
                    
                    //// Authenticate using DefaultAzureCredential
                    //var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
                    //var client = new SecretClient(new Uri(keyVaultUrl), new ManagedIdentityCredential());
                    //// Fetch the secret
                    //KeyVaultSecret secret = client.GetSecret(secretName);

                    //// Display the secret value
                    //Console.WriteLine($"Secret Value: {secret.Value}");


                    // Authenticate using ClientSecretCredential
                    var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                    // Create a SecretClient
                     var secretClient = new SecretClient(new Uri(keyVaultUrl), clientSecretCredential);

                    // Retrieve the secret
                     KeyVaultSecret secret = secretClient.GetSecret(secretName);
                    Console.WriteLine($"Secret Value: {secret.Value}");
                    secretvalue = secret.Value;
                    //var program = new Program();
                    //program.FetchDBValues(secret.Value);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                return secretvalue;
            }
            catch (Exception ex)
            {

                // throw;
            }
            return secretvalue;
        }
        public string FetchDBValues(string json)
        {
            var config = JObject.Parse(json);
            string jsonResult = "";
            //var jsonvalue = JsonConvert.DeserializeObject<string>(json);

            // Extract connection details
            var connection = config["ConnectionStrings"]["DefaultConnection"];
            string server = connection["Server"].ToString();
            string database = connection["Database"].ToString();
            string userId = connection["UserId"].ToString();
            string password = connection["Password"].ToString();

            // Build connection string
            string connectionString = $"Server={server};Database={database};User Id={userId};Password={password};";

            // SQL query
            string query = "SELECT StudentId, Name, DOB, Department, Course, Mobile FROM Student";

            try
            {
                List<TableRecord> records = new List<TableRecord>();

                // Establish connection
                using (SqlConnection dbconnection = new SqlConnection(connectionString))
                {
                    // Open connection
                    dbconnection.Open();

                    // Create a command
                    using (SqlCommand command = new SqlCommand(query, dbconnection))
                    {
                        // Execute the command and read the data
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if there are rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Retrieve data by column index or name
                                    var record = new TableRecord()
                                    {
                                        StudentId = reader.GetInt32(0),        // Column index 0
                                        FirstName = reader.GetString(1),
                                        DOB = reader.GetDateTime(2),// Column index 1
                                        Department = reader.GetString(3),    // Column index 2
                                        Course = reader.GetString(4),          // Column index 3
                                        Mobile = reader.GetString(5)
                                    };
                                    records.Add(record);


                                    // Output the data
                                   // Console.WriteLine($"ID: {demoId}, Name: {firstName} {Mobile}, Email: {email}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No rows found.");
                            }
                        }
                    }
                }

                //using (SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString))
                //{
                //    DataTable table = new DataTable();
                //    adapter.Fill(table);

                //    foreach (DataRow row in table.Rows)
                //    {
                //        Console.WriteLine($"ID: {row["DemoId"]}, Name: {row["Name"]} {row["Mobile"]}, Email: {row["Emailid"]}");
                //    }
                //}
               jsonResult = JsonConvert.SerializeObject(records); // Using Newtonsoft.Json
                return jsonResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return jsonResult;
        }
    }
}
