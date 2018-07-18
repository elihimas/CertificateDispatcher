using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificateDispather.Models
{
    public enum SendCertificateResult
    {
        SUCCESS = 1,
        INVALID_SHEET = 2,
        EMPTY_MODEL = 3,
        EMPTY_SHEET = 4
    }
}
