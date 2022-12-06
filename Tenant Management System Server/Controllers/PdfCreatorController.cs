using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant_Management_System_Server.Models;
using Tenant_Management_System_Server.Services;
using Tenant_Management_System_Server.Utility;

namespace Tenant_Management_System_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfCreatorController : ControllerBase
    {
        private IConverter _converter;
        private readonly TenantRegistrationFormService _tenantRegistrationFormService;
        private readonly TransactionService _transactionService;
        private readonly HomeownerAuthService _homeownerAuthService;
        private readonly TenantAuthService _tenantAuthService;

        public PdfCreatorController(IConverter converter, TenantRegistrationFormService tenantRegistrationFormService,
            TransactionService transactionService, HomeownerAuthService homeownerAuthService,
            TenantAuthService tenantAuthService)
        {
            _converter = converter;
            _tenantRegistrationFormService = tenantRegistrationFormService;
            _transactionService = transactionService;
            _homeownerAuthService = homeownerAuthService;
            _tenantAuthService = tenantAuthService;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult> CreateTenantRegistrationFormPDF(string userName)
        {
            var tenantRegistrationFormModel = await _tenantRegistrationFormService.GetByUserNameAsync(userName);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = $"{tenantRegistrationFormModel.UserName}.pdf",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(tenantRegistrationFormModel),
                WebSettings = { 
                    DefaultEncoding = "utf-8", 
                    UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "bootstrap.min.css") },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            //_converter.Convert(pdf);

            return File(_converter.Convert(pdf), 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                pdf.GlobalSettings.DocumentTitle);
        }

        [HttpGet("downloadBillMemoPDF/{Id}")]
        public async Task<ActionResult> CreateBillMemoPDF(string Id)
        {
            var transaction = await _transactionService.GetAsync(Id);
            if(transaction is null)
            {
                return BadRequest();
            }

            var homeowner = await _homeownerAuthService.GetByUserNameAsync(transaction.HomeownerUserName);

            var tenant = await _tenantAuthService.GetByUserNameAsync(transaction.TenantUserName);

            var utilityBillModelList = new List<UtilityBillModel>();
            int rent=0;

            foreach(var house in homeowner.HouseList)
            {
                foreach(var flat in house.FlatList)
                {
                    if(transaction.HouseId == house.HouseId && transaction.FlatId == flat.FlatId)
                    {
                        utilityBillModelList = flat.UtilityBillList;
                        rent = flat.Rent;
                        break;
                    }
                }
            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = $"{transaction.Id}.pdf",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = BillMemoGenerator.GetHTMLString(utilityBillModelList, rent,
                transaction.TransactionMonth, transaction.TransactionYear, tenant.FullName, homeowner.FullName),
                WebSettings = {
                    DefaultEncoding = "utf-8",
                    UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "bootstrap.min.css") },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            //_converter.Convert(pdf);

            return File(_converter.Convert(pdf),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                pdf.GlobalSettings.DocumentTitle);
        }
    }
}
