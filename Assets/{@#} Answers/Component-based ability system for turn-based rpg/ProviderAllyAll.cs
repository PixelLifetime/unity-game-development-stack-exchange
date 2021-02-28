using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "[Provider Ally All]", menuName = "[Providers]/[Provider Ally All]")]
public class ProviderAllyAll : Provider
{
	public override object Provide() => Object.FindObjectsOfType<Character>().Where((character) => character._Ally).ToArray();
}