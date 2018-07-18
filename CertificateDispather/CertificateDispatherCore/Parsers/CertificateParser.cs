using CertificateDispatcherModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CertificateDispatherCore.Parsers
{
    internal class CertificateParser
    {
        internal static Certificate Parse(string modelText)
        {
            var tags = new List<CertificateTag>();
            var pattern = new Regex(@"\{\{([A-Za-z0-9\-]+)\}\}");
            var match = pattern.Match(modelText);

            while (match.Success)
            {
                var participantActualDataKey = match.Groups[1].Value.ToLower();
                var tagText = match.ToString();

                tags.Add(new CertificateTag(participantActualDataKey, tagText));

                match = match.NextMatch();
            }


            return new Certificate(modelText, tags);
        }
    }
}
