using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CoinsManager : MonoBehaviour 
{
	public event Action<int, int> 	OnUpdateCoinsCollected;
	public event Action<Coin>		OnCollectCoin;

	public List<Coin>	coins;
	[SerializeField]
	private List<Coin>	_coinsToRemove;
	public GameObject 	coinPrefab;
	public Transform 	coinContainer;

	public int coinsOnStage;
	public int coinsCollected;
	[SerializeField]
	private int _coinIndexCount;
	void Start()
	{
		coins = new List<Coin> ();
		_coinsToRemove = new List<Coin> ();
		coinsOnStage = 0;
		coinsCollected = 0;
		_coinIndexCount = -1;
	}
	public void LoadCoin(int p_coinIndex)
	{
		coinsOnStage++;
		_coinIndexCount++;
		if (PrefsUtil.GetCoinCollected (GameSceneManager.currentLevelIndex, _coinIndexCount)) 
		{
			coinsCollected++;
			return;
		}
		
		TupleInt __pos = TupleInt.IndexToTuple (p_coinIndex, GridManager.gridSize);

		GameObject __coin = (GameObject)Instantiate (coinPrefab);
		__coin.name = "Coin";
		__coin.transform.parent = coinContainer;
		__coin.transform.localPosition = new Vector3 ((__pos.Item1 * 2f) - GridManager.gridSize.Item1 + 1f,
			(__pos.Item2 * -2f) + (GridManager.gridSize.Item2 - 1f));

		coins.Add (__coin.GetComponent<Coin> ());
		coins[coins.Count - 1].gridPos = new TupleInt(__pos.Item1, __pos.Item2);
		coins [coins.Count - 1].index = _coinIndexCount;

		__coin.GetComponent<SpriteRenderer>().sortingOrder = 
			Mathf.RoundToInt (((__pos.Item2 * -2f) + (GridManager.gridSize.Item2 - 1f)) * -10f) - 3;

	}

	public void CheckPlayerGotCoin(TupleInt p_playerPos)
	{
		//Check For collisions with player
		foreach (Coin __coin in coins)
			if (__coin.gameObject.activeSelf && __coin.gridPos.Equals (p_playerPos))
				CollectCoin (__coin);
	}

	private void CollectCoin(Coin p_coin)
	{
		//Set coin collected
		coinsCollected++;
		CallUpdateCoinsLabel ();
		OnCollectCoin (p_coin);
		p_coin.gameObject.SetActive (false);
		//Add coin to the remove list
		_coinsToRemove.Add (p_coin);
		//SFX
		SoundManager.GetInstance ().PlayEndOfLevelSFX ();
	}

	public void CallUpdateCoinsLabel()
	{
		if (OnUpdateCoinsCollected != null)
			OnUpdateCoinsCollected (coinsCollected, coinsOnStage);
	}
	public void SaveCollectedCoins()
	{
		foreach (Coin __coin in _coinsToRemove)
			PrefsUtil.SetCoinCollected (GameSceneManager.currentLevelIndex, __coin.index);
		_coinsToRemove.Clear ();
	}
}
