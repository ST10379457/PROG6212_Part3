using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ST10379457_PROG6212_POE.Pages
{
    public class AcademicManagerModel : PageModel
    {
        public List<Claims> ClaimsList1 { get; set; }

        public void OnGet()
        {
            ClaimsList1 = new List<Claims>();

            string connectionString = "Data Source=JOHNSLAPTOP;Initial Catalog=ContractMonthlyClaimSystemDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT ClaimID, UserID, ModuleName, ModuleCode, ClassGroup, Hours, ClaimAmount, Status, DateOfSubmission FROM Claims WHERE Status = 'pending'"; //(Microsoft, 2023), (IIE, 2012:86-115).

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Claims claims = new Claims
                            {
                                ClaimID = reader.GetInt32(0),
                                UserID = reader.GetInt32(1),
                                ModuleName = reader.GetString(2),
                                ModuleCode = reader.GetString(3),
                                ClassGroup = reader.GetInt32(4),
                                Hours = reader.GetInt32(5),
                                ClaimAmount = reader.GetFloat(6), 
                                Status = reader.GetString(7),
                                DateOfSubmission = reader.GetDateTime(8).ToString("yyyy-MM-dd")
                            };
                            ClaimsList1.Add(claims);
                        }
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

    public class Claims
    {
        public int ClaimID { get; set; }
        public int UserID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public int ClassGroup { get; set; }
        public int Hours { get; set; }
        public float ClaimAmount { get; set; } // REAL type mapped to float
        public string Status { get; set; }
        public string DateOfSubmission { get; set; }
    }
}
/* Reference list:
 * 
 * Create ASP.NET Core Web Application With SQL Server Database Connection and CRUD Operations. 2022. YouTube video, added by BoostMyTool. [Online]. Available at: https://www.youtube.com/watch?v=T-e554Zt3n4 [Accessed 13 May 2024].
 * The IIE. 2012. Databases [DBAS6211 Module Manual]. The Independent Institute of Education: Unpublished.
 * Microsoft Learn. 2023. Introduction to Razor Pages in ASP.NET Core, 7 October 2023. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio [Accessed 21 May 2024].
 */