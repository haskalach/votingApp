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
    }
}