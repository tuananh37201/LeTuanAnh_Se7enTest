using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public ParticleSystem[] goalEffect;
    public bool isGoal = false;

    private void Start()
    {
        goalEffect = FindObjectsOfType<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // khi va chạm với khung thành có tag là "Goal"
        if (collision.gameObject.CompareTag("Goal"))
        {
            isGoal = true;
            StartCoroutine(DestroyGameObject());

            // chạy particle effect
            foreach (ParticleSystem effect in goalEffect)
            {
                effect.Play();
            }
        }
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
