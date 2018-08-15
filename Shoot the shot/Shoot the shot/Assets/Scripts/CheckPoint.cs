using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    [SerializeField]
    Player player;

    private void OnTriggerEnter(Collider other)
    {
        player.AcceptCheckPoint(this);
    }
}
