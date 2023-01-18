using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SnakeController : MonoBehaviour
{
    private Direction facing = Direction.right;
    private Direction pressed = Direction.invalid;
    private Vector3 head;
    private int timer = 1;
    private int maxTimer = 50;
    [SerializeField] private int speed = 1;
    
    private int width = 36;
    private int height = 19;
    private bool isDead = false;
    private bool isShield = false;

    private Vector3 rightTurn = new Vector3(0, 0, 0);
    private Vector3 leftTurn = new Vector3(0, 0, 180);
    private Vector3 downTurn = new Vector3(0, 0, -90);
    private Vector3 upTurn = new Vector3(0, 0, 90);

    private int listCount = 3;
    [SerializeField] List<Transform> bodyList;
    [SerializeField] List<Vector3> positionList;
    [SerializeField] List<Vector3> rotationList;
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] GameObject uicontroller;
    

    void Start()
    {
        head = transform.position;

        positionList[0] = bodyList[0].position;
        positionList[1] = bodyList[1].position;
        positionList[2] = bodyList[2].position;
        maxTimer = (int)maxTimer / speed;
    }

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uicontroller.GetComponent<UI_Controller>().EnablePauseMenu();
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.A) && facing != Direction.right)
        {
            pressed = Direction.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && facing != Direction.left)
        {
            pressed = Direction.right;
        }
        else if (Input.GetKeyDown(KeyCode.W) && facing != Direction.down)
        {
            pressed = Direction.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && facing != Direction.up)
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
            facing = (pressed != Direction.invalid) ? pressed : Direction.right;
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
            if(head.x < 0) { head.x = width-1; }
            else if(head.x >= width) { head.x = 0; }
            else if(head.y < 0) { head.y = height-1; }
            else if(head.y >= height) { head.y = 0; }

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

    public void RemoveBodyPart()
    {
        if (listCount > 3)
        {
            GameObject lastBody = bodyList[listCount - 1].gameObject;
            bodyList.Remove(bodyList[listCount - 1]);
            positionList.Remove(positionList[listCount - 1]);
            rotationList.Remove(rotationList[listCount - 1]);
            Destroy(lastBody);
            listCount--;
        } 
    }

    private void SnakeDeath()
    {
        if (isShield) { return; }
        if (positionList.GetRange(1, listCount - 1).Contains(head))
        {
            Debug.Log("Death");
            isDead = true;
            uicontroller.GetComponent<UI_Controller>().EnableGameOver();
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

    public void ImplementPowerUp(Collectibles powerUpType)
    {
        Debug.Log("powerup");
        if (powerUpType == Collectibles.doubleSpeed) 
        { 
            maxTimer = 50;
            maxTimer = (int)maxTimer / (speed * 2);
        }
        else if(powerUpType == Collectibles.shield) { isShield = true; }
    } 
    public void ResetPowerUp()
    {
        Debug.Log("reset");
        maxTimer = 50;
        maxTimer = (int)maxTimer / speed;
        isShield = false;
    }
}
