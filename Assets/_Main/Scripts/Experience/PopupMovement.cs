using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMovement : MonoBehaviour
{
    public Transform targetZ;

    private void Start()
    {
        MovePopup();
    }

    private void LateUpdate()
    {
        MovePopup();
    }

    private void MovePopup()
    {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, targetZ.position.z);
        Vector3 lerpPos = Vector3.Lerp(transform.position, newPos, 3f * Time.deltaTime);
        transform.position = Vector3.Distance(transform.position, lerpPos) <= .001f ? newPos : lerpPos;
    }
}