using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupAnim : MonoBehaviour {

	void Start () {
		if (gameObject.tag.Equals("Candy"))
		{
			StartCoroutine(CandyMovement());
		}
		else if (gameObject.tag.Equals("Giftbox"))
		{
			StartCoroutine(GiftBoxMovement());
		}
		else if (gameObject.tag.Equals("Chest"))
		{
			StartCoroutine(ChestMovement());
		}
	}
	
	void Update () {
		
	}

	IEnumerator CandyMovement()
	{
		while (isActiveAndEnabled)
		{
			gameObject.transform.Rotate(0, 0, -1f);
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator GiftBoxMovement()
	{
		while (isActiveAndEnabled)
		{
			for (int i = 0; i < 20; i++)
			{
				gameObject.transform.Translate(0, 0.0125f, 0);
				yield return new WaitForEndOfFrame();
			}
			for (int i = 0; i < 20; i++)
			{
				gameObject.transform.Translate(0, -0.0125f, 0);
				yield return new WaitForEndOfFrame();
			}
		}
	}

	IEnumerator ChestMovement()
	{	
		//Nothing yet. Might not program the chest, it wouldn't make sense!
		yield return new WaitForEndOfFrame();
	}
}
