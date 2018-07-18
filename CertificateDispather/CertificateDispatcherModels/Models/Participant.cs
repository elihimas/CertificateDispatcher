using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificateDispatcherModels.Models
{
    public class Participant
    {
        public Dictionary<string, string> Data { private set; get; }

        public Participant()
        {
            Data = new Dictionary<string, string>();
        }
    }
}

