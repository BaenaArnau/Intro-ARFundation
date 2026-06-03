using UnityEngine;


public class TurnButton : MonoBehaviour
{
    public float turnDir;
    private bool _heldDown;


    void Update()
    {
        if(_heldDown)
        {
            if (Coche.Instance != null) Coche.Instance.Turn(turnDir);
        }
    }
    public void OnDown()
    {
        _heldDown = true;
        if (Coche.Instance != null) Coche.Instance.Turn(turnDir);
    }
    public void OnUp()
    {
        _heldDown = false;
        if (Coche.Instance != null) Coche.Instance.Turn(0f);
    }
}