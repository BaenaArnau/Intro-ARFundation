using UnityEngine;


public class AccelerateButton : MonoBehaviour
{
    public void OnDown()
    {
        Coche.Instance.doAccelerate = true;
    }
    
    public void OnUp()
    {
        Coche.Instance.doAccelerate = false;
    }
}