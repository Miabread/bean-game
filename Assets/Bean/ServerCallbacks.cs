using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class ServerCallbacks : GlobalEventListener
{
    public override void Connected(BoltConnection connection)
    {
        var playerInfo = connection.ConnectToken as PlayerInfo;
        var log = LogEvent.Create();
        log.Message = $"{playerInfo.name} joined the game";
        log.Send();
    }

    public override void Disconnected(BoltConnection connection)
    {
        var playerInfo = connection.ConnectToken as PlayerInfo;
        var log = LogEvent.Create();
        log.Message = $"{playerInfo.name} left the game";
        log.Send();
    }
}
