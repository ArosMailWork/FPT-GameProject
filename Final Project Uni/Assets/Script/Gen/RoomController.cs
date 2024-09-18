using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject Room;
    [SerializeField] private GameObject Up;
    [SerializeField] private GameObject Down;
    [SerializeField] private GameObject Left;
    [SerializeField] private GameObject Right;
    // Start is called before the first frame update
    void Start()
    {
        //Up.SetActive(false);
        //Down.SetActive(false);
        //Left.SetActive(false);
        //Right.SetActive(false);
    }
    public void SetConnector(List<Vector2Int> connection)
    {
        if (connection.Contains(new Vector2Int(-1, 0)))
        {
            //Debug.Log("Up");
            Up.SetActive(true);
        }
        if (connection.Contains(new Vector2Int(1, 0)))
        {
            //Debug.Log("Down");
            Down.SetActive(true);
        }
        if (connection.Contains(new Vector2Int(0, -1)))
        {
            //Debug.Log("Left");
            Left.SetActive(true);
        }
        if (connection.Contains(new Vector2Int(0, 1)))
        {
            //Debug.Log("Right");
            Right.SetActive(true);
        }
    }


}
