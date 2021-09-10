using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour {
	[SerializeField] string sceneName = "Intro";
	[SerializeField] private KeyCode reloadKey = KeyCode.Escape;   

	private void Update () {
		if (Input.GetKeyDown (reloadKey))
			Restart ();
	}

	public void Restart () {
		SceneManager.LoadScene (sceneName);
	}
}