using System.Text;
using Tenant_Management_System_Server.Models;
using Tenant_Management_System_Server.Models.TenantRegistrationForm;

namespace Tenant_Management_System_Server.Utility
{
    public class BillMemoGenerator
    {
        public static string GetHTMLString(List<UtilityBillModel> utilityBillModelList, int rent, int month, 
            int year, string tenantName, string homeownerName)
        {
            var sb = new StringBuilder();

            sb.AppendFormat(@"
                            <div class='container-xl px-4 mt-4'>
                              <!-- Bill in Tabular Form-->
                              <div class='row'>
                                <div class='col-lg-10 card mb-4'>
                                    <div class='card-header'>
                                        Bill Memo <br>
                                        Tenant Name: {0}<br>
                                        Homeowner Name: {1}<br>
                                        Month {2} Year {3} <br>
                                    </div>
                                    <div class='card-body p-0'>
                                    <!-- Billing history table-->
                                    <div class='table-responsive table-billing-history'>
                                        <table class='table mb-0'>
                                        <thead>
                                        <tr>
                                            <th class='border-gray-200' scope='col'>Bill Name</th>
                                            <th class='border-gray-200' scope='col'>Bill Amount</th>
                                        </tr>
                                        </thead>
                                        <tbody>",
                                        tenantName,
                                        homeownerName,
                                        month+1,
                                        year);

            foreach (var utilityBill in utilityBillModelList) {
                sb.AppendFormat(@"
                                        <tr>
                                            <td>{0}</td>
                                            <td>{1} ৳</td>
                                        </tr>",
                                        utilityBill.BillName,
                                        utilityBill.BillAmount);
            }

            sb.AppendFormat(@"
                                        <tr>
                                            <td>Flat Rent</td>
                                            <td>{0} ৳</td>
                                        </tr>",
                                        rent);

            sb.AppendFormat(@"
                                        <tr>
                                            <td>Total Amount</td>
                                            <td>{0} ৳</td>
                                        </tr>",
                                        getTotalAmount(rent, utilityBillModelList));

            //End
            sb.AppendFormat(@"
                                        </tbody>
                                        </table>
                                    </div>
                                    </div>
                                </div>
                              </div>
                            </div>
                            ");

            return sb.ToString();
        }

        public static int getTotalAmount(int rent, List<UtilityBillModel> utilityBillModels) {

            var totalAmount = 0;
            foreach (var utilityBill in utilityBillModels) {
                totalAmount = totalAmount + utilityBill.BillAmount;
            }    
            totalAmount = totalAmount + rent;
            return totalAmount;
        }
    }
}
