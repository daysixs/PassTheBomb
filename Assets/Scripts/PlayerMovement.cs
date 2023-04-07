using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;

    private Camera mainCamera;

    #region server

    [Command]
    private void CmdMove(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            return;
        }
        agent.SetDestination(hit.position);
    }

    #endregion server

    #region client

    //start method for the client who owns the object
    public override void OnStartAuthority()
    {
        mainCamera = Camera.main; // camera reference
    }

    [ClientCallback] // makes it a client only update (all clients)
    private void Update()
    {
        // make sure object belongs to the client
        if (!isOwned) // if(!hasAuthority) is the old function
        {
            return;
        }

        // check the right mouse buton input
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        // grab mouse cursor information
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // grab the scene where it is hit
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return;
        }

        CmdMove(hit.point);
    }

    #endregion client
}