# Netick-2d
## A Simple 2D Sample for Netick Networking Solution
### Built on Unity 2021.3.13f1

[Netick Official Website](https://netick.net)

Feature (Pros):

✔️ Client Side Prediction Input

✔️ Moving Platform Client Side Prediction

✔️ PlayerRef: A Networkable Peer ID to Create Behaviour Relation (Netick cannot Network Peer Source If you are Proxy)

✔️ Efficient Networkable (Pauseable) TickTimer

✔️ Easy Respawn/Despawn Mechanism (Tracking Alive Player with PlayerManager)




Cons: 

❌ Doesn't Support Multi-Peer (Sandbox)

❌ Doesn't have Scene Loading yet

# Project Walkthrough

##ConnectionManager

Handling Connections: Start Client/Server, OnClientJoined/Left

LevelManager: Handling Gameloop (De/Spawn Player)

PlayerController: PlayerInputHandler -- (PlayerInput) --> PlayerController

PlayerManager: Tracking Connected & Alive Players


