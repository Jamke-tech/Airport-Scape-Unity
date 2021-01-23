using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{

    
    public void YesPulsed()
    {
        GameManager.instance.YesPulsed();
    }
    public void NOPulsed()
    {
        GameManager.instance.NoPulsed();
    }

}
