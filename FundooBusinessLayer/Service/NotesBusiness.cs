using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using FundooRepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooBusinessLayer.Service
{
    public class NotesBusiness : INotesBusiness
    {

        private readonly INotesRepository _notesRepository;

        public NotesBusiness(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        public NotesDetails CreateNotes(CreateNoteRequest notesDetails, int userId)
        {
            try
            {
                if (notesDetails == null)
                    return null;
                else
                    return _notesRepository.CreateNotes(notesDetails, userId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool DeleteNote(int NoteId, int UserId)
        {

            if (NoteId <= 0 || UserId <= 0)
                return false;
            else
                return _notesRepository.DeleteNote(NoteId, UserId);

        }

        public List<NotesDetails> GetAllNotes(int id)
        {
            if (id <= 0)
                return null;
            else
                return _notesRepository.GetAllNotes(id);
        }

        public NotesDetails GetNote(int NoteId, int UserId)
        {

            if (NoteId <= 0 || UserId <= 0)
                return null;
            else
                return _notesRepository.GetNote(NoteId, UserId);

        }

        public NotesDetails UpdateNotes(NotesDetails notesDetails)
        {
            if (notesDetails == null)
                return null;
            else
                return _notesRepository.UpdateNotes(notesDetails);
        }
    }
}
