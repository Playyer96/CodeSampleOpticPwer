using DreamHouseStudios.SofasaLogistica;
using DreamHouseStudios.VR;
using Picking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static RandomProductsAndsShelfsSlots;

public class PickingManager : MonoBehaviour
{
    public static PickingManager instance;
    public Transform movableShelf;
    public SpectraUISettings spectraUISettings;
    public RandomProductsAndsShelfsSlots randomProductsAndsShelfsSlots;
    public List<RandomProductsAndsShelfsSlots.PickingPocketProductInfo> requestedList;
    public List<RandomProductsAndsShelfsSlots.PickingPocketProductInfo> pickedList;

    public List<ShelfInvoice> shelvesInvoices;

    //El int es el index
    public PickingEvent onProductCompleted;
    public UnityEvent onAllProductsCompleted;
    public NextShelf onNextShelf;
    public PercentChanged onPercentChanged;

    public int currentProductInPocket = 0;

    public static bool isInTrainingMode
    {
        get => Checklist.Get("main", "tutorial_mode");
    }

    public static bool CurrentProductCompleted
    {
        get
        {
            return instance.requestedList.Find(p =>
                       p.productCode == instance.pickedList[instance.currentProductInPocket].productCode &&
                       p.quantity == instance.pickedList[instance.currentProductInPocket].quantity) != null;
        }
    }

    public static bool ProductsCompleted
    {
        get
        {
            foreach (RandomProductsAndsShelfsSlots.PickingPocketProductInfo info in instance.requestedList)
            {
                if (instance.pickedList.Find(i => i.quantity == info.quantity && i.productCode == info.productCode) ==
                    null)
                    return false;
            }

            return true;
        }
    }

