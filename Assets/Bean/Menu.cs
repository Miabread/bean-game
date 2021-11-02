using System;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using UdpKit;
using TMPro;

public class Menu : GlobalEventListener
{
    public TMP_InputField playerName;

    private void Start()
    {
        playerName.text = PlayerInfo.SavedName;
    }

    public void OnValueChanged(string name)
    {
        PlayerInfo.SavedName = name;
    }

    public void Join()
    {
        BoltLauncher.StartClient();
    }

    public void Host()
    {
        BoltLauncher.StartServer();
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            var matchName = Guid.NewGuid().ToString();

            BoltMatchmaking.CreateSession(
                sessionID: matchName,
                sceneToLoad: "Game"
            );
        }
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        Debug.LogFormat("Session list updated: {0} total sessions", sessionList.Count);

        foreach (var session in sessionList)
        {
            var photonSession = session.Value as UdpSession;

            if (photonSession.Source == UdpSessionSource.Photon)
            {
                var playerInfo = new PlayerInfo();
                playerInfo.name = PlayerInfo.SavedName;

                BoltMatchmaking.JoinSession(photonSession, playerInfo);
            }
        }
    }
}
