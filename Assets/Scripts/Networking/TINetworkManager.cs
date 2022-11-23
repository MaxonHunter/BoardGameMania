using UnityEngine;
using Mirror;

public class TINetworkManager : NetworkManager
{

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log($"Player has connected: {conn}");
    }

}
