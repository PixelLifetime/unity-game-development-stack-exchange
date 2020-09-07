using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadSceneInUnity : MonoBehaviour
{
	[SerializeField] private string _sceneName = "maingame";
	public string _SceneName => this._sceneName;

	private AsyncOperation _asyncOperation;

	private IEnumerator LoadSceneAsyncProcess(string sceneName)
	{
		// Begin to load the Scene you have specified.
		this._asyncOperation = SceneManager.LoadSceneAsync(sceneName);

		// Don't let the Scene activate until you allow it to.
		this._asyncOperation.allowSceneActivation = false;

		while (!this._asyncOperation.isDone)
		{
			Debug.Log($"[scene]:{sceneName} [load progress]: {this._asyncOperation.progress}");

			yield return null;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) && this._asyncOperation == null)
		{
			Debug.Log("Started Scene Preloading");

			// Start scene preloading.
			this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: this._sceneName));
		}

		// Press the space key to activate the Scene.
		if (Input.GetKeyDown(KeyCode.Space) && this._asyncOperation != null)
		{
			Debug.Log("Allowed Scene Activation");

			this._asyncOperation.allowSceneActivation = true;
		}
	}
}