using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "[Action]", menuName = "[Actions]/[Action]")]
public abstract class Action : ScriptableObject
{
	public abstract void Execute(object target);
}