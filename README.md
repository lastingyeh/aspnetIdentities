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
    - securityLevel => SecurityLevelHandler (requirement.Level <= claimLevel)
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
  
## IdentityServer4
- Part 1
  - ApiOne
  - ApiTwo
  - IdentityServer
  - JavascriptClient
  - MvcClient

- Part 2
  - CustomIdentityServer
  - CustomResourceServer
  - CustomAccessRefreshApi

## Microservices

- IdentityServer
- ApiGateway
- Movies.API
- Movies.Client

## OAuth

- Api
- Client
- Server

## AuthorizationCode

  ![alt tag](https://github.com/lastingyeh/aspnetIdentities/blob/master/oauth/oauth-authorizationCode.png)

## Identity Server 4

### ClientCredentials
  
![alt tag](https://github.com/lastingyeh/aspnetIdentities/blob/master/identityserver4/id4-clientCredentials.png)

### Authorization Code

![alt tag](https://github.com/lastingyeh/aspnetIdentities/blob/master/identityserver4/id4-code_v1.png)

### Implicit

![alt tag](https://github.com/lastingyeh/aspnetIdentities/blob/master/identityserver4/id4-implicit.png)

### External [Facebook]

![alt tag](https://github.com/lastingyeh/aspnetIdentities/blob/master/identityserver4/id4-facebook.png)

## Docker

- docker-compose (/oauth)

      $ docker-compose up --build -d 

- Dev Https

       $ dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p [password]

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

- [OpenId](https://openid.net/specs/oauth-v2-multiple-response-types-1_0.html#ResponseTypesAndModes)

- [oidc-client](https://cdnjs.com/libraries/oidc-client)

- [IdentityModel/oidc-client-js](https://github.com/IdentityModel/oidc-client-js)
  
- [angular-auth-oidc-client](https://github.com/damienbod/angular-auth-oidc-client)

- [damienbod/AspNetCoreIdentityServer4ResourceOwnerPassword](https://github.com/damienbod/AspNetCoreIdentityServer4ResourceOwnerPassword)

- [aspnetrun/run-aspnet-identityserver4](https://github.com/aspnetrun/run-aspnet-identityserver4)