using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "[Ability]", menuName = "[Ability System]/[Ability]")]
public class Ability : ScriptableObject
{
	[Serializable]
	public class Data
	{
		[SerializeField] private Provider _targetsProvider;
		public Provider _TargetsProvider => this._targetsProvider;

		[SerializeField] private ProviderIBoolean _actionsExecutionConditionsProvider;
		public ProviderIBoolean _ActionsExecutionConditionsProvider => this._actionsExecutionConditionsProvider;

		// You don't necessary need to cache these, I am just doing that for the purpose of performance, but it should be negligible, so I am just micro-optimizing at this point.
		// It's more convenient to not cache these in case your provider can change conditions during runtime.
		private IBoolean[] _actionsExecutionConditions;
		public IBoolean[] _ActionsExecutionConditions
		{
			get
			{
				if (this._actionsExecutionConditions == null)
					this._actionsExecutionConditions = this._actionsExecutionConditionsProvider.Provide();

				return this._actionsExecutionConditions;
			}
		}

		public bool _ActionsExecutionConditionsResolvedValue
		{
			get
			{
				for (int a = 0; a < this._ActionsExecutionConditions.Length; a++)
				{
					if (!this._ActionsExecutionConditions[a]._Value)
						return false;
				}

				return true;
			}
		}

		[SerializeField] private Action[] _actions;
		public Action[] _Actions => this._actions;
	}

	[SerializeField] private Data[] _data;
	public Data[] _Data => this._data;

	// Like I didn't actually think through the name of this method.
	// Abilities can have different visual effects, sounds, etc. This goes out of the scope, so I just named it Apply because it's what it does.
	public void Apply()
	{
		for (int a = 0; a < this._data.Length; a++)
		{
			Data data = this._data[a];

			if (data._ActionsExecutionConditionsResolvedValue)
			{
				//Debug.Log($"Provider Result [Targets]: {data._TargetsProvider.Provide()}");

				object[] targets = (object[])data._TargetsProvider.Provide();

				//if (targets == null)
				//	Debug.Log($"Targets: null");
				//else
				//	Debug.Log($"Targets: {targets}");

				for (int b = 0; b < targets.Length; b++)
				{
					for (int c = 0; c < data._Actions.Length; c++)
					{
						data._Actions[c].Execute(targets[b]);
					}
				}
			}
		}
	}
}