using UnityEngine;


public class TurnButton : MonoBehaviour
{
    public float turnDir = 1f;
    private bool heldDown;


    void Update()
    {
        if(heldDown && Coche.instance != null)
        {
            Coche.instance.Turn(turnDir);
        }
    }
    
    public void OnDown()
    {
        heldDown = true;
    }
    
    public void OnUp()
    {
        heldDown = false;
    }
}