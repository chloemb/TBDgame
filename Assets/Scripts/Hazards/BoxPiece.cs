using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPiece : MonoBehaviour
{
    public GameObject playerOrigin;
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
