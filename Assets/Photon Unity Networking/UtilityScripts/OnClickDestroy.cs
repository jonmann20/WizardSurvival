using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class OnClickDestroy : Photon.MonoBehaviour
{
    public bool DestroyByRpc;

    void OnClick()
    {
        if (!DestroyByRpc)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
        {
            this.photonView.RPC("DestroyRpc", PhotonTargets.AllBuffered);
        }
    }

    [RPC]
    public void DestroyRpc()
    {
        GameObject.Destroy(this.gameObject);
        PhotonNetwork.UnAllocateViewID(this.photonView.viewID);
    }
}
