using System;
namespace API.Entities
{
    public class AppUser
    {
        public AppUser()
        {
        }

        /// <summary>
        /// Unique id for the user 
        /// Entity Framework will recognise this as an Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the user
        /// </summary>
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }       
    }
}
