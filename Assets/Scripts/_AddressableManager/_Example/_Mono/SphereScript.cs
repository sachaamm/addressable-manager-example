using UnityEngine;

public class SphereScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().material.color = StaticReferences.MyColor.color;
        var go = Instantiate(StaticReferences.MyPrefab.myPrefab);
        go.transform.position = transform.position + Vector3.left * 2;
    }
    
}
