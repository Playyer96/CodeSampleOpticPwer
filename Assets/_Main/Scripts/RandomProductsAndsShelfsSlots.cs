using System;
using DreamHouseStudios.SofasaLogistica;
using DreamHouseStudios.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RandomProductsAndsShelfsSlots : MonoBehaviour
{
    public List<Transform> shelfs, presets;
    public List<PickingPocketProductInfo> inventory;

    //poner privada
    public List<PickingPocketProductInfo> productsPocket;
    public OnRandomize onRandomize;

    private IEnumerator Start()
    {
        foreach (Transform s in shelfs)
        {
            s.gameObject.SetActive(false);
            s.gameObject.AddComponent<Shelf>();
            Rigidbody rb = s.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            //rb.isKinematic = true;
        }

        yield return new WaitForSeconds(1f);
        foreach (Transform s in shelfs)
            s.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        foreach (Transform s in shelfs)
            Destroy(s.GetComponent<Rigidbody>());
        yield return new WaitForEndOfFrame();
        Randomize();
        yield return new WaitForSeconds(.25f);

        foreach (PickingPocketProductInfo pppi in inventory)
        {
            for (int i = 0; i < pppi.parent.childCount; i++)
            {
                //pppi.parent.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Interactable inter = pppi.parent.GetChild(i).GetComponent<Interactable>();

                inter.onRelease.AddListener(delegate (Interactable interactable_)
                {
                    ProductTrigger pt = inter.GetComponent<ProductTrigger>();
                    Rigidbody ptRb = pt.GetComponent<Rigidbody>();
                    if (!pt.inMobileShelf)
                    {
                        StartCoroutine(ReleasedFromMobileShelf(ptRb, false));
                        Debug.Log("Suelta producto afuera de carrito.");
                    }
                    else
                    {
                        StartCoroutine(ReleasedFromMobileShelf(ptRb, true));
                        Debug.Log("Suelta producto dentro de carrito.");
                    }
                });
            }
        }
    }

    //IEnumerator ReleasedFromMobileShelf_;
    IEnumerator ReleasedFromMobileShelf(Rigidbody rb, bool isIn)
    {
        if (isIn)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            yield return new WaitForSeconds(2f);
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            //podría resultar un futuro bug, que causa que cuando alguien suelta un producto dentro del carro y antes de dos segundos
            //pueda agarrar otro producto y empujar al primero, luego de esos dos segundos el primero quedaría flotando.
        }
        else
        {
            yield return null;
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        //foreach (PickingPocketProductInfo pppi in inventory)
    //        {
    //            foreach (Interactable i in FindObjectsOfType<Interactable>())
    //            {
    //                //pppi.parent.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    //                //Interactable inter = pppi.parent.GetChild(i).GetComponent<Interactable>();

    //                i.onRelease.Invoke(i);
    //            }
    //        }
    //    }
    //    //Randomize();
    //}

    internal void Randomize()
    {
        inventory = new List<PickingPocketProductInfo>();
        productsPocket = new List<PickingPocketProductInfo>();

        foreach (Transform p in presets)
        {
            ProductsPreset pp = p.GetComponent<ProductsPreset>();
            if (pp == null)
                p.gameObject.AddComponent<ProductsPreset>();
            else
                pp.used = false;
            p.gameObject.SetActive(false);
            p.position = Vector3.zero;
        }

        foreach (Transform p in presets.FindAll(P => P.name.ToLower().Contains("caja")))
            p.GetComponent<ProductsPreset>().used = true;

        List<Transform> usedBoxes = presets.FindAll(P => P.GetComponent<ProductsPreset>().used);

        usedBoxes[Random.Range(0, usedBoxes.Count)].GetComponent<ProductsPreset>().used = false;

        List<Transform> unusedPresets = presets.FindAll(P => !P.GetComponent<ProductsPreset>().used);

        int rands = shelfs.Count > unusedPresets.Count
            ? shelfs.Count - unusedPresets.Count
            : unusedPresets.Count - shelfs.Count;

        foreach (Transform s in shelfs)
        {
            Shelf sh = s.GetComponent<Shelf>();

            List<Transform> p = presets.FindAll(pr => !pr.GetComponent<ProductsPreset>().used);
            if (p.Count <= 0) break;

            bool skip = Random.Range(0, 100) >= 50;

            if (skip && rands > 0)
            {
                rands--;
                continue;
            }

            Transform preset = p[Random.Range(0, p.Count)];
            preset.position = s.position;
            preset.rotation = s.rotation;

            preset.GetComponent<ProductsPreset>().used = true;
            preset.gameObject.SetActive(true);

            if (!preset.name.ToLower().Contains("caja"))
            {
                for (int i = 0; i < preset.childCount; i++)
                    preset.GetChild(i).gameObject.SetActive(false);
                for (int i = preset.childCount - 1; i >= Random.Range(1, preset.childCount); i--)
                    preset.GetChild(i).gameObject.SetActive(true);
            }

            //FALTA ASIGNARLE ID DE ESTANTERIA A LOS PRODUCTOS?
            ProductInvoice[] pi = preset.transform.GetComponentsInChildren<ProductInvoice>();
            if (pi.Length > 0)
            {
                PickingPocketProductInfo ppi = inventory.Find(_p => _p.productCode == pi[0].Product.productId);
                if (ppi != null)
                {
                    ppi.quantity += pi.Length;
                }
                else
                {
                    inventory.Add(new PickingPocketProductInfo()
                    {
                        parent = preset,
                        productCode = pi[0].Product.productId,
                        quantity = pi.Length,
                        shelfCode = (sh != null && sh.shelfInvoice != null) ? sh.shelfInvoice.Data.shelfId : ""
                    });
                }
            }

            for (int i = 0; i < preset.childCount; i++)
            {
                ProductTrigger pt = preset.GetChild(i).GetComponent<ProductTrigger>() ??
                                    preset.GetChild(i).gameObject.AddComponent<ProductTrigger>();
                if (pt.rb == null)
                    preset.GetChild(i).GetComponent<Rigidbody>();

                Interactable interactable = preset.GetChild(i).GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.onGrab.AddListener(delegate (Interactable inte) { pt.released = false; });
                    /*interactable.onRelease.AddListener(delegate (Interactable inte)
                    {
                        Debug.Log("Release");
                        if (pt.inMobileShelf && !pt.counted)
                        {
                            pt.counted = true;//contar en la lista de picked
                            pt.released = true;
                            Debug.Log("Release + count");
                            PickingManager.CountDiscountProduct(interactable.GetComponentsInChildren<ProductInvoice>()[0]);
                        }
                    });*/
                }
            }

            //if (preset.name.ToLower().Contains("caja"))
            //    foreach (Transform t in presets)
            //        if (t.name.ToLower().Contains("caja"))
            //            t.GetComponent<ProductsPreset>().used = true;
        }

        foreach (PickingPocketProductInfo ppi_ in inventory)
        {
            productsPocket.Add(new PickingPocketProductInfo()
            {
                parent = ppi_.parent,
                productCode = ppi_.productCode,
                quantity = Random.Range(1, Mathf.Min(5, ppi_.quantity + 1)),
                shelfCode = ppi_.shelfCode
            });

            for (int i = 0; i < ppi_.parent.childCount; i++)
            {
                Transform t_ = ppi_.parent.GetChild(i);
                ObjectReset or_ = t_.GetComponent<ObjectReset>();
                if (or_ != null)
                {
                    or_.resetPos_ = t_.position;
                    or_.resetRotation_ = t_.rotation;
                }
            }

        }

        Debug.Log("Máximo 5 productos");
        onRandomize.Invoke(productsPocket);
        //Debug.Log("Revisar valores de List<PickingPocketProductInfo>");
        //if (!fromStart)
        //    foreach (Transform t in presets)
        //        for (int i = 0; i < t.childCount; i++)
        //            t.GetChild(i).gameObject.GetComponent<Product>().Invoke("Restart", 1.5f);

        //habilitar solo el interactable del primer producto si es modo entrenamiento
        if (PickingManager.isInTrainingMode)
        {
            //Debug.Log("Training mode: " + PickingManager.isInTrainingMode.ToString());
            foreach (Transform p in presets)
            {
                for (int i = 0; i < p.childCount; i++)
                {
                    p.GetChild(i).GetComponent<Interactable>().enabled = false;
                    p.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }

            foreach (PickingPocketProductInfo pppi in productsPocket)
            {
                for (int i = 0; i < pppi.parent.childCount; i++)
                {
                    Interactable inter = pppi.parent.GetChild(i).GetComponent<Interactable>();

                    inter.enabled = true;

                    pppi.parent.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
            }
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        Randomize();
    //}




    public class ProductsPreset : MonoBehaviour
    {
        public bool used;
    }

    public class ProductTrigger : MonoBehaviour
    {
        public bool inMobileShelf, counted = false, released; //counted = ya se contó del inventario
        public Vector3 diff;
        public Rigidbody rb;
        [NonSerialized] public static bool firstGrabbed, firstReleased;

        public Interactable i_Interactable;
        //public float lastXApplied;

        public ProductInvoice productInvoice;

        private IEnumerator Start()
        {
            rb = GetComponent<Rigidbody>();
            i_Interactable = GetComponent<Interactable>();
            while (productInvoice == null)
            {
                productInvoice = GetComponentsInChildren<ProductInvoice>()[0];
                yield return new WaitForSeconds(.25f);
            }
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "EstanteriaMovil" && !inMobileShelf)
            {
                inMobileShelf = true;
                //Debug.LogError("Comentar las siguientes dos líneas.");
                //PickingManager.CountDiscountProduct(transform.GetComponentsInChildren<ProductInvoice>()[0]);//BORRARRRRRRR!!!!!
                //counted = true;//BORRARRRRRRR!!!!!
            }
        }*/

        private void OnTriggerStay(Collider other)
        {
            if (inMobileShelf) return;
            if (other.tag == "EstanteriaMovil" && !inMobileShelf)
            {
                //if (GetComponent<DreamHouseStudios.VR.Interactable>() != null)
                //{
                //    DreamHouseStudios.VR.Interactable i_Interactable = GetComponent<DreamHouseStudios.VR.Interactable>();
                //    if (!i_Interactable.beingGrabbed)
                //    {
                //        transform.parent = other.transform;
                //        i_Interactable.enabled = false;
                //    }
                //}
                if (!i_Interactable.beingGrabbed)
                {
                    transform.parent = other.transform;
                    inMobileShelf = true;

                    if (!PickingManager.instance) return;
                    if (!counted)
                    {
                        counted = true; //contar en la lista de picked
                        released = true;
                        Debug.Log("<color=blue>Release + count</color>");
                        PickingManager.CountDiscountProduct(productInvoice);
                    }
                }

                //Debug.LogError("Comentar esta y las siguientes dos líneas.");
                //PickingManager.CountDiscountProduct(transform.GetComponentsInChildren<ProductInvoice>()[0]);//BORRARRRRRRR!!!!!
                //counted = true;//BORRARRRRRRR!!!!!
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Exit");

            if (!PickingManager.instance) return;
            if (other.tag == "EstanteriaMovil" && inMobileShelf)
            {
                Debug.Log(name + " IsCounted: " + counted.ToString());
                if (counted)
                    PickingManager.CountDiscountProduct(transform.GetComponentsInChildren<ProductInvoice>()[0],
                        -1); //descontar de la lista de picked
                inMobileShelf = false;
                counted = false;
                transform.parent = null;
            }
        }
    }

    public class Shelf : MonoBehaviour
    {
        public ShelfInvoice shelfInvoice;

        private void OnCollisionEnter(Collision collision)
        {
            ShelfInvoice si = collision.transform.GetComponent<ShelfInvoice>();
            if (si != null)
            {
                shelfInvoice = si;
            }
        }
    }

    [System.Serializable]
    public class PickingPocketProductInfo
    {
        public string shelfCode, productCode;
        public int quantity;

        public Transform parent;
        //public Vector3 position;
        //public Quaternion rotation;
    }

    [System.Serializable]
    public class OnRandomize : UnityEvent<List<PickingPocketProductInfo>>
    {
    }
}