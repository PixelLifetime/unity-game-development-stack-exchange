using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[Boolean]", menuName = "[Primitive Types]/[Boolean]")]
public class Boolean : ScriptableObject, IBoolean
{
	[SerializeField] private bool _value;
	public bool Value
	{
		get => this._value;
		set => this._value = value;
	}
	public bool _Value => this._value;
}