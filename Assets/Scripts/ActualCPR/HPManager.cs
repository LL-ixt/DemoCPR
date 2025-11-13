using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    [Header("UI References")]
    public Image hpBarFill;
    [Header("HP Settings")]
    public float currentHP = 50f;
    public const float maxHP = 100f;
    [Header("Game Settings")]
    public float duration = 120f;
    private float timer = 0f;
    private bool gameOver = true;
    public EndingController endingController;
    public SceneTransitionManager sceneTransitionManager;
    public ActualManager actualManager;
    void OnEnable()
    {
        UpdateHPUI();
        gameOver = false;
    }

    void Update()
    {
        if (gameOver) return; 

        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer -= 1f;
            AddHP(-1f);
        }

        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            AddHP(-100f);
        }
    }

    public void AddHP(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);
        UpdateHPUI();

        if (currentHP >= maxHP)
        {
            EndGame(true);
        }
        else if (currentHP <= 0f)
        {
            EndGame(false);
        }
    }

    private void UpdateHPUI()
    {
        hpBarFill.fillAmount = currentHP / maxHP;
    }

    void EndGame(bool isWin)
    {
        actualManager.GoAway();
        gameOver = true;
        if (isWin)
        {
            endingController.PlayWakeUp();
        }
        else
        {
            sceneTransitionManager.GoToSceneAsync("ResultLose");
        }
    }
}

