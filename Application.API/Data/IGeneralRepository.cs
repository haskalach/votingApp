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
        Task<IEnumerable<User>> GetAllUsers ();
        Task<IEnumerable<User>> GetOrganizationUsers (int orgId, int userId);
        Task<IEnumerable<User>> GetOrganizationReferences (int orgId, int userId);
        Task<Photo> GetPhoto (int id);
        Task<PagedList<Voter>> GetVoters (VoterParams voterParam);
        Task<IEnumerable<Voter>> GetAllVoters ();
        Task<IEnumerable<Voter>> GetAllVotersByType (int VoterTypeId);
        Task<IEnumerable<Voter>> GetAllVotersByTypeFiltered (int VoterTypeId, VoterParams engenereParams);
        Task<PagedList<Voter>> GetReferenceVoters (int referenceId, VoterParams engenereParams);
        Task<Photo> GetMainPhoto (int userId);
        Task<IEnumerable<Organization>> GetOrganizations ();
        Task<Organization> GetOrganizationByType (int voterTypeId);
        Task<IEnumerable<VoterType>> GetOrganizationTypes ();
        Task<VoterType> GetOrganizationTypeById (int id);
        Task<Organization> GetOrganization (int orgId);
        Task<Voter> GetVoter (int code, int VoterTypeId, int? OrganzationId);
        Task<Voter> GetVoterById (int voterId, int OrganzationId);
        Task<VotingYears> GetVotingYear (int VoterId, int OrganizationId, string Year);
        Task<ConfigList> GetConfigList (int VoterTypeId, int OrganizationId);

    }
}