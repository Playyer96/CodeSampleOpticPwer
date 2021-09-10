using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace DreamHouseStudios.SofasaLogistica
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] Transform player = null;
        [SerializeField] GameObject[] teleports = null;
        [SerializeField] StudioEventEmitter teleportSound;
        public int i_ActualIndex;

        public void SetPlayerTransform(Transform teleportPoint)
        {
            PlaySound();
            player.SetPositionAndRotation(
                new Vector3(teleportPoint.position.x, teleportPoint.position.y, teleportPoint.position.z),
                new Quaternion(0, 0, 0, 0));
            teleportPoint.GetComponent<TeleportInedx>().LaunchEvent();
        }

        private void PlaySound()
        {
            if (teleportSound)
            {
                if (teleportSound.IsPlaying())
                    teleportSound.Stop();

                teleportSound.Play();
            }
        }

        public void ActivateTeleports(bool value)
        {
            for (int i = 0; i < teleports.Length; i++)
            {
                teleports[i].GetComponent<Collider>().enabled = value;
            }
        }
    }
}