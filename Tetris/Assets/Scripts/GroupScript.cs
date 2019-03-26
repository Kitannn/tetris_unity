using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupScript : MonoBehaviour
{
    public float lastFall;
    public float lastkey;
    public float timekeydown;

    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.roundVec2(child.position);

            // Not inside Border?
            if (!Grid.insideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < Grid.h; ++y)
            for (int x = 0; x < Grid.w; ++x)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == transform)
                        Grid.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.roundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }

        lastFall = Time.time;
        lastkey = Time.time;
        timekeydown = Time.time;

    }

    void groupfall()
    {
        // Modify position
        transform.position += new Vector3(0, -1, 0);

        // See if valid
        if (isValidGridPos())
        {
            // It's valid. Update grid.
            updateGrid();
        }
        else
        {
            // It's not valid. revert.
            transform.position += new Vector3(0, 1, 0);

            // Clear filled horizontal lines
            Grid.deleteFullRows();

            // Spawn next Group
            FindObjectOfType<SpawnerScript>().spawnNext();

            // Disable script
            enabled = false;
        }

        lastFall = Time.time;
    }

    bool getKey(KeyCode key)
    {
        bool keydown = Input.GetKeyDown(key);
        bool pressed = Input.GetKey(key) && Time.time - lastkey > 0.5f && Time.time - timekeydown > 0.05f;

        if (keydown)
            lastkey = Time.time;

        return keydown || pressed;
    }

    // Update is called once per frame
    void Update()
    {
        //left
        if (getKey(KeyCode.LeftArrow))
        {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);

            // See if valid
            if (isValidGridPos())
                // Its valid. Update grid.
                updateGrid();
            else
                // Its not valid. revert.
                transform.position += new Vector3(1, 0, 0);
        }

        //right
        else if (getKey(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(1, 0, 0);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                transform.position += new Vector3(-1, 0, 0);
        }

        //rotate
        else if (getKey(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);

            // See if valid
            if (isValidGridPos() && gameObject.tag != "Cube")
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                transform.Rotate(0, 0, 90);
        }

        //down
        else if (getKey(KeyCode.DownArrow) || Time.time - lastFall >= 1)
        {
            groupfall();
        }
        
        else if (getKey(KeyCode.Space))
        {
            while (enabled)
                groupfall();
        }
    }
}
