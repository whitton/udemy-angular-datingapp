using System.Collections.Generic;
using System.Linq;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        public UsersController(DataContext context)
        {
            Context = context;
        }

        public DataContext Context { get; }

        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> Users()
        {
            var users = Context.Users.ToList();
            return users;
        }       

        [HttpGet("{id}")]
        public ActionResult<AppUser> FetchUser(int id)
        {
            var user = Context.Users.Where(x => x.Id == id).FirstOrDefault();
            return user;
        }
    }
}
