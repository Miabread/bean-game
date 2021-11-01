using System.Collections;
using System.Collections.Generic;
using Photon.Bolt;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Player playerBehaviour;

    void OnTriggerEnter(Collider collision)
    {
        var other = collision.gameObject.GetComponent<Player>();

        if (!other || !playerBehaviour.state.IsTagged) return;

        TagEvent.Create(other.entity).Send();
    }
}
