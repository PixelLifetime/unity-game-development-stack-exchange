using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "[Condition]", menuName = "[Conditions]/[Condition]")]
public abstract class Condition : ScriptableObject, IBoolean
{
	public abstract bool _Value { get; }
}