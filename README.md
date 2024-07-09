# URL Shortener

URL shortener project in C# to create, manage, and track short links.

## Table of Contents

- [URL Shortener](#url-shortener)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Features](#features)
  - [Installation](#installation)
  - [Usage](#usage)
    - [Swagger](#swagger)
  - [Configuration](#configuration)
  - [Dependencies](#dependencies)

## Introduction

This URL shortener project is built in C# and provides a simple and efficient way to create and track short links. It is designed to help you quickly convert long URLs into short links that are easy to share and track.

## Features

- Create short URLs from long links

- Track link clicks and analytics


## Installation

1. Clone the repository:
```
git clone https://github.com/YunusAlamm/URL-Shortener.git
```

2. Navigate to the project directory:
```
cd url-shortener
```

3. Install dependencies:
```
dotnet restore
```

4. Build the project:
```
dotnet build
```

## Usage

Run the project:
```
dotnet run
```
ShutDown the project:
```
Ctrl+C (in the terminal)
```

Open your web browser and navigate to http://localhost:5273/swagger/index.html to access the URL shortener web interface.
Use the interface to create, manage, and track short URLs.



### Swagger
you can see api document on http://localhost:5273/swagger/v1/swagger.json

## Configuration

To customize the URL shortener, you can modify the configuration settings in the appsettings.json file.

```
{
  
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },

  "AllowedHosts": "*",

  "ConnectionStrings": {
    "DefaultConnection": "YourDatabaseConnectionString"
  },

    "Jwt": {
    "Issuer": "http://localhost",
    "Audience": "http://localhost",
    "Key": "Rm9yIGEgNTEyLWJpdCBrZXksIHlvdSBjYW4gdXNlIHRoaXMgZXhhbXBsZTogUW5Kb2FqZkFqZGxha0pkZmFsa2pkZmFsa2pkZmFsa2pkZmFsa2pkZmFsa2pkZmFsa2pkZmFsa2pkZmFsa2pkZmF"
}

}
```

## Dependencies 
## Project Dependencies

- **Microsoft.AspNetCore.Authentication.JwtBearer**: Version 8.0.5
- **Microsoft.AspNetCore.OpenApi**: Version 8.0.4
- **Microsoft.EntityFrameworkCore**: Version 8.0.4
- **Microsoft.EntityFrameworkCore.Tools**: Version 8.0.4
- **Npgsql**: Version 8.0.3
- **Npgsql.EntityFrameworkCore.PostgreSQL**: Version 8.0.4
- **Swashbuckle.AspNetCore**: Version 6.4.0
- **System.IdentityModel.Tokens.Jwt**: Version 7.5.2
