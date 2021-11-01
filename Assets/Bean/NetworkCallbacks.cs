using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using UdpKit;

[BoltGlobalBehaviour]
public class NetworkCallbacks : GlobalEventListener
{
    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {
        BoltNetwork.Instantiate(BoltPrefabs.Player, Player.spawnPosition, Player.spawnRotation);
    }

    public override void BoltShutdownBegin(AddCallback registerDoneCallback, UdpConnectionDisconnectReason disconnectReason)
    {
        Application.Quit();
    }
}
