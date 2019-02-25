using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{


    private GameObject Spawner;
    // Start is called before the first frame update
    void Awake()
    {
        Spawner = this.transform.parent.gameObject;
        GetComponent<MovingPlatform>().startPos = Spawner.GetComponent<MovingPlatform>().startPos;
        if (Spawner.GetComponent<MovingPlatform>()._leftRight)
            GetComponent<MovingPlatform>().StartingLeftRight = Spawner.GetComponent<MovingPlatform>()._leftRight;
        Debug.Log(GetComponentInParent<MovingPlatform>().startPos.x);
        Debug.Log(GetComponent<MovingPlatform>().StartingLeftRight);
    }

}
