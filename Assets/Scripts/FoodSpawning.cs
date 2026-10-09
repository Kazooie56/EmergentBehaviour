using UnityEngine;

public class FoodSpawning : MonoBehaviour
{
    public GameObject foodPrefab;
    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            Instantiate(foodPrefab, new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0), Quaternion.identity);
            timer = 0;
        }
    }
}