using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DreamHouseStudios.SofasaLogistica;
using Valve.VR.InteractionSystem;

public class CanvasManager : MonoBehaviour
{
    public TextReader textReader;

    [Header("PopUp")] public PopUp[] popUp;
    public Vector3 offset;

    [Header("Popup Image")] public GameObject popUpImageParent;
    public Image popUpImage;
    public bool followTransform;
    public Transform followPos;

    public SPR[] popUpImages;

    [Header("Array de posiciones")] public Positions[] group;

    [Header("Array de sprites del icono")] public SPR[] m_spr;
    public float framesIcono;
    public float fadeSpeed;
    public bool isFollow = false;
    public bool isAnim = false;
    public bool isPopupImageAnim = false;

    [Header("Referencias Fisicas de la escena")]
    public TR_Container container;

    [Header("Indice para pruebas")] public int indexStep;
    public SettingsManager sm;
    public Transform m_head;

    public AudioManager audioManager;
    public GameObject g_PopUpSoundFX;

    public bool b_PopUpFollowTransform = false;
    public Transform tr_TargetPopUp;
    public int i_rotate;
    public bool b_CanRotateIco = true;

    private IEnumerator corutineIco;
    private IEnumerator corutineAnimIco;
    private IEnumerator coroutinefollowTR;

    private void Awake()
    {
        textReader = GetComponent<TextReader>();
        StartCoroutine(SetPopUp(0, -1, 0));
        StartCoroutine(SetIcono(-1, 0));

        if (g_PopUpSoundFX)
            g_PopUpSoundFX.SetActive(false);

        if (popUpImage)
            StartCoroutine(SetPopUpImage(-1));
    }

    private void Start()
    {
        sm = FindObjectOfType<SettingsManager>();
        m_head = FindObjectOfType<Player>().hmdTransforms[0];
        container = FindObjectOfType<TR_Container>();
    }

    private void LateUpdate()
    {
        if (followTransform)
        {
            Vector3 newPos = new Vector3(followPos.position.x, followPos.position.y, followPos.position.z);
            Vector3 lerpPos = Vector3.Lerp(popUp[0].popUp_Tr.position, newPos, 3f * Time.deltaTime);
            popUp[0].popUp_Tr.position = Vector3.Distance(popUp[0].popUp_Tr.position, lerpPos) <= .001f ? newPos : lerpPos;
        }
    }

    private void Update()
    {
        if (b_CanRotateIco)
        {
            popUp[1].popUp_Tr.rotation = DesireRotation(popUp[1].popUp_Tr);
        }

        if (i_rotate == 1)
        {
            popUp[0].popUp_Tr.rotation = DesireRotation(popUp[0].popUp_Tr);
        }

        if (b_PopUpFollowTransform)
        {
            popUp[0].popUp_Tr.position = tr_TargetPopUp.position;
        }
    }

