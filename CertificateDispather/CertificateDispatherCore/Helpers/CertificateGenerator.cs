using System;
using System.Collections.Generic;
using System.Text;
using CertificateDispatcherModels.Models;

namespace CertificateDispatherCore.Helpers
{
    internal class CertificateGenerator
    {

        public static string GenerateCertificateForParticipant(Certificate certificate, Participant participant)
        {
            string result = $"<div style='text-align:center'>{certificate.ModelText}</center>";

            certificate.Tags.ForEach(tag =>
            {
                var participantValue = participant.Data[tag.ParticipantActualDataKey];
                result = result.Replace(tag.TagText, participantValue);
            });

            return result;
        }
    }
}
