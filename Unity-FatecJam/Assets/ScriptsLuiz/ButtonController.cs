using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite defaultImage;
    public Sprite pressedImage;

    public Color pressedColor;
    public Color defaultColor;

    public KeyCode keyToPress;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Use this code when we have the Buttons Sprites
        //if (Input.GetKeyDown(keyToPress))
        //{
        //    spriteRenderer.sprite = pressedImage;
        //}

        //if (Input.GetKeyUp(keyToPress))
        //{
        //    spriteRenderer.sprite = defaultImage;
        //}

        // Using this code while don't have button sprites
        if (Input.GetKeyDown(keyToPress)) { spriteRenderer.color = pressedColor; }
        if (Input.GetKeyUp(keyToPress)) { spriteRenderer.color = defaultColor; }
    }
}
