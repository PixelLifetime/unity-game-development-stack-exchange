using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "[Functional Conditions Group]", menuName = "[Conditions]/[Functional Conditions Group]")]
public class FunctionalConditionsGroup : Condition
{
	[NonSerialized] private List<Func<bool>> _functionalConditions = new List<Func<bool>>();

	public void Add(Func<bool> functionalCondition) => this._functionalConditions.Add(functionalCondition);
	public void Remove(Func<bool> functionalCondition) => this._functionalConditions.Remove(functionalCondition);

	public void Clear() => this._functionalConditions.Clear();

	public override bool _Value
	{
		get
		{
			for (int a = 0; a < this._functionalConditions.Count; a++)
			{
				if (!this._functionalConditions[a].Invoke())
					return false;
			}

			return true;
		}
	}
}