using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.DTOs
{
   public class JournalEntryResponse
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string EntryType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int MoodScore { get; set; }
        public DateTime RecordedAt { get; set; }
        public bool IsSynced { get; set; }


    }
}
