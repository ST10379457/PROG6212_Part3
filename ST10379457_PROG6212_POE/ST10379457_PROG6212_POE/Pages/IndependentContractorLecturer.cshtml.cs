using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ST10379457_PROG6212_POE.Pages
{
    public class IndependentContractorLecturerModel : PageModel
    {
        public int UserID { get; set; }
        public string LoginMessage { get; set; }
        public List<Claim1> ClaimsList2 { get; set; } = new List<Claim1>(); // default to empty list for all ways this page is rendered etc., always return empty lists over nothing unless you explicitly can expect nothing like Find....method-name-something which means nothing can be returned

        // Only works for OnGet, when you do OnPost then the below is not triggered, meaning no data is initialized, then ClaimsList2 is null for LoadClaimsForUser
        public void OnGet()
        {
            string userID = TempData["UserID"]?.ToString();
            if (!string.IsNullOrEmpty(userID))
            {
                LoginMessage = $"{userID} is logged in.";
                UserID = int.Parse(userID);
                LoadClaimsForUser();
            }
        }

        private void PostClaim(string ModuleName, string ModuleCode, int ClassGroup, int HoursWorked)
        {
            string connectionString = "Data Source=JOHNSLAPTOP;Initial Catalog=ContractMonthlyClaimSystemDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Claims (UserID, ModuleName, ModuleCode, ClassGroup, Hours, ClaimAmount, Status, DateOfSubmission) VALUES (@UserID, @ModuleName, @ModuleCode, @ClassGroup, @Hours, @ClaimAmount, @Status, @DateOfSubmission)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@ModuleName", ModuleName);
                    command.Parameters.AddWithValue("@ModuleCode", ModuleCode);
                    command.Parameters.AddWithValue("@ClassGroup", ClassGroup);
                    command.Parameters.AddWithValue("@Hours", HoursWorked);
                    command.Parameters.AddWithValue("@ClaimAmount", HoursWorked * 500); // Calculation for ClaimAmount
                    command.Parameters.AddWithValue("@Status", "Pending");
                    command.Parameters.AddWithValue("@DateOfSubmission", DateTime.Now); // Use current date

                    command.ExecuteNonQuery();
                }
            }
            LoadClaimsForUser();
        }

        private void LoadClaimsForUser()
        {
            string connectionString = "Data Source=JOHNSLAPTOP;Initial Catalog=ContractMonthlyClaimSystemDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ClaimID, UserID, ModuleName, ModuleCode, ClassGroup, Hours, ClaimAmount, Status, DateOfSubmission FROM Claims WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Claim1 claim = new Claim1
                            {
                                claimID2 = reader.GetInt32(0),
                                userID2 = reader.GetInt32(1),
                                moduleName2 = reader.GetString(2),
                                moduleCode2 = reader.GetString(3),
                                classGroup2 = reader.GetInt32(4),
                                hours2 = reader.GetInt32(5),
                                claimAmount2 = reader.GetFloat(6),
                                status2 = reader.GetString(7),
                                dateOfSubmission2 = reader.GetDateTime(8).ToString("yyyy-MM-dd")
                            };
                            ClaimsList2.Add(claim);
                        }
                    }
                }
            }
        }

        public IActionResult OnPost(string ModuleName, string ModuleCode, int ClassGroup, int HoursWorked)
        {

            PostClaim(ModuleName, ModuleCode, ClassGroup, HoursWorked);
            return RedirectToPage(); // Refresh the page to see updated claims
        }
    }

    public class Claim1
    {
        public int claimID2 { get; set; }
        public int userID2 { get; set; }
        public string moduleName2 { get; set; }
        public string moduleCode2 { get; set; }
        public int classGroup2 { get; set; }
        public int hours2 { get; set; }
        public float claimAmount2 { get; set; }
        public string status2 { get; set; }
        public string dateOfSubmission2 { get; set; }
    }
}
/* Reference list:
 * 
 * Create ASP.NET Core Web Application With SQL Server Database Connection and CRUD Operations. 2022. YouTube video, added by BoostMyTool. [Online]. Available at: https://www.youtube.com/watch?v=T-e554Zt3n4 [Accessed 13 May 2024].
 * The IIE. 2012. Databases [DBAS6211 Module Manual]. The Independent Institute of Education: Unpublished.
 * Microsoft Learn. 2023. Introduction to Razor Pages in ASP.NET Core, 7 October 2023. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio [Accessed 21 May 2024].
 */