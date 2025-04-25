# 🔐 KingJwtAuth — Custom JWT Authentication in ASP.NET Core (Cookie-Based)

A lightweight, **DIY JWT Authentication** system built for ASP.NET Core using cookies — no Identity, no bloat, just **clean and secure token handling**.

## ⚙️ Features

- 🔑 Secure JWT generation & validation
- 🍪 HttpOnly cookie-based token storage
- ✅ Custom attribute-based authorization
- 🧠 User data access via `HttpContext.Items`
- 🧼 Stateless, scalable, and Identity-free
- 📦 Easily pluggable into any ASP.NET Core project

---

## 🚀 How It Works

1. **Login** → Generates a JWT token with user info and stores it in an **HttpOnly cookie**.
2. **Request** → Custom `KingAttribute` reads and validates the cookie token.
3. **User Context** → Decoded user info is injected into `HttpContext.Items["CurrentUser"]`.
4. **Controller/View** → Access the current user cleanly with no session or database hit.

---

## 🛠️ Code Structure

```csharp
// 🔐 TokenAccessor.cs
var user = HttpContext.Items["CurrentUser"] as UserTokenDto;
// 🧭 KingAttribute.cs
OnActionExecuting → Validates JWT token from cookie and sets HttpContext.Items["CurrentUser"]
// 🍪 CookieService.cs
Append cookie securely with expiration, HttpOnly, and SameSite rules
[KingAttribute(KingAttributeEnum.UserRole.Admin)]
public IActionResult ProtectedPage()
{
    var user = HttpContext.Items["CurrentUser"] as UserTokenDto;
    if (user == null)
        return RedirectToAction("Index", "Home");

    return View("ProtectedPage", user);
}
<!-- ProtectedPage.cshtml -->
<span>User ID: @Model.UserId</span>
<span>User Role: @Model.Role</span>
```
🔐 Security Best Practices
 - Use HttpOnly cookies to prevent XSS.
 - Validate token expiration & signature.
 - Use SameSite=Lax or Strict.
 - Set Secure = true in production (HTTPS only).


 🤝 Credits
- Created by Alimohammad Eghbaldar with assistance from ChatGPT.
- If you find this useful, ⭐ star the repo and feel free to contribute!

📜 License : MIT
