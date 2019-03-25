using System.Collections.Generic;
using System.Threading.Tasks;
using Application.API.Helpers;
using Application.API.Models;

namespace Application.API.Data {
    public interface IGeneralRepository {
        void Add<T> (T entity) where T : class;
        void Delete<T> (T entity) where T : class;
        Task<bool> SaveAll ();

        Task<User> GetUser (int id);
        Task<Photo> GetPhoto (int id);
        Task<IEnumerable<Voter>> GetVoters ();
        Task<Photo> GetMainPhoto (int userId);

    }
}