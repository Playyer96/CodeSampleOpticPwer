using Picking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RandomProductsAndsShelfsSlots;

public class ShelfAndProgressMovement : MonoBehaviour
{
    public Transform targetX, targetZ;
    public bool moveInX;
    public TeleportInedx[] teleporters;

    IEnumerator Start()
    {
        if (!moveInX) yield break;

        teleporters = FindObjectsOfType<TeleportInedx>();

        while (true)
        {
            Vector3 pos = new Vector3(targetX.position.x > 20.752f ? 20.289f : 21.251f, transform.position.y, transform.position.z);
            //foreach (ProductTrigger pt in MovableShelf.instance.productTriggers)
            //{
            //    //pt.transform.position = pos + pt.diff;
            //    float d = pt.transform.position.x - transform.position.x;
            //    pt.transform.position = new Vector3(pos.x + d, pt.transform.position.y, pt.transform.position.z);
            //}
            transform.position = pos;
            yield return new WaitForSeconds(.25f);
        }
    }

    private void LateUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, targetZ.position.z);
        Vector3 lerpPos = Vector3.Lerp(transform.position, newPos, 3f * Time.deltaTime);
        transform.position = Vector3.Distance(transform.position, lerpPos) <= .001f ? newPos : lerpPos;

        //newPos = new Vector3(targetX.position.x > 20.752f ? 20.289f : 21.251f, transform.position.y, transform.position.z);

        //transform.position = Vector3.Lerp(transform.position, newPos, 10f * Time.deltaTime);

        //if (!moveInX) return;

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    targetX.position = teleporters[0].transform.position;
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //    targetX.position = teleporters[1].transform.position;
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //    targetX.position = teleporters[2].transform.position;
    }
}
