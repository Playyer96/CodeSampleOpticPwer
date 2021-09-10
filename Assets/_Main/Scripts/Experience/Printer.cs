using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;


namespace DreamHouseStudios.SofasaLogistica {
    public class Printer : MonoBehaviour {
        #region Components
        public static Printer instance;
        public ProductData product;

        [SerializeField] Transform printPoint = null;
        [SerializeField] Transform printDirection =null;
        public GameObject invoiceToPrint = null;
        [SerializeField] StudioEventEmitter eventEmitter = null;
        private TR_Container t_Container;

        public int i_Count = 0;

        IEnumerator printingInvoice;

        public bool b_CreateResidue;
        public GameObject g_Residue;
        public Transform t_ResidueSpawnPoint;
        public UnityEvent e_OnPrint;
        
        #endregion

        #region Unity Functions
        private void OnDestroy () {
            if (eventEmitter)
                eventEmitter.Stop ();
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;
            t_Container = FindObjectOfType<TR_Container>();

            if (e_OnPrint == null)
            {
                e_OnPrint = new UnityEvent();
            }
        }
        #endregion

        
        #region Functions
        public void PrintInvoice () {
            if (printingInvoice != null)
                StopCoroutine (PrintingInvoice ());

            printingInvoice = PrintingInvoice ();
            StartCoroutine (printingInvoice);
        }
        #endregion

        #region Coroutines
        IEnumerator PrintingInvoice () {
            
            yield return new WaitForEndOfFrame ();


            Debug.Log ("<color=red>Printing an Invoice...</color>");

            if(invoiceToPrint.GetComponent<ReceptionInvoice>())
            invoiceToPrint.GetComponent<ReceptionInvoice>().Product = product;

            GameObject boxInvoice = Instantiate (invoiceToPrint, printPoint.position, printPoint.rotation);
            e_OnPrint.Invoke();

            eventEmitter.Play ();

            if (LeanTween.isTweening (boxInvoice))
                LeanTween.cancel (boxInvoice);

            LeanTween.move (boxInvoice, printDirection, 0.5f);
            yield return new WaitForSeconds (0.5f);

            if (invoiceToPrint.GetComponent<ReceptionInvoice>())
            {
                invoiceToPrint.GetComponent<Collider>().enabled = true;
                invoiceToPrint.GetComponent<Rigidbody>().useGravity = true;
                invoiceToPrint.GetComponent<Rigidbody>().isKinematic = false;
                invoiceToPrint.GetComponent<ObjectReset>().resetPos = printDirection;
            }
            
            if (i_Count == 0)
            {
                t_Container.playerItems[4] = boxInvoice.transform;
                ObjectsLogic o_Ol = FindObjectOfType<ObjectsLogic>();
                o_Ol.i_Interactable[15] = boxInvoice.transform;
            }
            yield return new WaitForEndOfFrame();
            if (b_CreateResidue)
            {
                StartCoroutine(CreateResidue());
            }
        }
        
        public IEnumerator CreateResidue()
        {
            yield return new WaitForSeconds(.5f);
            GameObject g_Residue = Instantiate(this.g_Residue, t_ResidueSpawnPoint.position, Quaternion.identity);
            g_Residue.transform.position = new Vector3(t_ResidueSpawnPoint.position.x+Random.Range(0,0.07f),t_ResidueSpawnPoint.position.y,t_ResidueSpawnPoint.position.z-Random.Range(0,0.05f));
            g_Residue.transform.localEulerAngles = new Vector3(0,Random.Range(0,180),0);
        }
        #endregion
    }
}