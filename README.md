# Netick-2d
## A Simple 2D Sample for Netick Networking Solution
### Built on Unity 2021.3.21f1
### Render Pipeline: BiRP

[Netick Official Website](https://netick.net)

Feature (Pros):

✔️ Client Side Prediction Input

✔️ Moving Platform Client Side Prediction

✔️ PlayerRef: A Networkable Peer ID to Create Behaviour Relation (Netick cannot Network Peer Source If you are Proxy)

✔️ Efficient Networkable (Pauseable) TickTimer

✔️ Easy Respawn/Despawn Mechanism (Tracking Alive Player with PlayerManager)

✔️ Disable Domain Reload compatible


Cons: 

❌ Doesn't Support Multi-Peer (Sandbox)

❌ Doesn't have Scene Loading yet

# Project Walkthrough

### ConnectionManager

Handling Connections: Start Client/Server, OnClientJoined/Left

```cs
    public override void OnClientConnected(NetworkSandbox sandbox, NetworkConnection client)
    {
        LevelManager.Instance.OnNetworkPlayerConnected(client, PlayerRef.Create(client.Id + 2));
    }

    public override void OnClientDisconnected(NetworkSandbox sandbox, NetworkConnection client)
    
        LevelManager.Instance.OnNetworkPlayerDisconnected(PlayerRef.Create(client.Id + 2));
    }
```

### LevelManager

Handling Gameloop (De/Spawn Player)
```cs

    void SpawnPlayerExistance(NetworkPlayer player, PlayerRef playerRef)
    {
        var playerPlaceHolder = Sandbox.NetworkInstantiate(Sandbox.GetPrefab("Player"), Vector2.zero, Quaternion.identity, player);
    }
    
    public void SpawnPlayer(NetworkPlayer player, PlayerRef playerRef)
    {
        var playerObj = Sandbox.NetworkInstantiate(Sandbox.GetPrefab("PlayerController"), SpawnPosition.position, Quaternion.identity, player);
    }
```

### PlayerController/Setup/Input

PlayerInputHandler -- (PlayerInput) --> PlayerController

```cs
 public override void NetworkUpdate()
    {
        var input = Sandbox.GetInput<PlayerInput>();

        input.Horizontal = Input.GetAxis("Horizontal");
        input.Jump = Input.GetKey(KeyCode.Space);
    }

    public override void NetworkFixedUpdate()
    {
        if (FetchInput(out PlayerInput input))
            PlayerController.SetMove(input.Horizontal);
    }
```

### PlayerManager

Tracking Connected & Alive Players, Effective ways to get Remote Player Data

```cs
    public Dictionary<PlayerRef, Player> SpawnedPlayers { get; set; } = new();
    public Dictionary<PlayerRef, PlayerSetup> SpawnedPlayersObj { get; set; } = new();
```
## Project Flow
![alt text](https://github.com/StinkySteak/Netick-2d/blob/master/Assets/Resource/flow.png "project flow")

