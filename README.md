# campaign-codex

**campaign-codex** is a project designed to help Dungeon Masters (DMs) and players organize their D&D/TTRPG campaigns digitally, replacing physical notebooks with a centralized platform.

Currently hosted on <http://46.224.39.173>.

## Core Features
- **Campaign Management**: Create/join campaigns with DM and player roles
- **Interactive Map**: Upload campaign maps with draggable pins for locations
- **Wiki System**: Document NPCs, locations, items, and lore with rich text and images
- **Character Management**: Track player characters with stats, progression (Level 1-10 XP system), and details
- **Session Notes**: Personal note-taking with rich text editor per campaign/player
- **User Authentication**: Secure registration and login with role-based access (DM vs Player)
- **Photo Upload**: Image management via Cloudinary for maps, wiki entries, and characters

## Run Locally

1. Setup your `.env` file. Use `.env_sample` as a guideline

2. Install Docker and run the following.

```bash
docker compose up -d
```

3. Navigate to `localhost:80` to view the application.

## Contribute

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/campaign-codex.git
   cd campaign-codex
   ```

2. Restore .NET tools and install Husky locally
   ```bash
   dotnet tool restore
   dotnet husky install
   ```
