using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{

    private GameObject massGainFood = null;
    private GameObject massLossFood = null;
    [SerializeField] GameObject foodPrefab;
    [SerializeField] ScoreController scoreScript;
    private SnakeController snakeController;
    private int width = 36;
    private int height = 19;
    private Vector3 head;
    [SerializeField] private List<GameObject> massGainList;
    [SerializeField] private List<GameObject> massLossList;
    [SerializeField] private List<Vector3> positionList;


    enum Collectibles
    {
        massGain,
        massLoss,
        doubleSpeed,
        shield
    }
    void Start()
    {
        snakeController = GetComponent<SnakeController>();
        head = transform.position;
        MassGainFoodSpawner();
        MassLossFoodSpawner();
    }


    void Update()
    {
        head = transform.position;
        MassLossFoodDestroyer();
        MassGainFoodDestroyer();

    }
    private void MassGainFoodSpawner()
    {
        head = transform.position;
        Vector3 pos;
        if (massGainList.Count < 5)
        {
            massGainFood = Instantiate(foodPrefab);
            massGainFood.SetActive(true);
            do
            {
                pos = new Vector3((int)Random.Range(0, width - 1), (int)Random.Range(0, height - 1), 0);
            } while (snakeController.PositionCheck(pos) || positionList.Contains(pos));
            massGainFood.transform.position = pos;
            massGainList.Add(massGainFood);
            positionList.Add(pos);
            StartCoroutine(RandomSpawn(Collectibles.massGain));
            StartCoroutine(DestroyFood(massGainFood));
        }
    }
    private void MassLossFoodSpawner()
    {
        head = transform.position;
        Vector3 pos;
        if(massLossList.Count < 5)
        {
            massLossFood = Instantiate(foodPrefab);
            massLossFood.GetComponent<SpriteRenderer>().color = Color.red;
            massLossFood.SetActive(true);
            do
            {
                pos = new Vector3((int)Random.Range(0, width - 1), (int)Random.Range(0, height - 1), 0);
            } while (snakeController.PositionCheck(pos) || positionList.Contains(pos));
            massLossFood.transform.position = pos;
            massLossList.Add(massLossFood);
            positionList.Add(pos);
            StartCoroutine(RandomSpawn(Collectibles.massLoss));
            StartCoroutine(DestroyFood(massLossFood));
        }
    }
    private void MassGainFoodDestroyer()
    {
        for (int i = 0; i < massGainList.Count; i++)
        {
            if (head == massGainList[i].transform.position)
            {
                GameObject eatenFood = massGainList[i];
                massGainList.Remove(eatenFood);
                positionList.Remove(eatenFood.transform.position);
                Destroy(eatenFood);
                snakeController.AddBodyPart();
                scoreScript.ChangeScore(10);
                MassGainFoodSpawner();
            } 
        }
    }
    private void MassLossFoodDestroyer()
    {
        for (int i = 0; i < massLossList.Count; i++)
        {
            if (head == massLossList[i].transform.position)
            {
                GameObject eatenFood = massLossList[i];
                massLossList.Remove(eatenFood);
                positionList.Remove(eatenFood.transform.position);
                Destroy(eatenFood);
                snakeController.RemoveBodyPart();
                scoreScript.ChangeScore(-10);
                MassLossFoodSpawner();
            }
        }
    }

    
    IEnumerator RandomSpawn(Collectibles collectible)
    {
        int time = Random.Range(7, 15);
        yield return new WaitForSeconds(time);
        switch (collectible)
        {
            case Collectibles.massGain:
                MassGainFoodSpawner();
                break;
            case Collectibles.massLoss:
                MassLossFoodSpawner();
                break;
        }
    }
    IEnumerator DestroyFood(GameObject food)
    {
        yield return new WaitForSeconds(10);
        if (food != null)
        {
            massLossList.Remove(food);
            massGainList.Remove(food);
            positionList.Remove(food.transform.position);
            Destroy(food);
        }
    }
}
