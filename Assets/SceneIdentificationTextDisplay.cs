using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneIdentificationTextDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _sceneNameTextField;
	public TextMeshProUGUI _SceneNameTextField => this._sceneNameTextField;

	private void Awake()
	{
		this._sceneNameTextField.text = SceneManager.GetActiveScene().name;
	}

#if UNITY_EDITOR
	private void Reset()
	{
		this._sceneNameTextField = this.GetComponent<TextMeshProUGUI>();
	}
#endif
}