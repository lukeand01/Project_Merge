using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI pointText;

    public void UpdatePointText(int value)
    {
        pointText.text = "Points: " + value.ToString();
    }


}
