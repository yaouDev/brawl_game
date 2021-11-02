using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id { get; private set; }

    public void SetId(int playerId)
    {
        id = playerId;
    }
}
