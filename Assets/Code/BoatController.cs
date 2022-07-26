using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
	public float speed;
	public Transform motor;
	public float maxMotorAngle;
	public JohnBuoyancy Buoyancy;
	public Rigidbody Rigidbody;

	//John
	Vector2 inputMovement;
	Vector3 motorPos;
	Vector3 offset = new Vector3(0, 0.25f, 0);

	// Update is called once per frame
	void Update()
	{
		inputMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		if(inputMovement != Vector2.zero)
        {
			motorPos = motor.position;
			// Turning
			motor.rotation = transform.rotation * Quaternion.Euler(0, -inputMovement.x * maxMotorAngle, 0);

			// HACK: Forward motor
			if (motorPos.y < Buoyancy.waterLineHack)
			{
				//			Rigidbody.AddRelativeForce(0,0,speed * (underwaterVerts / (float)totalVerts));
				Rigidbody.AddForceAtPosition(motor.forward * speed * Time.deltaTime * inputMovement.y,
					motorPos + offset); // HACK: Offset hack to stop the boat flipping all the time!
			}
		}
		
	}
}
