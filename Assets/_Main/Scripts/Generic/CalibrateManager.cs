using System;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalibrateManager : MonoBehaviour
{
    public static CalibrateManager Instance;
    public string lastScene;

    public void Create()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Instance == this)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                lastScene = SceneManager.GetActiveScene().name;
                ScenesManager.instance.StartLoadScene("Calibration 1");
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                ScenesManager.instance.StartLoadScene(lastScene);
            }
        }
        else
        {
            return;
        }
    }
}