    public IEnumerator SetPopUp(int indexDoc, int indexTexto, int indexPosition)
    {
        if (indexTexto >= 0)
        {
            popUp[0].popUp_Tr.position = group[0].pos[indexPosition];
            popUp[0].popUp_Txt.text = textReader.GetText(indexDoc, indexTexto);
            if (i_rotate == 1)
            {
                //popUp[0].popUp_Tr.rotation = DesireRotation(popUp[0].popUp_Tr);
            }

            g_PopUpSoundFX.SetActive(true);
            yield return StartCoroutine(FadeCanvas(popUp[0].popUp_Tr.GetComponent<CanvasGroup>(), 0, 1));
        }
        else
        {
            popUp[0].popUp_Tr.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    public IEnumerator SetPopUp(int indexDoc, int indexTexto)
    {
        if (indexTexto >= 0)
        {
            popUp[0].popUp_Txt.text = textReader.GetText(indexDoc, indexTexto);
            if (i_rotate == 1)
            {
                //popUp[0].popUp_Tr.rotation = DesireRotation(popUp[0].popUp_Tr);
            }
            g_PopUpSoundFX.SetActive(true);
            yield return StartCoroutine(FadeCanvas(popUp[0].popUp_Tr.GetComponent<CanvasGroup>(), 0, 1));
        }
        else
        {
            popUp[0].popUp_Tr.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    public IEnumerator SetPopUpImage(int index)
    {
        if (index > -1)
        {
            popUpImageParent.SetActive(true);
            isPopupImageAnim = popUpImages[index].spr.Length > 1;

            if (isPopupImageAnim)
            {
                popUpImageParent.SetActive(true);
                int i = 0;

                while (isPopupImageAnim)
                {
                    popUpImage.sprite = popUpImages[index].spr[i];
                    yield return new WaitForSeconds(framesIcono);

                    i++;
                    if (i >= popUpImages[index].spr.Length)
                        i = 0;
                }
            }
            else
            {
                yield return new WaitForEndOfFrame();
                popUpImageParent.SetActive(true);
                popUpImage.sprite = popUpImages[index].spr[0];
            }
        }
        else
        {
            yield return new WaitForEndOfFrame();
            popUpImageParent.SetActive(false);
        }
    }

    public void StartSetIco(int indexElement, int indexPosition)
    {
        if (corutineIco != null)
        {
            StopCoroutine(corutineIco);
        }
        corutineIco = SetIcono(indexElement, indexPosition);
        StartCoroutine(corutineIco);
    }

    public IEnumerator SetIcono(int indexElement, int indexPosition)
    {
        if (indexElement >= 0)
        {
            StartAnimIco(indexElement);
            popUp[1].popUp_Tr.position = group[1].pos[indexPosition];
            popUp[1].popUp_Tr.rotation = DesireRotation(popUp[1].popUp_Tr);
            yield return StartCoroutine(FadeCanvas(popUp[1].popUp_Tr.GetComponent<CanvasGroup>(), 0, 1));
        }
        else
        {
            popUp[1].popUp_Tr.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    public void StartAnimIco(int indexIcono)
    {
        if (corutineAnimIco != null)
        {
            StopCoroutine(corutineAnimIco);
        }
        corutineAnimIco = SetAnimIco(indexIcono);
        StartCoroutine(corutineAnimIco);
    }

    public IEnumerator SetAnimIco(int indexIcono)
    {
        isAnim = m_spr[indexIcono].spr.Length > 1;
        if (isAnim)
        {
            int i = 0;
            while (isAnim)
            {
                popUp[1].popUp_Img.sprite = m_spr[indexIcono].spr[i];
                yield return new WaitForSeconds(framesIcono);

                i++;
                if (i >= m_spr[indexIcono].spr.Length)
                    i = 0;
            }
        }
        else
        {
            popUp[1].popUp_Img.sprite = m_spr[indexIcono].spr[0];
        }
    }

    private IEnumerator FadeCanvas(CanvasGroup cg, int from, int to)
    {
        cg.alpha = from;
        while (cg.alpha < to)
        {
            yield return null;
            cg.alpha = Mathf.MoveTowards(cg.alpha, to, fadeSpeed * Time.deltaTime);
        }
    }

    public Quaternion DesireRotation(Transform tr)
    {
        Vector3 relativePos = (tr.position - m_head.position);
        relativePos.y = 0;

        Quaternion desireRotation = Quaternion.LookRotation(relativePos);
        return desireRotation;
    }

    public IEnumerator FollowTransform(int indexSceneObject)
    {
        while (isFollow)
        {
            yield return null;
            popUp[1].popUp_Tr.position = container.playerItems[indexSceneObject].transform.position + offset;
            popUp[1].popUp_Tr.rotation = DesireRotation(popUp[1].popUp_Tr);
        }
    }


    public void SetFollowTR(Transform TR)
    {
        if(coroutinefollowTR != null)
        {StopCoroutine(coroutinefollowTR);}
        coroutinefollowTR = FollowTR(TR);
        StartCoroutine(coroutinefollowTR);
    }
    public IEnumerator FollowTR(Transform TR)
    {
        while (isFollow)
        {
            yield return null;
            popUp[1].popUp_Tr.position = TR.position + offset;
            popUp[1].popUp_Tr.rotation = DesireRotation(popUp[1].popUp_Tr);
        }
    }

    public IEnumerator Follow_Transform(Transform followTransform)
    {
        while (isFollow)
        {
            yield return null;
            popUp[1].popUp_Tr.position = followTransform.position + offset;
            popUp[1].popUp_Tr.rotation = DesireRotation(popUp[1].popUp_Tr);
        }
    }

    public void StepToStep(int index)
    {
        StopAllCoroutines();
        switch (index)
        {
            case 0:
                StartCoroutine(SetPopUp(1, 0, 0));
                StartCoroutine(SetIcono(-1, 0));
                break;

            case 1:
                StartCoroutine(SetPopUp(1, 1, 0));
                break;

            case 2:
                StartCoroutine(SetPopUp(1, 2, 0));
                StartCoroutine(SetIcono(0, 0));
                break;

            case 3:
                StartCoroutine(SetPopUp(1, 3, 0));
                StartCoroutine(SetIcono(1, 0));
                break;

            case 4:
                StartCoroutine(SetPopUp(1, 4, 0));
                StartCoroutine(SetIcono(2, 0));
                break;

            case 5:
                StartCoroutine(SetPopUp(1, 5, 0));
                StartCoroutine(SetIcono(-1, 0));
                audioManager.SetAudio(0, 21);
                break;
        }
    }

    public void KillAllCoroutines()
    {
        StopAllCoroutines();
        popUp[1].popUp_Tr.GetComponent<CanvasGroup>().alpha = 0;
        popUp[0].popUp_Tr.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void ChangeTransform(Transform target)
    {
        tr_TargetPopUp = target;
    }
}

[System.Serializable]
public class PopUp
{
    public Transform popUp_Tr;
    public Text popUp_Txt;
    public Image popUp_Img;
}

[System.Serializable]
public class SPR
{
    public Sprite[] spr;
}

[System.Serializable]
public class Positions
{
    public Vector3[] pos;
}