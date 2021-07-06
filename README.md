# Demo

## Basics

- Cookie auth [HttContext.SignInAsync]
  - Access to [Authorize] [/Home/Secret]
    - Direct to Action [/Home/Authenticate]
    - Add Grandmas.Cookie (header)
    - SignInAsync
    - RedirectToIndex
    - Access again
    - Successfully (header: Grandmas.Cookie)
  - Custom Required Policy
    - Policy [Authorize(Policy = "Claim.DoB")]
    - Role [Authorize(Role = "Admin")]
  - Custom Controller (OperationsController)
    - OperationAuthorizationRequirement
    - Custom authorization
  - Dynamic Claim added (trigger when [Authorize])
    - IClaimsTransformation
    - Add Custom claim
  - Custom Authorization Policy Provider (DefaultAuthorizationPolicyProvider)
    - Rank => RequireClaim("Rank")
    - ecurityLevel => SecurityLevelHandler (requirement.Level <= claimLevel)
    - AuthorizeAttribute inherited

## IdentityExample

- Cookie auth [Identity]
  - Register user [/Home/Register]
  - Access to [Authorize] [/Home/Secret]
    - Direct to Action [/Home/Login]
    - login success => Add Identity.Cookie (header)
    - access again
    - Successfully  
  - Add Email Confirmation process
    - Generate EmailConfirmation Token
    - Generate EmailConfirmation link
    - Send email with token and link
    - [page] EmailVerification page
    - client click link from email box
    - verify user & token
    - [page] VerifyEmail

## Basic OAuth

 ![alt tag](https://github.com/lastingyeh/aspnetSecurities/blob/master/oauth/basic-oauth.png)
  
## References
  
- [ASP.NET Core - Authentication & Authorization Tutorial (Claims/Identity/oAuth/oidc/IdentityServer4)](https://www.youtube.com/playlist?list=PLOeFnOV9YBa7dnrjpOG6lMpcyd7Wn7E8V)
