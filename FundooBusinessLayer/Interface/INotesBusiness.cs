using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooBusinessLayer.Interface
{
    public interface INotesBusiness
    {

        NotesDetails CreateNotes(CreateNoteRequest notesDetails, int userId);

        NotesDetails GetNote(int NoteId, int UserId);

        List<NotesDetails> GetAllNotes(int userId);

        List<NotesDetails> GetAllDeletedNotes(int userId);

        List<NotesDetails> GetAllArchivedNotes(int userId);

        List<NotesDetails> GetAllPinnedNotes(int userId);

        NotesDetails UpdateNotes(NotesDetails notesDetails);

        bool DeleteNote(int NoteId, int UserId);

        bool DeleteNotesPermanently(int userId);

        bool RestoreDeletedNotes(int noteId, int userId);

        List<NotesDetails> SortByReminderNotes(int userId);

    }
}
