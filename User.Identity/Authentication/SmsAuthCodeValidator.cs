using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Identity.Services;

namespace User.Identity.Authentication
{
    public class SmsAuthCodeValidator : IExtensionGrantValidator
    {
        private readonly IAuthCodeService _authCodeService;
        private readonly IUserService _userService;

        public SmsAuthCodeValidator(IAuthCodeService authCodeService, IUserService userService)
        {
            _authCodeService = authCodeService;
            _userService = userService;
        }

        public string GrantType => "sms_auth_code";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];
            var errorValidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
            {
                context.Result = errorValidationResult;
                return;
            }

            if (!_authCodeService.Validate(phone, authCode: code))
            {
                context.Result = errorValidationResult;
                return;
            }

            var userid = await _userService.CheckOrCreateAsync(phone);
            if (userid <= 0)
            {
                context.Result = errorValidationResult;
                return;
            }

            context.Result = new GrantValidationResult(userid.ToString(), GrantType);
        }
    }
}
