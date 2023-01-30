using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReplacement : MonoBehaviour
{
    private Vector3 groundPos = new Vector3(0, 1, 0);
    private void FixedUpdate() {
        if (this.transform.position.y < -1f) {
            RaycastHit hit;
            Physics.Raycast(new Vector3(this.transform.position.x, 100f, this.transform.position.z), Vector3.down, out hit, 100f);
            this.transform.position = hit.point + groundPos;
        }
    }
}
