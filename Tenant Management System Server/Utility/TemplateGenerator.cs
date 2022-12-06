using System.Reflection.Emit;
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
            //Personal Details
            sb.AppendFormat(@"
                        <!DOCTYPE html>
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='container-xl px-4 mt-4'>
                                    <div class='row'>
                                        <div class='col-xl-12'>
                                            <div class='card'>
                                                <div>
                                                    <div class='d-flex justify-content-beetween flex-row'>
                                                        <div>Personal Info</div>
                                                        <div>
                                                            <img class='img-thumbnail rounded mx-auto d-block img-account-profile' id='image'
                                                                src='{0}' alt='no image'>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class='card-body'>
                                                    <form>
                                                        <!-- Form Group (name)-->
                                                        <div class='mb-3'>
                                                            <label class='small mb-1' for='name'>Name</label>
                                                            <input class='form-control' id='name' type='text'
                                                                value='{1}'>
                                                        </div> 
                                                        <!-- Form Group (father name)-->
                                                        <div class='mb-3'>
                                                            <label class='small mb-1' for='fatherName'>Father Name</label>
                                                            <input class='form-control' id='fatherName' type='text'
                                                                value='{2}'>
                                                        </div>
                                                        <!-- Form Row-->
                                                        <div class='row gx-3 mb-3'>
                                                          <!-- Form Group (date of birth)-->
                                                          <div class='col-md-6'>
                                                            <label class='small mb-1' for='dateOfBirth'>Date of Birth</label>
                                                            <input class='form-control' id='dateOfBirth' type='date'
                                                                value='{3}'>
                                                          </div>
                                                          <!-- Form Group (marital status)-->
                                                          <div class='col-md-6'>
                                                            <label class='small mb-1' for='maritalStatus'>Marital Status</label>
                                                            <input class='form-control' id='maritalStatus' type='text'
                                                                value='{4}'>
                                                          </div>
                                                        </div>
                                                        <!-- Form Group (permanent address)-->
                                                        <div class='mb-3'>
                                                          <label class='small mb-1' for='permanentAddress'> Permanent Address </label>
                                                          <input class='form-control' id='permanentAddress' type='text'
                                                                 value='{5}'>
                                                        </div>
                                                        <!-- Form Row-->
                                                        <div class='row gx-3 mb-3'>
                                                            <!-- Form Group (occupation)-->
                                                            <div class='col-md-4'>
                                                            <label class='small mb-1' for='occupation'>Occupation</label>
                                                            <input class='form-control' id='occupation' type='text'
                                                                    value='{6}'>
                                                            </div>
                                                            <!-- Form Group (Address Of The Institution Or Work Place)-->
                                                            <div class='col-md-8'>
                                                            <label class='small mb-1' for='addressOfTheInstitutionOrWorkPlace'>
                                                                Address Of The Institution Or Work Place
                                                            </label>
                                                            <input class='form-control' id='addressOfTheInstitutionOrWorkPlace' type='text'
                                                                    value='{7}'>
                                                            </div>
                                                        </div>
                                                        <!-- Form Row-->
                                                        <div class='row gx-3 mb-3'>
                                                          <!-- Form Group (religion)-->
                                                          <div class='col-md-4'>
                                                            <label class='small mb-1' for='religion'>Religion</label>
                                                            <input class='form-control' id='religion' type='text'
                                                                   value='{8}'>
                                                          </div>
                                                          <!-- Form Group (phone number)-->
                                                          <div class='col-md-8'>
                                                            <label class='small mb-1' for='phoneNumber'>Phone Number</label>
                                                            <input class='form-control' id='phoneNumber' type='text'
                                                                   value='{9}'>
                                                          </div>
                                                        </div>
                                                        <!-- Form Group (email)-->
                                                        <div class='mb-3'>
                                                          <label class='small mb-1' for='email'>Email </label>
                                                          <input class='form-control' id='email' type='text'
                                                                 value='{10}''>
                                                        </div>
                                                        <!-- Form Group (NID number)-->
                                                        <div class='mb-3'>
                                                          <label class='small mb-1' for='nidNumber'>NID Number </label>
                                                          <input class='form-control' id='nidNumber' type='text'
                                                                 value='{11}'>
                                                        </div>
                                                        <!-- Form Group (passport number)-->
                                                        <div class='mb-3'>
                                                          <label class='small mb-1' for='passportNumber'>Passport Number </label>
                                                          <input class='form-control' id='passportNumber' type='text'
                                                                 value='{12}'>
                                                        </div>
                                                    </form>
                                                </div>
                                        ",
                        tenantRegistrationFormModel.PersonalInfo.ImageFile,
                        tenantRegistrationFormModel.PersonalInfo.Name,
                        tenantRegistrationFormModel.PersonalInfo.FatherName,
                        tenantRegistrationFormModel.PersonalInfo.DateOfBirth,
                        tenantRegistrationFormModel.PersonalInfo.MaritalStatus,
                        tenantRegistrationFormModel.PersonalInfo.PermanentAddress,
                        tenantRegistrationFormModel.PersonalInfo.Occupation,
                        tenantRegistrationFormModel.PersonalInfo.AddressOfTheInstitutionOrWorkPlace,
                        tenantRegistrationFormModel.PersonalInfo.Religion,
                        tenantRegistrationFormModel.PersonalInfo.PhoneNumber,
                        tenantRegistrationFormModel.PersonalInfo.Email,
                        tenantRegistrationFormModel.PersonalInfo.NIDNumber,
                        tenantRegistrationFormModel.PersonalInfo.PassportNumber);


            //present address
            sb.AppendFormat(@"
                            <div class='card-header'>
                              <div class='d-flex justify-content-center flex-row'>
                                <!-- header card-->
                                <div>Present Address</div>
                              </div>
                            </div>
                            <div class='card-body'>
                              <form>
                                <!-- Form Row-->
                                <div class='row gx-3 mb-3'>
                                  <!-- Form Group (Flat No)-->
                                  <div class='col-md-4'>
                                    <label class='small mb-1' for='flatNo'>Flat No</label>
                                    <input class='form-control' id='flatNo' type='text'
                                           value='{0}'>
                                  </div>
                                  <!-- Form Group (House No)-->
                                  <div class='col-md-4'>
                                    <label class='small mb-1' for='houseNo'>House No</label>
                                    <input class='form-control' id='houseNo' type='text'
                                           value='{1}'>
                                  </div>
                                  <!-- Form Group (ROad No)-->
                                  <div class='col-md-4'>
                                    <label class='small mb-1' for='roadNo'>Road No</label>
                                    <input class='form-control' id='roadNo' type='text'
                                           value='{2}'>
                                  </div>
                                </div>
                                <!-- Form Row-->
                                <div class='row gx-3 mb-3'>
                                  <!-- Form Group (Flat No)-->
                                  <div class='col-md-8'>
                                    <label class='small mb-1' for='area'>Area</label>
                                    <input class='form-control' id='area' type='text'
                                           value='{3}'>
                                  </div>
                                  <!-- Form Group (House No)-->
                                  <div class='col-md-4'>
                                    <label class='small mb-1' for='postalCode'>Postal Code</label>
                                    <input class='form-control' id='postalCode' type='text'
                                           value='{4}'>
                                  </div>
                                </div>
                              </form>
                            </div>
                            ",
                            tenantRegistrationFormModel.PresentAddress.FlatNo,
                            tenantRegistrationFormModel.PresentAddress.HouseNo,
                            tenantRegistrationFormModel.PresentAddress.RoadNo,
                            tenantRegistrationFormModel.PresentAddress.Area,
                            tenantRegistrationFormModel.PresentAddress.PostalCode);

            //Emergency Contact
            sb.AppendFormat(@"
                            <div class='card-header'>
                              <div class='d-flex justify-content-center flex-row'>
                                <!-- header card-->
                                <div>Emergency Contact</div>
                              </div>
                            </div>
                            <div class='card-body'>
                              <form>
                                <!-- Form Group (guardian name)-->
                                <div class='mb-3'>
                                  <label class='small mb-1' for='guardianName'>Guardian Name</label>
                                  <input class='form-control' id='guardianName' type='text'
                                         value='{0}'>
                                </div>
                                <!-- Form Group (relation)-->
                                <div class='mb-3'>
                                  <label class='small mb-1' for='relation'>Relation</label>
                                  <input class='form-control' id='relation' type='text'
                                         value='{1}'>
                                </div>
                                <!-- Form Group (relation)-->
                                <div class='mb-3'>
                                  <label class='small mb-1' for='guardianAddress'>Guardian Address</label>
                                  <input class='form-control' id='guardianAddress' type='text'
                                         value='{2}'>
                                </div>
                                <!-- Form Group (guardian phone number)-->
                                <div class='mb-3'>
                                  <label class='small mb-1' for='guardianPhoneNumber'>Guardian Phone Number</label>
                                  <input class='form-control' id='guardianPhoneNumber' type='text'
                                         value='{3}'>
                                </div>
                              </form>
                            </div>
                         ",
                         tenantRegistrationFormModel.EmergencyContact.GuardianName,
                         tenantRegistrationFormModel.EmergencyContact.Relation,
                         tenantRegistrationFormModel.EmergencyContact.GuardianAddress,
                         tenantRegistrationFormModel.EmergencyContact.GuardianPhoneNumber);


            //Driver Info
            sb.AppendFormat(@"
                            <div class='card-header'>
                              <div class='d-flex justify-content-center flex-row'>
                                <!-- header card-->
                                <div>Driver Info</div>
                              </div>
                            </div>
                            <div class='card-body'>
                              <form>
                                <!-- Form Group (driver name)-->
                                <div class='mb-3'>
                                  <label class='small mb-1' for='driverName'>Driver Name</label>
                                  <input class='form-control' id='driverName' type='text'
                                        value='{0}'>
                                </div>
                                <!-- Form Group (driver nid number)-->
                                <div class='mb-3'>
                                  <label class='small mb-1' for='driverNIDNumber'>Driver NID Number</label>
                                  <input class='form-control' id='driverNIDNumber' type='text'
                                         value='{1}'>
                                </div>
                                <!-- Form Group (driver phone number)-->
                                <div class='mb-3'>
                                  <label class='small mb-1' for='driverPhoneNumber'>Driver Phone Number</label>
                                  <input class='form-control' id='driverPhoneNumber' type='text'
                                         value='{2}'>
                                </div>
                                <!-- Form Group (driver permanent address)-->
                                <div class='mb-3'>
                                  <label class='small mb-1' for='driverPermanentAddress'>Driver Permanent Address</label>
                                  <input class='form-control' id='driverPermanentAddress' type='text'
                                         value='{3}'>
                                </div>
                              </form>
                            </div>
                            ",
                            tenantRegistrationFormModel.Driver.DriverName,
                            tenantRegistrationFormModel.Driver.DriverNIDNumber,
                            tenantRegistrationFormModel.Driver.DriverPhoneNumber,
                            tenantRegistrationFormModel.Driver.DriverPermanentAddress);


            //Housekeeper Info
            sb.AppendFormat(@"
                               <div class='card-header'>
                                  <div class='d-flex justify-content-center flex-row'>
                                    <!-- header card-->
                                    <div>Housekeeper Info</div>
                                  </div>
                                </div>
                                <div class='card-body'>
                                  <form>
                                    <!-- Form Group (driver name)-->
                                    <div class='mb-3'>
                                      <label class='small mb-1' for='housekeeperName'>Housekeeper Name</label>
                                      <input class='form-control' id='housekeeperName' type='text'
                                             value='{0}'>
                                    </div>
                                    <!-- Form Group (driver nid number)-->
                                    <div class='mb-3'>
                                      <label class='small mb-1' for='housekeeperNIDNumber'>Housekeeper NID Number</label>
                                      <input class='form-control' id='housekeeperNIDNumber' type='text'
                                             value='{1}'>
                                    </div>
                                    <!-- Form Group (driver phone number)-->
                                    <div class='mb-3'>
                                      <label class='small mb-1' for='housekeeperPhoneNumber'>Housekeeper Phone Number</label>
                                      <input class='form-control' id='housekeeperPhoneNumber' type='text'
                                             value='{2}'>
                                    </div>
                                    <!-- Form Group (driver permanent address)-->
                                    <div class='mb-3'>
                                      <label class='small mb-1' for='housekeeperPermanentAddress'>Housekeeper Permanent Address</label>
                                      <input class='form-control' id='housekeeperPermanentAddress' type='text'
                                             value='{3}'>
                                    </div>
                                  </form>
                                </div>
                            ",
                            tenantRegistrationFormModel.Housekeeper.HousekeeperName,
                            tenantRegistrationFormModel.Housekeeper.HousekeeperNIDNumber,
                            tenantRegistrationFormModel.Housekeeper.HousekeeperPhoneNumber,
                            tenantRegistrationFormModel.Housekeeper.HousekeeperPermanentAddress);


            //Ending
            sb.AppendFormat(@"
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </body>
                        </html>
                         ");

            return sb.ToString();
        }
    }
}
