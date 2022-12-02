using System.Text;
using Tenant_Management_System_Server.Models;
using Tenant_Management_System_Server.Models.TenantRegistrationForm;

namespace Tenant_Management_System_Server.Utility
{
    public static class TemplateGenerator
    {
        public static string GetHTMLString(TenantRegistrationFormModel tenantRegistrationFormModel)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Tenant Registration Form</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Name</th>
                                        <td>{0}</td>
                                    </tr>
                                    <tr>
                                        <th>Father Name</th>
                                        <td>{1}</td>
                                    </tr>
                                    <tr>
                                        <th>DateOfBirth</th>
                                        <td>{2}</td>
                                    </tr>
                                    <tr>
                                        <th>MaritalStatus</th>
                                        <td>{3}</td>
                                    </tr>
                                    <tr>
                                        <th>PhoneNumber</th>
                                        <td>{4}</td>
                                    </tr>
                                    <tr>
                                        <th>Image</th>
                                        <td><img src='{5}' class = 'imageFile'></td>
                                    </tr>
                                </table>
                            </body>
                        </html>",
                        tenantRegistrationFormModel.PersonalInfo.Name,
                        tenantRegistrationFormModel.PersonalInfo.FatherName,
                        tenantRegistrationFormModel.PersonalInfo.DateOfBirth,
                        tenantRegistrationFormModel.PersonalInfo.MaritalStatus,
                        tenantRegistrationFormModel.PersonalInfo.PhoneNumber,
                        tenantRegistrationFormModel.PersonalInfo.ImageFile);
            return sb.ToString();
        }
    }
}
