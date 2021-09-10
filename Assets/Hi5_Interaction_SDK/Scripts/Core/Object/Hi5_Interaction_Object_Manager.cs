using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hi5_Interaction_Core
{
	public class Hi5_Interaction_Object_Manager : MonoBehaviour
	{
		public static event Action<Hi5_Glove_Interaction_Hand, Hi5_Glove_Interaction_Item> OnPinchObject
		{
			add { mManager.onPinchObject += value; }
			remove { mManager.onPinchObject -= value; }
		}

		public static event Action<Hi5_Glove_Interaction_Hand, Hi5_Glove_Interaction_Item> OnUnpinchObject
		{
			add { mManager.onUnpinchObject += value; }
			remove { mManager.onUnpinchObject -= value; }
		}

		private event Action<Hi5_Glove_Interaction_Hand, Hi5_Glove_Interaction_Item> onUnpinchObject;

		private event Action<Hi5_Glove_Interaction_Hand, Hi5_Glove_Interaction_Item> onPinchObject;

		public static Hi5_Interaction_Object_Manager GetObjectManager()
		{
			return mManager;
		}

		private static Hi5_Interaction_Object_Manager mManager = null;

		//public Transform tempMove;
		private Dictionary<int, Hi5_Glove_Interaction_Item> mObjectDic = new Dictionary<int, Hi5_Glove_Interaction_Item>();

		#region unity system

		private void Awake()
		{
			transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
			mManager = this;
			AddObjecs();
		}

		private void OnDisable()
		{
			Hi5_Interaction_Message.GetInstance().UnRegisterMessage(PinchObject, Hi5_MessageKey.messagePinchObject);
			Hi5_Interaction_Message.GetInstance().UnRegisterMessage(FlyPinchObject, Hi5_MessageKey.messageFlyPinchObject);
			Hi5_Interaction_Message.GetInstance().UnRegisterMessage(UnPinchObject, Hi5_MessageKey.messageUnPinchObject);
			Hi5_Interaction_Message.GetInstance().UnRegisterMessage(GetObjectById, Hi5_MessageKey.messageGetObjecById);
			Hi5_Interaction_Message.GetInstance().UnRegisterMessage(Pinch2Object, Hi5_MessageKey.messagePinchObject2);
			Hi5_Interaction_Message.GetInstance().UnRegisterMessage(UnPinch2Object, Hi5_MessageKey.messageUnPinchObject2);
			Hi5_Interaction_Message.GetInstance().UnRegisterMessage(LiftObject, Hi5_MessageKey.messageLiftObject);
			mObjectDic.Clear();
		}

		private void OnEnable()
		{
			Hi5_Interaction_Message.GetInstance().RegisterMessage(PinchObject, Hi5_MessageKey.messagePinchObject);
			Hi5_Interaction_Message.GetInstance().RegisterMessage(FlyPinchObject, Hi5_MessageKey.messageFlyPinchObject);
			Hi5_Interaction_Message.GetInstance().RegisterMessage(GetObjectById, Hi5_MessageKey.messageGetObjecById);
			Hi5_Interaction_Message.GetInstance().RegisterMessage(UnPinchObject, Hi5_MessageKey.messageUnPinchObject);
			Hi5_Interaction_Message.GetInstance().RegisterMessage(Pinch2Object, Hi5_MessageKey.messagePinchObject2);
			Hi5_Interaction_Message.GetInstance().RegisterMessage(UnPinch2Object, Hi5_MessageKey.messageUnPinchObject2);
			Hi5_Interaction_Message.GetInstance().RegisterMessage(LiftObject, Hi5_MessageKey.messageLiftObject);

			//mObjectDic.Clear();
		}

		#endregion unity system

		internal Hi5_Glove_Interaction_Item GetItemById(int id)
		{
			if (mObjectDic.ContainsKey(id))
			{
				return mObjectDic[id];
			}
			else
				return null;
		}

		internal Dictionary<int, Hi5_Glove_Interaction_Item> GetItems()
		{
			return mObjectDic;
		}

		private void AddObjecs()
		{
			mObjectDic.Clear();
			Hi5_Glove_Interaction_Item[] objects = transform.GetComponentsInChildren<Hi5_Glove_Interaction_Item>();
			foreach (Hi5_Glove_Interaction_Item item in objects)
			{
				if (!mObjectDic.ContainsKey(item.idObject))
					mObjectDic.Add(item.idObject, item);
			}
		}

		public void AddObject(Hi5_Glove_Interaction_Item item, int key)
		{
			if (!mObjectDic.ContainsKey(key))
			{
				mObjectDic.Add(key, item);
			}
		}

		public void RemoveObject(int key)
		{
			if (mObjectDic.ContainsKey(key))
				mObjectDic.Remove(key);
		}

		private void UnPinchObject(string messageKey, object param1, object param2, object param3, object param4)
		{
			if (messageKey.CompareTo(Hi5_MessageKey.messageUnPinchObject) == 0)
			{
				//Debug.Log ("UnPinchObject");
				int objectId = (int)param1;
				Hi5_Glove_Interaction_Hand hand = param2 as Hi5_Glove_Interaction_Hand;
				if (mObjectDic.ContainsKey(objectId))
				{
					Hi5_Glove_Interaction_Item pinchObject = mObjectDic[objectId];
					if (pinchObject != null && pinchObject.mObjectType == EObject_Type.ECommon)
					{
						Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EPinch);
						bool isRelease = false;
						bool OtherHandRelease = false;
						if (state != null && state is Hi5_Object_State_Pinch)
						{
							Hi5_Object_State_Pinch pinchState = state as Hi5_Object_State_Pinch;
							if (pinchState != null && !pinchState.isTestRelease)
							{
								if (hand.m_IsLeftHand)
									isRelease = pinchState.CancelPinchHand(Hi5_Object_Pinch_Type.ELeft, out OtherHandRelease);
								else
									isRelease = pinchState.CancelPinchHand(Hi5_Object_Pinch_Type.ERight, out OtherHandRelease);
							}
						}
						if (OtherHandRelease)
						{
							Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messagePinchOtherHandRelease, hand, objectId);
						}

						if (isRelease)
						{
							if (!pinchObject.isTouchPlane)
							{
								//Debug.Log ("!pinchObject.isTouchPlane");

								pinchObject.ChangeState(E_Object_State.EMove);
								pinchObject.CalculateThrowMove(hand.mPalm.transform, hand);
								pinchObject.CleanRecord();

								//								if (Hi5_Interaction_Const.TestChangeState)
								//									hand.mState.ChangeState(E_Hand_State.ERelease);
								{
									Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(pinchObject.idObject,
										pinchObject.mObjectType,
									   hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
										EEventObjectType.EMove);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
								}

								{
									Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(pinchObject.idObject,
										hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
										EEventHandType.EThrow);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
								}
							}
							else
							{
								Debug.Log("pinchObject.isTouchPlane");

								//if (Hi5_Interaction_Const.TestChangeState)
								hand.mState.ChangeState(E_Hand_State.ERelease);
								pinchObject.ChangeState(E_Object_State.EStatic);
								Hi5_Object_State_Static staticState = pinchObject.mstatemanager.GetState(E_Object_State.EStatic) as Hi5_Object_State_Static;
								staticState.ResetPreTransform();
								{
									Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(pinchObject.idObject,
										pinchObject.mObjectType,
									   hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
										EEventObjectType.EStatic);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
								}

								{
									Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(pinchObject.idObject,
										hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
										EEventHandType.ERelease);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
								}
							}
						}
					}
					if (onUnpinchObject != null)
						onUnpinchObject(hand, pinchObject);
				}
			}
		}

		private void UnPinch2Object(string messageKey, object param1, object param2, object param3, object param4)
		{
			if (messageKey.CompareTo(Hi5_MessageKey.messageUnPinchObject2) == 0)
			{
				int objectId = (int)param1;

				//ruige 加入双手判断
				Hi5_Glove_Interaction_Hand hand = param2 as Hi5_Glove_Interaction_Hand;
				if (mObjectDic.ContainsKey(objectId))
				{
					Hi5_Glove_Interaction_Item pinchObject = mObjectDic[objectId];
					if (pinchObject != null && pinchObject.mObjectType == EObject_Type.ECommon)
					{
						Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EPinch);
						bool isRelease = true;
						bool OtherHandRelease = false;
						if (state != null && state is Hi5_Object_State_Pinch)
						{
							Hi5_Object_State_Pinch pinchState = state as Hi5_Object_State_Pinch;
							if (pinchState != null)
							{
								if (hand.m_IsLeftHand)
									isRelease = pinchState.CancelPinchHand(Hi5_Object_Pinch_Type.ELeft, out OtherHandRelease);
								else
									isRelease = pinchState.CancelPinchHand(Hi5_Object_Pinch_Type.ERight, out OtherHandRelease);
							}
						}

						if (OtherHandRelease)
						{
							Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messagePinchOtherHandRelease, hand, objectId);
						}

						if (isRelease)
						{
							if (!pinchObject.isTouchPlane)
							{
								pinchObject.ChangeState(E_Object_State.EMove);
								pinchObject.CalculateThrowMove(hand.mPalm.transform, hand);
								pinchObject.CleanRecord();

								{
									Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(pinchObject.idObject,
										pinchObject.mObjectType,
									   hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
										EEventObjectType.EMove);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
								}

								{
									Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(pinchObject.idObject,
										hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
										EEventHandType.EThrow);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
								}
							}
							else
							{
								pinchObject.ChangeState(E_Object_State.EStatic);
								Hi5_Object_State_Static staticState = pinchObject.mstatemanager.GetState(E_Object_State.EStatic) as Hi5_Object_State_Static;
								staticState.ResetPreTransform();

								{
									Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(pinchObject.idObject,
										pinchObject.mObjectType,
									   hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
										EEventObjectType.EStatic);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
								}

								{
									Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(pinchObject.idObject,
										hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
										EEventHandType.ERelease);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
								}
							}
						}
					}

					if (onUnpinchObject != null)
						onUnpinchObject(hand, pinchObject);
				}
			}
		}

		private void PinchObject(string messageKey, object param1, object param2, object param3, object param4)
		{
			if (messageKey.CompareTo(Hi5_MessageKey.messagePinchObject) == 0)
			{
				List<int> objectIds = param1 as List<int>;
				Hi5_Glove_Interaction_Hand hand = param2 as Hi5_Glove_Interaction_Hand;
				int objectId = (int)param3;
				if (mObjectDic.ContainsKey(objectId))
				{
					Hi5_Glove_Interaction_Item pinchObject = mObjectDic[objectId];
					hand.AddPinchObject(pinchObject.transform, hand.mVisibleHand.palm);
					pinchObject.SetIsKinematic(true);

					pinchObject.ChangeState(E_Object_State.EPinch);

					Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EPinch);
					if (state != null && (state is Hi5_Object_State_Pinch))
					{
						Hi5_Object_State_Pinch pinchState = state as Hi5_Object_State_Pinch;
						if (hand.m_IsLeftHand)
							pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ELeft, hand);
						else
							pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ERight, hand);
					}
					if (onPinchObject != null)
						onPinchObject(hand, pinchObject);
				}
			}
		}

		private void FlyPinchObject(string messageKey, object param1, object param2, object param3, object param4)
		{
			if (messageKey.CompareTo(Hi5_MessageKey.messageFlyPinchObject) == 0)
			{
				List<int> objectIds = param1 as List<int>;
				Hi5_Glove_Interaction_Hand hand = param2 as Hi5_Glove_Interaction_Hand;
				int objectId = (int)param3;

				if (mObjectDic.ContainsKey(objectId))
				{
					Hi5_Glove_Interaction_Item pinchObject = mObjectDic[objectId];
					if (pinchObject != null && pinchObject.mObjectType == EObject_Type.ECommon)
					{
						hand.AddPinchObject(pinchObject.transform, hand.mVisibleHand.palm);
						pinchObject.SetIsKinematic(true);

						//pinchObject.transform.position = hand.mPalm.transform.position;
						pinchObject.ChangeState(E_Object_State.EPinch);
						Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EPinch);
						if (state != null && (state is Hi5_Object_State_Pinch))
						{
							Hi5_Object_State_Pinch pinchState = state as Hi5_Object_State_Pinch;

							//pinchState.isTestRelease = true;
							if (hand.m_IsLeftHand)
								pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ELeft, hand);
							else
								pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ERight, hand);
						}
					}
					if (onPinchObject != null)
						onPinchObject(hand, pinchObject);
				}
			}
		}

		private void Pinch2Object(string messageKey, object param1, object param2, object param3, object param4)
		{
			if (messageKey.CompareTo(Hi5_MessageKey.messagePinchObject2) == 0)
			{
				List<int> objectIds = param1 as List<int>;
				Hi5_Glove_Interaction_Hand hand = param2 as Hi5_Glove_Interaction_Hand;
				int objectId = (int)param3;
				if (mObjectDic.ContainsKey(objectId))
				{
					//ruige 加入判断
					Hi5_Glove_Interaction_Item pinchObject = mObjectDic[objectId];
					if (pinchObject != null && pinchObject.mObjectType == EObject_Type.ECommon)
					{
						hand.AddPinch2Object(pinchObject.transform, hand.mVisibleHand.palm);

						pinchObject.SetIsKinematic(true);
						pinchObject.ChangeState(E_Object_State.EPinch);
						Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EPinch);
						if (state != null && (state is Hi5_Object_State_Pinch))
						{
							Hi5_Object_State_Pinch pinchState = state as Hi5_Object_State_Pinch;
							if (hand.m_IsLeftHand)
								pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ELeft, hand);
							else
								pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ERight, hand);
						}
					}
					if (onPinchObject != null)
						onPinchObject(hand, pinchObject);
				}
			}
		}

		private void LiftObject(string messageKey, object param1, object param2, object param3, object param4)
		{
			if (messageKey.CompareTo(Hi5_MessageKey.messageLiftObject) == 0)
			{
				Hi5_Glove_Interaction_Hand hand = param1 as Hi5_Glove_Interaction_Hand;
				int id = (int)param2;
				if (mObjectDic.ContainsKey(id))
				{
					Hi5_Glove_Interaction_Item pinchObject = mObjectDic[id];
					if (pinchObject != null && pinchObject.mObjectType == EObject_Type.ECommon)
					{
						Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EFlyLift);
						if (state is Hi5_Object_State_Fly_Lift)
						{
							(state as Hi5_Object_State_Fly_Lift).hand = hand;
						}
						pinchObject.mstatemanager.ChangeState(E_Object_State.EFlyLift);
					}
				}
			}
		}

		private void GetObjectById(string messageKey, object param1, object param2, object param3, object param4)
		{
			if (messageKey.CompareTo(Hi5_MessageKey.messageGetObjecById) == 0)
			{
				int id = (int)param1;
				if (mObjectDic.ContainsKey(id))
				{
					List<Hi5_Glove_Interaction_Item> transformTemp = param2 as List<Hi5_Glove_Interaction_Item>;
					transformTemp.Add(mObjectDic[id]);
					param2 = transformTemp as object;
				}
			}
		}
	}
}