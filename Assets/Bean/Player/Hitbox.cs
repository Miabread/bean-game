using System.Collections;
using System.Collections.Generic;
using Photon.Bolt;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Player player;

    void OnTriggerEnter(Collider collision)
    {
        var other = collision.gameObject.GetComponent<Player>();

        if (!other || !player.state.IsTagged || other.state.IsTagged) return;

        var evnt = TagEvent.Create(other.entity);
        evnt.TaggerName = player.state.Name;
        evnt.Send();
    }
}
