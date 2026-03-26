# FizzySteamworks

This is a community maintained repo forked from **[RayStorm](https://github.com/Raystorms/FizzySteamyMirror)**. 

Mirror **[docs](https://mirror-networking.com/docs/Transports/Fizzy.html)** and the official community **[Discord](https://discord.gg/N9QVxbM)**.

FizzySteamworks brings together **[Steam](https://store.steampowered.com)** and **[Mirror](https://github.com/vis2k/Mirror)** . It supports both the old SteamNetworking and the new SteamSockets. 



## Dependencies
### [Mirror](https://github.com/vis2k/Mirror)
FizzySteamworks is obviously dependant on Mirror which is a streamline, bug fixed, maintained version of UNET for Unity.

### [Steamworks.NET](https://github.com/rlabrecque/Steamworks.NET)
FizzySteamworks relies on Steamworks.NET to communicate with the [Steamworks API](https://partner.steamgames.com/doc/sdk), so you will need that installed and initalized properly before you can use this transport.

You have two options to solve this dependency
#### A) [Steamworks Foundation](https://github.com/heathen-engineering/SteamworksFoundation)
You can use Heathen's [Steamworks Foundation](https://github.com/heathen-engineering/SteamworksFoundation) which is an open source Unity Asset that integrates [Steamworks.NET](https://github.com/rlabrecque/Steamworks.NET) into the Unity engine handling most things for you. 
#### B) Raw [Steamworks.NET](https://github.com/rlabrecque/Steamworks.NET)
You can install [Steamworks.NET](https://github.com/rlabrecque/Steamworks.NET) your self and use your own code to initlaize and manage it. Note [Steamworks.NET](https://github.com/rlabrecque/Steamworks.NET) is a C# wrapper around the [Steamworks API](https://partner.steamgames.com/doc/sdk), not a plug-n-play asset for integrating Steam API. 
> Steamworks.NET is just a C# wrapper, It does not handle configuraiton, initalization or other aspects around the use of Steam API for you.

[Steamworks Foundaiton](https://github.com/heathen-engineering/SteamworksFoundation) has its own [documentation](https://kb.heathenengineering.com/assets/steamworks), [discord support](https://discord.gg/6X3xrRc), etc. if you have questions on its use please ask them.

### .NET 4.x
[Steamworks.NET](https://github.com/rlabrecque/Steamworks.NET) Requires .Net 4.x so be sure to update your project settings for that



## Installation
You have two options when installing the transport.
### Unity Package Manager
![Package Manager Add from Git URL](https://3689240098-files.gitbook.io/~/files/v0/b/gitbook-28427.appspot.com/o/assets%2F-MZWu8yFOWhCYCMkJFmR%2F-MkVzpUlxYzzEgvdinNv%2F-MkW6tqgBr-8XK0-iKQ2%2Fimage.png?alt=media&token=8a6451ed-473b-4a18-9385-cd87e41e774a)
1) Open the package manager and click the '+' (plus) button located in the upper left of the window
2) Select `Add package from git URL...` when prompted provide the following URL:  
    `https://github.com/Chykary/FizzySteamworks.git?path=/com.mirror.steamworks.net`

If you have issues with Unity's Package Manager check out this article on [Heathen's Knowledge Base](https://kb.heathenengineering.com/company/package-manager-install#troubleshooting) it might help you resolve.

### Manual
More error prone and subject to being out of date with the latest changes:
1. Download the latest [unitypackage](https://github.com/Chykary/FizzySteamworks/releases) from the release section.
2. Import the package into Unity.



## Setting Up
This assumes you have your dependencies installed, configured and working correctly
1. Install FizzySteamworks from package manager as discribed in the above Install step.
2. In your **"NetworkManager"** object replace **"KCP"** with **"FizzySteamworks"**.



## Server (Client / Server)
When running a server build you need build your server build and it needs to initalize Steam Game Server. **NOTE** Steam's server APIs are a different set of APIs than those used when running a Client build. To learn more about this please consult [Valve's documentation](https://partner.steamgames.com/doc/api/ISteamGameServer) on the subject. 
If your using Heathen's [Steamworks Foundaiton](https://github.com/heathen-engineering/SteamworksFoundation) they have [documentaiton](https://kb.heathenengineering.com/assets/steamworks/learning/core-concepts/game-server-browser) and [discord support](https://discord.gg/6X3xrRc) for this topic



## Host (Peer to Peer)
To have your game working with Steam Networking you need to make sure you have Steam running in the background and that the Steam API initalized correctly. You can then call StartHost and use Mirror as you normally would.



## Client (Client / Server or Peer to Peer)
To connect a client to a host or server you need the CSteamID of the target you wish to connect to this is used in place of IP/Port. If your creating a Peer to Peer architecture then you would use the CSteamID of the host Steam User, this is Steam user ID as a ulong value. If you are creating a Client Server architecture then you will be using the CSteamID issued to the Steam Game Server when it logs the Steam API on. This is an advanced use case supported by Heathen's Steamworks but requires additional custom code if your using Steamworks.NET directly.

> You cannot test a connection on 1 machine and or with 1 Steam account. 
> To test Steam P2P Networking you must have two machines, two steam accounts and they must both own a license to the game in question.

### Steps
1. Send the game to your buddy.
2. Your buddy needs the host or game server **steamID64** to be able to connect.
3. Place the **steamID64** into **"localhost"** then click **"Client"**
5. Then they will be connected to your server be that your machine as a P2P connection or yoru Steam Game Server as a Client Server connection.



## Testing your game locally
You cant connect to yourself locally while using **FizzySteamworks** since it's using Steams Networking which runs over Steam Client and addresses its connection based on the unique CSteamID of each actor. If you want to test your game locally you'll have to use **"Telepathy Transport"** instead of **"FizzySteamworks"**.