    void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        Checklist.Set("main", "tutorial_mode", spectraUISettings.experienMode == ExperienMode.Entrenamiento);
        shelvesInvoices.AddRange(FindObjectsOfType<ShelfInvoice>());
        yield return new WaitForEndOfFrame();
        UpdateCounters(0f);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    foreach (Interactable interactable in FindObjectsOfType<Interactable>())
        //    {
        //        interactable.onRelease.Invoke(interactable);
        //        Debug.Log("Fake release");
        //    }
        //}
        //    ProductGrab(requestedList[currentProductInPocket].productCode, requestedList[currentProductInPocket].shelfCode, requestedList[currentProductInPocket].quantity);
    }

    public void OnRandomizeProducts(List<RandomProductsAndsShelfsSlots.PickingPocketProductInfo> list)
    {
        requestedList = list;
        pickedList = new List<RandomProductsAndsShelfsSlots.PickingPocketProductInfo>();

        Checklist.ClearItems("randomproductsdoneinpocket");

        foreach (RandomProductsAndsShelfsSlots.PickingPocketProductInfo info in requestedList)
        {
            Checklist.AddOrUpdate("randomproductsdoneinpocket", info.productCode, false, true, delegate (bool v)
            {
                //aqui hay que colocar instrucciones para el siguiente paso?
            });

            pickedList.Add(new RandomProductsAndsShelfsSlots.PickingPocketProductInfo()
            { shelfCode = info.shelfCode, productCode = info.productCode, quantity = 0 });
        }

        spectraUISettings.pickingProducts = list;
    }

    //Grabar producto
    public void ProductGrab(string productCode, string shelfCode, int quantity)
    {
        RandomProductsAndsShelfsSlots.PickingPocketProductInfo given =
            requestedList.Find(p => p.productCode == productCode);
        if (given == null)
        {
            Debug.LogError("No existe el producto.");
            return;
        }

        if (given.shelfCode != shelfCode)
        {
            Debug.LogError("El producto no pertenece a la estanteria " + shelfCode);
            return;
        }

        if (!isInTrainingMode) return;

        if (given.quantity != quantity)
        {
            Debug.LogError("Las cantidades no coinciden.");
            return;
        }

        Checklist.Set("randomproductsdoneinpocket", productCode, true);
    }

    /// <summary>
    /// amount can be positive or negative
    /// </summary>
    /// <param name="productInvoice"></param>
    /// <param name="amount"></param>
    public static void CountDiscountProduct(ProductInvoice productInvoice, int amount = 1)
    {
        if (productInvoice == null) return;
        Debug.Log("Product invoice not null.");

        //if (amount > 0)
        //{
        //    //productInvoice.transform.parent.SetParent(instance.movableShelf, false);
        //    //Vector3 p0 = productInvoice.transform.parent.position;
        //    Vector3 p0 = instance.movableShelf.InverseTransformPoint(productInvoice.transform.parent.position);
        //    //Quaternion r0 = productInvoice.transform.parent.rotation;
        //    productInvoice.transform.parent.parent = instance.movableShelf;
        //    productInvoice.transform.parent.localPosition = p0;
        //    //productInvoice.transform.parent.rotation = r0;
        //}

        RandomProductsAndsShelfsSlots.PickingPocketProductInfo found = instance.pickedList.Find(p => p.productCode == productInvoice.Product.productId);

        if (found == null) return;

        found.quantity += amount;

        //productInvoice.transform.parent.parent = null;

        if (isInTrainingMode)
        {
            Debug.Log("Is in training mode.");
            if (CurrentProductCompleted)
            {
                //int givenIndex = instance.requestedList.FindIndex(i => i == given);
                int givenIndex =
                    instance.requestedList.FindIndex(i => i.productCode == productInvoice.Product.productId);

                instance.onProductCompleted.Invoke(givenIndex);
                instance.DisableAllInteractables();
                Debug.Log("Product completed.");

                if (givenIndex + 1 < instance.requestedList.Count)
                {
                    instance.currentProductInPocket = givenIndex + 1;
                    for (int i = 0; i < instance.requestedList[instance.currentProductInPocket].parent.childCount; i++)
                    {
                        Interactable interactable = instance.requestedList[instance.currentProductInPocket].parent
                            .GetChild(i).GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            interactable.enabled = true;
                            Rigidbody rb = interactable.transform.GetComponent<Rigidbody>();
                            rb.useGravity = true;
                            rb.constraints = RigidbodyConstraints.None;
                        }
                    }

                    //instance.requestedList[instance.currentProductInPocket].shelfCode

                    ShelfInvoice shelf = instance.shelvesInvoices.Find(si => si.Data.shelfId == instance.requestedList[instance.currentProductInPocket].shelfCode);

                    if (shelf != null)
                        instance.onNextShelf.Invoke(shelf);
                }
                else
                {
                    Debug.Log("Termina la lista!");
                    instance.onAllProductsCompleted.Invoke();
                    instance.DisableAllInteractables();
                }
            }

            instance.SetProductsInShelfAsKinematic();

            //Debug.Log("Completed: " + CurrentProductCompleted.ToString());
        }

        //requestedList
        //instance.pickedList

        float totalRequested = 0;

        float totalPicked = 0;

        instance.requestedList.ForEach(i => { totalRequested += i.quantity; });

        bool leftoverFound = false;

        instance.pickedList.ForEach(i =>
        {
            PickingPocketProductInfo requestedItem = instance.requestedList.Find(I => I.productCode == i.productCode);

            if (requestedItem != null)

                totalPicked += i.quantity > requestedItem.quantity ? 0f : i.quantity;
            //totalPicked += Mathf.Min(requestedItem.quantity, i.quantity);

            else

                leftoverFound = true;//si requestedItem es nulo quiere decir que es sobrante

        });

        //float _totalProgress = (totalPicked / totalRequested) * 100f;

        float _totalProgress = leftoverFound ? 0f : ((totalPicked / totalRequested) * 100f);

        ExperienceUI.instance.pickingProgress = _totalProgress;

        instance.onPercentChanged.Invoke(totalPicked / totalRequested);
    }

    public void SetProductsInShelfAsKinematic()
    {
        StartCoroutine("SetProductsInShelfAsKinematic_");
    }

    IEnumerator SetProductsInShelfAsKinematic_()
    {
        yield return new WaitForSeconds(1f);
        foreach (ProductTrigger pt in MovableShelf.instance.productTriggers)
        {
            if (pt.rb != null)
                pt.rb.isKinematic = true;
        }
    }

    public void DisableAllInteractables()
    {
        foreach (Transform t in instance.randomProductsAndsShelfsSlots.presets)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Interactable interactable = t.GetChild(i).GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.enabled = false;
                    //Rigidbody rb = interactable.transform.GetComponent<Rigidbody>();
                    //rb.useGravity = false;
                    //rb.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
    }

    public void GrabAllProducts()
    {
        Debug.Log("Revisar aqui el conteo de productos?");
    }

    public void UpdateCounters(float _totalProgress)
    {
        //ExperienceUI.instance.pickingProgress = _totalProgress;
        //ExperienceUI.instance.pickingProgress = Mathf.Min(100f, _totalProgress * 100f);//CHINO
        //ExperienceUI.instance.pickingProgress = (_totalProgress - Mathf.Max(0f, _totalProgress - 100f))*100f; //CHINO
        ExperienceUI.instance.pickingProgress = Mathf.Max(0f, (_totalProgress > 1f ? (1f - (_totalProgress - 1f)) : _totalProgress) * 100f);//CHINO
    }

    //public void Test(float percent)
    //{
    //    Debug.Log("Percent: " + percent.ToString());
    //}

    [System.Serializable]
    public class PickingEvent : UnityEvent<int>
    {
    }

    [System.Serializable]
    public class NextShelf : UnityEvent<ShelfInvoice>
    {
    }

    [System.Serializable]
    public class PercentChanged : UnityEvent<float>
    {
    }
}

//habilitar boton de siguiente en pocket cuando los productos de el pantallazo esten en la estanteria movil