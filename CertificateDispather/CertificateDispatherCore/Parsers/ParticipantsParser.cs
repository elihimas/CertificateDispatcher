using CertificateDispatcherModels.Models;
using System;
using System.Collections.Generic;

namespace CertificateDispatherCore.Parsers
{
    public class ParticipantsParser
    {
        private const string EmailHeader = "email";

        public static List<Participant> Parse(string participantsText)
        {
            var rows = participantsText.TrimEnd().Split("\n");
            var headers = rows[0].Split("\t");

            VerifyHeaders(headers);
            ToLower(headers);

            List<Participant> participants = new List<Participant>();

            for(int rowIndex = 1; rowIndex < rows.Length; rowIndex++)
            {
                var participant = new Participant();
                var participantData = rows[rowIndex].Split("\t");
                
                for(int colIndex = 0; colIndex < headers.Length; colIndex++)
                {
                    var header = headers[colIndex];
                    participant.Data[header] = participantData[colIndex];
                }

                participants.Add(participant);
            }

            return participants;
        }

        private static void VerifyHeaders(string[] headers)
        {
            foreach(var header in headers)
            {
                if (header.ToLower().Equals(EmailHeader))
                {
                    return;
                }
            }

            throw new ArgumentException();
        }

        private static void ToLower(string[] strings)
        {
            for(int i = 0; i< strings.Length; i++)
            {
                strings[i] = strings[i].ToLower();
            }
        }
    }
}