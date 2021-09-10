using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

namespace DreamHouseStudios.SofasaLogistica {
	public class Splitter : MonoBehaviour {

		#region Components
		[SerializeField] private float explosiveForce = 10f;
		[SerializeField] private LayerMask layer;
		[SerializeField] private Material matCross;

		[SerializeField] private Vector3 overlapBoxSizer;

		public UnityEvent onSlice;
		#endregion

		#region Unity Functions

		private void FixedUpdate () {
			//StartCutEffect ();
		}

		private void OnDrawGizmos () {
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube (transform.position, overlapBoxSizer);
		}

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == layer)
            {
                if (other.GetComponent<AnimatorTrigger>())
                {
                    other.GetComponent<AnimatorTrigger>().SetBool(true);
                }
            }
        }
        #endregion

        #region Functions

        void StartCutEffect () {

			Collider[] colliders = Physics.OverlapBox (transform.position, overlapBoxSizer, Quaternion.identity, layer);

			foreach (Collider c in colliders) {

				Destroy (c.gameObject);
				SlicedHull hull = c.gameObject.Slice (transform.position, transform.up);
				if (hull != null) {

					GameObject lower = hull.CreateLowerHull (c.gameObject, matCross);
					GameObject upper = hull.CreateUpperHull (c.gameObject, matCross);

					GameObject[] objs = new GameObject[] { lower, upper };

					foreach (GameObject obj in objs) {
						Rigidbody rb = obj.AddComponent<Rigidbody> ();
						obj.AddComponent<MeshCollider> ().convex = true;
						rb.AddExplosionForce (explosiveForce, transform.position, 5);
						obj.layer = LayerMask.NameToLayer ("Tape");
					}

					if (onSlice != null)
						onSlice.Invoke ();
				}
			}
		}
		#endregion
	}
}