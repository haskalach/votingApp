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

        public async Task<PagedList<Voter>> GetVoters (VoterParams voterParamas) {
            var voters = _context.Voters.Include (v => v.VoterType).Include (v => v.VotingYears).Include (v => v.VoterType).AsQueryable ();
            if (voterParamas.voterTypeId > 0) {
                voters = voters.Where (x => x.VoterTypeId == voterParamas.voterTypeId);
            }
            if (!string.IsNullOrEmpty (voterParamas.religion)) {
                voters = voters.Where (x => x.Religion == (voterParamas.religion).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.politic)) {

                voters = voters.Where (x => x.Politic == (voterParamas.politic).Trim ());
            }
            if (voterParamas.code > 0) {

                voters = voters.Where (x => x.Code == voterParamas.code);
            }
            if (!string.IsNullOrEmpty (voterParamas.firstNameArabic)) {

                voters = voters.Where (x => x.FirstNameArabic == (voterParamas.firstNameArabic).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.familyArabic)) {

                voters = voters.Where (x => x.FamilyArabic == (voterParamas.familyArabic).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.fatherNameArabic)) {

                voters = voters.Where (x => x.FatherNameArabic == (voterParamas.fatherNameArabic).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.subChapter)) {

                voters = voters.Where (x => x.SubChapter == (voterParamas.subChapter).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.school)) {

                voters = voters.Where (x => x.School.Contains ((voterParamas.school).Trim ()));
            }
            if (!string.IsNullOrEmpty (voterParamas.civilIdKadaa)) {

                voters = voters.Where (x => x.CivilIdKadaa == (voterParamas.civilIdKadaa).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.civilIdMouhavaza)) {

                voters = voters.Where (x => x.CivilIdMouhavaza == (voterParamas.civilIdMouhavaza).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.civilIdPlace)) {

                voters = voters.Where (x => x.CivilIdPlace == (voterParamas.civilIdPlace).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.civilIdRegion)) {

                voters = voters.Where (x => x.CivilIdRegion == (voterParamas.civilIdRegion).Trim ());
            }
            if (voterParamas.contacted != null) {
                voters = voters.Where (x => x.Contacted == voterParamas.contacted);
            }
            if (voterParamas.attend != null) {
                voters = voters.Where (x => x.Attend == voterParamas.attend);
            }
            if (voterParamas.abroad != null) {
                voters = voters.Where (x => x.Abroad == voterParamas.abroad);
            }
            if (voterParamas.referenceId > 0) {

                voters = voters.Where (x => x.ReferenceId == voterParamas.referenceId);
            }

            return await PagedList<Voter>.CreatAsync (voters, voterParamas.PageNumber, voterParamas.PageSize);
        }

        public async Task<IEnumerable<Voter>> GetAllVoters () {
            var voters = _context.Voters.Include (v => v.VoterType).ToListAsync ();

            return await voters;
        }
        public async Task<IEnumerable<Voter>> GetAllVotersByType (int voterTypeId) {
            var voters = _context.Voters.Where (v => v.VoterTypeId == voterTypeId).Include (v => v.VotingYears).ToListAsync ();

            return await voters;
        }
        public async Task<bool> SaveAll () {
            return await _context.SaveChangesAsync () > 0;
        }

        public async Task<IEnumerable<User>> GetOrganizationUsers (int orgId, int userId) {
            var users = await _context.Users.Where (u => u.OrganizationId == orgId && u.Id != userId).Include (u => u.UserRoles).ThenInclude (r => r.Role).ToListAsync ();
            return users;
        }

        public async Task<Voter> GetVoter (int code, int VoterTypeId, int? OrganzationId) {
            var voter = await _context.Voters.Where (v => v.VoterTypeId == VoterTypeId && v.Code == code).Include (v => v.VotingYears).Include (v => v.VoterType).FirstOrDefaultAsync ();
            if (OrganzationId > 0 && voter != null) {
                voter.VotingYears = voter.VotingYears.Where (y => y.OrganizationId == OrganzationId).ToList ();
            }
            return voter;
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

            if (users.Count > 0) {
                users = users.Where (u => userRolesId.Contains (u.Id)).ToList ();
            }

            return users;
        }

        private async Task<IEnumerable<int>> GetReferenceUserId () {
            var list = await _context.UserRoles.Where (r => r.RoleId == 5).Select (r => r.UserId).ToListAsync ();
            return list;
        }

        public async Task<PagedList<Voter>> GetReferenceVoters (int referenceId, VoterParams voterParamas) {
            var voters = _context.Voters.Where (v => v.ReferenceId == referenceId).Include (v => v.VotingYears).Include (v => v.VoterType).AsQueryable ();
            if (voterParamas.code > 0) {

                voters = voters.Where (x => x.Code == voterParamas.code);
            }
            if (!string.IsNullOrEmpty (voterParamas.firstNameArabic)) {

                voters = voters.Where (x => x.FirstNameArabic == (voterParamas.firstNameArabic).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.familyArabic)) {

                voters = voters.Where (x => x.FamilyArabic == (voterParamas.familyArabic).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.fatherNameArabic)) {

                voters = voters.Where (x => x.FatherNameArabic == (voterParamas.fatherNameArabic).Trim ());
            }
            if (voterParamas.contacted != null) {
                voters = voters.Where (x => x.Contacted == voterParamas.contacted);
            }
            if (voterParamas.attend != null) {
                voters = voters.Where (x => x.Attend == voterParamas.attend);
            }
            if (voterParamas.abroad != null) {
                voters = voters.Where (x => x.Abroad == voterParamas.abroad);
            }
            if (voterParamas.voted != null) {
                if (voterParamas.voted == true) {
                    voters = voters.Where (x => (x.VotingYears.Any (year => year.Year == DateTime.Today.Year.ToString ())));
                } else {
                    voters = voters.Where (x => x.VotingYears.Count == 0);
                }

            }
            return await PagedList<Voter>.CreatAsync (voters, voterParamas.PageNumber, voterParamas.PageSize);
        }

        public async Task<ConfigList> GetConfigList (int VoterTypeId, int OrganizationId) {
            var voters = await _context.Voters.Where (v => v.VoterTypeId == VoterTypeId).ToListAsync ();
            var organizationUsers = await GetOrganizationReferences (OrganizationId, 0);
            var CondifgList = new ConfigList () {
                religion = new List<string> (),
                politics = new List<string> (),
                subChapter = new List<string> (),
                civilIdMouhavaza = new List<string> (),
                civilIdKadaa = new List<string> (),
                civilIdRegion = new List<string> (),
                referenceUsers = new List<SelectObject> ()

            };

            foreach (var item in voters) {
                if (item.Religion != null && item.Religion.Trim () != "" && !CondifgList.religion.Contains (item.Religion)) {
                    CondifgList.religion.Add (item.Religion);
                }
                if (item.Politic != null && item.Politic.Trim () != "" && !CondifgList.politics.Contains (item.Politic)) {
                    CondifgList.politics.Add (item.Politic);
                }
                if (item.SubChapter != null && item.SubChapter.Trim () != "" && !CondifgList.subChapter.Contains (item.SubChapter)) {
                    CondifgList.subChapter.Add (item.SubChapter);
                }
                if (item.CivilIdMouhavaza != null && item.CivilIdMouhavaza.Trim () != "" && !CondifgList.civilIdMouhavaza.Contains (item.CivilIdMouhavaza)) {
                    CondifgList.civilIdMouhavaza.Add (item.CivilIdMouhavaza);
                }
                if (item.CivilIdKadaa != null && item.CivilIdKadaa.Trim () != "" && !CondifgList.civilIdKadaa.Contains (item.CivilIdKadaa)) {
                    CondifgList.civilIdKadaa.Add (item.CivilIdKadaa);
                }
                if (item.CivilIdRegion != null && item.CivilIdRegion.Trim () != "" && !CondifgList.civilIdRegion.Contains (item.CivilIdRegion)) {
                    CondifgList.civilIdRegion.Add (item.CivilIdRegion);
                }
            }
            foreach (var user in organizationUsers) {
                var selectObject = new SelectObject () {
                    Value = user.Id,
                    Name = user.UserName
                };
                CondifgList.referenceUsers.Add (selectObject);
            }
            return CondifgList;
        }

        public async Task<IEnumerable<User>> GetAllUsers () {
            var users = await _context.Users.ToListAsync ();
            return users;
        }

        public async Task<Organization> GetOrganizationByType (int voterTypeId) {
            var organization = await _context.Organizations.Where (o => o.VoterTypeId == voterTypeId).FirstOrDefaultAsync ();
            return organization;
        }

        public async Task<IEnumerable<Voter>> GetAllVotersByTypeFiltered (int VoterTypeId, VoterParams voterParamas) {
            var voters = _context.Voters.Where (v => v.VoterTypeId == VoterTypeId).Include (v => v.VotingYears).AsQueryable ();;
            if (voterParamas.voterTypeId > 0) {
                voters = voters.Where (x => x.VoterTypeId == voterParamas.voterTypeId);
            }
            if (!string.IsNullOrEmpty (voterParamas.religion)) {
                voters = voters.Where (x => x.Religion == (voterParamas.religion).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.politic)) {

                voters = voters.Where (x => x.Politic == (voterParamas.politic).Trim ());
            }
            if (voterParamas.code > 0) {

                voters = voters.Where (x => x.Code == voterParamas.code);
            }
            if (!string.IsNullOrEmpty (voterParamas.firstNameArabic)) {

                voters = voters.Where (x => x.FirstNameArabic == (voterParamas.firstNameArabic).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.familyArabic)) {

                voters = voters.Where (x => x.FamilyArabic == (voterParamas.familyArabic).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.fatherNameArabic)) {

                voters = voters.Where (x => x.FatherNameArabic == (voterParamas.fatherNameArabic).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.subChapter)) {

                voters = voters.Where (x => x.SubChapter == (voterParamas.subChapter).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.school)) {

                voters = voters.Where (x => x.School.Contains ((voterParamas.school).Trim ()));
            }
            if (!string.IsNullOrEmpty (voterParamas.civilIdKadaa)) {

                voters = voters.Where (x => x.CivilIdKadaa == (voterParamas.civilIdKadaa).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.civilIdMouhavaza)) {

                voters = voters.Where (x => x.CivilIdMouhavaza == (voterParamas.civilIdMouhavaza).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.civilIdPlace)) {

                voters = voters.Where (x => x.CivilIdPlace == (voterParamas.civilIdPlace).Trim ());
            }
            if (!string.IsNullOrEmpty (voterParamas.civilIdRegion)) {

                voters = voters.Where (x => x.CivilIdRegion == (voterParamas.civilIdRegion).Trim ());
            }
            if (voterParamas.contacted != null) {
                voters = voters.Where (x => x.Contacted == voterParamas.contacted);
            }
            if (voterParamas.attend != null) {
                voters = voters.Where (x => x.Attend == voterParamas.attend);
            }
            if (voterParamas.abroad != null) {
                voters = voters.Where (x => x.Abroad == voterParamas.abroad);
            }
            if (voterParamas.referenceId > 0) {

                voters = voters.Where (x => x.ReferenceId == voterParamas.referenceId);
            }
            return await voters.ToListAsync ();
        }

        public async Task<VoterType> GetOrganizationTypeById (int id) {
            var organizationType = await _context.VoterTypes.Where (ot => ot.Id == id).Include (v => v.Voters).FirstOrDefaultAsync ();
            return organizationType;
        }
    }
}