using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodController : MonoBehaviour
{
    [SerializeField] private AudioController audiocontroller;
    private GameObject massGainFood = null;
    private GameObject massLossFood = null;
    private GameObject powerUp = null;
    private Collectibles powerUpType;
    [SerializeField] GameObject blueFood;
    [SerializeField] GameObject redFood;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject doubleSpeed;
    [SerializeField] GameObject doubleScore;
    [SerializeField] ScoreController scoreScript;
    private SnakeController snakeController;
    private int width = 36;
    private int height = 19;
    private int scoreMultiplier = 1;
    private Vector3 head;
    [SerializeField] private List<GameObject> massGainList;
    [SerializeField] private List<GameObject> massLossList;
    [SerializeField] private List<Vector3> positionList;

    
    void Start()
    {
        snakeController = GetComponent<SnakeController>();
        head = transform.position;
        MassGainFoodSpawner();
        MassLossFoodSpawner();
        StartCoroutine(RandomSpawn(Collectibles.powerUp));
    }


    void Update()
    {
        head = transform.position;
        MassLossFoodDestroyer();
        MassGainFoodDestroyer();
        PowerUpDestroyer();
    }
    private void MassGainFoodSpawner()
    {
        Vector3 pos;
        if (massGainList.Count < 5)
        {
            massGainFood = Instantiate(blueFood);
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
        Vector3 pos;
        if(massLossList.Count < 5)
        {
            massLossFood = Instantiate(redFood);
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
                audiocontroller.Play(Sounds.pickup);
                GameObject eatenFood = massGainList[i];
                massGainList.Remove(eatenFood);
                positionList.Remove(eatenFood.transform.position);
                Destroy(eatenFood);
                snakeController.AddBodyPart();
                scoreScript.ChangeScore(10 * scoreMultiplier);
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
                audiocontroller.Play(Sounds.pickup);
                GameObject eatenFood = massLossList[i];
                massLossList.Remove(eatenFood);
                positionList.Remove(eatenFood.transform.position);
                Destroy(eatenFood);
                snakeController.RemoveBodyPart();
                scoreScript.ChangeScore(-10* scoreMultiplier);
                MassLossFoodSpawner();
            }
        }
    }

    private void PowerUpSpawner()
    {
        Vector3 pos;
        if (powerUp)
        {
            return;
        }
        else
        {
            int i = (int)Random.Range(1, 4);
            if(i == 1) 
            { 
                powerUpType = Collectibles.shield;
                powerUp = Instantiate(shield); 
            }
            else if(i == 2) 
            {
                powerUpType = Collectibles.doublePoint;
                powerUp = Instantiate(doubleScore); 
            }
            else if(i == 3) 
            {
                powerUpType = Collectibles.doubleSpeed;
                powerUp = Instantiate(doubleSpeed); 
            }
            powerUp.SetActive(true);
            do
            {
                pos = new Vector3((int)Random.Range(0, width - 1), (int)Random.Range(0, height - 1), 0);
            } while (snakeController.PositionCheck(pos) || positionList.Contains(pos));
            powerUp.transform.position = pos;
            positionList.Add(pos);
            StartCoroutine(DestroyFood(powerUp));
            StartCoroutine(RandomSpawn(Collectibles.powerUp));
        }
    }

    private void PowerUpDestroyer()
    {
        if (powerUp)
        {
            if (head == powerUp.transform.position)
            {
                audiocontroller.Play(Sounds.pickup);
                positionList.Remove(powerUp.transform.position);
                snakeController.ImplementPowerUp(powerUpType);
                scoreScript.PowerUpText(powerUpType);
                Invoke("ResetPowerUps", 5);
                if (powerUpType == Collectibles.doublePoint)
                {
                    scoreMultiplier = 2;
                }
                Destroy(powerUp);
                Debug.Log("snake ate powerUp");
                StartCoroutine(RandomSpawn(Collectibles.powerUp));
            }
        }  
    }
    private void ResetPowerUps()
    {
        scoreScript.PowerUpText(Collectibles.powerUp);
        snakeController.ResetPowerUp();
        scoreMultiplier = 1;
    }
    IEnumerator RandomSpawn(Collectibles collectible)
    {
        int time = Random.Range(9, 15);
        yield return new WaitForSeconds(time);
        switch (collectible)
        {
            case Collectibles.massGain:
                MassGainFoodSpawner();
                break;
            case Collectibles.massLoss:
                MassLossFoodSpawner();
                break;
            case Collectibles.powerUp:
                PowerUpSpawner();
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
