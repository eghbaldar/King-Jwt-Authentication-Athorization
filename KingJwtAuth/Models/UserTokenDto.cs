namespace KingJwtAuth.Models
{
    public class UserTokenDto
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        // NOTE: // this is the User's Token's Expiration DateTime
        // NOTE: // this value is different from Cookie's Expiration
        public DateTime Exp { get; set; }
    }
}
/*
 * 
 * 
 * The token expiration time (Exp) and the cookie expiration time can be different from each other. Here's how they differ:
 * 
 * 
 * 
 * 1. Token Expiration:
In a real-world application, the expiration time for a JWT (JSON Web Token) is typically set based on the desired session duration or token validity period. You generally want the token to be valid for a reasonable amount of time but not so long that it poses a security risk in case of a token leak.

Common Token Expiration Durations:
Short-lived tokens (15 minutes to 1 hour):

When to use: This is common for access tokens. These tokens are used to authenticate API requests and typically have a short lifespan for better security.

Why: Short-lived tokens limit the time window an attacker can use a stolen token.

Example: An access token could be valid for 15 minutes to 1 hour. After that, the user needs to request a new token, usually via a refresh token.

Long-lived tokens (1 day to 7 days):

When to use: This might be the expiration time for a refresh token, which allows the user to get a new access token without needing to log in again.

Why: Refresh tokens can be long-lived because they are typically stored securely, such as in an HttpOnly cookie. They allow a user to maintain their session for a longer period without needing to reauthenticate.

Example: A refresh token could expire in 1 week, and it allows the user to obtain a new access token without logging in.

Typical Token Expiration Scheme:
Access Token: 15 minutes to 1 hour

Refresh Token: 1 week to 1 month

This expiration period can vary depending on your application’s needs.

2. Cookie Expiration:
Cookies used to store tokens, like the JWT, also need an expiration date. The expiration for the cookie should be consistent with the token expiration, but you may want to keep the cookie alive a bit longer to allow for refresh scenarios.

Common Cookie Expiration Durations:
Short-lived cookies (1 hour to 1 day):

When to use: Use for access tokens that expire quickly (e.g., 1 hour).

Why: You want the cookie to be removed from the user's browser once the token expires for security purposes. This minimizes the risk of a stale or compromised token.

Example: The cookie could expire in 1 hour, and upon token expiration, the user is logged out.

Long-lived cookies (1 week to 1 month):

When to use: Use for refresh tokens that last longer than access tokens. This allows the user to maintain their session for an extended period without requiring them to re-authenticate.

Why: A long-lived cookie can be used to maintain a session even when the access token has expired. When the access token expires, the refresh token stored in the cookie can be used to obtain a new access token.

Example: The cookie could expire in 1 week or 30 days to maintain the user session.

Typical Cookie Expiration Scheme:
Access Token Cookie: 1 hour to 1 day

Refresh Token Cookie: 1 week to 1 month

3. Real-World Example:
Let’s assume your app has the following flow:

Login Process:

The user logs in using their credentials.

The system generates:

Access token: Expires in 15 minutes (for API calls).

Refresh token: Expires in 7 days (to allow re-authentication).

Access Token Expiration:

The access token will expire after 15 minutes.

If the user tries to make an API request after that time, the server will reject the request with a 401 Unauthorized response.

Refresh Token:

The refresh token, which is securely stored in an HttpOnly cookie, will last for 7 days.

When the access token expires, the client can send the refresh token in the request, and the server will issue a new access token (without requiring the user to log in again).

Cookie Expiration:

The cookie holding the access token may have an expiration time of 1 hour (matching the expiration of the access token).

The cookie holding the refresh token may expire in 7 days (matching the expiration of the refresh token).

4. Token and Cookie Expiration Handling:
When the access token expires, the client can use the refresh token stored in the cookie to obtain a new access token. This request can happen behind the scenes (e.g., via an API call).

Refresh token expiration: If the refresh token expires (7 days in this case), the user is logged out and will need to log in again.

Cookie expiration: If the cookie expires before the token or refresh token expiration, the user will be logged out, and they need to log in again.

5. Key Takeaways:
Access token expiration: 15 minutes to 1 hour is typical for access tokens.

Refresh token expiration: 7 days to 30 days is common for refresh tokens.

Cookie expiration: Should be aligned with the token expiration duration to ensure security.

Short-lived cookies for access tokens (1 hour to 1 day).

Long-lived cookies for refresh tokens (1 week to 1 month).

Security Considerations:
HttpOnly and Secure cookies: Always store your tokens in HttpOnly cookies to prevent client-side JavaScript from accessing them.

SameSite attribute: Use SameSite to ensure that the cookies are not sent along with cross-site requests (helps prevent CSRF attacks).

Refresh token rotation: Rotate refresh tokens regularly to reduce the risk of token theft.

Access token expiration: Short expiration helps limit the damage in case of token leakage.

In summary, the expiration times should be tailored based on the needs of your application, with shorter expiration times for access tokens and longer expiration times for refresh tokens. The cookies should be set to match the expiration times of the respective tokens they hold.
 */