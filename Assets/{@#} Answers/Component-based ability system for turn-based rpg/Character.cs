using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] private string _name = "Default Character Name";
	public string _Name => this._name;

	[SerializeField] private int _level = 5;
	public int _Level => this._level;

	[SerializeField] private float _maxHealth = 150.0f;
	public float _MaxHealth => this._maxHealth;

	private float _health = 100.0f;
	public float _Health => this._health;

	[SerializeField] private bool _ally = true;
	public bool _Ally => this._ally;

	public void Heal(float amount)
	{
		this._health += amount;
		if (this._health > this._maxHealth)
			this._health = this._maxHealth;

		Debug.Log($"[Character] {this._name}: healed - {amount} | health - {this._health}");
	}

	[SerializeField] private FunctionalConditionsGroup _levelRelatedFunctionalConditionsGroup;
	public FunctionalConditionsGroup _LevelRelatedFunctionalConditionsGroup => this._levelRelatedFunctionalConditionsGroup;

	private void Awake()
	{
		// In our fictional world a character can only be healed if its level is less than 10.
		this._levelRelatedFunctionalConditionsGroup.Add(() => this._level < 10);
	}
}