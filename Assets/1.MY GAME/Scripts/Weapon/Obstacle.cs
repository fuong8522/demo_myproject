using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Rigidbody body;
    public BoxCollider box;
    public MeshRenderer material;
    public Material gold;
    public Material red;
    public Material green;
    public bool checkRed = false;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        transform.forward = MovementPlayer.instance.transform.forward;
        transform.SetParent(SpawnManager.instance.transform, false);
    }
    void Update()
    {
        if(MovementPlayer.instance.checkCreate)
        {
            body.isKinematic = true;
            material.material = gold;
            this.gameObject.name = "Obstacle";
            box.isTrigger = false;


        }
        if (!body.isKinematic )
        {
            Vector3 playerPosition = MovementPlayer.instance.transform.position;
            Vector3 playerForward = MovementPlayer.instance.transform.forward;

            float distanceFromPlayer = 3f;
            Vector3 objectPosition = playerPosition + playerForward * distanceFromPlayer;
            transform.position = objectPosition;
            transform.forward = MovementPlayer.instance.transform.forward;

        }
        if(body.isKinematic)
        {
            material.material = gold;
        }
        if(material.material == red)
        {
            checkRed = true;
        }

    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (material.material == gold)
            {
            }
            else
            {
                material.material = red;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && material.material != gold)
        {
            material.material = green;
        }
    }


}
