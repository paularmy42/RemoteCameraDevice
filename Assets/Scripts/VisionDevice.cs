using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.Events;

public class VisionDevice : MonoBehaviour
{
    public GameObject cameraScreen;
    private Transform cameraRig;
    private Transform remoteCamera;
    private GameObject clonedScreen;

    private Rigidbody deviceRB;
    private float deviceSpeed;
    // Use this for initialization
    void Start ()
    {
        deviceRB = this.GetComponent("Rigidbody") as Rigidbody;
        //Get Camera Rig
        cameraRig = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset);
        //Spawn camera plane to position in front of camera rig
        //if (GameObject.Find(cameraScreen.name + "(Clone)") == null)
        //{
        //    clonedScreen = Instantiate(cameraScreen, cameraRig.position + cameraRig.transform.forward * 0.2f, cameraRig.rotation);
        //    clonedScreen.transform.SetParent(cameraRig);
        //}
        //remoteCamera = transform.Find("RemoteCamera");
        //Debug.Log(remoteCamera);

    }

    // Update is called once per frame
    void Update ()
    {
        if (deviceRB.IsSleeping())
        {
            if (GameObject.Find(cameraScreen.name + "(Clone)") == null)
            {
                clonedScreen = Instantiate(cameraScreen, cameraRig.position + cameraRig.transform.forward * 0.2f, cameraRig.rotation);
                clonedScreen.transform.SetParent(cameraRig);
            }
            remoteCamera = transform.Find("RemoteCamera");
        }
        remoteCamera.rotation = cameraRig.rotation;
	}

    private void OnDestroy()
    {
        Destroy(clonedScreen);
    }
}
