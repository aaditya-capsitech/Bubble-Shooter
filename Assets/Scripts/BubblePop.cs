using UnityEngine;

public class BubblePop : MonoBehaviour
{
    public static BubblePop instance;
    public ParticleSystem popEffect;

    private void Awake()
    {
        instance = this;
    }
    public void PlayEffect(Vector3 position, Color color)
    {
        if (popEffect == null) return;
        ParticleSystem ps = Instantiate(popEffect, position, Quaternion.identity);

        var main = ps.main;
        main.startColor = color;

        ps.Play();
        Destroy(ps.gameObject, main.duration + main.startLifetime.constantMax);
    }
}
