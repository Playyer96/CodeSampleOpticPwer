using DreamHouseStudios.SofasaLogistica;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class PackingIcons : MonoBehaviour
{
    public TestPocketPacking TPP;
    public ProductInvoice PI;
    private bool cajaArmada = false;

    //================IconoFUncionamiento

    public float speedFrame;
    public Sprite[] sprGrab;
    public Sprite[] sprScan;
    private Sprite[] close;
    public Image img;
    private bool isAnim;
    private bool isFollow;
    public Transform icoTR;
    public Vector3 offset;
    private Transform m_head;
    public Transform groupItems;

    public IEnumerator coroutinefollowTR;
    public IEnumerator startAnimIco;

    public SpectraUISettings settings;

    private void Start()
    {
        TPP = FindObjectOfType<TestPocketPacking>();
        m_head = FindObjectOfType<Player>().hmdTransforms[0];
    }

    public void SetProductInvoices(ProductInvoice pi)
    {
        PI = pi;
    }

    public void OnSetIcoScan()
    {
        if(settings.experienMode == ExperienMode.Evaluacion)
        {return;}
            
        isFollow = true;
        StartAnimIco(sprScan);
        SetFollowTR(PI.transform);
    }

    public void OnScanMoreProducts()
    {
        if(settings.experienMode == ExperienMode.Evaluacion)
        {return;}
        for (int i = 0; i < TPP.productsOnScene.Count; i++)
        {
            if (TPP.productsOnScene[i].GetComponentInChildren<ProductInvoice>().storedInBox == false)
            {
                isFollow = true;
                StartAnimIco(sprScan);
                SetFollowTR(groupItems);
                return;
            }
        }
        OnHideIco();
    }

    public void OnHideIco()
    {
        icoTR.gameObject.SetActive(false);
    }

    public void OnSetIcoGrab()
    {
        if(settings.experienMode == ExperienMode.Evaluacion)
        {return;}
        if (cajaArmada)
        {
            isFollow = true;
            StartAnimIco(sprGrab);
            SetFollowTR(PI.transform);
        }
    }

    public void OnSetGrabDople()
    {
        if(settings.experienMode == ExperienMode.Evaluacion)
        {return;}
        for (int i = 0; i < TPP.productsOnScene.Count; i++)
        {
            if (TPP.productsOnScene[i].GetComponentInChildren<ProductInvoice>().Product.productId == PI.Product.productId && TPP.productsOnScene[i].GetComponentInChildren<ProductInvoice>().storedInBox == false)
            {
                isFollow = true;
                StartAnimIco(sprGrab);
                SetFollowTR(TPP.productsOnScene[i].transform);
                return;
            }
        }
        OnScanMoreProducts();
    }

    public void SetCajaBool(bool val)
    {
        cajaArmada = val;
    }

    private IEnumerator AnimIco(Sprite[] sprites)
    {
        icoTR.gameObject.SetActive(true);
        yield return null;
        isAnim = sprites.Length > 1;
        if (isAnim)
        {
            int i = 0;
            while (isAnim)
            {
                img.sprite = sprites[i];
                yield return new WaitForSeconds(speedFrame);

                i++;
                if (i >= sprites.Length)
                    i = 0;
            }
        }
    }

    public void StartAnimIco(Sprite[] sprt)
    {
        if(settings.experienMode == ExperienMode.Evaluacion)
        {return;}
        if (startAnimIco != null)
        { StopCoroutine(startAnimIco); }
        startAnimIco = AnimIco(sprt);
        StartCoroutine(startAnimIco);
    }

    public void SetFollowTR(Transform TR)
    {
        if(settings.experienMode == ExperienMode.Evaluacion)
        {return;}
        if (coroutinefollowTR != null)
        { StopCoroutine(coroutinefollowTR); }
        coroutinefollowTR = FollowTR(TR);
        StartCoroutine(coroutinefollowTR);
    }

    private IEnumerator FollowTR(Transform TR)
    {
        while (isFollow)
        {
            yield return null;
            icoTR.position = TR.position + offset;
            icoTR.rotation = DesireRotation(icoTR);
        }
    }

    private Quaternion DesireRotation(Transform tr)
    {
        Vector3 relativePos = (tr.position - m_head.position);
        relativePos.y = 0;

        Quaternion desireRotation = Quaternion.LookRotation(relativePos);
        return desireRotation;
    }
}