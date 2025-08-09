using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    public float lifeTime = 2f;
    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
