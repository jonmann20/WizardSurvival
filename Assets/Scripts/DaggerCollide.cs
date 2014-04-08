using UnityEngine;
using System.Collections;

public class DaggerCollide : MonoBehaviour {

	public GameObject thisHolder;

	void OnTriggerStay(Collider coll)
	{
		/*if(coll.collider.gameObject.tag == "Player")
		{
			if(coll.collider.gameObject.transform.parent.GetComponent<PhotonView>().isMine)
			{
				if( thisHolder.transform.GetComponent<FollowClosestPlayer>().attacking == true )
				{
					//print ("Giving damage");
					coll.collider.gameObject.transform.parent.GetComponent<PlayerController>().TakeDamage(20, transform);
				}
				
			}
		}*/
	}
}
