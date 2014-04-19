using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]
public class SyncScript : Photon.MonoBehaviour
{
	private Vector3 latestCorrectPos;
	private Vector3 onUpdatePos;
	private Quaternion latestCorrectRot;
	private Quaternion onUpdateRot;
	private Vector3 latestCorrectScale;
	private Vector3 onUpdateScale;
	private float fraction;

	public bool debug = false;

	public void Awake()
	{
		if (photonView.isMine)
		{
			this.enabled = false;   // due to this, Update() is not called on the owner client.
		}
		
		latestCorrectPos = transform.position;
		latestCorrectRot = transform.rotation;
		latestCorrectScale = transform.localScale;
		onUpdatePos = transform.position;
		onUpdateRot = transform.rotation;
		onUpdateScale = transform.localScale;
	}
	
	/// <summary>
	/// While script is observed (in a PhotonView), this is called by PUN with a stream to write or read.
	/// </summary>
	/// <remarks>
	/// The property stream.isWriting is true for the owner of a PhotonView. This is the only client that
	/// should write into the stream. Others will receive the content written by the owner and can read it.
	/// 
	/// Note: Send only what you actually want to consume/use, too!
	/// Note: If the owner doesn't write something into the stream, PUN won't send anything.
	/// </remarks>
	/// <param name="stream">Read or write stream to pass state of this GameObject (or whatever else).</param>
	/// <param name="info">Some info about the sender of this stream, who is the owner of this PhotonView (and GameObject).</param>
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		//timeout_timer = TIMEOUT_MAX;

		if (stream.isWriting)
		{
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			Vector3 scale = transform.localScale;

			print("sending pos: " + pos.ToString() + " rot: " + rot.ToString() + " scale: " + scale.ToString() + "rand: " + Random.Range(0.1f, 100.0f));

			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			stream.Serialize(ref scale);
		}
		else
		{
			// Receive latest state information
			Vector3 pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			Vector3 scale = Vector3.zero;

			print("receiving pos: " + pos.ToString() + " rot: " + rot.ToString() + " scale: " + scale.ToString() + "rand: " + Random.Range(0.1f, 100.0f));

			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			stream.Serialize(ref scale);

			if(debug)
			{
				print("pos: " + pos + " rot: " + rot + " scale: " + scale + "rand: " + Random.Range(0.1f, 5.0f));
			}

			latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
			latestCorrectRot = rot;
			latestCorrectScale = scale;

			onUpdatePos = transform.position;  // we interpolate from here to latestCorrectPos
			onUpdateRot = transform.rotation;
			onUpdateScale = transform.localScale;
			fraction = 0;                           // reset the fraction we alreay moved. see Update()
		}
	}
	
	public void Update()
	{
		// We get 10 updates per sec. sometimes a few less or one or two more, depending on variation of lag.
		// Due to that we want to reach the correct position in a little over 100ms. This way, we usually avoid a stop.
		// Lerp() gets a fraction value between 0 and 1. This is how far we went from A to B.
		//
		// Our fraction variable would reach 1 in 100ms if we multiply deltaTime by 10.
		// We want it to take a bit longer, so we multiply with 9 instead.

		fraction = fraction + Time.deltaTime * 9;
		transform.position = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction);    // set our pos between A and B
		transform.rotation = Quaternion.Lerp(onUpdateRot, latestCorrectRot, fraction);
		transform.localScale = Vector3.Lerp(onUpdateScale, latestCorrectScale, fraction);
	}
}
