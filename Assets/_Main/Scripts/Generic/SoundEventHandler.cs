using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace DreamHouseStudios.ConfinedSpaces
{
	public class SoundEventHandler : MonoBehaviour
	{
		[EventRef]
		public string Event;

		[HideInInspector]
		public EventInstance inst;

		[Header("Gizmos")]
		public Color gColor = Color.cyan;

		[Range(0, 0.5f)]
		public float gizmoSize = 0.03f;

		private bool isRunning;
		private bool wasCreated;

		private void Awake()
		{
			Create();
		}

		public void Create()
		{
			if (wasCreated)
				return;

			inst = RuntimeManager.CreateInstance(Event);
			wasCreated = true;
		}

		public void Set3DAttributes()
		{
			inst.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
		}

		public void Play()
		{
			inst.start();

			isRunning = true;
			if (gameObject.activeInHierarchy)
				StartCoroutine(UpdateSoundPos());
		}

		private IEnumerator UpdateSoundPos()
		{
			PLAYBACK_STATE pState = PLAYBACK_STATE.PLAYING;
			inst.getPlaybackState(out pState);

			while ((pState == PLAYBACK_STATE.PLAYING || pState == PLAYBACK_STATE.STARTING) && isRunning)
			{
				Set3DAttributes();
				inst.getPlaybackState(out pState);

				yield return 0;
			}
			isRunning = false;
		}

		public void Stop(FMOD.Studio.STOP_MODE mode)
		{
			inst.stop(mode);
			if (isRunning)
				StopAllCoroutines();
		}

		private void OnDisable()
		{
			Stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}
}