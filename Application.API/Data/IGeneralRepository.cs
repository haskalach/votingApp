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
        Task<IEnumerable<User>> GetOrganizationReferences (int orgId, int userId);
        Task<Photo> GetPhoto (int id);
        Task<PagedList<Voter>> GetVoters (VoterParams engenereParams);
        Task<Photo> GetMainPhoto (int userId);
        Task<IEnumerable<Organization>> GetOrganizations ();
        Task<IEnumerable<VoterType>> GetOrganizationTypes ();
        Task<Organization> GetOrganization (int orgId);
        Task<Voter> GetVoter (int code,int VoterTypeId);
        Task<Voter> GetVoterById(int voterId,int OrganzationId);
        Task<VotingYears> GetVotingYear (int VoterId,int OrganizationId,string Year);

    }
}