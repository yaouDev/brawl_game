using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MyButton
{
    public override void Trigger(GameObject go)
    {
        GameManager.instance.StartGame();
    }
}
