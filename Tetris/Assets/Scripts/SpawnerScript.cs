using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] groups;
    public int i;

    public GameObject createGroup(Vector3 v)
    {
        GameObject group = Instantiate(groups[i], transform.position, Quaternion.identity);
        return group;
    }

    public void spawnNext()
    {
        createGroup(transform.position);
        i = Random.Range(0, groups.Length);
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnNext();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
