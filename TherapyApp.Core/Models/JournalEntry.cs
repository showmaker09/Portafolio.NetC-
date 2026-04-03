using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.Models
{
   public  class JournalEntry
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string EntryType { get; set; } = string.Empty; // "feeling", "thought", "note"
        public string Content { get; set; } = string.Empty; 
        public int MoodScore { get; set; }                    // 1 al 10
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SyncedAt { get; set; }
        public bool IsSynced { get; set; } = false;

        // Navegación
        public Patient Patient { get; set; } = null!;

    }
}
