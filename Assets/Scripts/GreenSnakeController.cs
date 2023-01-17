using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GreenSnakeController : MonoBehaviour
{
    private Direction facing = Direction.left;
    private Direction pressed = Direction.invalid;
    private Vector3 head;
    private int timer = 1;
    private int maxTimer = 50;
    [SerializeField] private int speed = 1;

    private int width = 36;
    private int height = 19;
    private bool isDead = false;

    private Vector3 rightTurn = new Vector3(0, 0, 0);
    private Vector3 leftTurn = new Vector3(0, 0, 180);
    private Vector3 downTurn = new Vector3(0, 0, -90);
    private Vector3 upTurn = new Vector3(0, 0, 90);

    private int listCount = 5;
    [SerializeField] List<Transform> bodyList;
    [SerializeField] List<Vector3> positionList;
    [SerializeField] List<Vector3> rotationList;
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] UI_Controller uicontroller;
    [SerializeField] RedSnakeController redSnakeScript;


    void Start()
    {
        head = transform.position;

        positionList[0] = bodyList[0].position;
        positionList[1] = bodyList[1].position;
        positionList[2] = bodyList[2].position;
        positionList[3] = bodyList[3].position;
        positionList[4] = bodyList[4].position;

        maxTimer = (int)maxTimer / speed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uicontroller.GetComponent<UI_Controller>().EnablePauseMenu();
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && facing != Direction.right)
        {
            pressed = Direction.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && facing != Direction.left)
        {
            pressed = Direction.right;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && facing != Direction.down)
        {
            pressed = Direction.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && facing != Direction.up)
        {
            pressed = Direction.down;
        }
    }

    private void FixedUpdate()
    {

        if (!isDead)
        {
            MovementController();
        }
        SnakeDeath();
    }

    private void MovementController()
    {

        if (timer < maxTimer)   //Time Delay
        {
            timer++;
        }
        else
        {
            facing = (pressed != Direction.invalid) ? pressed : Direction.left;
            switch (facing)
            {
                case Direction.left:
                    head.x += -1f;
                    transform.eulerAngles = leftTurn;
                    break;
                case Direction.right:
                    head.x += 1f;
                    transform.eulerAngles = rightTurn;
                    break;
                case Direction.down:
                    head.y += -1f;
                    transform.eulerAngles = downTurn;
                    break;
                case Direction.up:
                    head.y += 1f;
                    transform.eulerAngles = upTurn;
                    break;
            }
            if (head.x < 0) { head.x = width - 1; }
            else if (head.x >= width) { head.x = 0; }
            else if (head.y < 0) { head.y = height - 1; }
            else if (head.y >= height) { head.y = 0; }

            transform.position = head;
            timer = 1;

            for (int i = 1; i < listCount; i++)
            {
                //incrementing using previous co-ordinates
                bodyList[i].position = positionList[i - 1];
                bodyList[i].eulerAngles = rotationList[i - 1];
            }

            for (int i = 0; i < listCount; i++)
            {
                //updation of latest co-ordinates
                positionList[i] = bodyList[i].position;
                rotationList[i] = bodyList[i].eulerAngles;
            }
        }
    }


    public void AddBodyPart()
    {
        GameObject newbody;

        newbody = Instantiate(bodyPrefab);
        newbody.transform.position = positionList[listCount - 1];
        newbody.transform.eulerAngles = rotationList[listCount - 1];
        bodyList.Add(newbody.transform);
        positionList.Add(positionList[listCount - 1]);
        rotationList.Add(rotationList[listCount - 1]);
        listCount++;
    }

    private void SnakeDeath()
    {
        if (positionList.GetRange(1, listCount - 1).Contains(head))
        {
            Debug.Log("Death");
            isDead = true;
            uicontroller.EnableGameOver("Red Wins !!!");
            return;
        }
        else if (redSnakeScript.PositionCheck(head))
        {
            uicontroller.EnableGameOver("Green Wins !!!");
        }
    }

    public bool PositionCheck(Vector3 pos)
    {
        if (positionList.Contains(pos))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
