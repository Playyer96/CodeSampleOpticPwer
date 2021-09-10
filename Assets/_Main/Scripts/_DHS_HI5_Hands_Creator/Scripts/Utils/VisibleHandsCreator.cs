#if UNITY_EDITOR
using HI5;
using Hi5_Interaction_Core;
using Hi5_Interaction_Interface;
using UnityEditorInternal;
using UnityEngine;

namespace DreamHouseStudios.WayGroup.Util
{
    public class VisibleHandsCreator : MonoBehaviour
    {
        [Header("Hand Base")]
        [SerializeField]
        private Hi5_Hand_Visible_Hand originalHand;

        [Header("New Hand Parameters")]
        [SerializeField]
        private HI5_VIVEInstance mGloveHand;

        [SerializeField]
        private Transform newHand;

        [SerializeField]
        private Transform handRenderer;

        [SerializeField]
        private bool isLeft;

        private Transform originalHandBone;

        private Transform newHandBone;

        private Hi5_Hand_Visible_Hand hand;

        public void CreateHand()
        {
            ConfigureHand();
        }

        private void ConfigureHand()
        {
            hand = newHand.gameObject.GetComponent<Hi5_Hand_Visible_Hand>() != null ? newHand.gameObject.GetComponent<Hi5_Hand_Visible_Hand>() : newHand.gameObject.AddComponent<Hi5_Hand_Visible_Hand>();
            newHand.name = originalHand.name;

            if (newHand.GetComponent<Hi5_Interface_Hand>() == null)
                newHand.gameObject.AddComponent<Hi5_Interface_Hand>();

            hand.mGlove_Hand = mGloveHand;
            hand.renderTransform = handRenderer;

            hand.armTransform = newHand.transform.GetChild(0);
            hand.handTransform = hand.armTransform.GetChild(0);

            hand.m_IsLeftHand = isLeft;

            originalHandBone = originalHand.transform.GetChild(0);
            newHandBone = newHand.GetChild(0);

            newHandBone.name = originalHand.name;

            ConfigureFingers(originalHandBone.GetChild(0), newHandBone.GetChild(0));
            ConfigurePalmAndNails(originalHandBone.GetChild(0), newHandBone.GetChild(0));
        }

        private void ConfigurePalmAndNails(Transform originalPalm, Transform newPalm)
        {
            CloneAndAssign(originalPalm.Find("PalmCollider"), newPalm);
            CloneAndAssign(originalPalm.Find("NailObjects"), newPalm);
        }

        private void ConfigureFingers(Transform originalPalm, Transform newPalm)
        {
            ConfigureThumb(originalPalm.GetChild(0), newPalm.GetChild(0));
            ConfigureOtherFinger(originalPalm.GetChild(1), newPalm.GetChild(1));
            ConfigureOtherFinger(originalPalm.GetChild(2), newPalm.GetChild(2));
            ConfigureOtherFinger(originalPalm.GetChild(3), newPalm.GetChild(3));
            ConfigureOtherFinger(originalPalm.GetChild(4), newPalm.GetChild(4));
        }

        private void ConfigureThumb(Transform originalFingers, Transform newFingers)
        {
            newFingers.name = originalFingers.name;

            CopyAndPasteComponent<Hi5_Hand_Visible_Thumb_Finger>(originalFingers.gameObject, newFingers.gameObject);
            DiveToSonsRecursively(originalFingers, newFingers);
        }

        private void ConfigureOtherFinger(Transform originalFingers, Transform newFingers)
        {
            newFingers.name = originalFingers.name;

            CopyAndPasteComponent<Hi5_Hand_Visible_Finger>(originalFingers.gameObject, newFingers.gameObject);
            DiveToSonsRecursively(originalFingers, newFingers);
        }

        private void CopyAndPasteComponent<T>(GameObject original, GameObject newObj) where T : MonoBehaviour
        {
            T originalComponent = original.GetComponent<T>();
            T newComponent = newObj.GetComponent<T>() != null ? newObj.GetComponent<T>() : newObj.AddComponent<T>();

            if (ComponentUtility.CopyComponent(originalComponent))
                ComponentUtility.PasteComponentValues(newComponent);
        }

        private void DiveToSonsRecursively(Transform parent, Transform newParent)
        {
            //Debug.logf("{0} / {1}", parent.name, newParent != null ? newParent.name : "Nada");
            Transform cSon = null;
            Transform nSon = null;

            for (int i = 0; i < parent.childCount; i++)
            {
                cSon = parent.GetChild(i);
                if (newParent != null)
                {
                    if (newParent.childCount > i)
                    {
                        nSon = newParent.GetChild(i);
                        nSon.name = cSon.name;
                    }
                    else
                        CloneAndAssign(cSon, newParent);
                }
                if (i == 0)
                    DiveToSonsRecursively(cSon, nSon);
            }
        }

        private void CloneAndAssign(Transform original, Transform parent)
        {
            Transform neoSon = parent.Find(original.name) == null ? Instantiate(original, parent) : parent.Find(original.name);
            neoSon.localScale = original.localScale;
            neoSon.localEulerAngles = original.localEulerAngles;
            neoSon.localPosition = original.localPosition;
            neoSon.name = original.name;

            Collider[] colls = neoSon.GetComponentsInChildren<Collider>();
            foreach (Collider c in colls)
                DestroyImmediate(c);


            //Logger.Log("Adding son '{0}' to {1}", neoSon.name, parent.name);
        }
    }
}
#endif