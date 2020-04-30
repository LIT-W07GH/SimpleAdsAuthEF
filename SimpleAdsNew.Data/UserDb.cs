using System.Data.SqlClient;
using System.Linq;

namespace SimpleAdsNew.Data
{
    public class UserDb
    {
        private string _connectionString;

        public UserDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = hash;
            using (var context = new SimpleAdsContext(_connectionString))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isValidPassword)
            {
                return user; //success!!
            }

            return null;
        }

        public User GetByEmail(string email)
        {
            using (var context = new SimpleAdsContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }

        public bool IsEmailAvailable(string email)
        {
            using (var context = new SimpleAdsContext(_connectionString))
            {
                bool isUsed = context.Users.Any(u => u.Email == email);
                return !isUsed;
            }
        }
    }
}