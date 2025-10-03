# NETCoreBBS
ASP.NET Core Light Forum NETCoreBBS

ASP.NET Core + EF Core PostgreSQL + Bootstrap

.NET Core Cross-platform Light Forum

[Technical Introduction](http://www.cnblogs.com/linezero/p/NETCoreBBS.html)

## Notes

In the past, this project used .NET Core 3.1 and SQLite. These were unstable on recent versions nowadays. SQLite had some drawbacks in the project.<br/>
Now, it's upgraded to .NET Core 9.0 included NuGet packages. The database engine was switched to PostgreSQL to be more open and permissive.<br/>
Docker support was added by using [`docker-compose.yml`](docker-compose.yml).

## Development

1. 
```sh
git clone https://github.com/vjm-dev/NETCoreBBS.git
```
2. Run Docker Desktop
3. For migrations, requires `dotnet-ef` installed by running:
```sh
dotnet tool install --global dotnet-ef
```
To update:
```sh
dotnet tool update --global dotnet-ef
```
4. Go to local cloned repository directory and run:
```sh
dotnet ef migrations add InitialCreate --project src\Infrastructure --startup-project src\NetCoreBBS
```
5. Open `NetCoreBBS.sln` in Visual Studio 2022
6. In the Visual Studio Ouput window panel, wait until Docker finishes pulling and setting all containers
7. Click Debug -> Start Debugging to run, or click `docker-compose` in the toolbar

Note: The default port is 80, which may conflict with your local port. You can change the port in [`docker-compose.yml`](docker-compose.yml):
```yaml
  netcorebbs:
    image: ${DOCKER_REGISTRY-}netcorebbs
    build:
      context: .
      dockerfile: src/NetCoreBBS/Dockerfile
    ports:
      - "80:8080" # <-- Change the first port here(e.g. "5000:8080"), 8080 depends on src/NetCoreBBS/Dockerfile
    ...
```
You can access pgAdmin with [`localhost:8080`](http://localhost:8080), log in using the credentials from `PGADMIN_DEFAULT_EMAIL` and `PGADMIN_DEFAULT_PASSWORD` from `pgadmin:` in [`docker-compose.yml`](docker-compose.yml), connect postgres-bbs (General > Name: `postgres-bbs`, Connection > Hostname: `postgres`, Port: `5432`, Username: `postgresadmin`, Password: `my_password` (all that is in `postgres:`)) and check the tables in postgres-bbs > Databases > netcorebbs > Schemas > public > Tables.

## Features

1. Node functionality
2. Topic publishing
3. Topic replies
4. Topic filtering
5. User login/registration
6. Topic pinning
7. Admin panel
8. User profile center

## Admin panel

After registering with the username admin, the account is set as an administrator by default. After logging in, you'll see the admin center option.

## License
NETCoreBBS is licensed under [MIT](LICENSE).
