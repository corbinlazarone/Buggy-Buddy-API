using BuggyBuddyAPI.Contracts;
using BuggyBuddyAPI.Models;
using BuggyBuddyAPI.Models.Request;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BuggyBuddyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuggyController : ControllerBase
    {
        private readonly IBuggyContract _buggyRepo;

        public BuggyController(IBuggyContract buggyRepo)
        {
            _buggyRepo = buggyRepo;
        }

        // GET: grabs all buggys in db
        [HttpGet]
        [Route("getAllBuggys")]
        public async Task<IActionResult> GetAll()
        {
           try
            {
                var result = await _buggyRepo.GetAllBuggys();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: grab buggy by user id.
        [HttpGet]
        [Route("GetBuggy/{userid}")]
        public async Task<IActionResult> GetUserBuggy(string userId)
        {
            try
            {
                var result = await _buggyRepo.GetUserBuggyUrls(userId);
                if (result != new List<BuggyModel>())
                {
                    return Ok(result);
                }

                return StatusCode(400, "Buggy could not be found for that id.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: add buggy url to user.
        [HttpPost]
        [Route("AddBuggyUrl")]
        public async Task<IActionResult> CreateBuggy([FromBody] BuggyRequest _buggyRequest)
        {
            try
            {
                var result = await _buggyRepo.AddBuggyUrlToUser(_buggyRequest.UserId, _buggyRequest.URL);

                if (result == "Buggy url successfully added!")
                {
                    return Ok(result);
                }
                else if (result == "Buggy url not valid.")
                {
                    return StatusCode(400, "Url is not valid, or is not formatted correctly.");
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       // DELETE: delete buggy by buggyId and userId.
       [HttpDelete]
        [Route("deleteBuggyUrl/{buggyId}/{userID}")]
        public async Task<IActionResult> DeleteBuggy(string buggyId, string userId)
        {
            try
            {
                var result = await _buggyRepo.DeleteBuggy(buggyId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
