using UnityEngine;
using Valve.VR.InteractionSystem;

public class PauseButton : MonoBehaviour
{
    public static bool isPause = false;
    public Transform cameraPause;
    public Transform player;
    
    public void CallPause()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>().transform;
        }
 
        if (!isPause)
        {
            cameraPause.SetPositionAndRotation(player.GetComponent<Player>().hmdTransforms[0].position, player.GetComponent<Player>().hmdTransforms[0].rotation);
            player.gameObject.SetActive(false);
            cameraPause.gameObject.SetActive(true);
        }
        else
        {
            player.gameObject.SetActive(true);
            cameraPause.gameObject.SetActive(false);
        }
        isPause = !isPause;
    }


}
