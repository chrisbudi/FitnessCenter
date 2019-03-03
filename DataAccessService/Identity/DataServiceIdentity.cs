using System.Linq;
using ViewModel.Identity;

namespace DataAccessService.Identity
{

    public class DataServiceIdentity : DbDataAccessIdentity
    {
        //public static ApplicationDbContext Db = new ApplicationDbContext();
        public User GetUserByName(string name)
        {
            var user = (from p in DbIdentity.Users
                        where p.UserName == name
                        select new User()
                        {
                            Email = p.Email,
                            UserName = p.UserName,
                            Id = p.Id
                        }).SingleOrDefault();
            return user;
        }

    }
}

