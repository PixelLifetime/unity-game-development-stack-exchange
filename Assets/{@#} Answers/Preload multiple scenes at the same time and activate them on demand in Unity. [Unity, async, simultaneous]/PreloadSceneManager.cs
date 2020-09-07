using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class PreloadSceneManager : MonoBehaviour
{
	private Dictionary<string, AsyncOperation> _sceneName_sceneLoadAsyncOperation = new Dictionary<string, AsyncOperation>();

	public AsyncOperation PreloadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
	{
		if (this._sceneName_sceneLoadAsyncOperation.TryGetValue(key: sceneName, value: out AsyncOperation loadSceneAsyncOperation))
			return loadSceneAsyncOperation;

		loadSceneAsyncOperation = SceneManager.LoadSceneAsync(
			sceneName: sceneName,
			mode: loadSceneMode
		);

		loadSceneAsyncOperation.allowSceneActivation = false;

		this._sceneName_sceneLoadAsyncOperation.Add(
			key: sceneName,
			value: loadSceneAsyncOperation
		);
		
		return loadSceneAsyncOperation;
	}

	public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
	{
		if (this._sceneName_sceneLoadAsyncOperation.TryGetValue(key: sceneName, value: out AsyncOperation loadSceneAsyncOperation))
		{
			loadSceneAsyncOperation.allowSceneActivation = true;

			return loadSceneAsyncOperation;
		}
		
		loadSceneAsyncOperation = SceneManager.LoadSceneAsync(
			sceneName: sceneName,
			mode: loadSceneMode
		);
		
		return loadSceneAsyncOperation;
	}

	private Dictionary<string, AsyncOperationHandle<SceneInstance>> _sceneName_sceneLoadAsyncOperationHandle = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

	public AsyncOperationHandle<SceneInstance> AddressablesPreloadSceneAsync(string key, LoadSceneMode loadSceneMode)
	{
		if (this._sceneName_sceneLoadAsyncOperationHandle.TryGetValue(key: key, value: out AsyncOperationHandle<SceneInstance> loadSceneAsyncOperationHandle))
			return loadSceneAsyncOperationHandle;

		loadSceneAsyncOperationHandle = Addressables.LoadSceneAsync(
			key: key,
			loadMode: loadSceneMode,
			activateOnLoad: false
		);
		
		this._sceneName_sceneLoadAsyncOperationHandle.Add(
			key: key,
			value: loadSceneAsyncOperationHandle
		);

		return loadSceneAsyncOperationHandle;
	}

	public AsyncOperationHandle<SceneInstance> AddressablesLoadSceneAsync(string key, LoadSceneMode loadSceneMode)
	{
		if (this._sceneName_sceneLoadAsyncOperationHandle.TryGetValue(key: key, value: out AsyncOperationHandle<SceneInstance> loadSceneAsyncOperationHandle))
		{
			loadSceneAsyncOperationHandle.Result.ActivateAsync();

			return loadSceneAsyncOperationHandle;
		}

		loadSceneAsyncOperationHandle = Addressables.LoadSceneAsync(
			key: key,
			loadMode: loadSceneMode,
			activateOnLoad: true
		);

		return loadSceneAsyncOperationHandle;
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(target: this.gameObject);
	}
}