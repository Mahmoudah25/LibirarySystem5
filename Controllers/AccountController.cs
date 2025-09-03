using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentManagmentSystem.DTO;
using StudentManagmentSystem.Migrations;
using StudentManagmentSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> usermanger;
        private readonly IConfiguration config;
        public AccountController(UserManager<ApplicationUser> usermanger,IConfiguration config) 
        {
            this.usermanger = usermanger;
            this.config = config;
        }

        //Register
        [HttpPost("Register")]
        public async Task <IActionResult> Register(RegisterDTO UserFromRequest)
        {
            if (ModelState.IsValid) 
            {
                //SaveDb
                ApplicationUser user = new ApplicationUser();
                user.UserName = UserFromRequest.UserName;
                user.Email = UserFromRequest.Email;
                IdentityResult result = await usermanger.CreateAsync(user,UserFromRequest.Password);
                if (result.Succeeded) 
                {
                    return Ok("Created");
                }
                foreach (var item in result.Errors) 
                {
                    ModelState.AddModelError("Password",item.Description);
                }
            }
            return BadRequest(ModelState);
        }


        //Login
        [HttpPost("LogIn")]
        public async Task <IActionResult> LogIn(LogInDTO UserFromRequest)
        {
            if (ModelState.IsValid)
            {
                //Check
                ApplicationUser UserfromDB = await usermanger.FindByNameAsync(UserFromRequest.UserName);
                if (UserfromDB != null) 
                {
                    bool found = await usermanger.CheckPasswordAsync(UserfromDB, UserFromRequest.Password);
                    if (found == true) 
                    {
                        //Generate Token


                        List<Claim> userClaims = new List<Claim>();

                        //Token Generated id Change(JWt Predefined Claims )
                        userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        userClaims.Add(new Claim(ClaimTypes.NameIdentifier, UserfromDB.UserName));
                        userClaims.Add(new Claim(ClaimTypes.Name, UserfromDB.UserName));
                        var UserRoles = await usermanger.GetRolesAsync(UserfromDB);
                        foreach (var RoleName in UserRoles)
                        {
                            userClaims.Add(new Claim(ClaimTypes.Role, RoleName));
                        }

                        var SignInKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            config["JWT:SecurityKey"]));

                        SigningCredentials signingCredentials =
                            new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

                        //Design Token
                        JwtSecurityToken MyToken = new JwtSecurityToken(
                            audience: config["JWT:AudienceIp"],
                            issuer: config["JWT:IssuerIP"],
                            expires:DateTime.Now.AddHours(24),
                            claims: userClaims,
                            signingCredentials: signingCredentials


                            );

                        //genreate Token Response
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(MyToken),
                            expiration = DateTime.Now.AddHours(24)
                        });
                    }
                }
                ModelState.AddModelError("UserName", "UserName OR Password Wrong");
            }
            return BadRequest(ModelState);
        }
    }
}
