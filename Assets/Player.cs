using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] bool isMoving;
    [SerializeField] Vector3Int point;
    [SerializeField] float speed;
    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
        isMoving = Vector3.Distance(transform.position, point) > 0.3f;
        transform.LookAt(point);

        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(new Vector2(0, -1));
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(new Vector2(0, 1));
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(new Vector2(-1, 0));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(new Vector2(1, 0));
            }
        }
    }

    public void SetPoint(Vector3Int newpoint)
    {
        point = newpoint;
    }

    public void Move(Vector2 dir)
    {
        if (!isMoving)
        {
            transform.position = Vector3Int.RoundToInt(transform.position);
            if (Physics.Raycast(transform.position, new Vector3(dir.x, 0, dir.y), out RaycastHit hit))
            {
                point = Vector3Int.RoundToInt(hit.point + (hit.normal / 2));
            }
        }
    }
}
