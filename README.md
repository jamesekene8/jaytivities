# Jaytivities: Social Events App

A social app for creating and joining activities (meet-ups, drinks, film nights and so on). Users can follow each other, upload profile photos, and chat live on each activity's page. It's built with an **ASP.NET Core 7** Clean Architecture API and a **React + TypeScript** client, and deployed as a Docker image to Fly.io through GitHub Actions.

![.NET](https://img.shields.io/badge/.NET_7-512BD4?logo=dotnet&logoColor=white)
![React](https://img.shields.io/badge/React-20232A?logo=react&logoColor=61DAFB)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?logo=typescript&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)

<!-- TODO: add screenshots, e.g. docs/screenshots/dashboard.png, activity-chat.png, profile.png -->

## Features

- **Activities:** create, edit, cancel, join or leave, with a filterable dashboard, a date picker and a calendar
- **Real-time comments** on each activity using **SignalR**
- **Profiles:** bio, photo gallery with crop and upload (Cloudinary), and a "set main photo" option
- **Follow / followers** with live follower counts
- **Auth:** ASP.NET Core Identity, JWT with **refresh tokens**, and **Facebook login**
- **Error handling:** global exception middleware, plus client-side 404/500/validation pages

## Architecture

```
API/              Controllers, SignalR ChatHub, identity & token service, exception middleware
Application/      CQRS handlers (MediatR), validation (FluentValidation), AutoMapper profiles
Domain/           Entities: Activity, ActivityAttendee, AppUser, Photo, Comment, UserFollowing
Infrastructure/   Cloudinary photo accessor, current-user accessor
Persistence/      EF Core DbContext, migrations, seed data
client-app/       React 18 + TypeScript + Vite, MobX stores, Semantic UI, Formik/Yup
```

The client builds into `API/wwwroot`, so the API serves the whole app. On every push to `master`, the GitHub Actions workflow builds a Docker image and pushes it to Docker Hub for Fly.io to deploy.

## Running locally

**Prerequisites:** .NET 7 SDK, Node 18+, PostgreSQL, and Cloudinary and Facebook developer apps (optional; only needed for photos and Facebook login).

```bash
cd API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=5432;Database=reactivities;User Id=postgres;Password=postgres;"
dotnet user-secrets set "TokenKey" "<a long random string, 64+ characters>"
dotnet user-secrets set "Cloudinary:ApiKey" "<key>"
dotnet user-secrets set "Cloudinary:ApiSecret" "<secret>"
dotnet user-secrets set "Facebook:ApiSecret" "<secret>"
dotnet run          # migrates + seeds demo users and activities

cd ../client-app
npm install
npm run dev
```

## Acknowledgements

Built while following a .NET + React course (the "Reactivities" project). I extended it with my own deployment pipeline to Fly.io via GitHub Actions and Docker Hub. A fresh .NET 9 rebuild is in progress in [Reactivities](https://github.com/jamesekene8/Reactivities).

## Author

**James Uguna**: [GitHub](https://github.com/jamesekene8)
