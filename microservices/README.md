# Micorservices with Identity server 4

## Use Dev Https

```
$ dotnet dev-certs https --clean
$ dotnet dev-certs https --trust
```

## Docker support

```
$ docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build
```

## Test 

1. Movies.Client
http://host.docker.internal:8002

2. ApiGateway
http://host.docker.internal:8010

3. Movies.API
http://host.docker.internal:8001

4. identityserver
http://host.docker.internal:8000