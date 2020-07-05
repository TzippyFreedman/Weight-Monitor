using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Subscriber.Services;
using Subscriber.Services.Models;
using Subscriber.WebApi.Models;

namespace Subscriber.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Register(SubscriberDTO userRegister)
        {

            UserModel userToRegister = _mapper.Map<UserModel>(userRegister);
            UserFileModel userFileToRegister = _mapper.Map<UserFileModel>(userRegister);
            UserModel userAdded = await _userService.RegisterAsync(userToRegister, userFileToRegister);
            if (userAdded == null)
            {
                  Log.Information("User with email {@email} requested to create but already exists", userAdded.Email);
                throw new Exception("Bad Request: Patient with email ${ userAdded.Email } requested to create but already exists");
                //   throw new HttpResponseException(HttpStatusCode.NotFound);
                // return BadRequest($"patient with id:{newPatient.PatientId} already exists");
            }




            else
            {
                Log.Information("User with email {@email} created successfully", userAdded.Email);
                return StatusCode((int)HttpStatusCode.Created);
            }
        }
        [HttpPost]
        public int Login(SubscriberDTO userRegister)
        {

            return 1;
        }
        [HttpGet]
        public int Get(SubscriberDTO userRegister)
        {

            return 1;
        }

    }
}
