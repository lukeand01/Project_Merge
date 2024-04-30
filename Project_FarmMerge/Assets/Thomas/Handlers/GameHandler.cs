using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    public BallHandler ballHandler {  get; private set; }
    public PowerHandler powerHandler { get; private set; }
  
    public SoundHandler soundHandler { get; private set; }  

    public AdHandler adHandler { get; private set; }





    [Separator("OBJECT TO CHANGE POSITION")]
    [SerializeField] Transform leftWall;
    [SerializeField] Transform rightWall;
    [SerializeField] Transform spawnPosition;
    [SerializeField] Transform linePosition;
    [SerializeField] Transform endLinePosition;
    [SerializeField] Transform groundPosition;
    [SerializeField] Transform topLinePosition;
    [SerializeField] Transform ballContainer;

    [Separator("DEATH LINE")]
    [SerializeField] Transform deathLine;

    public bool hasUsedHealth {  get; private set; } //


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        ballHandler = GetComponent<BallHandler>();
        powerHandler = GetComponent<PowerHandler>();
        soundHandler = GetComponent<SoundHandler>();
        adHandler = GetComponent<AdHandler>();

        DontDestroyOnLoad(gameObject);


        deathPosY = deathLine.transform.localPosition.y * 0.9f;
    }

   
    public float deathPosY {  get; private set; }

    public void Start()
    {
        //at the start we put the walls at teh right place based in the position of the thing.
        // Camera cam = Camera.main;

        leftWall.position = UIHandler.instance.leftWallPosRef.position;
        rightWall.position = UIHandler.instance.rightWallPosRef.position;
        spawnPosition.position =UIHandler.instance.spawnPosRef.position;
        linePosition.position = UIHandler.instance.spawnPosRef.position;
        endLinePosition.position = UIHandler.instance.groundPosRef.position;
        groundPosition.position = UIHandler.instance.groundPosRef.position;
        topLinePosition.position = UIHandler.instance.topLinePosRef.position;
        //ballContainer.position = UIHandler.instance.groundPosRef.position;




        TimeModifier = 1;
    }


    public float TimeModifier { get; private set; }


    public void ResumeGame()
    {
        TimeModifier = 1;
        Time.timeScale = 1;    
    }
    public void StopGameForUI()
    {
        Time.timeScale = 0.001f;
        TimeModifier = 100000;
    }
    public void StopGame()
    {
        Time.timeScale = 0;
    }




    public void ReloadScene()
    {
        ResumeGame();
        StartCoroutine(ResetSceneProcess());

    }

    IEnumerator ResetSceneProcess()
    {
        //i take a time to do everything and start again.

        ballHandler.ResetBallHandler();
        PlayerHandler.instance.ResetPlayer();
        UIHandler.instance.inputUI.ResetPowerAmmo();
        UIHandler.instance.queueUI.StopAllCoroutines();


        yield return new WaitForSeconds(1);

        //we announce the start.
        //
        ballHandler.StartBallHandler();


    }

    public void StartTheGameFromWhereItWas()
    {
       
        StartCoroutine(Process());
    }

    IEnumerator Process()
    {

        UseHealth();
        ballHandler.DestroyAllObjectsAboveACertainY();
        UIHandler.instance.queueUI.StopAllCoroutines();

        yield return new WaitForSeconds(1);

        ballHandler.StartGameAgain();

    }
    


    public void UseHealth()
    {
        hasUsedHealth = true;
    }
}
