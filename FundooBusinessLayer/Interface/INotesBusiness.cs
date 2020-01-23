using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooBusinessLayer.Interface
{
    public interface INotesBusiness
    {

        NoteResponseModel CreateNotes(NoteRequest notesDetails, int userId);

        NoteResponseModel GetNote(int NoteId, int UserId);

        List<NoteResponseModel> GetAllNotes(int userId);

        List<NoteResponseModel> GetAllDeletedNotes(int userId);

        List<NoteResponseModel> GetAllArchivedNotes(int userId);

        List<NoteResponseModel> GetAllPinnedNotes(int userId);

        NoteResponseModel UpdateNotes(int noteId, int userId, NoteRequest updateNotesDetails);

        bool DeleteNote(int NoteId, int UserId);

        bool DeleteNotesPermanently(int userId);

        bool RestoreDeletedNotes(int noteId, int userId);

        List<NoteResponseModel> SortByReminderNotes(int userId);

    }
}
