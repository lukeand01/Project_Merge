using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
    //everytime it requires.
    GameHandler handler;

    MergeBall currentMergeBall;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform spawnPosition;
    [SerializeField] LineRenderer line;
    [SerializeField] List<MergeBall> mergeBallList = new();
    [SerializeField] List<MergeBall> specialMergeBallList = new(); //
    Dictionary<MergeBallType, MergeBall> mergeBallDictionary = new();

    [SerializeField]List<MergeBall> queueList = new();

    [SerializeField] Transform leftLimitation;
    [SerializeField] Transform rightLimitation;
    [SerializeField] Transform lineEnd;
    [SerializeField] Transform ballContainer;
    Vector3 lastPos;
    


    bool hasStarted;

    [Separator("DEBUG")]
    [SerializeField] bool debugStart;

    bool isHolding;

    bool cannotControlCurrentBall;

    //things i might want to call
    //destroy the fellas
    //activate ui on them.

    public Action<MergeBallType, OrderWhoType, OrderWhatType> eventBallOrder;
    public void OnBallOrder(MergeBallType mergeBallType, OrderWhoType whoOrder, OrderWhatType whatOrder) => eventBallOrder?.Invoke(mergeBallType, whoOrder, whatOrder);


    //lets make this better
    
    public bool CanSpawnNextBall {  get; private set; }

    [Separator("SOUND")]
    [SerializeField] AudioClip createNewCurrentBallClip;
    [SerializeField] AudioClip[] standardMergeClip;
    [SerializeField] AudioClip giftSoundClip;
    private void Awake()
    {
        GetMergeBallRef();
        handler = GetComponent<GameHandler>();
        SetDeath();
        CanSpawnNextBall = true;
    }

    

    private void Start()
    {
        StartBallHandler();
    }


    #region GET REF
    void GetMergeBallRef()
    {
        foreach (var item in mergeBallList)
        {
            mergeBallDictionary.Add(item.mergeType, item);
        }
    }


    #endregion


    public void StartBallHandler()
    {
        SpawnNewCurrentBall(true);
        CreateQueueList();
        ResetBonus();
        SetCamera();

        CanSpawnNextBall = true;
        cannotControlCurrentBall = false;

        spawnTotal = 0.7f;
    }


    #region CREATE QUEUE






    [SerializeField] int queueLimit = 4;
    public void CreateQueueList()
    {
        //we will spawn the first fella and then create teh queue.

        for (int i = 0; i < queueLimit; i++)
        {
            CreateNextQueueItem(true);
        }

        UIHandler.instance.queueUI.SetQueue(queueList);


    }
    void CreateNextQueueItem(bool firstBatch = false)
    {
        int specialChance = UnityEngine.Random.Range(0, 100);

        if(specialChance > 97 && !firstBatch && specialMergeBallList.Count > 0)
        {
            //then we take it from the especial list.
            int randomSpecial = UnityEngine.Random.Range(0, specialMergeBallList.Count);
            queueList.Add(specialMergeBallList[randomSpecial]);
            UIHandler.instance.queueUI.MoveQueue(specialMergeBallList[randomSpecial]);
            return;
        }

        int random = UnityEngine.Random.Range(0, 4);
        queueList.Add(mergeBallList[random]);
        if(!firstBatch)
        {

            UIHandler.instance.queueUI.MoveQueue(mergeBallList[random]);
        }
    }

    void ClearQueue()
    {
        queueList.Clear();
        UIHandler.instance.queueUI.SetQueue(queueList);

    }

    #endregion

    float spawnCurrent;
    float spawnTotal;

    //now we inform that we have someone waiting.
    private void Update()
    {
        //it keep trying to follow the touch lastposition

        HandleBonus();
        HandleDeath();

        line.gameObject.SetActive(currentMergeBall != null && isHolding);



        if(currentMergeBall == null)
        {

            if (CanSpawnNextBall)
            {

                if (spawnCurrent >= spawnTotal)
                {
  
                    SpawnNewCurrentBall();
                }
                else
                {

                    spawnCurrent += Time.deltaTime;
                }



            }

            return;
        }
        if (!CanSpawnNextBall)
        {
            Debug.Log("cannot spawn next ball");
            return;
        }






        Vector3 newPosition = Vector3.Lerp(currentMergeBall.transform.position, lastPos, moveSpeed * Time.deltaTime);
        currentMergeBall.transform.position = newPosition;
        line.transform.position = newPosition;

        newPosition.y = lineEnd.transform.position.y;
        lineEnd.transform.position = newPosition;

        line.SetPosition(0, line.transform.position);
        line.SetPosition(1, lineEnd.position);

    }

    #region SPAWN


    public void SpawnNewCurrentBall(bool isFirst = false)
    {
        //we get a random selection of the lowers balls.
        isHolding = false;

        MergeBall target = null;

        spawnCurrent = 0;

        if (isFirst)
        {
            int index = UnityEngine.Random.Range(0, 4);
            target = mergeBallList[index];
        }
        else
        {
            target = queueList[0];
        }

        MergeBall newObject = Instantiate(target, spawnPosition.position, Quaternion.identity);
        newObject.SetUp(this);
        newObject.transform.Rotate(new Vector3(0, 180, 0));
        newObject.MakeNotReactive();
        currentMergeBall = newObject;
        newObject.transform.SetParent(ballContainer);

        Vector3 originalScale = newObject.transform.localScale;
        newObject.transform.localScale = Vector3.zero;
        newObject.transform.DOScale(originalScale, 0.15f);

        lastPos = spawnPosition.position;


        GameHandler.instance.soundHandler.CreateSFX(createNewCurrentBallClip, 0.6f);

        if (!isFirst)
        {
            queueList.RemoveAt(0);
            CreateNextQueueItem();
        }

    }
    public void SpawnNewAtPosition(int index, MergeBall firstBall, MergeBall secondBall)
    {
        

        if(index > mergeBallList.Count)
        {
            Debug.Log("then we just destroy this felçla");
            Destroy(firstBall.gameObject);
            if (secondBall != null) secondBall.OrderDestruction();
            return;
        }

        Color color = Utils.GetRandomColor();

        float turnValueModifier = GetTurnModifier(firstBall.transform.position, color);

        //we award point based in the thing.
        AddBonus(5 * turnValueModifier);


        MergeBall newObject = Instantiate(mergeBallList[index + 1], firstBall.transform.position, Quaternion.identity);
        newObject.SetUp(this);
        newObject.DropIt();
        newObject.transform.Rotate(new Vector3(0, 180, 0));
        newObject.transform.SetParent(ballContainer);
        //ChangeScale(cheerObject.transform, index + 1);


        if(newObject.mergeClip != null)
        {
            GameHandler.instance.soundHandler.CreateSFX(newObject.mergeClip);
        }
        else
        {
            int random = UnityEngine.Random.Range(0, standardMergeClip.Length);
            GameHandler.instance.soundHandler.CreateSFX(standardMergeClip[random]);
        }

        if (isBonusHappening)
        {
            ShakeCamera();
        }


        float score = GetMergeTypeScore(newObject.mergeType);
        PlayerHandler.instance.AddPoints((int)score * (int)turnValueModifier * (int)bonusValue);


        FadeUI scoreObject = Instantiate(fadeTemplate, firstBall.transform.position, Quaternion.identity);
        scoreObject.transform.SetParent(UIHandler.instance.transform);
        scoreObject.SetUp(score.ToString("f0"), color);
        scoreObject.ChangeScaleModifier(1.1f);

        firstBall.OrderDestruction();

        if (secondBall != null) secondBall.OrderDestruction();

    }

 

    #endregion


    #region GETTERS AND UTILS
    float GetMergeTypeScore(MergeBallType mergeType)
    {
        return ((float)mergeType * (float)mergeType) + 1 * 1.5f;
    }
    float GetSpawnTypeScore(MergeBallType mergeType)
    {
        return ((float)mergeType + 1) * 1.5f;
    }

    void ChangeScale(Transform target, int index)
    {
        float sizeValue = GetSizeForBallType(index);
        Vector3 size = new Vector3(sizeValue, sizeValue, sizeValue);
        target.transform.localScale = size;
    }

    float GetSizeForBallType(int index)
    {
        float baseSize = 0.8f;
        float increase = 0.2f * index;
        return baseSize + increase;
    }

    public void DestroyThisAndGivePoint(MergeBall merge)
    {

        Destroy(merge.gameObject);
    }
    #endregion

    #region INPUT
    public void ReceiveMoveInput(Vector3 pos)
    {

        if (handler.powerHandler.IsPowerActive()) return;
        if (currentMergeBall == null) return;
        if (cannotControlCurrentBall) return;

        float clampedValueX = pos.x;
        clampedValueX = Mathf.Clamp(clampedValueX, leftLimitation.transform.position.x, rightLimitation.transform.position.x);
        lastPos = new Vector3(clampedValueX, currentMergeBall.transform.position.y, currentMergeBall.transform.position.z);
        isHolding = true;
        //and there are limits.

    }
    public void ReceiveDecisionInput()
    {
        //if you cancel or act.

        if (handler.powerHandler.IsPowerActive()) return;
        if (currentMergeBall == null) return;
        if (cannotControlCurrentBall) return;

        int score = (int)GetSpawnTypeScore(currentMergeBall.mergeType);
        PlayerHandler.instance.AddPoints(score);

        currentMergeBall.MakeReactive();
        currentMergeBall.DropIt();
        currentMergeBall = null;
        isHolding = false;


        

        ResetTurn();
    }

    void OrderSpawnNewBall()
    {
        SpawnNewCurrentBall();
    }
    #endregion

    #region ROUND
    //everytime 
    [Separator("ROUND")]
    [SerializeField] FadeUI fadeTemplate;

    int mergeSpree;

    void ResetTurn()
    {
        //this happens when you throw another ball

        mergeSpree = 0;
    }

    float GetTurnModifier(Vector3 mergePos, Color color)
    {
        //we call this everytime we merge two fellas.
        //we create the commemoration here and give especial bonus based in the mergespree
        mergeSpree += 1;

        float scoreModifier = GetScoreModifier();



        if (mergeSpree > 1)
        {
            //thebn we start doing this stuff.
            FadeUI cheerObject = Instantiate(fadeTemplate, mergePos + new Vector3(0,0.5f,0), Quaternion.identity);
            cheerObject.transform.SetParent(UIHandler.instance.transform);

            cheerObject.SetUp(GetCheerPhraseBasedInRound(), color);

            cheerObject.ChangeScaleModifier(1.2f);
            cheerObject.ChangeColorModifier(0.6f);



        }


        return scoreModifier;
    }

    float GetScoreModifier()
    {
        
        if(mergeSpree == 1)
        {
            return 1;
        }
        if(mergeSpree == 2)
        {
            return 1.5f;
        }
        if(mergeSpree == 3)
        {
            return 2;
        }
        if(mergeSpree >= 4 && mergeSpree < 6)
        {
            return 2.5f;
        }
        if(mergeSpree >= 6)
        {
            return 3;
        }


        Debug.LogError("0 score modifier");
        return 0;

    }

    string GetCheerPhraseBasedInRound()
    {
        List<string> cheerList = new()
        {
            "Good!",
            "Awesome!",
            "Perfect"
        };

        if(mergeSpree >= 1 && mergeSpree <= 2)
        {
            return cheerList[0];
        }

        if(mergeSpree > 2 && mergeSpree <= 4)
        {
            return cheerList[1];
        }

        if (mergeSpree > 4) return cheerList[2];


        return "ERROR";
    }


    public void DebugSpawnFade(Vector3 pos)
    {
        FadeUI newObject = Instantiate(fadeTemplate, pos, Quaternion.identity);
        newObject.transform.SetParent(UIHandler.instance.transform);
        newObject.SetUp("teste", Utils.GetRandomColor());
    }

    #endregion

    #region BONUS
    //we check the bonus here

    float bonusCurrent = 0;
    float bonusTotal = 100;
    bool isBonusHappening = false;
    float bonusValue = 1;


    float bonusAmount = 0; 
    float bonusGiftCurrent;
    float bonusGiftTotal;
    bool alreadyTriggeredBonusGift;

    bool isBonusOnCooldown;

    float bonusFailureCurrent;
    float bonusFailureTotal;




    //every something is merge we add score. the modifier is higher when we create 
    //if you do enough points during the bonustime you gain an ability and a bunch of points.

    void ResetBonus()
    {
        bonusTotal = 60;
        bonusCurrent = 0;
        bonusValue = 1;
        bonusAmount = 0;

        bonusGiftCurrent = 0;
        bonusGiftTotal = 1; //this is just a fake number to work because there is no bunus amount. which is required faor the calculation

        bonusFailureTotal = 20;
        bonusFailureCurrent = 0;

        UIHandler.instance.HardUpdateMergeBonusFill(bonusCurrent, bonusTotal);
        UIHandler.instance.HardUpdateMergeBonusGiftFill(bonusGiftCurrent, bonusGiftTotal);
        UIHandler.instance.UpdateMergeText("Merge More For Bonus!");
    }

    void HandleBonus()
    {
        if (!isBonusHappening)
        {
            if(bonusCurrent > 0)
            {
                bonusCurrent -= Time.deltaTime * 0.8f;
                UIHandler.instance.UpdateMergeBonusFill(bonusCurrent, bonusTotal);
            }
        }
        else
        {
            bonusFailureCurrent += Time.deltaTime;

            if(bonusFailureCurrent > bonusFailureTotal)
            {
                Debug.Log("stop because of this");
                StopBonus();
                bonusFailureCurrent = 0;
            }
        }
        


    }
    void AddBonus(float value)
    {



        if (isBonusOnCooldown)
        {
            return;
        }

        if (isBonusHappening)
        {
            AddBonusGift(value);
            return;
        }

        bonusCurrent += value;
        bonusCurrent = Mathf.Clamp(bonusCurrent, 0, bonusTotal);
        UIHandler.instance.UpdateMergeBonusFill(bonusCurrent, bonusTotal);

        if(bonusCurrent >= bonusTotal)
        {
            StartBonus();
        }
    }

    void AddBonusGift(float amount)
    {
        if (alreadyTriggeredBonusGift) return;

        bonusGiftCurrent += amount;
        bonusGiftCurrent = Mathf.Clamp(bonusGiftCurrent, 0, bonusGiftTotal);
        UIHandler.instance.UpdateMergeBonusGiftFill(bonusGiftCurrent, bonusGiftTotal);

        if (bonusGiftCurrent >= bonusGiftTotal)
        {
            StartBonusGift();

        }
        

    }


    void StartBonusGift()
    {
        //in case we reached this 
        alreadyTriggeredBonusGift = true;
        //we give a random powerup.
        GiveARandomPower();
        StopBonus();

        GameHandler.instance.soundHandler.CreateSFX(giftSoundClip);
        UIHandler.instance.playerUI.GainTextEffect();
    }

    void GiveARandomPower()
    {
        //we need to inform the 
        int roll = UnityEngine.Random.Range(0, 3);

        PowerType power = PowerType.Wipe;

        if(roll == 0)
        {
            power = PowerType.Wipe;
        }
        if(roll == 1)
        {
            power = PowerType.Upgrade;
        }
        if(roll == 2)
        {
            power = PowerType.DestroyTarget;
        }

        UIHandler.instance.inputUI.ChangeEspecificPowerAmmo(power, 1);

    }

    [ContextMenu("DEBUG START BONUS")]
    void StartBonus()
    {


        //make the ui jump a bit.
        //bring the other bar.
        //
        UIHandler.instance.TriggerBonus();
        UIHandler.instance.UpdateMergeText("Merge More For A Gift!");
        bonusAmount += 1;
        bonusGiftTotal = GetCurrentBonusGiftbasedInBonusAmount();

        isBonusHappening = true;
        alreadyTriggeredBonusGift = false;


        GameHandler.instance.soundHandler.ControlBonusAudioSource(true);

        bonusValue = 2;
    }

    

    void StopBonus()
    {
        UIHandler.instance.StopBonus();
        isBonusHappening = false;

        bonusCurrent = 0;
        bonusGiftCurrent = 0;

        UIHandler.instance.UpdateMergeBonusFill(bonusCurrent, bonusTotal);
        UIHandler.instance.UpdateMergeBonusGiftFill(bonusGiftCurrent, bonusGiftTotal);
        UIHandler.instance.UpdateMergeText("Bonus Is In Cooldown!");

        isBonusOnCooldown = true;
        Invoke(nameof(RemoveBonusCooldown), 2f);

        GameHandler.instance.soundHandler.ControlBonusAudioSource(false);
    }

    void RemoveBonusCooldown()
    {
        UIHandler.instance.UpdateMergeText("Merge More For Bonus!");
        isBonusOnCooldown = false;
    }

    float GetCurrentBonusGiftbasedInBonusAmount()
    {
        
        float clampedValue = 100 + (bonusAmount * 25);
        clampedValue = Mathf.Clamp(clampedValue, 20, 150);

        return clampedValue; 
    }







    #endregion

    #region CAMERA

    Vector3 originalCameraPos;
    Camera cam;
    void SetCamera()
    {
        cam = Camera.main;
        originalCameraPos = cam.transform.position; 
    }

    void ShakeCamera()
    {
        StopCoroutine(nameof(ShakeCameraProcess));
        StartCoroutine(ShakeCameraProcess());
    }
    IEnumerator ShakeCameraProcess()
    {
        float offset = 0.035f;

        for (int i = 0; i < 25; i++)
        {
            float randomX = UnityEngine.Random.Range(-offset, offset);
            float randomY = UnityEngine.Random.Range(-offset, offset);

            cam.transform.position = originalCameraPos + new Vector3(randomX, randomY, 0);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        cam.transform.position = originalCameraPos;

    }

    #endregion


    #region DEATH

    //

    int quantityOutside = 0;

    float deathCurrent;
    float deathTotal;

    void SetDeath()
    {
        deathTotal = 3;
    }

    [SerializeField] AudioClip fuseBurningClip;

    //i can only call one fella. but if one is removed the other shouldnt be.
    bool isDeath;
    void HandleDeath()
    {
        if(quantityOutside > 0)
        {

            if (!isDeath)
            {
                isDeath = true;
                GameHandler.instance.soundHandler.ControlDeathAudioSource(true);
            }

            deathCurrent -= Time.deltaTime;
            deathCurrent = Mathf.Clamp(deathCurrent, 0, deathTotal);

        }
        else
        {

            if(deathCurrent > 0)
            {
                GameHandler.instance.soundHandler.ControlDeathAudioSource(false);
            }


            isDeath = false;
            deathCurrent = deathTotal;
        }

        if(deathCurrent <= 0)
        {

            GameHandler.instance.soundHandler.ControlDeathAudioSource(false);
            UIHandler.instance.deathLineUI.StartDeathUI(PlayerHandler.instance.points);
        }

        UIHandler.instance.deathLineUI.UpdateDeathLineBar(deathCurrent, deathTotal);   
        UIHandler.instance.deathLineUI.ControlHolder(quantityOutside > 0);
    }

    [ContextMenu("DEBUG FORCE OUTSIDE")]
    public void AddToOutside()
    {
        quantityOutside++;
    }

    [ContextMenu("DEBUG FORCE REMOVE OUTSIDE")]
    public void RemoveToOutside()
    {
        quantityOutside--;
    }

    

    #endregion

    public void ResetBallHandler()
    {
        ClearEverything();
        ClearQueue();
        cannotControlCurrentBall = true;
        isDeath = false;

        GameHandler.instance.soundHandler.ControlBonusAudioSource(false);
        GameHandler.instance.soundHandler.ControlDeathAudioSource(false);
    }
    public void ClearEverything()
    {
        CanSpawnNextBall = false;

        if (currentMergeBall != null)
        {
            currentMergeBall.DestructionEffect();
            currentMergeBall = null;
        }

        for (int i = 0; i < ballContainer.transform.childCount; i++)
        {
           MergeBall ball = ballContainer.GetChild(i).GetComponent<MergeBall>();
            
            if(ball == null) continue;

            ball.DestructionEffect();
        }

    }
    public void StartGameAgain()
    {

        CanSpawnNextBall = true;
        cannotControlCurrentBall = false;
    }


    public void DestroyAllObjectsAboveACertainY()
    {
        //dsetryo everyone above 500 y
        cannotControlCurrentBall = true;


        for (int i = 0; i < ballContainer.childCount; i++)
        {
            if(ballContainer.transform.GetChild(i).transform.localPosition.y > 500)
            {
                MergeBall ball = ballContainer.transform.GetChild(i).GetComponent<MergeBall>();

                if (ball == null) continue;

                if (!ball.isDropped) continue;



                ball.DestructionEffect();
            }
        }

    }

}

public enum OrderWhoType
{
    All,
    AllBelow,
    AllAbove,
    Only

}
public enum OrderWhatType
{
    Destroy,
    Upgrade,
    StartTargetUI,
    StopTargetUI

}