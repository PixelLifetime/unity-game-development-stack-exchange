using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProvider<TValue>
{
	TValue Provide();
}