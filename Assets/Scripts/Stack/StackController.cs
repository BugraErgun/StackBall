using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField]
    private StackPartController[] stackPartControllers;

    public void ShatterAllParts()
    {
        if (transform.parent !=null)
        {
            transform.parent = null;
            FindObjectOfType<Player>().IncreaseBrokenStacks();
        }
        foreach (StackPartController item in stackPartControllers)
        {
            item.Shatter();
        }
        StartCoroutine(RemoveParts());
    }

    IEnumerator RemoveParts()
    {
        yield return new WaitForSeconds(1f);
        
        Destroy(gameObject);

    }
}
