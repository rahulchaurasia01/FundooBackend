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

        Task<List<NoteResponseModel>> GetAllNotes(int userId, string search);

        Task<List<NoteResponseModel>> GetAllDeletedNotes(int userId);

        Task<List<NoteResponseModel>> GetAllArchivedNotes(int userId);

        Task<List<NoteResponseModel>> GetAllPinnedNotes(int userId);

        Task<List<NoteResponseModel>> GetAllReminderNotes(int userId);

        Task<NoteResponseModel> UpdateNotes(int noteId, int userId, UpdateNoteRequest updateNotesDetails);

        Task<NoteResponseModel> AddlabelsToNote(int NoteId, int UserId, AddLabelNoteRequest addLabelNote);

        Task<bool> SendToTrash(ListOfDeleteNotes deleteNotes, int UserId);

        Task<bool> DeleteNotePermantely(ListOfDeleteNotes deleteNotes, int UserId);

        Task<NoteResponseModel> UpdateRemoveReminder(int NoteId, ReminderRequest reminder, int userId);

        Task<List<NoteResponseModel>> PinOrUnPinTheNote(ListOfPinnedNotes pinnedNotes, int userId);

        Task<List<NoteResponseModel>> ArchiveUnArchiveTheNote(ListOfArchiveNotes archiveRequest, int userId);

        Task<List<NoteResponseModel>> ColorTheNote(ListOfColorNotes colorRequest, int userId);

        Task<NoteResponseModel> AddUpdateImage(int NoteId, ImageRequest imageRequest, int userId);

        Task<NoteResponseModel> RemoveImage(int NoteId, int userId);

        Task<NoteResponseModel> AddUpdateCollaborator(int NoteId, CollaboratorsRequest collaboratorsRequest, int userId);

        Task<bool> BulkDeleteNote(int userId);

        Task<bool> RestoreDeletedNotes(int noteId, int userId);

        

    }
}
