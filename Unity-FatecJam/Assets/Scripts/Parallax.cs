using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public struct ParallaxItem
{
    public SpriteRenderer prefab; // referência ao GameObject base
    public float velocity;
    public int qtdPerParalax; // quantidade específica por item
}

public class Parallax : MonoBehaviour
{
    [SerializeField] public float velocityMultiplier = 1f;
    [SerializeField] public List<ParallaxItem> parallaxList = new List<ParallaxItem>();
    public float transitionDuration = 5f;
    public float stopDuration = 3f;

    // Armazena os clones gerados
    private Dictionary<ParallaxItem, List<Transform>> clones = new Dictionary<ParallaxItem, List<Transform>>();

    private void Start()
    {
        foreach (var item in parallaxList)
        {
            if (item.prefab == null) continue;

            var list = new List<Transform>();

            float spriteWidth = item.prefab.bounds.size.x;

            item.prefab.gameObject.SetActive(false); // desliga o prefab original

            for (int i = 0; i < item.qtdPerParalax; i++)
            {
                GameObject cloneObj = Instantiate(item.prefab.gameObject, transform);
                cloneObj.SetActive(true);

                Transform cloneT = cloneObj.transform;
                cloneT.position = new Vector3(i * spriteWidth, item.prefab.transform.position.y, item.prefab.transform.position.z);
                cloneT.localScale = item.prefab.transform.localScale + Vector3.one * 0.01f;

                list.Add(cloneT);
            }

            clones[item] = list;
        }
    }

    private void FixedUpdate()
    {
        if (velocityMultiplier < 0.01)
        {
            return;
        }

        foreach (var item in parallaxList)
        {
            if (item.velocity < 0.01f) continue;
            if (!clones.ContainsKey(item)) continue;

            float speed = item.velocity * velocityMultiplier;
            float spriteWidth = item.prefab.bounds.size.x;

            var list = clones[item];
            for (int i = 0; i < list.Count; i++)
            {
                Transform t = list[i];
                t.position += Vector3.left * speed * Time.fixedDeltaTime;

                // Se saiu totalmente da tela, manda pro final
                if (t.position.x < -spriteWidth)
                {
                    float maxX = float.MinValue;
                    foreach (var other in list)
                        if (other.position.x > maxX)
                            maxX = other.position.x;

                    t.position = new Vector3(maxX + spriteWidth, t.position.y, t.position.z);
                }
            }
        }
    }

    async public void Hide(float delay)
    {
        if (parallaxList.Count == 1) return;
        await Task.Delay((int)(delay * 1000f));
        foreach (List<Transform> parallax in clones.Values.ToArray())
        {
            parallax.ForEach((transfom) =>
            {
                SpriteRenderer sprite = transfom.GetComponent<SpriteRenderer>();
                sprite.FadeSprite(sprite.color.a, 0f, transitionDuration);
            });
        }

        await Task.Delay((int)(transitionDuration * 1000f));
        velocityMultiplier = 0;
    }

    async public void Show(float delay)
    {
        await Task.Delay((int)(delay * 1000f));
        velocityMultiplier = 1;
        foreach (List<Transform> parallax in clones.Values.ToArray())
        {
            parallax.ForEach((transfom) =>
            {
                SpriteRenderer sprite = transfom.GetComponent<SpriteRenderer>();
                sprite.FadeSprite(sprite.color.a, 1f, transitionDuration);
            });
        }
    }

    async public void Stop(float delay)
    {
        await Task.Delay((int)(delay * 1000f));
        this.SmoothParallaxVelocity(velocityMultiplier, 0f, stopDuration);
    }

    async public void Go(float delay)
    {
        await Task.Delay((int)(delay * 1000f));
        this.SmoothParallaxVelocity(velocityMultiplier, 1f, stopDuration);
    }
}
