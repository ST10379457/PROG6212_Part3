using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System;
using System.Data.SqlClient;

namespace ST10379457_PROG6212_POE.Pages
{
    public class UserLoginModel : PageModel
    {
        public string LoginMessage { get; set; }

        public string userID;
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }//(BoostMyTool, 2022).

        public void OnGet()
        {
        }

        public IActionResult OnPost(string email, string password, string roleFullName)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(roleFullName))
            {
                LoginMessage = "Please enter email, password, and select a role.";//(Microsoft, 2023).
                return Page();
            }

            // Map full role names to database abbreviations (IC, PC, AM)
            string role = roleFullName switch
            {
                "Independent Contractor Lecturer" => "IC",
                "Programme Coordinator" => "PC",
                "Academic Manager" => "AM",
                _ => null
            };

            if (role == null)
            {
                LoginMessage = "Invalid role selected."; //(Microsoft, 2023).
                return Page();
            }

            try
            {
                string connectionString = "Data Source=JOHNSLAPTOP;Initial Catalog=ContractMonthlyClaimSystemDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Query the Users table to check email, password, and role
                    string sql = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password AND Role = @Role"; //(Microsoft, 2023), (IIE, 2012:86-115).

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Role", role);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // User authenticated successfully
                                userID = reader["UserID"].ToString();
                                Username = reader["FullName"].ToString();
                                Email = reader["Email"].ToString();
                                Phone_Number = reader["PhoneNumber"].ToString();
                                Role = reader["Role"].ToString();
                                //TempData["UserID"] = userID;

                                string filePath = "UserLoggedIn.txt";

                                try
                                { 

                                    // Replace the file content with "1"
                                    System.IO.File.WriteAllText(filePath, userID);

                                    // Read the first line of the file
                                    string userLoggedIn = System.IO.File.ReadAllText(filePath);

                                    // Display the first line
                                    //Console.WriteLine("The first line in the file is:");
                                    Console.WriteLine(userLoggedIn);
                                }
                                catch (FileNotFoundException)
                                {
                                    Console.WriteLine("The file 'file.txt' was not found.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"An error occurred: {ex.Message}");
                                }

                                    // Redirect based on role
                                if (Role == "IC")
                                {   
                                    // Redirect to the Independent Contractor Lecturer Dashboard
                                    return RedirectToPage("/IndependentContractorLecturer");
                                }
                                else if (Role == "PC")
                                {
                                    return RedirectToPage("/ProgrammeCoordinator");
                                }
                                else if (Role == "AM")
                                {
                                    return RedirectToPage("/AcademicManager");
                                }
                                else
                                {
                                    // Redirect to a default dashboard or other roles' dashboard
                                    return RedirectToPage("/DefaultDashboard");
                                }
                            }
                            else
                            {
                                // Invalid credentials or role
                                LoginMessage = "Invalid email, password, or role.1";
                                return Page();
                            }
                            //(Microsoft, 2023).
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoginMessage = "Error occurred while logging in. Please try again later.2";
                Console.WriteLine(ex.Message);
                return Page();
            }
        }
    }
}
/* Reference list:
 * 
 * Create ASP.NET Core Web Application With SQL Server Database Connection and CRUD Operations. 2022. YouTube video, added by BoostMyTool. [Online]. Available at: https://www.youtube.com/watch?v=T-e554Zt3n4 [Accessed 13 May 2024].
 * The IIE. 2012. Databases [DBAS6211 Module Manual]. The Independent Institute of Education: Unpublished.
 * Microsoft Learn. 2023. Introduction to Razor Pages in ASP.NET Core, 7 October 2023. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio [Accessed 21 May 2024].
 */