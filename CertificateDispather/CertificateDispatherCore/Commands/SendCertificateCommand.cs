using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SendGrid;
using SendGrid.Helpers.Mail;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using CertificateDispatherCore.Parsers;
using CertificateDispatcherModels.Models;
using CertificateDispatherCore.Helpers;

namespace CertificateDispatherCore.Commands
{
    public class SendCertificateCommand
    {
        private const string apiKey = "COLOCAR_API_KEY";
        private const string subject = "Certificado";
        private const string AttachmentFileName = "certificado.pdf";
        private const string EmailBody = "Olá! segue em anexo o seu certificado";
        private const string EmailHeader = "email";

        private Certificate Certificate;
        private List<Participant> Participants;
        private string BackgroundImagePath;

        public SendCertificateCommand(string modelText, List<Participant> participants, string backgroundImagePath)
        {
            Certificate = CertificateParser.Parse(modelText);
            Participants = participants;
            BackgroundImagePath = backgroundImagePath;
        }

        public async Task Execute()
        {
            foreach (Participant participant in Participants)
            {
                var certificatePdfStrem = GeneratePDFStream(participant);
                await SendToPaticipant(participant, certificatePdfStrem);
            }
        }

        private MemoryStream GeneratePDFStream(Participant participant)
        {
            var certificateText = CertificateGenerator.GenerateCertificateForParticipant(Certificate, participant);

            var rotatedA4 = PageSize.A4.Rotate();
            var doc = new Document(rotatedA4);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

            doc.Open();
            if (BackgroundImagePath != null)
            {
                var backgroundImage = Image.GetInstance(BackgroundImagePath);
                backgroundImage.Alignment = Image.UNDERLYING;
                backgroundImage.ScaleAbsolute(rotatedA4);
                backgroundImage.SetAbsolutePosition(0, 0);

                doc.Add(backgroundImage);
            }

            using (var htmlWorker = new HTMLWorker(doc))
            {

                //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                using (var sr = new StringReader(certificateText))
                {
                    //Parse the HTML
                    htmlWorker.Parse(sr);
                }
            }

            writer.CloseStream = false;
            doc.Close();

            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task SendToPaticipant(Participant participant, MemoryStream certificatePdfStrem)
        {
            var client = new SendGridClient(apiKey);
            var participantEmail = participant.Data[EmailHeader];

            var from = new EmailAddress("dedeco@even3.com.br", "Dedeco Junior");
            var to = new EmailAddress(participantEmail, participantEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, EmailBody, EmailBody);
            msg.AddAttachment(AttachmentFileName, Convert.ToBase64String(certificatePdfStrem.GetBuffer()));

            await client.SendEmailAsync(msg);
        }
    }
}
