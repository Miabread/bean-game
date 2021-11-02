using System;
using Photon.Bolt;
using UnityEngine;

public class PlayerInfo : IProtocolToken
{
    public string name;

    public void Write(UdpKit.UdpPacket packet)
    {
        packet.WriteString(name);
    }

    public void Read(UdpKit.UdpPacket packet)
    {
        name = packet.ReadString();
    }

    public static int MaxNameLength = 16;

    public static string SavedName
    {
        get
        {
            return PlayerPrefs.GetString("name", "Player Name").Truncate(MaxNameLength);
        }
        set
        {
            PlayerPrefs.SetString("name", value.Truncate(MaxNameLength));
        }
    }
}
