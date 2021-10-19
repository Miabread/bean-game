using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

[BoltGlobalBehaviour]
public class NetworkCallbacks : GlobalEventListener
{
    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {
        BoltNetwork.Instantiate(BoltPrefabs.Player, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public override void Disconnected(BoltConnection connection)
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
