using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.Events;

public class TeleportEvent : UnityEvent<Transform>
{
    //Do stuff here
}

public class DeployCamera : MonoBehaviour
{

    public VRTK_InteractableObject linkedObject;
    public VRTK_ControllerEvents controllerEvents;
    public GameObject projectile;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 100f;
    public float projectileLife = 10f;

    public VRTK_BasicTeleport teleporter;

    private float timeScale = 0.2f;

    protected virtual void OnEnable()
    {
        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);
        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed += InteractableObjectUsed;
        }
        //controllerEvents = (controllerEvents == null ? GetComponent<VRTK_ControllerEvents>() : controllerEvents);
        //if (controllerEvents == null)
        //{
        //    VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
        //    return;
        //}
        //controllerEvents.TriggerClicked += DoTriggerClicked;
    }

    protected virtual void OnDisable()
    {
        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
        }
        //if (controllerEvents != null)
        //{
        //    controllerEvents.TriggerClicked -= DoTriggerClicked;
        //}
    }

    //private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    //{
    //    Debug.Log("Trigger clicked");
    //    FireProjectile();
    //}

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        if (GameObject.Find(projectile .name + "(Clone)") == null)
        {
            FireProjectile();
        }
        else
        {
            //lerp scale camera view object
            Transform cameraPlane = GameObject.FindGameObjectWithTag("CameraPlane").transform;
            Debug.Log(string.Format("Camera Plane Object is {0}", cameraPlane));
            //LerpUp(cameraPlane);
            var destination = GameObject.Find(projectile.name + "(Clone)");
            //Debug.Log(string.Format("Destination is {0}", destination));
            teleporter.ForceTeleport(destination.transform.position, null);
            Destroy(destination);
        }
    }

    public void LerpUp(Transform objTransform)
    {
        Debug.Log(string.Format("Start Lerping: {0}", objTransform.localScale));
        float progress = 0;
        Vector3 finalScale = new Vector3(0.3f, 0.3f, 1.0f);
        Vector3 startingScale = objTransform.localScale;
        while (progress <= 1)
        {
            objTransform.localScale = Vector3.Lerp(startingScale, finalScale, progress);
            progress += Time.deltaTime * timeScale;
            Debug.Log("Lerping " + progress);
            //yield return null;
        }
        objTransform.localScale = finalScale;
        Debug.Log(string.Format("Finished Lerping: {0}", objTransform.localScale));

    }

    protected virtual void FireProjectile()
    {
        if (projectile != null && projectileSpawnPoint != null)
        {
            GameObject clonedProjectile = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Rigidbody projectileRigidbody = clonedProjectile.GetComponent<Rigidbody>();
            float destroyTime = 0f;
            if (projectileRigidbody != null)
            {
                projectileRigidbody.AddForce(clonedProjectile.transform.forward * projectileSpeed);
                Debug.Log(string.Format("Projectile: {0}, Prefab: {1}", projectileRigidbody, clonedProjectile));
                destroyTime = projectileLife;
                Destroy(clonedProjectile, destroyTime);
            }
        }
    }
}
