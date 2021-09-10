using Hi5_Interaction_Core;
using UnityEngine;

namespace DreamHouseStudios.WayGroup
{
	[RequireComponent(typeof(Hi5_Glove_Interaction_Item))]
	public abstract class InteractiveObject : MonoBehaviour
	{
		private Hi5_Glove_Interaction_Item mItem;

		protected virtual void Start()
		{
			mItem = GetComponent<Hi5_Glove_Interaction_Item>();

			Hi5_Interaction_Object_Manager.OnPinchObject += PinchObject;
			Hi5_Interaction_Object_Manager.OnUnpinchObject += UnpinchObject;
		}

		private void PinchObject(Hi5_Glove_Interaction_Hand hand, Hi5_Glove_Interaction_Item item)
		{
			if (item == mItem)
				GrabObject();
		}

		private void UnpinchObject(Hi5_Glove_Interaction_Hand hand, Hi5_Glove_Interaction_Item item)
		{
			if (item == mItem)
				ReleaseObject();
		}

		protected abstract void GrabObject();

		protected abstract void ReleaseObject();

		private void OnDestroy()
		{
			Hi5_Interaction_Object_Manager.OnPinchObject -= PinchObject;
			Hi5_Interaction_Object_Manager.OnUnpinchObject -= UnpinchObject;
		}
	}
}