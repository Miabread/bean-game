using Photon.Bolt;

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
}
