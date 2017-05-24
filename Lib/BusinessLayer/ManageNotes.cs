using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace BusinessLayer
{
    public class ManageNotes
    {
        DevOpsEntities DevOpsContext;
        public string userName { get; set; }
        public string noteType { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/15/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageNotes()
        {
            DevOpsContext = new DevOpsEntities();
        }
        public ManageNotes(int id, string noteType)
        {
            if (!string.IsNullOrWhiteSpace(noteType))
                this.noteType = noteType;
            DevOpsContext = new DevOpsEntities();
        }
        public ManageNotes(DevOpsEntities context)
        {
            DevOpsContext = context;
        }

        /// <summary>   The type to string. </summary>
        private static Dictionary<System.Type, String> typeToString = new Dictionary<Type, string>()
        {
            {typeof(ViewModel.Note), "Notes" },
            {typeof(ViewModel.ConfigVariable), "ConfigVariables" },
            {typeof(ViewModel.EnvironmentDtoVariable), "EnvironmentVariables"},
        };

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a note. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/15/2017. </remarks>
        ///
        /// <param name="id">       The identifier. </param>
        /// <param name="noteType"> Type of the note. </param>
        ///
        /// <returns>   The note. </returns>
        ///-------------------------------------------------------------------------------------------------
        //public List<ViewModel.Note> GetNote(string id, string noteType)
        public List<ViewModel.Note> GetNote(int id, string noteType)
        {
            List<ViewModel.Note> notes = new List<ViewModel.Note>();
            int searchId;
            searchId = id;
            var efNotes = DevOpsContext.Notes.Where(x => x.note_type.ToLower() == noteType.ToLower()
                                                        && x.note_id == searchId
                                                    ).ToList();
            if (efNotes != null && efNotes.Any())
            {
                foreach (var note in efNotes)
                {
                    notes.Add(new ViewModel.Note()
                    {
                        id = note.id,
                        noteId = note.note_id,
                        noteType = note.note_type,
                        noteText = note.text,
                        createDate = note.create_date,
                        modifyDate = note.modify_date,
                        userName = note.last_modify_user,
                    });
                }
            }
            else
            {
                notes.Add(new ViewModel.Note()
                {
                    noteId = id,
                    noteType = noteType,
                    noteText = string.Empty,
                });
            }
            return notes;
        }

        public ViewModel.Note GetNote(int id, string noteType, DateTime createDate)
        {
            int searchId;
            searchId = id;
            var efNote = DevOpsContext.Notes.Where(x => x.note_type.ToLower() == noteType.ToLower()
                                                    && x.note_id == searchId
                                                    //&& x.create_date == createDate
                                                    ).FirstOrDefault();
            if (efNote != null)
                return (new ViewModel.Note()
                {
                    id = efNote.id,
                    noteId = efNote.note_id,
                    noteType = efNote.note_type,
                    noteText = efNote.text,
                    createDate = efNote.create_date,
                    modifyDate = efNote.modify_date,
                    userName = efNote.last_modify_user,
                });
            else
                return (new ViewModel.Note()
                {
                    noteId = id,
                    noteType = noteType,
                    noteText = string.Empty,
                });
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the note described by note. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/15/2017. </remarks>
        ///
        /// <param name="note"> The note. </param>
        ///
        /// <returns>   A ViewModel.Note. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Note UpdateNote(ViewModel.NoteDto note)
        {
            return UpdateNote(new ViewModel.Note()
            {
                noteId = note.noteId,
                noteType = note.noteType,
                noteText = note.noteText,
            });
        }

        public ViewModel.Note UpdateNote(ViewModel.Note note)
        {
            EFDataModel.DevOps.Note newEfNote;
            DateTime createDate = note.createDate ?? DateTime.Now;
            DateTime modifyDate = note.modifyDate ?? DateTime.Now;
            string user;
            string newNoteType;

            if (string.IsNullOrWhiteSpace(note.userName))
                user = Environment.UserName;
            else
                user = note.userName;

            if (!string.IsNullOrWhiteSpace(note.noteType))
                newNoteType = note.noteType;
            else if (!string.IsNullOrWhiteSpace(this.noteType))
                newNoteType = this.noteType;
            else
                newNoteType = "ConfigVariables";

            var efNote = DevOpsContext.Notes.Where(x => x.note_type.ToLower() == newNoteType.ToLower()
                                                        && x.note_id == note.noteId
                                                    //&& x.create_date == createDate
                                                    ).FirstOrDefault();
            if (efNote != null)
            {
                efNote.text = note.noteText;
                efNote.modify_date = modifyDate;
                efNote.last_modify_user = user;
            }
            else
            {
                newEfNote = new EFDataModel.DevOps.Note()
                {
                    note_id = note.noteId,
                    note_type = newNoteType,
                    create_date = createDate,
                    modify_date = modifyDate,
                    last_modify_user = user,
                    text = note.noteText,
                };
                DevOpsContext.Notes.Add(newEfNote);
            }
            DevOpsContext.SaveChanges();
            return GetNote(note.noteId, newNoteType, createDate);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the note by type. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/15/2017. </remarks>
        ///
        /// <param name="id">       The identifier. </param>
        /// <param name="noteType"> Type of the note. </param>
        ///
        /// <returns>   A ViewModel.Note. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Note DeleteNoteByType(int id, string noteType, DateTime? createDate = null)
        {
            DevOpsContext.Notes.Remove(DevOpsContext.Notes.Where(x => x.note_id == id
                                                                    && x.note_type == noteType
                                                                //&& x.create_date == createDate
                                                                ).FirstOrDefault());
            DevOpsContext.SaveChanges();
            EFDataModel.DevOps.Note deletedEfNote = DevOpsContext.Notes.Where(x => x.note_id == id
                                                && x.note_type == noteType
                                            ).FirstOrDefault();
            if (deletedEfNote != null)
                return new ViewModel.Note()
                {
                    id = deletedEfNote.id,
                    noteId = deletedEfNote.note_id,
                    noteType = deletedEfNote.note_type,
                    createDate = deletedEfNote.create_date,
                    modifyDate = deletedEfNote.modify_date,
                    userName = deletedEfNote.last_modify_user,
                    noteText = deletedEfNote.text,
                };
            else
                return new ViewModel.Note();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the note by identifier described by ID. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/15/2017. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   A ViewModel.Note. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Note DeleteNoteById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
