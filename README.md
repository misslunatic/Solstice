# TechMod
### A "Wisdom of the Crowds" Moderation Bot
The intent of this bot is to allow a community to handle situations when the moderators are not available,
while also reducing the potential for abuse.
#### Table of Contents
- [Installation](#installation)
- [Usage](#the-idea)
- - [For Moderators](#for-moderators)
- - [For Members](#for-members)
- [Things Used](#things-used)
- [License](#license)
## Installation
### 1. Getting files
`git clone` the repository into your desired location.

### 2. Setting up Discord
Go to https://discord.com/developers/applications, and create a new Application. This will be your bot.
Go to the `Bot` page, turn on Server Members Intent, and generate a token. Keep this token, as it will 
allow access to your bot.

### 3. Setting up the project
Create a file named `appsettings.json` next to `TechMod.csproj`.
Fill it out like so:

```json
{
  "token": "INSERT TOKEN HERE"
}
```
Set the properties of this item to copy to the output directory if newer. The program looks for this next to the executable.

***Make sure to put this into .GITIGNORE*** if you are planning to upload this. This gives access to the bot, but you can reset it if needed.
#### 3.1 Setting up the database (SQLite)
Don't worry, it's not that complex.

First, change the `DatabasePath` in `Config.cs` to `"[MyPath]/data.db"`. One path you can use is just the project directory,
though make sure you .GITIGNORE this.

To prepare changes to the database, which you may need to do immediately anyways, run these in the directory with the `.csproj`:
`dotnet ef migrations add InitialCreate`, and to apply those changes and create the database you run `dotnet ef database update`.

Change InitialCreate to something else if you're messing with it.

### 4. Adding your bot
Go back to the Applications portal and back to your bot. Go to `OAuth2`>`URL Generator`.
You want to select the scopes `bot` and `applications.commands`.
Then you want to select the permissions `Send Messages`, `Manage Channels`, `Moderate Members`, `Use Slash Commands`. 
If you add more features, you may need to select additional ones.

This will generate an invite link; Save this and put it in your browser, and invite the bot to your server.

### 5. ???
### 6. Profit!!!

## Usage
### The idea
As you can see, votes are being used. To prevent people from just abusing this, there must be a 
minimum amount of votes on an option for it to count, coupled with an option taking priority in the case of a tie.

![Example 2](/TechMod/Images/example2.png) 

To improve readability, the progress bar will be ***red*** when the result would be No, and ***green*** if it would be Yes.

### For Moderators
Note that below commands are restricted to those who have both Manage Channels and Moderate Members.

If you are extra concerned about abuse, you can restrict using this to a specific role that you give to trusted members
with `/config setrole @MyRole`, and to remove it you can use `/config removerole`.

To adjust to the size of your community, and to prevent abuse, voting with this requires at least N (By default 4) 
votes for an action to take place. To change this, use `/config minimum 0-255`.

To break ties, it can prioritize an option. This is set to `No` by default. You can change this using `/config priority Yes/No` 

The duration and the length of the progress bar for votes can be found in `Config.cs`.

The ranges commands can used can also be adjusted in that file.
### For Members
#### Locking a channel temporarily

![Example 1](/TechMod/Images/example.png) 

If you need to stop an argument or other drama, you can use this to prevent everyone from sending messages in the channel for 
anywhere between 30 seconds and 30 minutes. Use `/lock vote [Minutes]` to begin a vote (It's always in minutes, use decimal points.)

#### Muting a member temporarily

![Example 3](/TechMod/Images/example3.png) 

If someone is being a jerk, is posting inappropriate content, etc, you can time them out for anywhere between 30 seconds to 30 minutes. 

#### Clearing Messages

![Example 4](/TechMod/Images/example4.png) 

You can remove anywhere between 10 to 200 messages, from either everyone or a specific person.

## Things used
Discord.NET: https://github.com/discord-net/Discord.Net

## License
Licensed under the MIT License. Have fun.