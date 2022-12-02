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

        public PdfCreatorController(IConverter converter, TenantRegistrationFormService tenantRegistrationFormService)
        {
            _converter = converter;
            _tenantRegistrationFormService = tenantRegistrationFormService;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult> CreatePDF(string userName)
        {
            var tenantRegistrationFormModel = await _tenantRegistrationFormService.GetByUserNameAsync(userName);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "RegistrationForm.pdf",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(tenantRegistrationFormModel),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
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
            //return Ok("Successfully created PDF document.");
        }
    }
}
