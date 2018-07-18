using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CertificateDispather.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Localization;
using CertificateDispatherCore.Commands;
using CertificateDispatherCore.Parsers;
using CertificateDispatcherModels.Models;

namespace CertificateDispather.Controllers
{
    public class HomeController : Controller
    {
        private const string BACKGROUND_IMAGE_PATH = "backgroundImagePath";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GenerateCertificatesAndDispatch(string modelText, string participantsText)
        {
            int sendCertificateResult;

            try
            {
                if (string.IsNullOrEmpty(modelText))
                {
                    sendCertificateResult = (int)SendCertificateResult.EMPTY_MODEL;
                }
                else if (string.IsNullOrEmpty(participantsText))
                {
                    sendCertificateResult = (int)SendCertificateResult.EMPTY_SHEET;
                }
                else
                {
                    List<Participant> participants = ParticipantsParser.Parse(participantsText);

                    var backgroundImagePath = HttpContext.Session.GetString(BACKGROUND_IMAGE_PATH);

                    var sendCertificateCommand = new SendCertificateCommand(modelText, participants, backgroundImagePath);
                    await sendCertificateCommand.Execute();

                    sendCertificateResult = (int)SendCertificateResult.SUCCESS;
                }
            }
            catch (ArgumentException)
            {
                sendCertificateResult = (int)SendCertificateResult.INVALID_SHEET;
            }

            return new JsonResult(new { result = sendCertificateResult });
        }

        [HttpPost("UploadBackgroundImage")]
        public async Task<IActionResult> UploadBackgroundImage(IFormFile file)
        {

            // full path to file in temp location
            var backgroundImagePath = Path.GetTempFileName();

            if (file.Length > 0)
            {
                using (var stream = new FileStream(backgroundImagePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    HttpContext.Session.SetString("backgroundImagePath", backgroundImagePath);
                }
            }

            return NoContent();
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
