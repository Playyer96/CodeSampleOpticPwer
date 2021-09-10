using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DreamHouseSpectra.Networking.Data;
using DreamHouseSpectra.Util;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace DreamHouseSpectra.Networking
{
	public class MultipartData
	{
		public string id;
		public string name;
		public byte[] bytes;
	}

	public class WebServiceManager : MonoBehaviour
	{
		public static event Action<bool> OnLoadingData
		{
			add
			{
				if (_inst != null)
					_inst.loadingData += value;
			}
			remove
			{
				if (_inst != null)
					_inst.loadingData -= value;
			}
		}

		private event Action<bool> loadingData;

		private static WebServiceManager _inst;

		private static WebServiceManager instance
		{
			get
			{
				if (_inst == null)
				{
					GameObject g = new GameObject("Web Services Manager");

					_inst = g.AddComponent<WebServiceManager>();
					_inst.Initialize();
				}
				return _inst;
			}
		}

		private bool isLoading;

		public static bool IsLoading
		{
			get { return _inst.isLoading; }
		}

		private void Awake()
		{
			if (_inst == null)
			{
				_inst = this;
				Initialize();
			}
			else
				Destroy(gameObject);
		}

		private void Initialize()
		{
			DontDestroyOnLoad(_inst);
			_inst.TriggerLoadingStatus(false);

			SceneManager.sceneUnloaded += _inst.ClearLoadingEvent;
		}

		public static void GetDataFromEndpoint<K>(string url, string token, Action<K> onSucceded, Action<Error> onFailed, bool isList = false)
		{
			instance.StartCoroutine(instance.CallGetData(url, token, onSucceded, onFailed, isList));
		}

		public static void GetDataFromEndpoint(string url, string headerName, string headerValue, Action<string> onSucceded, Action<string> onFailed)
		{
			instance.StartCoroutine(instance.CallGetData(url, headerName, headerValue, onSucceded, onFailed));
		}

		public static void SendJsonData(string url, object json, string token, Action<string> onComplete, Action<string> onFailed, bool convertObject = true)
		{
			instance.StartCoroutine(instance.Upload(url, json, token, onComplete, onFailed, convertObject));
		}

		public static void SendJsonData<T>(string url, object data, string token, Action<T> onComplete, Action<Error> onFailed)
		{
			instance.StartCoroutine(instance.Upload(url, data, token, onComplete, onFailed));
		}

		public static void LoadTexture(string url, Action<Texture2D> onImageLoaded, Action<string> onFailLoadImage)
		{
			instance.StartCoroutine(instance.LoadImage(url, onImageLoaded, onFailLoadImage));
		}

		public static void DownloadFile(string url, string fileName, bool forceDownload, Action<string, string> onCompleted, Action<string> onFailed)
		{
			instance.StartCoroutine(instance.DownloadFileFromStreaming(url, fileName, onCompleted, onFailed));
		}

		public static void UploadFiles(string url, object data, List<MultipartData> multpartDatas, Action onCompleted, Action<Error> onFailed)
		{
			instance.StartCoroutine(instance.UploadMultipartData(url, data, multpartDatas, onCompleted, onFailed));
		}

		public static void UploadFiles<T>(string url, object data, List<MultipartData> multpartDatas, Action<T> onCompleted, Action<Error> onFailed)
		{
			instance.StartCoroutine(instance.UploadMultipartData(url, data, multpartDatas, onCompleted, onFailed));
		}

		private IEnumerator UploadMultipartData<T>(string url, object data, List<MultipartData> multpartDatas, Action<T> onCompleted, Action<Error> onFailed)
		{
			string json = JsonUtility.ToJson(data);
			Debug.LogFormat("Post: {0}\nUrl: {1}", json, url);

			List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
			formData.Add(new MultipartFormDataSection("data", json));

			foreach (MultipartData multpartData in multpartDatas)
			{
				Debug.LogFormat("Adding data to form: {0} - Bytes count {1}", multpartData.id, multpartData.bytes != null ? multpartData.bytes.Length : 0);
				formData.Add(new MultipartFormFileSection(multpartData.id, multpartData.bytes, multpartData.name, string.Empty));
			}
			UnityWebRequest www = UnityWebRequest.Post(url, formData);

			TriggerLoadingStatus(true);
			yield return www.SendWebRequest();
			TriggerLoadingStatus(false);

			Error err = new Error();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.LogWarning("Failed response: " + www.error + " " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
				try
				{
					err = JsonUtility.FromJson<Error>(www.downloadHandler.text);
				}
				catch (Exception)
				{
					err.error = www.error;
					err.message = www.downloadHandler.text;
				}
				onFailed(err);
			}
			else
			{
				Debug.Log("Succes response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
				onCompleted(JsonUtility.FromJson<T>(www.downloadHandler.text));
			}
		}

		private IEnumerator UploadMultipartData(string url, object data, List<MultipartData> multpartDatas, Action onCompleted, Action<Error> onFailed)
		{
			string json = JsonUtility.ToJson(data);
			Debug.LogFormat("Post: {0}\nUrl: {1}", json, url);

			List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
			formData.Add(new MultipartFormDataSection("data", json));

			foreach (MultipartData multpartData in multpartDatas)
			{
				Debug.LogFormat("Adding data to form: {0} - Bytes count {1}", multpartData.id, multpartData.bytes != null ? multpartData.bytes.Length : 0);
				formData.Add(new MultipartFormFileSection(multpartData.id, multpartData.bytes, multpartData.name, string.Empty));
			}
			UnityWebRequest www = UnityWebRequest.Post(url, formData);

			TriggerLoadingStatus(true);
			yield return www.SendWebRequest();
			TriggerLoadingStatus(false);

			Error err = new Error();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.LogWarning("Failed response: " + www.error + " " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
				try
				{
					err = JsonUtility.FromJson<Error>(www.downloadHandler.text);
				}
				catch (Exception)
				{
					err.error = www.error;
					err.message = www.downloadHandler.text;
				}
				onFailed(err);
			}
			else
			{
				Debug.Log("Succes response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
				onCompleted();
			}
		}

		private IEnumerator DownloadFileFromStreaming(string url, string fileName, Action<string, string> onCompleted, Action<string> onFailed)
		{
			string path = Path.Combine(Application.streamingAssetsPath, fileName);

			Debug.LogFormat("Searching in Streaming Assets: {0}", path);

			UnityWebRequest uwr = new UnityWebRequest(path);

			yield return uwr.SendWebRequest();

			if (string.IsNullOrEmpty(uwr.error))
			{
				Debug.LogFormat("File successfully loaded from streaming assets: {0}", path);
				onCompleted(path, fileName);
			}
			else
			{
				Debug.LogWarningFormat(uwr.error);
				onFailed(uwr.error);
			}
		}

		private IEnumerator DownloadFileFromURL(string url, string fileName, bool forceDownload, Action<string, string> onCompleted, Action<string> onFailed)
		{
			Debug.LogFormat("Dowloading {0}  from: {1}", fileName, url);

			string path = Path.Combine(Application.persistentDataPath, fileName);
			if (Tools.FileExistInPath(Application.persistentDataPath, fileName) && !forceDownload)
			{
				Debug.LogFormat("File {0} was already downloaded in persistent data path: {1}", fileName, path);
				onCompleted(path, fileName);

				yield break;
			}

			UnityWebRequest uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
			uwr.downloadHandler = new DownloadHandlerFile(path);

			yield return uwr.SendWebRequest();

			if (uwr.isNetworkError || uwr.isHttpError || !string.IsNullOrEmpty(uwr.error))
			{
				if (File.Exists(path))
				{
					File.Delete(path);
					Debug.LogFormat("Delete corrupted file: {0}", path);
				}
				Debug.LogWarningFormat("Failed download: {0}", uwr.error);
				onFailed(uwr.error);
			}
			else
			{
				Debug.LogFormat("File successfully downloaded and saved to {0}", path);
				onCompleted(path, fileName);
			}
		}

		private IEnumerator LoadImage(string url, Action<Texture2D> onImageLoaded, Action<string> onFailLoadImage)
		{
			Debug.LogFormat("Downloading image from: {0}", url);

			UnityWebRequest www = new UnityWebRequest(url);
			DownloadHandlerTexture dht = new DownloadHandlerTexture();

			www.downloadHandler = dht;

			yield return www;

			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogWarningFormat("Cannot load image, reason: {0}", www.error);
				onFailLoadImage(www.error);
			}
			else
			{
				if (dht.texture != null)
				{
					Debug.LogFormat("Image download succeded from url: {0}", url);
					onImageLoaded(dht.texture);
				}
				else
					onFailLoadImage("Texture is null");
			}
		}

		private IEnumerator Upload<T>(string url, object data, string token, Action<T> onComplete, Action<Error> onFailed)
		{
			Error err = new Error();

			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				if (onFailed != null)
				{
					err.error = "NO HAY CONEXIÓN";
					err.message = "No se cuenta con una conexión a internet, por favor intente conectar la aplicación a internet.";

					Debug.LogWarning(err.error);
					onFailed(err);
				}
				yield break;
			}

			string json = string.Empty;
			try
			{
				json = JsonUtility.ToJson(data);
			}
			catch
			{
				json = data.ToString();
			}
			Debug.Log("Post: " + json + "\nUrl: " + url);

			WWWForm form = new WWWForm();
			form.AddField("data", json);

			UnityWebRequest www = UnityWebRequest.Post(url, form);
			www.downloadHandler = new DownloadHandlerBuffer();

			if (!string.IsNullOrEmpty(token))
				www.SetRequestHeader("authorization", token);

			TriggerLoadingStatus(true);
			yield return www.SendWebRequest();
			TriggerLoadingStatus(false);

			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogWarning("Failed response: " + www.error + " " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
				try
				{
					err = JsonUtility.FromJson<Error>(www.downloadHandler.text);
				}
				catch (Exception)
				{
					err.error = www.error;
					err.message = www.downloadHandler.text;
				}

				if (err == null)
					err = new Error();

				onFailed(err);
			}
			else
			{
				if (www.responseCode == 200)
				{
					Debug.Log("Succes response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
					try
					{
						onComplete(JsonUtility.FromJson<T>(www.downloadHandler.text));
					}
					catch (Exception e)
					{
						if (err == null)
							err = new Error();

						err.message = e.Message;
						onFailed(err);
					}
				}
				else
				{
					Debug.LogWarning("Failed response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
					err = JsonUtility.FromJson<Error>(www.downloadHandler.text);

					if (err == null)
						err = new Error();

					onFailed(err);
				}
			}
		}

		private IEnumerator Upload(string url, object data, string token, Action<string> onComplete, Action<string> onFailed, bool convertObject)
		{
			Error err = new Error();

			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				if (onFailed != null)
				{
					err.error = "NO HAY CONEXIÓN";
					err.message = "No se cuenta con una conexión a internet, por favor intente conectar la aplicación a internet.";

					Debug.LogWarning(err.error);
					onFailed(err.message);
				}
				yield break;
			}
			string json = convertObject ? JsonUtility.ToJson(data) : data.ToString();
			Debug.LogFormat("Post: {0}\nUrl: {1}", json, url);

			WWWForm form = new WWWForm();
			form.AddField("data", json);

			UnityWebRequest www = UnityWebRequest.Post(url, form);
			www.downloadHandler = new DownloadHandlerBuffer();

			if (!string.IsNullOrEmpty(token))
				www.SetRequestHeader("authorization", token);

			TriggerLoadingStatus(true);
			yield return www.SendWebRequest();
			TriggerLoadingStatus(false);

			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogWarning("Failed response: " + www.error + " " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
				try
				{
					err = JsonUtility.FromJson<Error>(www.downloadHandler.text);
				}
				catch (Exception)
				{
					err.error = www.error;
					err.message = www.downloadHandler.text;
				}

				if (err == null)
					err = new Error();

				onFailed(err.message);
			}
			else
			{
				if (www.responseCode == 200)
				{
					Debug.Log("Succes response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
					onComplete(www.downloadHandler.text);
				}
				else
				{
					Debug.LogWarning("Failed response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
					err = JsonUtility.FromJson<Error>(www.downloadHandler.text);

					if (err == null)
						err = new Error();

					onFailed(err.message);
				}
			}
		}

		private IEnumerator CallGetData<K>(string url, string token, Action<K> onSucceded, Action<Error> onFailed, bool isList)
		{
			Error err = new Error();

			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				if (onFailed != null)
				{
					err.error = "NO HAY CONEXIÓN";
					err.message = "No se cuenta con una conexión a internet, por favor intente conectar la aplicación a internet.";

					Debug.LogWarning(err.error);
					onFailed(err);
				}
				yield break;
			}
			Debug.Log("Getting data from url: " + url);

			UnityWebRequest www = UnityWebRequest.Get(url);
			www.downloadHandler = new DownloadHandlerBuffer();

			if (!string.IsNullOrEmpty(token))
				www.SetRequestHeader("authorization", token);

			TriggerLoadingStatus(true);
			yield return www.SendWebRequest();
			TriggerLoadingStatus(false);

			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogWarning("Failed request: " + www.error + "\nurl: " + url);

				err.error = www.error;
				err.message = www.downloadHandler.text;

				onFailed(err);
			}
			else
			{
				string json = isList ? "{\"data\":" + www.downloadHandler.text + "}" : www.downloadHandler.text;
				Debug.Log("Succes response: " + json + "\nCode: " + www.responseCode + "\nUrl: " + url);

				if (www.responseCode == 200)
					onSucceded(JsonUtility.FromJson<K>(json));
				else
				{
					err = JsonUtility.FromJson<Error>(json);
					onFailed(err);
				}
			}
		}

		private IEnumerator CallGetData(string url, string headerName, string headerValue, Action<string> onSucceded, Action<string> onFailed)
		{
			UnityWebRequest www = UnityWebRequest.Get(url);
			www.SetRequestHeader(headerName, headerValue);

			TriggerLoadingStatus(true);
			yield return www.SendWebRequest();

			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogWarning("Failed request: " + www.error + "\nurl: " + url);
				onFailed(www.error);
			}
			else
			{
				Debug.Log("Succes response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);

				if (www.responseCode == 200)
					onSucceded(www.downloadHandler.text);
				else
					onFailed(www.downloadHandler.text);
			}
			TriggerLoadingStatus(false);
		}

		private void TriggerLoadingStatus(bool loading)
		{
			isLoading = loading;
			if (loadingData != null)
				loadingData(isLoading);
		}

		private void ClearLoadingEvent(Scene scene)
		{
			loadingData = null;
		}
	}
}