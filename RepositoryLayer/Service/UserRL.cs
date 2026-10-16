
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext db;

        public UserRL(FundooContext db)
        {
            this.db = db;
        }

        public UserModel? GetUser(int id)
        {
            UserEntity? user = db.Users.Find(id);

            if (user == null)
            {
                return null;
            }

            return ToModel(user);
        }

        public UserModel PostUser(UserModel user)
        {
            UserEntity entity = ToEntity(user);

            db.Users.Add(entity);
            db.SaveChanges();

            user.Id = entity.Id;

            return user;
        }

        public bool EmailExists(string email)
        {
            return db.Users.Any(u => u.Email == email);
        }

        public UserModel? GetUserByEmail(string email)
        {
            UserEntity? user =
                db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            return ToModel(user);
        }

        public void SaveResetToken(
            UserModel user,
            string token,
            DateTime expiry)
        {
            UserEntity? userToUpdate = db.Users.Find(user.Id);

            if (userToUpdate == null)
            {
                return;
            }

            userToUpdate.ResetToken = token;
            userToUpdate.TokenExpiry = expiry;

            db.SaveChanges();
        }

        public bool ResetPassword(
            int userId,
            string token,
            string passwordHash)
        {
            UserEntity? user = db.Users.Find(userId);

            if (user == null ||
                user.ResetToken != token ||
                user.TokenExpiry == null ||
                user.TokenExpiry <= DateTime.UtcNow)
            {
                return false;
            }

            user.PasswordHash = passwordHash;
            user.ResetToken = null;
            user.TokenExpiry = null;

            db.SaveChanges();

            return true;
        }

        public void ClearResetToken(int userId)
        {
            UserEntity? user = db.Users.Find(userId);

            if (user == null)
            {
                return;
            }

            user.ResetToken = null;
            user.TokenExpiry = null;

            db.SaveChanges();
        }

        private static UserModel ToModel(UserEntity user)
        {
            return new UserModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ContactNo = user.ContactNo,
                PasswordHash = user.PasswordHash,
                ResetToken = user.ResetToken,
                TokenExpiry = user.TokenExpiry
            };
        }

        private static UserEntity ToEntity(UserModel user)
        {
            return new UserEntity
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ContactNo = user.ContactNo,
                PasswordHash = user.PasswordHash,
                ResetToken = user.ResetToken,
                TokenExpiry = user.TokenExpiry
            };
        }
    }
}
