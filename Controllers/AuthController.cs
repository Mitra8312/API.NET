using APISolovki.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using APISolovki.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.AspNetCore.Identity;

namespace APISolovki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public static Employee employee = new Employee();
        public static Prisoner prisoner = new Prisoner();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly PrisonSolovkiContext _context;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
            _context = new PrisonSolovkiContext();
        }

        [HttpPost("register-prisoner")]
        public async Task<ActionResult<Prisoner>> RegisterPrisoner(Prisoner request)
        {
            request.IdPrisoner = null;
            var passwordHash = HashString(request.PasswordPrisoner + request.Salt);

            prisoner.FirstNamePrisoner = request.FirstNamePrisoner;
            prisoner.SecondNamePrisoner = request.SecondNamePrisoner;
            prisoner.MiddleNamePrisoner = request.MiddleNamePrisoner;
            prisoner.LoginPrisoner = request.LoginPrisoner;
            prisoner.CasteId = request.CasteId;
            prisoner.WorkId = request.WorkId;
            prisoner.NationId = request.NationId;
            prisoner.ArticleOfTheConclusionId = request.ArticleOfTheConclusionId;
            prisoner.HealthId = request.HealthId;
            prisoner.IndividualOffersId = request.IndividualOffersId;
            prisoner.TypeOfActivityId = request.TypeOfActivityId;

            prisoner.PasswordPrisoner = passwordHash;
            prisoner.Salt = request.Salt;


            _context.Prisoners.Add(prisoner);
            await _context.SaveChangesAsync();

            return Ok(prisoner); 
        }

        [HttpPost("register-employee")]
        public async Task<ActionResult<Employee>> RegisterEmployee(Employee request)
        {
            request.IdEmployee = null;
            var passwordHash = HashString(request.PasswordEmployee + request.SaltEmployee);

            employee.LoginEmployee = request.LoginEmployee;
            employee.FirstNameEmployee = request.FirstNameEmployee;
            employee.SecondNameEmployee = request.SecondNameEmployee;
            employee.MiddleNameEmployee = request.MiddleNameEmployee;
            employee.PostId = request.PostId;
            employee.PasswordEmployee = passwordHash;
            employee.SaltEmployee = request.SaltEmployee;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpPost("login-prisoner")]
        public async Task<ActionResult<string>> LoginPrisoner(UserRegister request)
        {
            List<Prisoner> prisonerList = new List<Prisoner>();

            prisonerList = await _context.Prisoners.ToListAsync();

            foreach (var item in prisonerList)
            {
                if (item.LoginPrisoner == request.UserLogin)
                {
                    if (HashString(request.UserPassword + item.Salt).Equals(item.PasswordPrisoner))
                    {
                        string token = CreateTokenPrisoner(item);
                        return Ok(token);
                    }
                    else
                    {
                        return BadRequest("Wrong Password");
                    }
                }
            }
            return BadRequest("User not found");
        }

        [HttpPost("login-employee")]
        public async Task<ActionResult<string>> LoginEmployee(UserRegister request)
        {

            List<Employee> employeerList = new List<Employee>();
            employeerList = await _context.Employees.ToListAsync();

            foreach (var item in employeerList)
            {
                if (item.LoginEmployee == request.UserLogin)
                {
                    if (HashString(request.UserPassword + item.SaltEmployee).Equals(item.PasswordEmployee))
                    {
                        string token = CreateTokenAdmin(item);
                        return Ok(token);
                    }
                    else
                    {
                        return BadRequest("Wrong Password");
                    }
                }
            }

            return BadRequest("User not found");
        }
         

        [HttpGet("GetMe"), Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }

        private string CreateTokenAdmin(Employee user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.LoginEmployee),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private string CreateTokenPrisoner(Prisoner user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.LoginPrisoner),
                new Claim(ClaimTypes.Role, "Prisoner")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private static string HashString(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
