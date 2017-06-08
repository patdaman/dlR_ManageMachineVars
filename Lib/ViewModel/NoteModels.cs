using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class Note
    {
        public int id { get; set; }
        public int noteId { get; set; }
        //public string noteId { get; set; }
        public string noteType { get; set; }
        public string noteText { get; set; }
        public Nullable<DateTime> createDate { get; set; }
        public Nullable<DateTime> modifyDate { get; set; }
        public string userName { get; set; }
    }
    public class NoteDto
    {
        public int noteId { get; set; }
        public string noteType { get; set; }
        public string noteText { get; set; }
    }
}
