# Identity Server

## [Configuration](https://localhost:5000/.well-known/openid-configuration)

## [Template](https://github.com/IdentityServer/IdentityServer4.Templates)
    
      $ dotnet new -i identityserver4.templates

## Authorization Code

- response_type
  - code
- Authentication Request
```url
GET /authorize?
    response_type=code
    &scope=openid%20profile%20email
    &client_id=s6BhdRkqt3
    &state=af0ifjsldkj
    &redirect_uri=https%3A%2F%2Fclient.example.org%2Fcb
```

## ClientCredentials

## Implicit Flow

- response_type
  - id_token
  - token
- Authentication Request
```url
GET /authorize?
    response_type=id_token%20token
    &client_id=s6BhdRkqt3
    &redirect_uri=https%3A%2F%2Fclient.example.org%2Fcb
    &scope=openid%20profile
    &state=af0ifjsldkj
    &nonce=n-0S6_WzA2Mj
```
## AuthDb (sqlserver) 

    $ docker-compose up --build -d authdb

### Migration

    $ dotnet ef migrations add InitAppDbMigration -c AppDbContext -o Data/Migrations/AppDbContextDb

    $ dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/PersistedGrantDb

    $ dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/ConfigurationDb

### Update

    $ dotnet ef database update -c AppDbContext

    $ dotnet ef database update -c PersistedGrantDbContext

    $ dotnet ef database update -c ConfigurationDbContext

### Drop

    $ dotnet ef database drop -c ConfigurationDbContext

## Certificate

```powershell
# create certificate
$cert = New-SelfSignedCertificate -Subject "CN=IdentityServerEx" -CertStoreLocation cert:\CurrentUser\My -Provider "Microsoft strong Cryptographic Provider"

# get current certificate
Get-ChildItem -Path cert:\CurrentUser\My

# remove certificate 
Remove-Item -Path ("cert:\CurrentUser\My\<certificateId>")

# check $cert
$cert = Get-ChildItem -Path cert:\CurrentUser\My | ?{$_.Subject -eq "CN=IdentityServerEx"}

# get credential
$cred = Get-Credential

# export cert
Export-PfxCertificate -Cert $cert -Password $cred.Password -FilePath "./ids_cert.pfx"

```
## Dotnet user-secrets
```bash
# create user-secrets
$ dotnet user-secrets init


# set user-secrets
$ dotnet user-secrets set "Facebook:AppId" "id-valuexxx"
$ dotnet user-secrets set "Facebook:AppSecret" "secret-valuexxxx"

# list user-secrets
$ dotnet user-secrets list
```