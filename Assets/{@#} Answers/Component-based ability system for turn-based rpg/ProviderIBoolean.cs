using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[Provider IBoolean]", menuName = "[Providers]/[Provider IBoolean]")]
public class ProviderIBoolean : ScriptableObject, IProvider<IBoolean[]>
{
	[SerializeField] private Boolean[] _booleans;
	public Boolean[] _Booleans => this._booleans;

	[SerializeField] private Condition[] _conditions;
	public Condition[] _Conditions => this._conditions;

	public IBoolean[] Provide()
	{
		IBoolean[] booleans = new IBoolean[this._booleans.Length + this._conditions.Length];

		int a = 0;

		this._booleans.CopyTo(booleans, a);
		a += this._booleans.Length;

		this._conditions.CopyTo(booleans, a);
		a += this._conditions.Length;

		return booleans;
	}
}