using System.Collections;
using System.Collections.Generic;
using Photon.Bolt;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public PlayerBehaviour playerBehaviour;

    void OnTriggerEnter(Collider collision)
    {
        var other = collision.gameObject.GetComponent<PlayerBehaviour>();

        if (!other || !playerBehaviour.state.IsTagged) return;

        TagEvent.Create(other.entity).Send();
    }
}
