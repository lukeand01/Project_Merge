using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{

    GameObject holder;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void CallPause()
    {
        if (holder.activeInHierarchy)
        {
            holder.SetActive(false);
            GameHandler.instance.ResumeGame();
        }
        else
        {
            holder.SetActive(true);
            GameHandler.instance.StopGameForUI();
        }
    }



}
