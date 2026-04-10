using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.DTOs
{
    public class JournalEntryRequest
    {
        public Guid PatientId { get; set; }
        public string EntryType { get; set; } = string.Empty; // "feeling","thought","note"
        public string Content { get; set; } = string.Empty;
        public int MoodScore { get; set; }

    }
}
