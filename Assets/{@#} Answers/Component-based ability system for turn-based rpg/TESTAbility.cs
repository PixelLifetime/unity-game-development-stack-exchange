using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TESTAbility : MonoBehaviour
{
	[SerializeField] private Ability _ability;
	public Ability _Ability => this._ability;

	private void Start()
	{
		this._ability.Apply();
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(TESTAbility)), CanEditMultipleObjects]
	private class TESTAbilityEditor : Editor
	{
		private TESTAbility _target;

		private void OnEnable()
		{
			this._target = (TESTAbility)this.target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Apply Ability"))
				this._target._Ability.Apply();
		}
	}
#endif
}