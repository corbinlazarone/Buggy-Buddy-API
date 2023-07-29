using BuggyBuddyAPI.Contracts;
using BuggyBuddyAPI.Models;
using BuggyBuddyAPI.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace buggyBuddyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserContract _userRepo;

        public UserController(IUserContract userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var result = await _userRepo.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var user = await _userRepo.GetUserById(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserIDByEmail/{email}")]
        public async Task<IActionResult> GetUserIDByEmail(string email)
        {
            try
            {
                var result = await _userRepo.GetUserIDByEmail(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequests _userRequest)
        {
            try
            {
                var result = await _userRepo.CreateUser(_userRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("LoginUser/{email}/{password}")]
        public async Task<IActionResult> LoginUser(string email, string password)
        {
            try
            {
                var user = await _userRepo.Login(email, password);

                if (user == null)
                {
                    return BadRequest();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUser/{userid}")]
        public async Task<IActionResult> DeleteUser(string userid)
        {
            try
            {
                var result = await _userRepo.DeleteUser(userid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}