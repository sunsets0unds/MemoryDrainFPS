using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawn : MonoBehaviour
{
    public float waitTime;
    public GameObject boxType;
    private GameObject currentBox;
    private bool respawning = false;

    // Start is called before the first frame update
    void Start()
    {
        SpawnBox();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBox == null && respawning == false)
        {
            StartCoroutine(RespawnBox());
        }
    }

    void SpawnBox()
    {
        currentBox = Instantiate(boxType, this.transform);
    }

    IEnumerator RespawnBox()
    {
        respawning = true;
        yield return new WaitForSeconds(waitTime);
        SpawnBox();
        respawning = false;
    }
}
