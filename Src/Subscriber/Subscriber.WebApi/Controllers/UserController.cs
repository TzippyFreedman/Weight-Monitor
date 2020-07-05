using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Subscriber.Services;
using Subscriber.Services.Models;
using Subscriber.WebApi.Models;
using Microsoft.AspNetCore.Routing;
namespace Subscriber.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<Guid>> Login(LoginDTO userLogin)
        {

            Guid patientCardId = await _userService.LoginAsync(userLogin.Email, userLogin.Password);
            if (patientCardId.Equals(Guid.Empty))
            {
                return Unauthorized();
            }
            else
            {
                return patientCardId;
            }
        }

        [HttpGet]
        [Route("{userCardId:Guid}")]
        public async Task<ActionResult<UserFileDTO>> GetUserFileById(Guid userCardId)
        {

            UserFileModel file = await _userService.GetUserFileById(userCardId);
            if (file == null)
            {
                return NoContent();
            }
            

            return _mapper.Map<UserFileDTO>(file);
        }

    }
}
