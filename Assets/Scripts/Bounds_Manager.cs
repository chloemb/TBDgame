using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds_Manager : MonoBehaviour
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
        internal void Update()
        {
            var pos = _rb.position;

            if (pos.x < _bounds.x)
            {
                pos.x = _bounds.z;
            }
            else if (pos.x > _bounds.z)
            {
                pos.x = _bounds.x;
            }

            if (pos.y < _bounds.y)
            {
                pos.y = _bounds.w;
            }
            else if (pos.y > _bounds.w)
            {
                pos.y = _bounds.y;
            }

            _rb.position = pos;
        }
}
