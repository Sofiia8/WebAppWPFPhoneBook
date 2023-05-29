using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebApiIdentity.Models;
using System.Linq;

namespace WebApiIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class RolesApiController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public RolesApiController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            return Ok(_roleManager.Roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRolesByUserId(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return Ok(await _userManager.GetRolesAsync(user));
            }
            return BadRequest();
        }

        [HttpPost("edit/{userId}")]
        public async Task<IActionResult> EditRolesForUser(string userId, [FromBody] string[] userNewRoles)
        {
            //JsonReader jsR = JsonReader(userNewRoles);
            var dop = userNewRoles.ToList<string>();
            List<string> ListNewRoles = dop as List<string>;
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IList<string> currentRoles = await _userManager.GetRolesAsync(user);
                IdentityResult result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
                result = await _userManager.AddToRolesAsync(user, ListNewRoles);
                if (result.Succeeded)
                    return Ok();
            }
            return BadRequest();
        }
    }
}
