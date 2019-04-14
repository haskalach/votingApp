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

        public async Task<IEnumerable<Organization>> GetOrganizations()
        {
            var organizations = await _context.Organizations.Include(o=>o.Users).Include(o =>o.OrganizationType).ToListAsync ();
            return organizations;
        }
        public async Task<Organization> GetOrganization(int orgId)
        {
            var organization = await _context.Organizations.Include(o=>o.Users).Include(o =>o.OrganizationType).Where(o=>o.Id == orgId).FirstOrDefaultAsync();
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

        public async Task<IEnumerable<Voter>> GetVoters () {
            var voters = await _context.Voters.ToListAsync ();
            return voters;
        }
        public async Task<bool> SaveAll () {
            return await _context.SaveChangesAsync () > 0;
        }
    }
}