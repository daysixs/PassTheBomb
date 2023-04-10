using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class BombPassing : NetworkBehaviour
{
    [SerializeField] private GameObject bombPrefab;

    [SerializeField] private CinemachineVirtualCamera cam;
    //public Camera cam;

    public float range = 100f;
    public Vector3 offset;

    [Command] 
    public void CmdPassBomb(NetworkIdentity bomb, NetworkIdentity player)
    {

        bomb.RemoveClientAuthority();
        bomb.AssignClientAuthority(player.connectionToClient);

    }

    [ClientRpc]
    private void Spawn()
    {
        GameObject bomb = Instantiate(bombPrefab);
        NetworkServer.Spawn(bomb, connectionToClient);
    }

    public override void OnStartAuthority()
    {
        if(isLocalPlayer)
        {
            cam.gameObject.SetActive(true);
        }

        if(isServer)
        {
            Spawn();
            Debug.Log("You got bomb");
        }
        
    }


    [ClientCallback]
    private void Update()
    {
        
        if (!isOwned) 
        {
            return;
        }


        //if (bomb.isOwned)
        //{
        //    bomb.gameObject.SetActive(true);
        //}
        //else
        //{
        //    bomb.gameObject.SetActive(false);
        //}

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * range);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                if (hit.collider.TryGetComponent(out NetworkIdentity targetPlayer))
                {
                    Debug.Log(hit.collider.name + " network identity" + targetPlayer.netId);
                    //CmdPassBomb(bomb, targetPlayer);
                }

            }

        }
    }

}
