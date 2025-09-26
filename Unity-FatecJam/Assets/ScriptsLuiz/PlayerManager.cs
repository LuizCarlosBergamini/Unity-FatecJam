using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int life;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            life = 30; // Initial life
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RemoveLife(int amount)
    {
        life -= amount;
        if (life <= 0 )
        {
            //Debug.Log("Game Over!");
        }
    }

    public void AddLife(int amount)
    {
        life += amount;
        if (life >= 30 )
        {
            life = 30; // Cap life at 30
        }
    }
}
