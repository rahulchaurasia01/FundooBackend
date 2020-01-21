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

        NotesDetails UpdateNotes(NotesDetails notesDetails);

        List<NotesDetails> GetAllNotes(int id);

        NotesDetails GetNote(int NoteId, int UserId);

        bool DeleteNote(int NoteId, int UserId);

    }
}
