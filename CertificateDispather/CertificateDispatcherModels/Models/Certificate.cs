using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CertificateDispatcherModels.Models
{
    public class Certificate
    {
        public string ModelText { get; private set; }
        public List<CertificateTag> Tags { get; private set; }

        public Certificate(string modelText, List<CertificateTag> tags)
        {
            ModelText = modelText;
            Tags = tags;
        }
    }
}
