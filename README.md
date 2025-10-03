# NETCoreBBS
ASP.NET Core Light Forum NETCoreBBS

ASP.NET Core + EF Core Sqlite + Bootstrap

.NET Core Cross-platform Light Forum

[Technical Introduction](http://www.cnblogs.com/linezero/p/NETCoreBBS.html)

## Development

1. `git clone https://github.com/vjm-dev/NETCoreBBS.git`
2. Open `NetCoreBBS.sln` in Visual Studio 2019
3. Click Debug -> Start Debugging to run, or click NetCoreBBS in the toolbar

Note: The default port is 80, which may conflict with your local port. You can change .UseUrls("http://*:80") in Program.cs and then modify the startup URL.

## Features

1. Node functionality
1. Topic publishing
2. Topic replies
3. Topic filtering
3. User login/registration
4. Topic pinning
5. Admin panel
6. User profile center

## Admin panel

After registering with the username admin, the account is set as an administrator by default. After logging in, you'll see the admin center option.

## License
NETCoreBBS is licensed under [MIT](LICENSE).
