using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsManager : MonoBehaviour
{
    private static Vector4 _bounds;

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    internal void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        var cam = Camera.main;
        var bottomleft = cam.ViewportToWorldPoint(Vector3.zero);
        var topright = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        _bounds.x = bottomleft.x;
        _bounds.y = bottomleft.y;
        _bounds.z = topright.x;
        _bounds.w = topright.y;
    }

    // Update is called once per frame
    internal void FixedUpdate()
    {
        var pos = _rb.position;

        if (pos.x < _bounds.x)
        {
            pos.x = _bounds.z;
            DestroyTrail();
        }
        else if (pos.x > _bounds.z)
        {
            pos.x = _bounds.x;
            DestroyTrail();
        }

        if (pos.y < _bounds.y)
        {
            pos.y = _bounds.w;
            DestroyTrail();
        }
        else if (pos.y > _bounds.w)
        {
            pos.y = _bounds.y;
            DestroyTrail();
        }

        _rb.position = pos;
    }

    // Destroy trail if it currently exists
    private void DestroyTrail()
    {
        if (_rb.gameObject.transform.Find("DashTrail(Clone)"))
        {
            Destroy(_rb.gameObject.transform.Find("DashTrail(Clone)").gameObject);
        }
    }
}