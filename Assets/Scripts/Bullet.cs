using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
		public GameObject explosion;
		// Use this for initialization
		void Start ()
		{
			
				Destroy (gameObject, 2);
		}
	
		// Update is called once per frame
		void OnTriggerEnter2D (Collider2D other)
		{

				if (other.tag == "Dummy") {
						Destroy (gameObject);
						Debug.Log ("Hit");
				}
		}
}
