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

    private List<string> logMessages = new List<string>();

    public override void OnEvent(LogEvent evnt)
    {
        logMessages.Add(evnt.Message);
    }

    void OnGUI()
    {
        var maxMessages = Mathf.Min(4, logMessages.Count);

        GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 400, 100), GUI.skin.box);

        for (int i = maxMessages; i > 0; --i)
        {
            GUILayout.Label(logMessages[logMessages.Count - i]);
        }

        GUILayout.EndArea();
    }

    public override void BoltShutdownBegin(AddCallback registerDoneCallback, UdpConnectionDisconnectReason disconnectReason)
    {
        Application.Quit();
    }
}
