using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AccionTutorial : MonoBehaviour
{
    [SerializeField] public SpritesAccionTutorial[] s_TutorialScreens;
    public Image[] i_images;
    public Transform[] t_Positions;
    public float f_FramesSprite;
    public Transform t_target;
    public bool b_IsFollow;
    public float f_fadeSpeed;
    public CanvasGroup cg;
    public bool isAnim = false;
    public int i_IndexPos;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        StopAnim();
    }

    private void Update()
    {
        transform.position = t_Positions[i_IndexPos].position;
        transform.rotation = DesireRotation();
    }

    public IEnumerator ActiveScreen(int i_IndexSprite, int i_indexPosition)
    {
        i_IndexPos = i_indexPosition;
        if (i_IndexSprite >= 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            yield return StartCoroutine(AnimSprite(i_IndexSprite));
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public IEnumerator AnimSprite(int indexSprite)
    {
        int i = 0;
        i_images[0].sprite = s_TutorialScreens[indexSprite].s_TopSprite[i];
        i_images[1].sprite = s_TutorialScreens[indexSprite].s_BottomSprite[i];
        i_images[0].SetNativeSize();
        i_images[1].SetNativeSize();
        i_images[0].rectTransform.sizeDelta = new Vector2(i_images[0].rectTransform.rect.width / s_TutorialScreens[indexSprite].f_ScaleTop, i_images[0].rectTransform.rect.height / s_TutorialScreens[indexSprite].f_ScaleTop);
        i_images[1].rectTransform.sizeDelta = new Vector2(i_images[1].rectTransform.rect.width / s_TutorialScreens[indexSprite].f_ScaleBottom, i_images[1].rectTransform.rect.height / s_TutorialScreens[indexSprite].f_ScaleBottom);
        yield return StartCoroutine(FadeCanvas(cg, 0, 1));
        isAnim = true;
        while (isAnim)
        {
            i_images[0].sprite = s_TutorialScreens[indexSprite].s_TopSprite[i];
            i_images[1].sprite = s_TutorialScreens[indexSprite].s_BottomSprite[i];
            
            i_images[0].SetNativeSize();
            i_images[1].SetNativeSize();
            i_images[0].rectTransform.sizeDelta = new Vector2(i_images[0].rectTransform.rect.width / s_TutorialScreens[indexSprite].f_ScaleTop, i_images[0].rectTransform.rect.height / s_TutorialScreens[indexSprite].f_ScaleTop);
            i_images[1].rectTransform.sizeDelta = new Vector2(i_images[1].rectTransform.rect.width / s_TutorialScreens[indexSprite].f_ScaleBottom, i_images[1].rectTransform.rect.height / s_TutorialScreens[indexSprite].f_ScaleBottom);
            
            yield return new WaitForSeconds(f_FramesSprite);
            i++;
            if (i >= s_TutorialScreens[indexSprite].s_TopSprite.Length)
            {
                i = 0;
            }
        }

        StopAllCoroutines();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    Quaternion DesireRotation()
    {
        Vector3 relativePos = (transform.position - t_target.position);
        relativePos.y = 0;
        Quaternion desireRotation = Quaternion.LookRotation(relativePos);
        return desireRotation;
    }

    IEnumerator FadeCanvas(CanvasGroup cg, int from, int to)
    {
        cg.alpha = from;
        while (cg.alpha < to)
        {
            yield return null;
            cg.alpha = Mathf.MoveTowards(cg.alpha, to, f_fadeSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < t_Positions.Length; i++)
        {
            Gizmos.DrawWireSphere(t_Positions[i].position, 0.05f);
        }
    }

    public void StopAnim()
    {
        isAnim = false;
        StopAllCoroutines();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void CallAnimation(string val)
    {
        char[] a = val.ToCharArray();
        StopAllCoroutines();
        StartCoroutine(ActiveScreen(a[0] - '0', a[1] - '0'));
    }
}

[System.Serializable]
public class SpritesAccionTutorial
{
    public Sprite[] s_TopSprite;
    public Sprite[] s_BottomSprite;
    public float f_ScaleTop;
    public float f_ScaleBottom;
}