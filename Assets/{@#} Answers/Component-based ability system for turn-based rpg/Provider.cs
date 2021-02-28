using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Provider : ScriptableObject, IProvider<object>
{
	public abstract object Provide();
}