using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ToolsetEquipment : MonoBehaviour
    {
        #region Component
        [Space(10f), Header("Epp Items")]
        [SerializeField] GameObject penEpp = null;
        [SerializeField] GameObject markerEpp = null;
        [SerializeField] GameObject bisturiEpp = null;

        [Space(10f), Header("Items Cartuchera")]
        [SerializeField] GameObject pen = null;
        [SerializeField] GameObject marker = null;
        [SerializeField] GameObject bisturi = null;

        public bool progressIsSet = false;
        public bool isSet = false;

        bool bisturiSet = false, penSet = false, markerSet = false;

        IEnumerator checkIfComplete;
        WaitForEndOfFrame waitForEndOfFrame;
        WaitForSeconds oneSecond;

        public bool needReport;
        private ReportBackend rp;

        public bool IsSet { get { return isSet; } }
        #endregion

        #region Unity Functions
        private void Start()
        {
            waitForEndOfFrame = new WaitForEndOfFrame();
            oneSecond = new WaitForSeconds(1f);

            bisturi.SetActive(false);
            marker.SetActive(false);
            pen.SetActive(false);

            if (needReport && rp == null)
            {
                rp = GetComponent<ReportBackend>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
                switch (other.gameObject.name)
                {
                    case "bisturi":
                        bisturi.SetActive(true);
                        bisturiEpp.GetComponent<Collider>().enabled = false;
                        bisturiSet = true;

                        DisableRenderers(other.gameObject);
                   
                    break;
                    case "pen":
                        pen.SetActive(true);
                        penEpp.GetComponent<Collider>().enabled = false;
                        penSet = true;
                        DisableRenderers(other.gameObject);
                    break;
                    case "marker":
                        marker.SetActive(true);
                        markerEpp.GetComponent<Collider>().enabled = false;
                        markerSet = true;
                        DisableRenderers(other.gameObject);
                    break;
                }
                if (other.GetComponent<FxOnGrab>())
                    other.GetComponent<FxOnGrab>().PlaySound(2);
        }
        #endregion

        #region Functions
        public void DisableRenderers(GameObject other)
        {
            MeshRenderer[] meshRenderers = other.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].enabled = false;
            }
        }

        public void CheckForToolSet()
        {
            if (checkIfComplete != null)
                StopCoroutine(checkIfComplete);

            checkIfComplete = CheckIfComplete();
            StartCoroutine(checkIfComplete);
        }

        public void StopCheckIfComplete()
        {
            if (checkIfComplete != null)
                StopCoroutine(checkIfComplete);
        }

        IEnumerator CheckIfComplete()
        {
            yield return waitForEndOfFrame;

            bool KillWhenComplete = true;
            while (KillWhenComplete)
            {
                yield return oneSecond;
                if(bisturiSet && penSet && markerSet)
                {
                    isSet = true;
                    if (needReport)
                    {
                        rp.isReported = true;
                    }
                }

                if (isSet) KillWhenComplete = false;
            }
        }
        #endregion
    }
}
