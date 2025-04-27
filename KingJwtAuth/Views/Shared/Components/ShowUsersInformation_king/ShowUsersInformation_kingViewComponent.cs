using KingJwtAuth.Attributes;
using KingJwtAuth.Views.Shared.Components._KingBaseComponent;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KingJwtAuth.Views.Shared.Components.ShowUsersInformation_king
{
    public class ShowUsersInformation_kingViewComponent : KingBaseViewComponent
    {
        //NOTE: to use [IHttpContextAccessor] you have to add [builder.Services.AddHttpContextAccessor(); to program.cs
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ShowUsersInformation_kingViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IViewComponentResult Invoke()
        {
            var context = _httpContextAccessor.HttpContext;
            CheckAuthorization(context, new[] { KingAttributeEnum.UserRole.Admin, KingAttributeEnum.UserRole.User });
            return View("index");
        }
    }
}
