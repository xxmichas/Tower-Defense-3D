using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public Text MoneyText;
    public Text LivesText;

    public static int Lives;
    public static int Money;

    public static int MoneyBonus;

    [Header("Level Money and Lives")]

    //Zmieniamy te wartości w edytorze w zależności od poziomu
    public int levelMoney = 0;
    public int levelLives = 0;

    public static bool LevelLost;
    // Start is called before the first frame update
    void Start()
    {
        LevelLost = false;

        Money = levelMoney;
        Lives = levelLives;

        MoneyBonus = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        MoneyText.text = Money + "$";
        LivesText.text = "Lives: " + Lives;

        if (Lives <= 0 && !LevelLost)
        {
            GameManager.GameOver = true;
            LevelLost = true;
        }
    }
}
