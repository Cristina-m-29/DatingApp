using System.Threading.Tasks;
using System.Collections.Generic;
using DatingApp.API.Models;
using DatingApp.API.Helpers;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
         void add<T>(T entity) where T: class; 
         void delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoForUser(int userId);
         Task<Like> GetLike(int userId, int recipientId);
         Task<Message> GetMessage(int id); 
         Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
         Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}