using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Subscriber.WebApi.Models;

namespace Subscriber.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {


        [HttpPost]
        public bool register(RegisterDTO userRegister)
        {
            return true;
        }
        [HttpPost]
        public int login(RegisterDTO userRegister)
        {

            return 1;
        }
        [HttpGet]
        public int get(RegisterDTO userRegister)
        {

            return 1;
        }

    }
}
