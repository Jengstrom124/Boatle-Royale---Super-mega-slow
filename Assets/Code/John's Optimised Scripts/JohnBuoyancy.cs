using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Cams mostly hack buoyancy
public class JohnBuoyancy : MonoBehaviour
{
	public float splashVelocityThreshold;
	public float forceScalar;
	public float waterLineHack; // HACK

	public int underwaterVerts;
	public float dragScalar;

	public static event Action<GameObject, Vector3, Vector3> OnSplash;
	public static event Action<GameObject> OnDestroyed;

	Vector3 worldVertPos;

	//John Fixes
	public Rigidbody rb;
	public Mesh mesh;
	float totalVertCount;

	Vector3 myTransform;
	Vector3 worldVertPosCal;
	Vector3 forceAmount;
	Vector3 forcePosition;

	public List<Vector3> underwaterVertArray = new List<Vector3>();
	public List<Vector3> underwaterNormArray = new List<Vector3>();

	public bool calculateForces;
	public float forcesUpdateDelay = 0.05f;

	/*public struct Vertex
    {
		public Vector3 pos;
	}*/

    private void Awake()
    {
		totalVertCount = mesh.normals.Length;

        for (var index = 0; index < (int)totalVertCount; index++)
        {
			//if (mesh.vertices[index].y < 0.75f)
			{
				//Preventing duplicate references
				if (!underwaterVertArray.Contains(mesh.vertices[index]))
					underwaterVertArray.Add(mesh.vertices[index]);

				if(!underwaterNormArray.Contains(mesh.normals[index]))
					underwaterNormArray.Add(mesh.normals[index]);
			}
        }
    }

    private void Start()
    {
		//using a coroutine to calculate forces rather then update - as we don't need the accuracy of update
        StartCoroutine(CalculateForcesCoroutine());
    }

    void Update()
	{
		//These checks only need to be made on the gameobject itself, not each individual vertex

		myTransform = transform.position;

        // HACK to remove sunken boats
        if (myTransform.y < waterLineHack - 10f)
		{
			DestroyParentGO();
		}


		//--------------
		//CalculateForces();
	}

	private void FixedUpdate()
	{
		// Drag for percentage underwater
		if(underwaterVerts > 0)
        {
			rb.drag = (underwaterVerts / totalVertCount) * dragScalar;
			rb.angularDrag = (underwaterVerts / totalVertCount) * dragScalar;
        }
	}

    /*void CalculateForces()
    {
		underwaterVerts = 0;
		for (var index = 0; index < underwaterVertArray.Count; index++)
		{
			worldVertPosCal = transform.TransformDirection(underwaterVertArray[index]);
			worldVertPos = myTransform + worldVertPosCal;
			if (worldVertPos.y < waterLineHack)
			{
				// Splashes only on surface of water plane
				if (worldVertPos.y > waterLineHack - 0.1f)
				{
					if (rb.velocity.magnitude > splashVelocityThreshold || rb.angularVelocity.magnitude > splashVelocityThreshold)
					{
						//print(rb.velocity.magnitude);
						if (OnSplash != null)
						{
							OnSplash.Invoke(gameObject, worldVertPos, rb.velocity);
						}
					}
				}
				forceAmount = (transform.TransformDirection(-underwaterNormArray[index]) * forceScalar) * Time.deltaTime;
				forcePosition = myTransform + worldVertPosCal;
				rb.AddForceAtPosition(forceAmount, forcePosition, ForceMode.Force);
				underwaterVerts++;
			}
		}
	}*/

    private IEnumerator CalculateForcesCoroutine()
    {
        do
        {
            underwaterVerts = 0;
            for (var index = 0; index < underwaterVertArray.Count; index++)
            {
                worldVertPosCal = transform.TransformDirection(underwaterVertArray[index]);
                worldVertPos = myTransform + worldVertPosCal;
                if (worldVertPos.y < waterLineHack)
                {
                    // Splashes only on surface of water plane
                    if (worldVertPos.y > waterLineHack - 0.1f)
                    {
                        if (rb.velocity.magnitude > splashVelocityThreshold || rb.angularVelocity.magnitude > splashVelocityThreshold)
                        {
                            //print(rb.velocity.magnitude);
                            if (OnSplash != null)
                            {
                                OnSplash.Invoke(gameObject, worldVertPos, rb.velocity);
                            }
                        }
                    }
                    forceAmount = (transform.TransformDirection(-underwaterNormArray[index]) * forceScalar) * Time.deltaTime;
                    forcePosition = myTransform + worldVertPosCal;
                    rb.AddForceAtPosition(forceAmount, forcePosition, ForceMode.Force);
                    underwaterVerts++;
                }
            }

            yield return new WaitForSeconds(forcesUpdateDelay);
        }
        while (calculateForces);
    }

    private void DestroyParentGO()
	{
		if (OnDestroyed != null)
		{
			OnDestroyed.Invoke(gameObject);
		}
		Destroy(gameObject);
	}
}
