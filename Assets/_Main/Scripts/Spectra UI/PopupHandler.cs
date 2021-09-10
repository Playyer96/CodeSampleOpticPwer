using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupHandler : MonoBehaviour
{
	private static PopupHandler _inst;

	private static GameObject GameObject
	{
		get
		{
			if (!CheckInstance())
				return null;

			return _inst.gameObject;
		}
	}

	[SerializeField]
	private Image center;

	[SerializeField]
	private Text textTitle;

	[SerializeField]
	private Text textInfo;

	[SerializeField]
	private List<PopupColor> popupColors;

	private Action onHide;

	private bool isConfigured;

	private void Awake()
	{
		Setup();
	}

	/// <summary>
	/// Call this method from another manager or using the Awake(), to configure singleton instance
	/// </summary>
	public void Setup()
	{
		if (isConfigured)
			return;

		_inst = this;
		isConfigured = true;
	}

	public static void ShowSimplePopup(string title, string message, float showTime, PopupType type = PopupType.Informative, Action onHide = null)
	{
		if (!CheckInstance())
			return;

		_inst.OnShowSimplePopup(title, message, showTime, type, onHide);
	}

	private void OnShowSimplePopup(string title, string message, float showTime, PopupType type, Action onHide)
	{
		this.onHide = onHide;
		center.rectTransform.localScale = Vector3.zero;

		textTitle.text = title;
		textInfo.text = message;

		center.color = popupColors.Find(c => c.type == type).color;

		gameObject.SetActive(true);

		LeanTween.scale(center.rectTransform, Vector3.one, 0.25f);
		LeanTween.scale(center.rectTransform, Vector3.zero, 0.25f).setDelay(showTime).setOnComplete(Hide);
	}

	private void Hide()
	{
		gameObject.SetActive(false);
		if (onHide != null)
			onHide();
	}

	private static bool CheckInstance()
	{
		if (_inst == null)
			Debug.LogWarning("Popup instance is not configured, please call 'Setup()' method from a manager script or let gameobject turned on at start of the scene");

		return _inst != null;
	}
}

[Serializable]
public class PopupColor
{
	public PopupType type;
	public Color color;
}

public enum PopupType
{
	Informative,
	Warning
}