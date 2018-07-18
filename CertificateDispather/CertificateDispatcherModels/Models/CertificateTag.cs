using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CertificateDispatcherModels.Models
{
    public class CertificateTag
    {
        public string ParticipantActualDataKey { get; private set; }
        public string TagText { get; private set; }

        public CertificateTag(string participantActualDataKey, string tagText)
        {
            ParticipantActualDataKey = participantActualDataKey;
            TagText = tagText;
        }
    }
}
