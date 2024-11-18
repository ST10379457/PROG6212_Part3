using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ST10379457_PROG6212_POE.Pages
{
    public class ProgrammeCoordinatorModel : PageModel
    {
        public List<Claim> ClaimsList { get; set; }

        public void OnGet()
        {
            ClaimsList = new List<Claim>();

            string connectionString = "Data Source=JOHNSLAPTOP;Initial Catalog=ContractMonthlyClaimSystemDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ClaimID, UserID, ModuleName, ModuleCode, ClassGroup, Hours, ClaimAmount, Status, DateOfSubmission FROM Claims WHERE Status = 'pending'"; //(Microsoft, 2023), (IIE, 2012:86-115).

                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Claim claim = new Claim
                        {
                            claimID = reader.GetInt32(0),
                            userID = reader.GetInt32(1),
                            moduleName = reader.GetString(2),
                            moduleCode = reader.GetString(3),
                            classGroup = reader.GetInt32(4),
                            hours = reader.GetInt32(5),
                            claimAmount = reader.GetFloat(6), 
                            status = reader.GetString(7),
                            dateOfSubmission = reader.GetDateTime(8).ToString("yyyy-MM-dd")
                        };
                        ClaimsList.Add(claim);
                    }
                }
            }
        }
        //(BoostMyTool, 2022).

        public IActionResult OnPostAccept(int claimID)
        {
            UpdateClaimStatus(claimID, "Accepted");
            return RedirectToPage();
        }

        public IActionResult OnPostReject(int claimID)
        {
            UpdateClaimStatus(claimID, "Rejected");
            return RedirectToPage();
        }

        private void UpdateClaimStatus(int claimID, string newStatus)
        {
            string connectionString = "Data Source=JOHNSLAPTOP;Initial Catalog=ContractMonthlyClaimSystemDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Claims SET Status = @Status WHERE ClaimID = @ClaimID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", newStatus);
                    command.Parameters.AddWithValue("@ClaimID", claimID);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public class Claim
    {
        public int claimID { get; set; }
        public int userID { get; set; }
        public string moduleName { get; set; }
        public string moduleCode { get; set; }
        public int classGroup { get; set; }
        public int hours { get; set; }
        public float claimAmount { get; set; }
        public string status { get; set; }
        public string dateOfSubmission { get; set; }
    }
}
/* Reference list:
 * 
 * Create ASP.NET Core Web Application With SQL Server Database Connection and CRUD Operations. 2022. YouTube video, added by BoostMyTool. [Online]. Available at: https://www.youtube.com/watch?v=T-e554Zt3n4 [Accessed 13 May 2024].
 * The IIE. 2012. Databases [DBAS6211 Module Manual]. The Independent Institute of Education: Unpublished.
 * Microsoft Learn. 2023. Introduction to Razor Pages in ASP.NET Core, 7 October 2023. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio [Accessed 21 May 2024].
 */