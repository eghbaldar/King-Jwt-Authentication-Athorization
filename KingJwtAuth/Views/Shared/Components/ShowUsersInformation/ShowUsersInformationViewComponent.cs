using KingJwtAuth.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace KingJwtAuth.Views.Shared.Components.ShowUsersInformation
{
    [KingAttribute(KingAttributeEnum.UserRole.Admin, KingAttributeEnum.UserRole.User)]
    public class ShowUsersInformationViewComponent : ViewComponent
    {
        //NOTE: to use [IHttpContextAccessor] you have to add [builder.Services.AddHttpContextAccessor(); to program.cs
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ShowUsersInformationViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IViewComponentResult Invoke()
        {
            var context = _httpContextAccessor.HttpContext;
            var user = context.User?.Identity?.IsAuthenticated;
            return View("index");
        }
    }
}