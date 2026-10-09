using UnityEngine;

public class Wolf : MonoBehaviour
{
    public GameObject wolfPrefab;
    public float energy = 50;
    public float speed = 1f;
    public float huntRadius = 6f;
    public float energyPerRabbit = 40;
    public float reproduceThreshold = 100;

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

        // look for rabbits
        GameObject[] rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
        GameObject closestRabbit = null;
        float rabbitDistance = huntRadius;
        foreach (GameObject r in rabbits)
        {
            // any rabbit outside of huntRadius is ignored
            // for each rabbit inside of huntRadius, measure how far away it is from the wolf.
            // if that rabbit is the closest, then that rabbit becomes "closestRabbit"
            float distance = Vector3.Distance(transform.position, r.transform.position);
            if (distance < rabbitDistance)
            {
                rabbitDistance = distance;
                closestRabbit = r;
            }
        }

        if (closestRabbit != null)
        {
            // move towards rabbit, if it gets close enough, it dies.
            transform.position = Vector3.MoveTowards(transform.position, closestRabbit.transform.position, speed * Time.deltaTime);
            if (rabbitDistance < 1f)
            {
                Destroy(closestRabbit);
                energy += energyPerRabbit;
            }
        }
        else
        {
            // move somewhere
            transform.position += direction * speed * Time.deltaTime;
        }

        // make a baby if you have enough energy
        if (energy > reproduceThreshold)
        {
            energy = energy / 2;
            Instantiate(wolfPrefab, new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0), Quaternion.identity);
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