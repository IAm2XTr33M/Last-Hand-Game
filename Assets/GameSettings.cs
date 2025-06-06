using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;

    public int desiredPointsWinAmount = 21;

    private void Awake()
    {
        if(instance == null) { instance = this; } else { Destroy(this); }
    }
}
