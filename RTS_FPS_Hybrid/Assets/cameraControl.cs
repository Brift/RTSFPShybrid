using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour 
{
	public GameObject playerDrone;
	private Vector3 initialOffset;
	
	
	
	// Use this for initialization
	void Start() 
	{
		initialOffset = transform.position - playerDrone.transform.position;
	}
	
	
	
	// Update is called once per frame
	void LateUpdate() 
	{
		transform.position = playerDrone.transform.position + initialOffset;
	}
}