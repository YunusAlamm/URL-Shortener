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

This URL shortener project is built in C# and provides a simple and efficient way to create, manage, and track short links. It is designed to help you quickly convert long URLs into short, manageable links that are easy to share and track.

## Features

- Create short URLs from long links
- Manage and edit existing short URLs
- Track link clicks and analytics
- User-friendly web interface

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

Open your web browser and navigate to http://localhost:5000 to access the URL shortener web interface.
Use the interface to create, manage, and track short URLs.

### Swagger
you can see api document on http://localhost:5273

## Configuration

To customize the URL shortener, you can modify the configuration settings in the appsettings.json file.

```
{
  "ConnectionStrings": {
    "DefaultConnection": "YourDatabaseConnectionString"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

## Dependencies 
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0)