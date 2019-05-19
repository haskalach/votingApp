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
        Task<IEnumerable<User>> GetOrganizationUsers (int orgId, int userId);
        Task<Photo> GetPhoto (int id);
        Task<PagedList<Engeneres>> GetEngeneres (EngenereParams engenereParams);
        Task<Photo> GetMainPhoto (int userId);
        Task<IEnumerable<Organization>> GetOrganizations ();
        Task<IEnumerable<OrganizationType>> GetOrganizationTypes ();
        Task<Organization> GetOrganization (int orgId);
        Task<Engeneres> GetEngenere(int code);

    }
}