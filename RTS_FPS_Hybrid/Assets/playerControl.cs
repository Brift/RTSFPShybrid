using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour 
{
	public string playerName;
	
	public int team;
	//public int inGameID;
	public int health;
	public int healthRegen;
	//public int armor;
	public int attackDamage;
	public float attackSpeed;
	//public int points;
	//public int tokens;
	public float movementSpeed;
	
	
	
	void Start()
	{
		
	}
	
	void FixedUpdate () 
	{
		//float moveHorizontal = Input.GetAxis("Horizontal");
		//float moveVertical = Input.GetAxis("Vertical");
		
		float moveVertical = Input.GetAxis("Mouse ScrollWheel");
		
		Vector3 direction = new Vector3(0.0f,(moveVertical*(-10)),0.0f);
		
		if(Input.GetKey("d"))
		{
			direction += new Vector3(1.0f,0.0f,0.0f);
		}
		if(Input.GetKey("a"))
		{
			direction += new Vector3(-1.0f,0.0f,0.0f);
		}if(Input.GetKey("w"))
		{
			direction += new Vector3(0.0f,0.0f,1.0f);
		}if(Input.GetKey("s"))
		{
			direction += new Vector3(0.0f,0.0f,-1.0f);
		}
		
		rigidbody.AddForce(direction * movementSpeed * Time.deltaTime);
		
	}
}