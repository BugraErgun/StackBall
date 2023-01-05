using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPartController : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private StackController stackController;
    private Collider col;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        stackController = transform.parent.GetComponent<StackController>();
        col = GetComponent<Collider>();

    }

    public void Shatter()
    {
        rb.isKinematic = false;
        col.enabled = false;

        Vector3 forcePoint = transform.parent.position;
        float parentXpos = transform.position.x;
        float xPos = meshRenderer.bounds.center.x;

        Vector3 subDirection = (parentXpos - xPos < 0) ? Vector3.right : Vector3.left;
        //if (parentXpos-xPos<0)
        //{
        //    subDirection = Vector3.right;
        //}
        //else
        //{
        //    subDirection = Vector3.left;
        //}

        Vector3 dir = (Vector3.up * 1.5f + subDirection).normalized;

        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);

        rb.AddForceAtPosition(dir * force, forcePoint, ForceMode.Impulse);
        rb.AddTorque(Vector3.left * torque);
        rb.velocity = Vector3.down;
    }

    public void RemoveAllChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(null);
            i--;
        }
    }
}
