using System;
using FMODUnity;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace DreamHouseStudios.SofasaLogistica {
	public class Pocket : MonoBehaviour {
		#region Components
		public event Action OnAttachedToHand;

		[SerializeField] private Interactable interactable;

		[SerializeField] private SteamVR_Action_Boolean scannAction;

		[SerializeField] private UIPocket uiScreen;
		[SerializeField] StudioEventEmitter eventEmitter;

		private bool isOnHand;
		#endregion

		#region Unity Functions
		private void Start () {
			scannAction.AddOnStateDownListener (StateDown, SteamVR_Input_Sources.LeftHand);
			scannAction.AddOnStateDownListener (StateDown, SteamVR_Input_Sources.RightHand);

			scannAction.AddOnStateUpListener (StateUp, SteamVR_Input_Sources.LeftHand);
			scannAction.AddOnStateUpListener (StateUp, SteamVR_Input_Sources.RightHand);

			//uiScreen.Hide ();
		}

		private void OnDestroy () {
			eventEmitter.Stop ();
		}
		#endregion

		#region Functions
		private void StateDown (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
			if (!isOnHand || fromSource != interactable.attachedToHand.handType)
				return;

			eventEmitter.Play ();
			uiScreen.TryScanInvoice ();
		}

		private void StateUp (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
			if (!isOnHand || fromSource != interactable.attachedToHand.handType)
				return;

			uiScreen.StopScan ();
		}

		public void SetOnHand () {
			if (OnAttachedToHand != null) OnAttachedToHand ();

			isOnHand = true;
			uiScreen.Show ();
		}

		public void SetDetachHand () {
			isOnHand = false;
			uiScreen.Hide ();
		}
		#endregion
	}
}