using Microsoft.AspNetCore.Mvc;
using Qu.API.Models;
using Qu.Data;
using Qu.Dto;
using static Qu.Utility.Enums;
using Qu.Api.FilterAttributes;
using Microsoft.AspNetCore.JsonPatch;
using Qu.Api.Helpers.Claims;
using Qu.Utility;
using System.Globalization;

namespace Qu.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILookupService _lookupService;
        private readonly IAppLogger _logger;
        private readonly UserClaimDetails _currentuser;

        public UserController(IUserClaim userContext, ILookupService lookupService, IAppLogger appLogger)
        {
            _currentuser = userContext.CurrentUser;
            _lookupService = lookupService;
            _logger = appLogger;
        }

        [HttpGet]
        [BasicAuthorization]
        public async Task<IActionResult> Get()
        {
            User user = await dbUser.Get(_currentuser.userid);
            if (user == null || user.userid <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Authorize, (int)MessageType.Authorize.WrongUsernameOrPassword));
            }

            _logger.Log(LogCategory.Trace_0, nameof(UserController), nameof(Login), Events.Database, (int)EventType.Database.Read, Message.Authorize, (int)MessageType.Authorize.LoggedIn);

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<User>(_lookupService).CreateResponse(true, EventStatus.Success, Message.Authorize, (int)MessageType.Authorize.LoggedIn, user));
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Authorize, (int)MessageType.Authorize.WrongUsernameOrPassword));
            }

            User user = await dbUser.Get(email);
            if (user == null || user.userid <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Authorize, (int)MessageType.Authorize.WrongUsernameOrPassword));
            }

            if (email != user.email ||
                password != user.password)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Authorize, (int)MessageType.Authorize.WrongUsernameOrPassword));
            }

            User User = await dbUser.Get(user.userid);
            if (user == null || user.userid <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Authorize, (int)MessageType.Authorize.WrongUsernameOrPassword));
            }

            _logger.Log(LogCategory.Trace_0, nameof(UserController), nameof(Login), Events.Database, (int)EventType.Database.Read, Message.Authorize, (int)MessageType.Authorize.LoggedIn);

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<User>(_lookupService).CreateResponse(true, EventStatus.Success, Message.Authorize, (int)MessageType.Authorize.LoggedIn, User));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User us)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Validation, (int)MessageType.Validation.ValidationError));
            }

            User user = await dbUser.Get(us.email);
            if (user != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Record, (int)MessageType.Record.AlreadyExist));
            }

            if (await dbUser.Insert(us) < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Exception, (int)MessageType.Exception.Unexpected));
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Success, Message.Record, (int)MessageType.Record.Created));
        }

        [HttpPut]
        [BasicAuthorization]
        public async Task<IActionResult> Update([FromForm] User us)
        {
            if (!ModelState.IsValid || us.userid <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Validation, (int)MessageType.Validation.ValidationError));
            }

            User user = await dbUser.Get(_currentuser.userid);
            if (user == null || user.userid <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Authorize, (int)MessageType.Authorize.WrongUsernameOrPassword));
            }

            if (us.logo_file != null && us.logo_file.Length > 0)
            {
                // Resmi kaydeder
                ImageSaver imageSaver = new();

                string logoPath = imageSaver.SaveImage(us.logo_file);
                if (!string.IsNullOrEmpty(logoPath))
                {
                    user.logo_path = logoPath;
                }
            }

            user.birthday = us.birthday;
            user.surname = us.surname;
            user.id_no = us.id_no;
            user.name = us.name;
            user.password = us.password;
            user.gsm = us.gsm;
            user.id_no = us.id_no;
            user.is_active = us.is_active;
            user.gender_type = us.gender_type;
            user.working_with_bankid = us.working_with_bankid;
            user.bank_account_code = us.bank_account_code;
            user.iban = us.iban;

            if (await dbUser.Update(user) < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Exception, (int)MessageType.Exception.Unexpected));
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Success, Message.Record, (int)MessageType.Record.Updated));
        }

        [HttpPatch]
        [BasicAuthorization]
        public async Task<IActionResult> Deactive([FromBody] JsonPatchDocument<User> patchDocument)
        {
            if (_currentuser.userid <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Exception, (int)MessageType.Exception.Unexpected));
            }

            if (!ModelState.IsValid || patchDocument == null || patchDocument.Operations.Count <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Exception, (int)MessageType.Exception.Unexpected));
            }

            if (patchDocument.Operations.Any(o => !o.path.Equals("/is_active", StringComparison.OrdinalIgnoreCase)))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Exception, (int)MessageType.Exception.Unexpected));
            }

            User user = await dbUser.Get(_currentuser.userid);
            if (user == null || user.userid <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Authorize, (int)MessageType.Authorize.WrongUsernameOrPassword));
            }

            if (_currentuser.userid != user.userid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Exception, (int)MessageType.Exception.Unexpected));
            }

            // a model item value changed to posted value
            patchDocument.ApplyTo(user);

            if (await dbUser.Update(user) < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Danger, Message.Exception, (int)MessageType.Exception.Unexpected));
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>(_lookupService).CreateResponse(true, EventStatus.Success, Message.Record, (int)MessageType.Record.Updated));
        }
    }
}
