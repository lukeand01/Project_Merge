using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathLine : MonoBehaviour
{

    //while a ball stay in this place we increase this thing.
    //

    List<MergeBall> mergeBallList = new();
    BoxCollider2D myCollider;

    [SerializeField] LayerMask layer;

    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        endButtonHealth.SetUp(this);


    }

    private void Update()
    {




        if(myCollider != null)
        {
            //CheckDeathLine();
        }
    }

    void CheckDeathLine()
    {
        //we will be checking based on its own collider.
        Debug.Log("we check this");

        Vector2 size = myCollider.size;

        // Get the center of the BoxCollider2D in local space
        Vector2 center = myCollider.offset;

        // Convert the center from local space to world space
        Vector2 origin = (Vector2)transform.TransformPoint(center);

        // Calculate the half extents of the box
        float halfWidth = size.x * 0.5f;
        float halfHeight = size.y * 0.5f;

        // Perform the box cast
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, new Vector2(halfWidth, halfHeight), 0f, Vector2.zero, 0f, layer);

        if(hits.Length > 0 )
        {
            Debug.Log("there is someone " + hits.Length);
            Debug.Log("name " + hits[0].collider.name);
        }
        else
        {
            Debug.Log("there is no one");
        }

    }

    //if it stays for long enough then we do the thing.


    //what if i checkbased basde 


    //always when we load stuff we check to make the actual floor working.
    #region UI
    [SerializeField] GameObject deathLineHolder;
    [SerializeField] Image deathLineBar;
    [SerializeField] SpriteRenderer actualFloor;
    [SerializeField] GameObject fakeFloor;
    [SerializeField] GameObject deathUIHolder;
    [SerializeField] EndButton endButtonHealth;
    [SerializeField] TextMeshProUGUI newScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;



    public void ControlHolder(bool isVisible)
    {
        deathLineHolder.SetActive(isVisible);



    }

    public void UpdateDeathLineBar(float current, float total)
    {
        deathLineBar.fillAmount = current / total;
    }

    [ContextMenu("START DEATH UI")]
    public void DebugStartDeathUI()
    {
        StartDeathUI(100);
    }

    public void StartDeathUI(int scoreGained)
    {
        if (deathUIHolder.activeInHierarchy) return;

        deathUIHolder.gameObject.SetActive(true);

        actualFloor.enabled = false;
        fakeFloor.gameObject.SetActive(true);
        GameHandler.instance.StopGameForUI();

        newScoreText.text = "Your Score: " + scoreGained.ToString();
        highScoreText.text = "NO HIGHSCORE YEET";

        endButtonHealth.StartEndButton();

        StartCoroutine(CountScoreProcess(scoreGained));
    }


    IEnumerator CountScoreProcess(int totalScore)
    {
        //we keep counting and making it faster and faster as it goes and every x value we give a praise.

        yield return new WaitForSecondsRealtime(0.5f);
    }


    public void ReceiveEndInputPlayAgain()
    {
        Debug.Log("play again");
        deathUIHolder.gameObject.SetActive(false);
        GameHandler.instance.ReloadScene();

    }
    public void ReceiveEndInputResumeGame()
    {
        Debug.Log("Resume game");
        deathUIHolder.gameObject.SetActive(false);
        GameHandler.instance.adHandler.RequestRewardAd(RewardType.GetAnotherHealth);
        GameHandler.instance.UseHealth();
        

    }

    public void CallNewGame()
    {
        deathUIHolder.gameObject.SetActive(false);
        GameHandler.instance.ReloadScene();
    }

    

    #endregion


}
