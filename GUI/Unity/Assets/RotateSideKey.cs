using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSideKey : MonoBehaviour
{
    ReadCube readCube;
    CubeState cubeState;
    
    
    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            readCube.ReadState();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                RotateFace(cubeState.front, true, Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                RotateFace(cubeState.back, false, Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                RotateFace(cubeState.up, false, Vector3.up);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateFace(cubeState.down, true, Vector3.up);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                RotateFace(cubeState.left, false, Vector3.forward);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                RotateFace(cubeState.right, true, Vector3.forward);
            }
        }
        else
        {
            //readCube.ReadState();
            if (Input.GetKeyDown(KeyCode.F))
            {
                RotateFace(cubeState.front, false, Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                RotateFace(cubeState.back, true, Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                RotateFace(cubeState.up, true, Vector3.up);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateFace(cubeState.down, false, Vector3.up);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                RotateFace(cubeState.left, true, Vector3.forward);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                RotateFace(cubeState.right, false, Vector3.forward);
            }
        }
        }
    }

    void RotateFace(List<GameObject> side, bool clockwise, Vector3 rotationAxis)
    {
        GroupSide(side);
        float angle = clockwise ? 90f : -90f;
        side[4].transform.parent.Rotate(rotationAxis, angle);
        UnGroupSide(side);
    }

    void GroupSide(List<GameObject> side)
    {
        foreach (GameObject face in side)
        {
            // Attach the parent of each face (the little cube)
            // to the parent of the 4th index (the little cube in the middle)
            // Unless it is already the 4th index
            if (face != side[4])
            {
                
                face.transform.parent.transform.parent = side[4].transform.parent;
            }
        }
    }

    void UnGroupSide(List<GameObject> littleCubes)
    {
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                
                littleCube.transform.parent.transform.parent = littleCubes[4].transform.parent.transform.parent;
            }
        }
    }


}
