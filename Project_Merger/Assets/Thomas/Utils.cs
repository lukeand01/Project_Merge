using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
   //

  

    public static Color GetRandomColor()
    {
        
        List<Color> colorlIst = new()
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow
        };
        int random = Random.Range(0, colorlIst.Count);

        return colorlIst[random];
    }

}
