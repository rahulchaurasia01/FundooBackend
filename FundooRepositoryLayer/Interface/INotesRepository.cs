using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepositoryLayer.Interface
{
    public interface INotesRepository
    {

        Task<NoteResponseModel> CreateNotes(NoteRequest notesDetails, int userId);

        Task<NoteResponseModel> GetNote(int NoteId, int UserId);

        Task<List<NoteResponseModel>> GetAllNotes(int userId);

        Task<List<NoteResponseModel>> GetAllDeletedNotes(int userId);

        Task<List<NoteResponseModel>> GetAllArchivedNotes(int userId);

        Task<List<NoteResponseModel>> GetAllPinnedNotes(int userId);

        Task<NoteResponseModel> UpdateNotes(int noteId, int userId, NoteRequest updateNotesDetails);

        Task<bool> DeleteNote(int NoteId, int UserId);

        Task<bool> DeleteNotesPermanently(int userId);

        Task<bool> RestoreDeletedNotes(int noteId, int userId);

        Task<List<NoteResponseModel>> SortByReminderNotes(int userId);

    }
}
