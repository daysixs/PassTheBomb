using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class BombPassing : NetworkBehaviour
{
    [SerializeField] private GameObject bombPrefab;

    [SerializeField] private CinemachineVirtualCamera cam;


    private NetworkIdentity bombId;

    public float range = 100f;
    public Vector3 offset;

    [Command] 
    public void CmdPassBomb(NetworkIdentity bomb, NetworkIdentity player)
    {
        
        bomb.RemoveClientAuthority();
        bomb.AssignClientAuthority(player.connectionToClient);
        bombId = bomb;
        Debug.Log("THE BOMB IS ON "+ bombId.connectionToClient.identity.netId);
    }



    [Command]
    private void Spawn()
    {
        GameObject bomb = Instantiate(bombPrefab, transform.position + offset, Quaternion.identity);
        NetworkServer.Spawn(bomb,connectionToClient);
        bombId = bomb.GetComponent<NetworkIdentity>();   
    }

    
    public override void OnStartAuthority()
    {
        if(isLocalPlayer)
        {
            cam.gameObject.SetActive(true);
        }

        if (isServer)
        {
            Spawn();
        }

    }


    [ClientCallback]
    private void Update()
    {
        
        if (!isOwned) 
        {
            return;
        }

        if(bombId == null)
        {
            Debug.Log("BOMB ID NULL");
            return;
        }

        if (!bombId.isOwned)
        {
            Debug.Log("You got no bomb");
            return;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * range);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                if (hit.collider.TryGetComponent(out NetworkIdentity targetPlayer))
                {
                    Debug.Log("Bomb" + bombId + "Pass to" + hit.collider.name + " network identity" + targetPlayer.netId);
                    bombId.gameObject.transform.position = targetPlayer.gameObject.transform.position + new Vector3(offset.x, offset.y, -offset.z);
                    CmdPassBomb(bombId, targetPlayer);

                }
            }

        }



    }

}
