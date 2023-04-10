using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBillboarding : MonoBehaviour
{
    [Header("Billboard Condition")]
    public bool useStaticBillboard;

    private Camera playerCam;

    private void Start()
    {
        playerCam = Camera.main;
    }
    void LateUpdate()
    {
        if (!useStaticBillboard)
            transform.LookAt(playerCam.transform);
        else
            transform.rotation = playerCam.transform.rotation;

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
