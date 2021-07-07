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

## OAuth

 ![alt tag](https://github.com/lastingyeh/aspnetIdentities/blob/master/oauth/basic-oauth.png)

### Dockerize

- docker-compose (/oauth)

      $ docker-compose up --build -d 

- Dev Https

      $ dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p <password>

      $ dotnet dev-certs https --trust

  - set at docker-compose.yaml
    ```yaml
    environment:
        - ASPNETCORE_URLS=https://+:443
        - ASPNETCORE_ENVIRONMENT=Development
        - ASPNETCORE_Kestrel__Certificates__Default__Password=<password>
        - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
    volumes:
      - ~/.aspnet/https:/https:ro 
    ```

## References
  
- [ASP.NET Core - Authentication & Authorization Tutorial (Claims/Identity/oAuth/oidc/IdentityServer4)](https://www.youtube.com/playlist?list=PLOeFnOV9YBa7dnrjpOG6lMpcyd7Wn7E8V)
