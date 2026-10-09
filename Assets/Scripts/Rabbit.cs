using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public GameObject rabbitPrefab;
    public float energy = 50;
    public float speed = 2;
    public float foodRadius = 6;
    public float energyPerFood = 10;
    public float fleeRadius = 5f;

    Vector3 direction;

    void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    }

    void Update()
    {
        // die without energy
        energy -= Time.deltaTime;
        if (energy <= 0)
        {
            Destroy(gameObject);
            return;
        }

        // Every wolf tagged is added to an array
        GameObject[] wolves = GameObject.FindGameObjectsWithTag("Wolf");
        GameObject closestWolf = null;
        float wolfDistance = fleeRadius;

        foreach (GameObject w in wolves)
        {
            // any wolf outside of fleeRadius is ignored
            // for each wolf inside of fleeRadius, measure how far away it is from the rabbit.
            // if that wolf is the closest, then that wolf becomes "closestWolf"
            float distance = Vector3.Distance(transform.position, w.transform.position);
            if (distance < wolfDistance)
            {
                wolfDistance = distance;
                closestWolf = w;
            }
        }

        // if a wolf is in range
        if (closestWolf != null)
        {
            // subtracting the wolf's position from our rabbit gives you a vector2 that points in the direction the wolf is heading
            // normalize takes the values and uses pythagorean theorem to make the distance = 1.
            // so when we use away to transform the position, it's equal to the speed it moves when it isn't running away.
            Vector3 away = (transform.position - closestWolf.transform.position).normalized;
            transform.position += away * speed * Time.deltaTime;
        }

        else
        {
            // look for food
            GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
            GameObject closestFood = null;
            float foodDist = foodRadius;
            foreach (GameObject f in foods)
            {
                float distance = Vector3.Distance(transform.position, f.transform.position);
                if (distance < foodDist)
                {
                    foodDist = distance;
                    closestFood = f;
                }
            }

            if (closestFood != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, closestFood.transform.position, speed * Time.deltaTime);
                if (foodDist < 1f)
                {
                    Destroy(closestFood);
                    energy += energyPerFood;
                }
            }
            else
            {
                transform.position += direction * speed * Time.deltaTime;
            }
        }

        // make a baby if you have enough energy
        if (energy > 100)
        {
            energy = energy / 2;
            Instantiate(rabbitPrefab, new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0), Quaternion.identity);
        }

        // keep on screen
        if (transform.position.x > 8)
        {
            transform.position = new Vector3(8, transform.position.y, 0);
        }
        if (transform.position.x < -8)
        {
            transform.position = new Vector3(-8, transform.position.y, 0);
        }
        if (transform.position.y > 4)
        {
            transform.position = new Vector3(transform.position.x, 4, 0);
        }
        if (transform.position.y < -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }
    }
}