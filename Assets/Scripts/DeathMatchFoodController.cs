using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMatchFoodController : MonoBehaviour
{

    private GameObject food = null;
    [SerializeField] GameObject foodPrefab;
    [SerializeField] ScoreController redScoreScript;
    [SerializeField] ScoreController greenScoreScript;
    [SerializeField] GameObject redSnake;
    [SerializeField] GameObject greenSnake;
    private RedSnakeController redSnakeController;
    private GreenSnakeController greenSnakeController;
    private Vector3 redHead;
    private Vector3 greenHead;
    private int width = 36;
    private int height = 19;

    void Start()
    {
        redSnakeController = redSnake.GetComponent<RedSnakeController>();
        greenSnakeController = greenSnake.GetComponent<GreenSnakeController>();
        redHead = redSnake.transform.position;
        greenHead = greenSnake.transform.position;
        FoodSpawner();
    }


    void Update()
    {
        FoodSpawner();
    }

    private void FoodSpawner()
    {
        redHead = redSnake.transform.position;
        greenHead = greenSnake.transform.position;
        Vector3 pos;
        if (food)
        {
            if (redHead == food.transform.position)
            {
                Destroy(food);
                greenSnakeController.AddBodyPart();
                redScoreScript.ChangeScore(10);
            }
            else if (greenHead == food.transform.position)
            {
                Destroy(food);
                redSnakeController.AddBodyPart();
                greenScoreScript.ChangeScore(10);
            }
            else
            {
                return;
            }
        }
        else
        {
            food = Instantiate(foodPrefab);
            food.SetActive(true);
            do
            {
                pos = new Vector3((int)Random.Range(0, width - 1), (int)Random.Range(0, height - 1), 0);
            } while (redSnakeController.PositionCheck(pos) || greenSnakeController.PositionCheck(pos));
            food.transform.position = pos;
        }
    }
}
