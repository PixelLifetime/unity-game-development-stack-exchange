
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[Heal Action]", menuName = "[Actions]/[Heal Action]")]
public class HealAction : Action
{
	[SerializeField] private float _healAmount;
	public float _HealAmount => this._healAmount;

	public override void Execute(object target)
	{
		Character character = (Character)target;

		// Here we check specific conditions for characters.
		if (character._LevelRelatedFunctionalConditionsGroup._Value)
			character.Heal(this._healAmount);
	}
}