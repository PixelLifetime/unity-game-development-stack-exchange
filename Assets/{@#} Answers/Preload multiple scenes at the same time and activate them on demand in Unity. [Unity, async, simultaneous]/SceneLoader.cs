using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	private IEnumerator LoadSceneAsyncProcess(string sceneName, AsyncOperation asyncOperation)
	{
		while (!asyncOperation.isDone)
		{
			Debug.Log($"[scene]:{sceneName} [percent complete]: {asyncOperation.progress}");

			yield return null;
		}
	}

	private IEnumerator LoadSceneAsyncProcess(string sceneName, AsyncOperationHandle<SceneInstance> asyncOperationHandle)
	{
		while (!asyncOperationHandle.IsDone)
		{
			Debug.Log($"[scene]:{sceneName} [percent complete]: {asyncOperationHandle.PercentComplete}");

			yield return null;
		}
	}

	[SerializeField] private PreloadSceneManager _preloadSceneManager;
	public PreloadSceneManager _PreloadSceneManager => this._preloadSceneManager;

	[SerializeField] private string _sceneNameOrKey = "[Scene] Preload Scene 1";
	public string _SceneNameOrKey => this._sceneNameOrKey;

	[SerializeField] private KeyCode _loadSceneKeyCode = KeyCode.Alpha1;
	public KeyCode _LoadSceneKeyCode => this._loadSceneKeyCode;

	[SerializeField] private LoadSceneMode _loadSceneMode = LoadSceneMode.Additive;
	public LoadSceneMode _LoadSceneMode => this._loadSceneMode;

	private void Update()
	{
		//if (Input.GetKeyDown(key: KeyCode.Return))
		//{
		//	// Start scene preloading.

		//	Debug.Log($"Preload [scene]: {this._sceneName}");

		//	AsyncOperation asyncOperation = this._preloadSceneManager.PreloadSceneAsync(
		//		sceneName: this._sceneName,
		//		loadSceneMode: LoadSceneMode.Additive
		//	);

		//	this.StartCoroutine(
		//		routine: this.LoadSceneAsyncProcess(
		//			sceneName: this._sceneName,
		//			asyncOperation: asyncOperation
		//		)
		//	);
		//}
		
		//if (Input.GetKeyDown(key: this._loadSceneKeyCode))
		//{
		//	Debug.Log($"Load [scene]: {this._sceneName}");

		//	AsyncOperation asyncOperation = this._preloadSceneManager.LoadSceneAsync(
		//		sceneName: this._sceneName,
		//		loadSceneMode: LoadSceneMode.Additive
		//	);
		//}

		if (Input.GetKeyDown(key: KeyCode.Return))
		{
			// Start scene preloading.

			Debug.Log($"Preload [scene]: {this._sceneNameOrKey}");

			AsyncOperationHandle<SceneInstance> asyncOperationHandle = this._preloadSceneManager.AddressablesPreloadSceneAsync(
				key: this._sceneNameOrKey,
				loadSceneMode: this._loadSceneMode
			);

			this.StartCoroutine(
				routine: this.LoadSceneAsyncProcess(
					sceneName: this._sceneNameOrKey,
					asyncOperationHandle: asyncOperationHandle
				)
			);
		}
		
		if (Input.GetKeyDown(key: this._loadSceneKeyCode))
		{
			Debug.Log($"Load [scene]: {this._sceneNameOrKey}");

			this._preloadSceneManager.AddressablesLoadSceneAsync(
				key: this._sceneNameOrKey,
				loadSceneMode: this._loadSceneMode
			);
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(target: this.gameObject);
	}
}