using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
	public class BoxContainerBehaviour : BoxState {
		#region Components
		[SerializeField] private Splitter splitter;
		[SerializeField] private List<GameObject> objs;
		public StudioEventEmitter fxCutter;
		public StudioEventEmitter fxBoxDrop;
		#endregion

		#region Unity Functions
		private void OnDestroy () {
			Mute ();
		}
		private void Start () {
			boxStates = BoxStates.SealedBox;
			splitter.onSlice.AddListener (SlicedBox);
		}
		private void FixedUpdate () {
			BoxStateChanger ();
		}

		private void OnCollisionEnter (Collision other) {
			if (other.transform.tag == "Floor") {
				if (fxBoxDrop) {
					if (fxBoxDrop.IsPlaying ()) {
						return;
					}
					fxBoxDrop.Play ();
				}
			}
		}
		#endregion

		#region Functions
		private void SlicedBox () {
			boxStates = BoxStates.OpenBox;
			objs.ForEach (o => o.SetActive (true));
			if (fxCutter) {
				if (fxCutter.IsPlaying ()) {
					return;
				}
				fxCutter.Play ();
			}
		}

		private void Mute () {
			if (fxCutter)
				fxCutter.Stop ();
			if (fxBoxDrop)
				fxBoxDrop.Stop ();
		}
		#endregion
	}
}