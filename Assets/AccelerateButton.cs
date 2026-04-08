using UnityEngine;


public class AccelerateButton : MonoBehaviour
{
    public void OnDown()
    {
        Coche.instance.doAccelerate = true;
    }


    public void OnUp()
    {
        Coche.instance.doAccelerate = false;
    }
}