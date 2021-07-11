# Identity Server

- [configuration](https://localhost:5000/.well-known/openid-configuration)

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
