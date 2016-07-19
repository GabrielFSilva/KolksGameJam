using UnityEngine;
using System.Collections;
using System;

public class CustomCollider : MonoBehaviour 
{
	public event Action <CustomCollider, Collision> onCollisionEnter;
	public event Action <CustomCollider, Collision> onCollisionExit;
	public event Action <CustomCollider, Collision> onCollisionStay;

	public event Action <CustomCollider, Collider> onTriggerEnter;
	public event Action <CustomCollider, Collider> onTriggerExit;
	public event Action <CustomCollider, Collider> onTriggerStay;


	public event Action <CustomCollider, Collision2D> onCollisionEnter2D;
	public event Action <CustomCollider, Collision2D> onCollisionExit2D;
	public event Action <CustomCollider, Collision2D> onCollisionStay2D;

	public event Action <CustomCollider, Collider2D> onTriggerEnter2D;
	public event Action <CustomCollider, Collider2D> onTriggerExit2D;
	public event Action <CustomCollider, Collider2D> onTriggerStay2D;
	//3D
	void OnCollisionEnter(Collision collision) 
	{
		if (onCollisionEnter != null)
			onCollisionEnter(this, collision);		
	}
	void OnCollisionExit(Collision collision) 
	{
		if (onCollisionExit != null)
			onCollisionExit(this, collision);
	}
	void OnCollisionStay(Collision collision) 
	{
		if (onCollisionStay != null)
			onCollisionStay(this, collision);
	}


	void OnTriggerEnter(Collider collider) 
	{
		if (onTriggerEnter != null)
			onTriggerEnter(this, collider);
	}
	void OnTriggerExit(Collider collider) 
	{
		if (onTriggerExit != null)
			onTriggerExit(this, collider);
	}
	void OnTriggerStay(Collider collider) 
	{
		if (onTriggerStay != null)
			onTriggerStay(this, collider);
	}

	//2D
	void OnCollisionEnter2D(Collision2D collision) 
	{
		if (onCollisionEnter2D != null)
			onCollisionEnter2D(this, collision);		
	}
	void OnCollisionExit2D(Collision2D collision) 
	{
		if (onCollisionExit2D != null)
			onCollisionExit2D(this, collision);
	}
	void OnCollisionStay2D(Collision2D collision) 
	{
		if (onCollisionStay2D != null)
			onCollisionStay2D(this, collision);
	}

	void OnTriggerEnter2D(Collider2D collider) 
	{
		if (onTriggerEnter2D != null)
			onTriggerEnter2D(this, collider);
	}
	void OnTriggerExit2D(Collider2D collider) 
	{
		if (onTriggerExit2D != null)
			onTriggerExit2D(this, collider);
	}
	void OnTriggerStay2D(Collider2D collider) 
	{
		if (onTriggerStay2D != null)
			onTriggerStay2D(this, collider);
	}
}
