using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.API.Helpers;
using Application.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.API.Data {
    public class GeneralRepository : IGeneralRepository {
        private readonly DataContext _context;
        public GeneralRepository (DataContext context) {
            _context = context;

        }
        public void Add<T> (T entity) where T : class {
            _context.Add (entity);
        }

        public void Delete<T> (T entity) where T : class {
            _context.Remove (entity);
        }

        public async Task<Photo> GetMainPhoto (int userId) {
            var photo = await _context.Photos.Where (u => u.UserId == userId).FirstOrDefaultAsync (p => p.IsMain);
            return photo;
        }

        public async Task<IEnumerable<Organization>> GetOrganizations () {
            var organizations = await _context.Organizations.Include (o => o.Users).Include (o => o.VoterType).ToListAsync ();
            return organizations;
        }
        public async Task<IEnumerable<VoterType>> GetOrganizationTypes () {
            var organizationTypes = await _context.VoterTypes.ToListAsync ();
            return organizationTypes;
        }
        public async Task<Organization> GetOrganization (int orgId) {
            var organization = await _context.Organizations.Include (o => o.Users).Include (o => o.VoterType).Where (o => o.Id == orgId).FirstOrDefaultAsync ();
            return organization;
        }

        public async Task<Photo> GetPhoto (int id) {
            var photo = await _context.Photos.FirstOrDefaultAsync (p => p.Id == id);
            return photo;
        }

        public async Task<User> GetUser (int id) {
            var user = await _context.Users.Include (p => p.Photos).FirstOrDefaultAsync (u => u.Id == id);
            return user;
        }

        public async Task<PagedList<Voter>> GetVoters (VoterParams engenereParams) {
            var voters = _context.Voters.Include (v => v.VoterType).AsQueryable ();
            if (engenereParams.voterTypeId > 0) {
                voters = voters.Where (x => x.VoterTypeId == engenereParams.voterTypeId);
            }
            if (!string.IsNullOrEmpty (engenereParams.religion)) {
                voters = voters.Where (x => x.Religion == (engenereParams.religion).Trim ());
            }
            if (!string.IsNullOrEmpty (engenereParams.politic)) {

                voters = voters.Where (x => x.Politic == (engenereParams.politic).Trim ());
            }
            return await PagedList<Voter>.CreatAsync (voters, engenereParams.PageNumber, engenereParams.PageSize);
        }
        public async Task<bool> SaveAll () {
            return await _context.SaveChangesAsync () > 0;
        }

        public async Task<IEnumerable<User>> GetOrganizationUsers (int orgId, int userId) {
            var users = await _context.Users.Where (u => u.OrganizationId == orgId && u.Id != userId).Include (u => u.UserRoles).ThenInclude (r => r.Role).ToListAsync ();
            return users;
        }

        public async Task<Voter> GetVoter (int code, int VoterTypeId) {
            var eng = _context.Voters.AsQueryable ();
            switch (VoterTypeId) {
                case 1:
                    eng = eng.Where (x => x.CodeEngenere == code);
                    break;

                case 2:
                    eng = eng.Where (x => x.CodePharmacist == code);
                    break;
                default:
                    break;
            }

            return await eng.FirstOrDefaultAsync ();
        }

        public async Task<VotingYears> GetVotingYear (int VoterId, int OrganizationId, string Year) {
            var VotingYear = await _context.VotingYears.Where (v => v.VoterId == VoterId).Where (v => v.OrganizationId == OrganizationId).Where (v => v.Year == Year).FirstOrDefaultAsync ();
            return VotingYear;
        }

        public async Task<Voter> GetVoterById (int VoterId, int OrganzationId) {
            var Voter = await _context.Voters.Where (v => v.Id == VoterId).Include (v => v.VotingYears).Include (v => v.VoterType).Include (v => v.Reference).FirstOrDefaultAsync ();
            Voter.VotingYears = Voter.VotingYears.Where (y => y.OrganizationId == OrganzationId).ToList ();
            return Voter;
        }

        public async Task<IEnumerable<User>> GetOrganizationReferences (int orgId, int userId) {
            var userRolesId = await GetReferenceUserId ();
            var users = await _context.Users.Where (u => u.OrganizationId == orgId && u.Id != userId).Include (u => u.UserRoles).ThenInclude (r => r.Role).ToListAsync ();
            users = users.Where (u => userRolesId.Contains (u.Id)).ToList ();
            return users;
        }

        private async Task<IEnumerable<int>> GetReferenceUserId () {
            var list = await _context.UserRoles.Where (r => r.RoleId == 5).Select (r => r.UserId).ToListAsync ();
            return list;
        }

        public async Task<PagedList<Voter>> GetReferenceVoters (int referenceId, VoterParams engenereParams) {
            var voters = _context.Voters.Where (v => v.ReferenceId == referenceId).Include (v => v.VoterType).AsQueryable ();
            if (engenereParams.voterTypeId > 0) {
                voters = voters.Where (x => x.VoterTypeId == engenereParams.voterTypeId);
            }
            if (!string.IsNullOrEmpty (engenereParams.religion)) {
                voters = voters.Where (x => x.Religion == (engenereParams.religion).Trim ());
            }
            if (!string.IsNullOrEmpty (engenereParams.politic)) {

                voters = voters.Where (x => x.Politic == (engenereParams.politic).Trim ());
            }
            return await PagedList<Voter>.CreatAsync (voters, engenereParams.PageNumber, engenereParams.PageSize);
        }
    }
}