using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroyHelper : MonoBehaviour
{
    public Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void KillPlayer()
    {
        player.DestroyMe();
    }
}
