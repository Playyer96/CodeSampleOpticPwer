using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using FMODUnity;

namespace DreamHouseStudios.SofasaLogistica {
	public class BoxAssemble : BoxState {
		#region Components

		public CircularDrive backFlapLeftCircularDrive = null;
        public CircularDrive frontFlapRightCircularDrive = null;
		public CircularDrive flapLeftCircularDrive = null;
		public CircularDrive flapRightCircularDrive = null;

		public LinearDrive linearDriveSideOne = null;
		public LinearDrive linearDriveSideTwo = null;

        [SerializeField] StudioEventEmitter stapler;
        [SerializeField] StudioEventEmitter sealedBox;
        [SerializeField] StudioEventEmitter dropProduct;

		bool boxOpen = false;
		bool isReadyToSealed = false;
        bool isSealed = false;

        [SerializeField] List<GameObject> products = new List<GameObject>();
        [SerializeField] List<GameObject> onBoxProducts = new List<GameObject>();

		bool flap1Close, flap2Close, flap3Close, flap4Close;


        public bool IsSealed { get { return isSealed; } }
		#endregion Components

		#region Unity Functions

		private void Start () {
			Init ();
		}

		private void Update () {
			//Tick ();
		}

		private void OnCollisionEnter (Collision other) {
			if (other.gameObject.tag == "Stapler" && boxStates == BoxStates.OpenBox && isReadyToSealed) {
				boxStates = BoxStates.SealedBox;
                isSealed = true;
				BoxStateChanger ();

                if (sealedBox)
                {
                    if (sealedBox.IsPlaying())
                        sealedBox.Stop();

                    sealedBox.Play();
                }
            }

            for (int i = 0; i < products.Count; i++)
            {
            if(other.gameObject == products[i])
                {
                    products[i].SetActive(false);
                    onBoxProducts[i].SetActive(true);

                    if (dropProduct)
                    {
                        if (dropProduct.IsPlaying())
                            dropProduct.Stop();

                        dropProduct.Play();
                    }
                }
            }
		}

		#endregion Unity Functions

		#region Functions

        public void OnGrap()
        {
            if (boxStates != BoxStates.OpenBox && !IsSealed)
            {
                boxStates = BoxStates.OpenBox;
                BoxStateChanger();
                isReadyToSealed = true;
            }
        }

		private void Init () {
			boxStates = BoxStates.FlatBox;

			isReadyToSealed = false;

			BoxStateChanger ();
		}

		private void Tick () {

			if (linearDriveSideOne) {
				float distanceSideOne = Vector3.Distance (linearDriveSideOne.transform.position, linearDriveSideOne.endPosition.position);
			}
			if (linearDriveSideTwo) {
				float distanceSideTwo = Vector3.Distance (linearDriveSideTwo.transform.position, linearDriveSideTwo.endPosition.position);
			}

			if (linearDriveSideOne.linearMapping.value >= 0.85f && linearDriveSideTwo.linearMapping.value >= 0.85f && boxStates == BoxStates.FlatBox) {
				if (linearDriveSideOne.GetComponent<Interactable> ().attachedToHand ||
					linearDriveSideTwo.GetComponent<Interactable> ().attachedToHand) return;
				else {
					boxStates = BoxStates.OpenBox;
					boxOpen = true;
					BoxStateChanger ();
					Debug.Log (boxStates);
				}
			}
			if (boxOpen && boxStates != BoxStates.SealedBox) {
				if ((flapLeftCircularDrive.outAngle >= -100 && flapLeftCircularDrive.outAngle <= -80)) flap1Close = true;
				else flap1Close = false;

				if ((flapRightCircularDrive.outAngle <= 100 && flapRightCircularDrive.outAngle >= 80)) flap2Close = true;
				else flap2Close = false;

				if ((backFlapLeftCircularDrive.outAngle <= 100 && backFlapLeftCircularDrive.outAngle >= 80)) flap3Close = true;
				else flap3Close = false;

				if (frontFlapRightCircularDrive.outAngle <= -3 && frontFlapRightCircularDrive.outAngle >= -15) flap4Close = true;
				else flap4Close = false;

				if (flap1Close && flap2Close && flap3Close && flap4Close) {
					isReadyToSealed = true;
					boxStates = BoxStates.SealedBox;
					Debug.Log ("Is Ready To Be Sealed");
				} else {
					isReadyToSealed = false;
				}
			}
		}
		#endregion Functions
	}
}