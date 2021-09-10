using UnityEngine;
using UnityEngine.Events;

public class GrabProduct : MonoBehaviour
{
    public static bool b_canCount = false;
    public bool b_ProductAcction = false;
    public bool b_isGrab;
    public DreamHouseStudios.VR.Interactable i_Intearctable;
    public UnityEvent e_OnGrab;
    void Start()
    {
        i_Intearctable = GetComponent<DreamHouseStudios.VR.Interactable>();
        if (e_OnGrab == null)
        {
            e_OnGrab = new UnityEvent();
        }
    }

    private void Update()
    {
        if (b_canCount)
        {
            b_isGrab = i_Intearctable.beingGrabbed;
            if (!b_isGrab || b_ProductAcction)
            {
                return;
            }
            else
            {
                if (b_isGrab && !b_ProductAcction)
                {
                    b_ProductAcction = true;
                    e_OnGrab.Invoke();
                }
            }
        }
        else
        {
            return;
        }
    }
}
