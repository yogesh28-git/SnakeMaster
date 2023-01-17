using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{

    private GameObject food = null;
    [SerializeField] GameObject foodPrefab;
    [SerializeField] ScoreController scoreScript;
    private SnakeController snakeController;
    private Vector3 head;

    void Start()
    {
        snakeController = GetComponent<SnakeController>();
        head = transform.position;
        FoodSpawner();
    }


    void Update()
    {
        FoodSpawner();
    }

    private void FoodSpawner()
    {
        head = transform.position;
        Vector3 pos;
        if (food)
        {
            if (head == food.transform.position)
            {
                Destroy(food);
                snakeController.AddBodyPart();
                scoreScript.ChangeScore();
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
            pos = snakeController.RandomSpawnPosition();
            food.transform.position = pos;
        }
    }
}
