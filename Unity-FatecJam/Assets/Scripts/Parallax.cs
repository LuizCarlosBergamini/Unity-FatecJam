using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ParallaxItem
{
    public SpriteRenderer texture;
    public float velocity;
    public float zIndex;
}

public class Parallax : MonoBehaviour
{
    [SerializeField] private float velocityMultiplier = 1f;
    [SerializeField] private int qtdPerParalax = 3;
    [SerializeField] public List<ParallaxItem> parallaxList = new List<ParallaxItem>();

    private Dictionary<ParallaxItem, List<Transform>> clones = new Dictionary<ParallaxItem, List<Transform>>();

    private void Start()
    {
        foreach (var item in parallaxList)
        {
            var list = new List<Transform>();
            float spriteWidth = item.texture.bounds.size.x;

            item.texture.gameObject.SetActive(false);

            for (int i = 0; i < qtdPerParalax; i++)
            {
                SpriteRenderer clone = Instantiate(item.texture, transform);
                clone.gameObject.SetActive(true);
                clone.transform.position = new Vector3(i * spriteWidth, item.texture.transform.position.y, item.zIndex);
                clone.transform.localScale = item.texture.transform.localScale;

                list.Add(clone.transform);
            }

            clones[item] = list;
        }
    }

    private void FixedUpdate()
    {
        foreach (var item in parallaxList)
        {
            float speed = item.velocity * velocityMultiplier;
            float spriteWidth = item.texture.bounds.size.x;

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
}