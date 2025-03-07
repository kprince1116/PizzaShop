using Microsoft.EntityFrameworkCore;
using Pizzashop.Data;
using DAL.Interfaces;
// using DAL.Models;
using Pizzashop.DAL.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DAL.Models;
using Npgsql.Internal.TypeHandlers;

namespace Pizzashop.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PizzaShopContext _db;

        public UserRepository(PizzaShopContext db)
        {
            _db = db;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _db.Users
       .Include(u => u.UserroleNavigation)
       .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailWithRole(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailFor(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateUser(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmailForProfile(string email)
        {
            return await _db.Users.Include(u => u.UserroleNavigation).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task updateUser(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public List<Country> GetCountries()
        {
            return _db.Countries.ToList();
        }

        public List<State> GetStates(int countryId)
        {
            return _db.States.Where(s => s.Country == countryId).ToList();
        }

        public List<City> GetCities(int stateId)
        {
            return _db.Cities.Where(c => c.State == stateId).ToList();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> ChangePassword(User existingUser)
        {
            _db.Users.Update(existingUser);
            await _db.SaveChangesAsync();
            return existingUser;
        }

        public async Task<List<UserListviewmodel>> GetUserList(int pageNumber, int pageSize, string searchTerm)
        {
            var query = _db.Users.Where(u => !u.Isdeleted).Select(u => new
            {
                u.UserId,
                u.Firstname,
                u.Lastname,
                u.Username,
                u.Email,
                u.Phonenumber,
                u.ProfileImage,
                Status = u.Status ?? true,
                Userrole = u.Userrole,
                Rolename = _db.Userroles.Where(r => r.Roleid == u.Userrole).Select(r => r.Rolename).FirstOrDefault()
            });

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u => u.Firstname.Contains(searchTerm) ||
                                         u.Lastname.Contains(searchTerm) ||
                                         u.Email.Contains(searchTerm));
            }

            var userList = await query.Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            return userList.Select(u => new UserListviewmodel
            {
                UserId = u.UserId,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Email = u.Email,
                Phonenumber = u.Phonenumber,
                ProfileImage = u.ProfileImage,
                Status = u.Status,
                userrole = u.Rolename,
            }).ToList();
        }

        public Task<int> GetUsercount(string searchTerm)
        {
            var query = _db.Users.Where(u => !u.Isdeleted);

            return query.CountAsync();
        }

        public async Task<bool> AddUserAsync(AddUserviewmodel user)
        {

            var newuser = new User
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Userrole = user.Userrole,
                Status = user.Status,
                Country = user.Country,
                State = user.State,
                City = user.City,
                Phonenumber = user.Phonenumber,
                Address = user.Address,
                Zipcode = user.Zipcode,
            };

            if (user.ProfileImage != null)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filename = $"{Guid.NewGuid()}_{user.ProfileImage.FileName}";
            string filepath = Path.Combine(uploadsFolder, filename);

            using (FileStream fileStream = new FileStream(filepath, FileMode.Create))
            {
                await user.ProfileImage.CopyToAsync(fileStream);
            }

            newuser.ProfileImage = $"/uploads/{filename}"; 
            
        }

             _db.Users.AddAsync(newuser);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserById(int UserId)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
        }

        public Task<User> GetUserByEmailForChangePassword(string email)
        {
            return _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateUserforChangePassword(User existingUser)
        {
            _db.Users.Update(existingUser);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User existingUser)
        {

            

            _db.Users.Update(existingUser);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Userrole>> GetRolesAsync()
        {
            return _db.Userroles.ToList();
        }

        public async Task<IEnumerable<Country>> GetCountryAsync()
        {
            return _db.Countries.ToList();
        }

        public async Task<IEnumerable<State>> GetStateAsync()
        {
            return _db.States.ToList();
        }

        public async Task<IEnumerable<City>> GetCityAsync()
        {
            return _db.Cities.ToList();
        }

        public async Task<User> GetUserByIdForDelete(int UserId)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
        }

        public async Task DeleteUserAsync(User existingUser)
        {
            _db.Users.Update(existingUser);
            await _db.SaveChangesAsync();
        }

    }
}

