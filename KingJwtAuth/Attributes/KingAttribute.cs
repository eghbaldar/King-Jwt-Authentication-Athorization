﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using static KingJwtAuth.Attributes.KingAttributeEnum;
using KingJwtAuth.Services.Token;
using KingJwtAuth.Models;
using KingJwtAuth.Services.UserLogs;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using KingJwtAuth.Services.UserRefreshToken;
using KingJwtAuth.Entities;
using KingJwtAuth.Context;
using System.Security.Claims;
using KingJwtAuth.Services;
using System.Reflection;

namespace KingJwtAuth.Attributes
{    /// <summary>
     /// What is the difference between this attribute and [KingAttribute]?
     /// In [KingAttribute], the method fires only if the user is authenticated.
     /// But in [KingCheckUserAttribute], the method runs either way—whether the user is authenticated or unauthenticated.
     /// So what's the point of using [KingCheckUserAttribute]? The point is, if the target Razor page (in *.cshtml)
     /// has some parts that need to access and check the user, this attribute ([KingCheckUserAttribute]) helps.
     /// On the other hand, GUESTs (unauthenticated users) are able to see the material in it.
     /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class KingAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserRole[] _roles;
        //NOTE: means you can pass one or many enum values naturally without needing an array manually!
        public KingAttribute(params UserRole[] roles)
        {
            _roles = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpContext httpContext = context.HttpContext;

            // Access the DI container from HttpContext
            var serviceProvider = context.HttpContext.RequestServices;
            var userRefreshTokenService = serviceProvider.GetRequiredService<UserRefreshTokenService>();
            var userLogService = serviceProvider.GetRequiredService<UserLogsService>();
            var usersSuspiciousService = serviceProvider.GetRequiredService<UsersSuspiciousService>();

            // check user's token
            var user = CheckAccessLogic(httpContext, userRefreshTokenService, _roles);
            if (user != null)
            {
                //========================= authenticated user
                // Set User's ban
                string methodName = context.ActionDescriptor.DisplayName?.Split('(')[0].Split('.').Last(); // for example: IActionResult GetAllUsers() => this line returns: GetAllUsers
                if (CheckUserBan(
                    httpContext,
                    usersSuspiciousService,
                    (user == null) ? Guid.Empty : Guid.Parse(user.UserId),
                    httpContext.Request.Path,
                    methodName))
                {
                    context.Result = new RedirectToActionResult(TokenStatics.BandPageAction, TokenStatics.BandPageController, null);
                    return;
                }
                // record the log
                Log(httpContext, userLogService, true, methodName);
            }
            else
            {
                //=========================  unauthenticated user + redirect to main page or ...
                // Set User's ban
                string methodName = context.ActionDescriptor.DisplayName?.Split('(')[0].Split('.').Last(); // for example: IActionResult GetAllUsers() => this line returns: GetAllUsers
                if (CheckUserBan(
                    httpContext,
                    usersSuspiciousService,
                    (user == null) ? Guid.Empty : Guid.Parse(user.UserId),
                    httpContext.Request.Path,
                    methodName))
                {
                    context.Result = new RedirectToActionResult(TokenStatics.BandPageAction, TokenStatics.BandPageController, null);
                    return;
                }
                // record the log                
                Log(httpContext, userLogService, false, methodName);
                context.Result = new RedirectToActionResult(TokenStatics.DestinationActionAfterLogout, TokenStatics.DestinationControllerAfterLogout, null);
            }
        }
        private bool CheckUserBan(HttpContext httpContext, UsersSuspiciousService service,Guid? userId,string requestPath,string methodName)
        {
            var ip = httpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            return service.CheckForBan(ip, userAgent, userId, requestPath,methodName);
        }
        private UserTokenDto CheckAccessLogic(HttpContext httpContext, UserRefreshTokenService refreshTokenService, params UserRole[] roles)
        {
            // get the cookie
            var cookies = httpContext.Request.Cookies;
            cookies.TryGetValue(TokenStatics.AuthCookieName, out string? token);
            if (token == null)
            {
                // check refreshToken whether is valid or not based on expiration of AccessToken Cookie
                token = CheckRefreshCookie(httpContext, refreshTokenService);
                if (token == null) { return null; }
            }
            // validation process
            // NOTE: outoutUserTokenDto is a class that will be filled from cookie and we want to compare its decoded information to user cliams
            TokenAccessService tokenService = new TokenAccessService();
            var valid = tokenService.ValidateKingToken(token, TokenStatics.AccessTokenKey, out UserTokenDto outUserTokenDto);

            // check role
            if (valid && (roles.Any(r => r.ToString() == outUserTokenDto.Role)))
            {
                //NOTE: Method [1] : to transfer the user's information
                httpContext.Items["CurrentUser"] = outUserTokenDto;
                //NOTE: Method [2] : to transfer the user's information
                //var claims = new List<Claim> {
                //    new Claim("UserId", outUserTokenDto.UserId),
                //    new Claim("Role", outUserTokenDto.Role),
                //};
                //NOTE: Update the method [2]
                //the following method Highly recommended by the King! though.
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, outUserTokenDto.UserId),
                    new Claim(ClaimTypes.Role, outUserTokenDto.Role),
                };
                var identity = new ClaimsIdentity(claims, "custom");
                httpContext.User = new ClaimsPrincipal(identity);
                //

                return outUserTokenDto;
            }
            else return null;
        }
        private string CheckRefreshCookie(HttpContext httpContext, UserRefreshTokenService refreshTokenService)
        {
            // get the cookie
            var cookies = httpContext.Request.Cookies;
            cookies.TryGetValue(TokenStatics.RefreshCookieName, out string? outToken);
            if (outToken == null) return null;

            // get user' refreshToken from DB
            var encodedToken = EncryptionHelper.DecryptEncryptedGuid(outToken, TokenStatics.RefreshTokenKey);

            var userRefreshToken = refreshTokenService.GetRefreshTokenByToken(encodedToken);

            if (userRefreshToken == null) return null;
            if (encodedToken != userRefreshToken.Token) return null;


            // generate a new accessToken & refreshToken
            CookieService cookieService = new CookieService(httpContext);


            ///////////////////////////////////////////////////////////////            
            ///                      RefreshToken
            //NOTE: Be careful here, we don't need to create RefreshToken cookie anymore!
            ///////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////
            ///                      AccessToken
            ///////////////////////////////////////////////////////////////


            var exp = DateTimeOffset.UtcNow.AddMinutes(TokenStatics.ExpirationDayAuthCookie);// TODO: change [AddMinutes] to [AddDays]
            // generate token
            TokenAccessService tokenService = new TokenAccessService();
            UserTokenDto userTokenDto = new UserTokenDto()
            {
                UserId = userRefreshToken.UserId.ToString(),
                Role = userRefreshToken.Role,
                Exp = exp.UtcDateTime, // Convert DateTimeOffset to DateTime (UTC time)
            };
            string newAccessToken = tokenService.GenerateToken(userTokenDto, TokenStatics.AccessTokenKey);
            // generate cookie
            var checkCookie = cookieService.GenerateCookie(TokenStatics.AuthCookieName, newAccessToken, exp);

            ///////////////////////////////////////////////////////////////
            ///                      Returned Value
            ///////////////////////////////////////////////////////////////
            return newAccessToken;
        }
        private void Log(HttpContext httpContext, UserLogsService userLogService, bool auth, string methodname)
        {
            userLogService.PostUserLog(new RequestUserLogsServiceDto
            {
                IP = httpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = httpContext.Request.Headers["User-Agent"].ToString(),
                Method = httpContext.Request.Method,
                RequestPath = httpContext.Request.Path,
                Auth = auth,
                MethodName = methodname
            });
        }
    }
}
