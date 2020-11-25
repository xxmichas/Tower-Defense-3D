using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject ShopMenu;
    private bool IsShopOpened = false;
    private bool IsTowerMenuOpened = false;

    public Transform PopUpLocation;
    public GameObject NotEnoughMoneyPopUp;
    public GameObject CooldownPopUp;

    public Transform TowerSpawnLocation;
    public Transform Towers;

    public GameObject Tower1;
    public GameObject Tower2;
    public GameObject Tower3;
    public GameObject Tower4;
    public GameObject Tower5;
    public GameObject AuraTower2;
    public GameObject SupportTower1;
    public GameObject SupportTower2;

    public GameObject Tower1Upgraded;
    public GameObject Tower2Upgraded;
    public GameObject Tower3Upgraded;
    public GameObject Tower4Upgraded;
    public GameObject Tower5Upgraded;
    public GameObject AuraTower2Upgraded;
    public GameObject SupportTower1Upgraded;
    public GameObject SupportTower2Upgraded;

    //Upgrade values
    private string Tower1UpDamage = "100";
    private string Tower1UpAttackRate = "1.5/s";
    private string Tower2UpDamage = "200";
    private string Tower2UpAttackRate = "1/s";
    private string Tower3UpDamage = "4";
    private string Tower3UpAttackRate = "50/s";
    private string Tower4UpDamage = "50";
    private string Tower4UpAttackRate = "0.5/s";
    private string Tower5UpDamage = "3";
    private string Tower5UpAttackRate = "50/s";
    private string AuraTower2UpDamage = "0";
    private string AuraTower2UpAttackRate = "const";
    private string SupportTower1UpDamage = "0";
    private string SupportTower1UpAttackRate = "const";
    private string SupportTower2UpDamage = "0";
    private string SupportTower2UpAttackRate = "const";

    //Upgrade cost is always TowerCost * 1.5
    private int Tower1Cost = 100;
    private int Tower2Cost = 250;
    private int Tower3Cost = 150;
    private int Tower4Cost = 300;
    private int Tower5Cost = 150;
    private int AuraTower2Cost = 200;
    private int SupportTower1Cost = 100;
    private int SupportTower2Cost = 200;

    public GameObject TowerMenu;

    public GameObject Tower1Grid;
    public GameObject Tower2Grid;
    public GameObject Tower3Grid;
    public GameObject Tower4Grid;
    public GameObject Tower5Grid;
    public GameObject AuraTower2Grid;
    public GameObject SupportTower1Grid;
    public GameObject SupportTower2Grid;

    public Transform TowerMenuContents;

    private List<GameObject> TowersSpawned = new List<GameObject>();
    // Update is called once per frame
    void Start()
    {
        ShopMenu.SetActive(false);
        TowerMenu.SetActive(false);
    }
    void Update()
    {
        if (!GameManager.Victory && !Stats.LevelLost && !GameManager.GameIsPaused)
        {
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ShopKey", "E"))))
            {
                if (IsShopOpened)
                {
                    CloseShop();
                }
                else
                {
                    CloseTowerMenu();
                    OpenShop();
                }
            }

            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TowerMenuKey", "R"))))
            {
                if (IsTowerMenuOpened)
                {
                    CloseTowerMenu();
                }
                else
                {
                    CloseShop();
                    OpenTowerMenu();
                }
            }
        }
    }

    public void OpenShop()
    {
        ShopMenu.SetActive(true);
        IsShopOpened = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        MouseLook.PauseCameraMovement = true;
    }

    public void CloseShop()
    {
        ShopMenu.SetActive(false);
        IsShopOpened = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MouseLook.PauseCameraMovement = false;
    }

    private void NotEnoughMoney()
    {
        Instantiate(NotEnoughMoneyPopUp, PopUpLocation.position, PopUpLocation.rotation, PopUpLocation);
    }

    public void Buy1()
    {
        if (Stats.Money >= Tower1Cost) {
            TowersSpawned.Add(Instantiate(Tower1, TowerSpawnLocation.position, TowerSpawnLocation.rotation, Towers));
            GameObject Temp = Instantiate(Tower1Grid, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), TowerMenuContents);

            int Cost = Tower1Cost;
            Stats.Money -= Tower1Cost;
            GameObject UpgradePrefab = Tower1Upgraded;
            string UpDamage = Tower1UpDamage;
            string UpAttackRate = Tower1UpAttackRate;

            GameObject TempSell = Temp.transform.Find("TowerContents").transform.Find("Sell").gameObject;
            TempSell.GetComponent<Button>().onClick.AddListener(delegate { Sell(TempSell, Cost); });

            GameObject TempUpgrade = Temp.transform.Find("TowerContents").transform.Find("Upgrade").gameObject;
            TempUpgrade.GetComponent<Button>().onClick.AddListener(delegate { Upgrade(TempUpgrade, Cost, UpgradePrefab, UpDamage, UpAttackRate); });
        }
        else
        {
            if (PopUpLocation.childCount > 0)
            {
                Destroy(PopUpLocation.GetChild(0).gameObject);
            }
            NotEnoughMoney();
        }
    }
    public void Buy2()
    {
        if (Stats.Money >= Tower2Cost)
        {
            //Spawns the tower and adds it to the list
            TowersSpawned.Add(Instantiate(Tower2, TowerSpawnLocation.position, TowerSpawnLocation.rotation, Towers));
            //Adds a grid to tower menu
            GameObject Temp = Instantiate(Tower2Grid, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), TowerMenuContents);
            //Subtracts the Cost money and set correct values for upgrade and sell buttons
            int Cost = Tower2Cost;
            Stats.Money -= Tower2Cost;
            GameObject UpgradePrefab = Tower2Upgraded;
            string UpDamage = Tower2UpDamage;
            string UpAttackRate = Tower2UpAttackRate;
            //Adds an onClick function to a Sell button
            GameObject TempSell = Temp.transform.Find("TowerContents").transform.Find("Sell").gameObject;
            TempSell.GetComponent<Button>().onClick.AddListener(delegate { Sell(TempSell, Cost); });
            //Adds an onClick function to an Upgrade button
            GameObject TempUpgrade = Temp.transform.Find("TowerContents").transform.Find("Upgrade").gameObject;
            TempUpgrade.GetComponent<Button>().onClick.AddListener(delegate { Upgrade(TempUpgrade, Cost, UpgradePrefab, UpDamage, UpAttackRate); });
        }
        else
        {
            if (PopUpLocation.childCount > 0)
            {
                Destroy(PopUpLocation.GetChild(0).gameObject);
            }
            NotEnoughMoney();
        }
    }
    public void Buy3()
    {
        if (Stats.Money >= Tower3Cost)
        {
            TowersSpawned.Add(Instantiate(Tower3, TowerSpawnLocation.position, TowerSpawnLocation.rotation, Towers));
            GameObject Temp = Instantiate(Tower3Grid, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), TowerMenuContents);

            int Cost = Tower3Cost;
            Stats.Money -= Tower3Cost;
            GameObject UpgradePrefab = Tower3Upgraded;
            string UpDamage = Tower3UpDamage;
            string UpAttackRate = Tower3UpAttackRate;

            GameObject TempSell = Temp.transform.Find("TowerContents").transform.Find("Sell").gameObject;
            TempSell.GetComponent<Button>().onClick.AddListener(delegate { Sell(TempSell, Cost); });

            GameObject TempUpgrade = Temp.transform.Find("TowerContents").transform.Find("Upgrade").gameObject;
            TempUpgrade.GetComponent<Button>().onClick.AddListener(delegate { Upgrade(TempUpgrade, Cost, UpgradePrefab, UpDamage, UpAttackRate); });
        }
        else
        {
            if (PopUpLocation.childCount > 0)
            {
                Destroy(PopUpLocation.GetChild(0).gameObject);
            }
            NotEnoughMoney();
        }
    }
    public void Buy4()
    {
        if (Stats.Money >= Tower5Cost)
        {
            TowersSpawned.Add(Instantiate(Tower5, TowerSpawnLocation.position, TowerSpawnLocation.rotation, Towers));
            GameObject Temp = Instantiate(Tower5Grid, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), TowerMenuContents);

            int Cost = Tower5Cost;
            Stats.Money -= Tower5Cost;
            GameObject UpgradePrefab = Tower5Upgraded;
            string UpDamage = Tower5UpDamage;
            string UpAttackRate = Tower5UpAttackRate;

            GameObject TempSell = Temp.transform.Find("TowerContents").transform.Find("Sell").gameObject;
            TempSell.GetComponent<Button>().onClick.AddListener(delegate { Sell(TempSell, Cost); });

            GameObject TempUpgrade = Temp.transform.Find("TowerContents").transform.Find("Upgrade").gameObject;
            TempUpgrade.GetComponent<Button>().onClick.AddListener(delegate { Upgrade(TempUpgrade, Cost, UpgradePrefab, UpDamage, UpAttackRate); });
        }
        else
        {
            if (PopUpLocation.childCount > 0)
            {
                Destroy(PopUpLocation.GetChild(0).gameObject);
            }
            NotEnoughMoney();
        }
    }
    public void Buy5()
    {
        if (Stats.Money >= AuraTower2Cost)
        {
            TowersSpawned.Add(Instantiate(AuraTower2, TowerSpawnLocation.position, TowerSpawnLocation.rotation, Towers));
            GameObject Temp = Instantiate(AuraTower2Grid, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), TowerMenuContents);

            int Cost = AuraTower2Cost;
            Stats.Money -= AuraTower2Cost;
            GameObject UpgradePrefab = AuraTower2Upgraded;
            string UpDamage = AuraTower2UpDamage;
            string UpAttackRate = AuraTower2UpAttackRate;

            GameObject TempSell = Temp.transform.Find("TowerContents").transform.Find("Sell").gameObject;
            TempSell.GetComponent<Button>().onClick.AddListener(delegate { Sell(TempSell, Cost); });

            GameObject TempUpgrade = Temp.transform.Find("TowerContents").transform.Find("Upgrade").gameObject;
            TempUpgrade.GetComponent<Button>().onClick.AddListener(delegate { Upgrade(TempUpgrade, Cost, UpgradePrefab, UpDamage, UpAttackRate); });
        }
        else
        {
            if (PopUpLocation.childCount > 0)
            {
                Destroy(PopUpLocation.GetChild(0).gameObject);
            }
            NotEnoughMoney();
        }
    }
    public void Buy6()
    {
        if (Stats.Money >= Tower4Cost)
        {
            TowersSpawned.Add(Instantiate(Tower4, TowerSpawnLocation.position, TowerSpawnLocation.rotation, Towers));
            GameObject Temp = Instantiate(Tower4Grid, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), TowerMenuContents);

            int Cost = Tower4Cost;
            Stats.Money -= Tower4Cost;
            GameObject UpgradePrefab = Tower4Upgraded;
            string UpDamage = Tower4UpDamage;
            string UpAttackRate = Tower4UpAttackRate;

            GameObject TempSell = Temp.transform.Find("TowerContents").transform.Find("Sell").gameObject;
            TempSell.GetComponent<Button>().onClick.AddListener(delegate { Sell(TempSell, Cost); });

            GameObject TempUpgrade = Temp.transform.Find("TowerContents").transform.Find("Upgrade").gameObject;
            TempUpgrade.GetComponent<Button>().onClick.AddListener(delegate { Upgrade(TempUpgrade, Cost, UpgradePrefab, UpDamage, UpAttackRate); });
        }
        else
        {
            if (PopUpLocation.childCount > 0)
            {
                Destroy(PopUpLocation.GetChild(0).gameObject);
            }
            NotEnoughMoney();
        }
    }
    public void Buy7()
    {
        if (TowersSpawned.Find(x => x.name.Contains("SupportTower1")))
        {
            GameObject PopUp = Instantiate(CooldownPopUp, PopUpLocation.position, PopUpLocation.rotation, PopUpLocation);
            PopUp.transform.Find("Text").GetComponent<Text>().text = "You can only purchase\n1 Cooldown Tower";
        }
        else
        {
            if (Stats.Money >= SupportTower1Cost)
            {
                TowersSpawned.Add(Instantiate(SupportTower1, TowerSpawnLocation.position, TowerSpawnLocation.rotation, Towers));
                GameObject Temp = Instantiate(SupportTower1Grid, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), TowerMenuContents);

                int Cost = SupportTower1Cost;
                Stats.Money -= SupportTower1Cost;
                GameObject UpgradePrefab = SupportTower1Upgraded;
                string UpDamage = SupportTower1UpDamage;
                string UpAttackRate = SupportTower1UpAttackRate;

                GameObject TempSell = Temp.transform.Find("TowerContents").transform.Find("Sell").gameObject;
                TempSell.GetComponent<Button>().onClick.AddListener(delegate { Sell(TempSell, Cost); });

                GameObject TempUpgrade = Temp.transform.Find("TowerContents").transform.Find("Upgrade").gameObject;
                TempUpgrade.GetComponent<Button>().onClick.AddListener(delegate { Upgrade(TempUpgrade, Cost, UpgradePrefab, UpDamage, UpAttackRate); });
            }
            else
            {
                if (PopUpLocation.childCount > 0)
                {
                    Destroy(PopUpLocation.GetChild(0).gameObject);
                }
                NotEnoughMoney();
            }
        }
    }
    public void Buy8()
    {
        if (TowersSpawned.Find(x => x.name.Contains("SupportTower2")))
        {
            GameObject PopUp = Instantiate(CooldownPopUp, PopUpLocation.position, PopUpLocation.rotation, PopUpLocation);
            PopUp.transform.Find("Text").GetComponent<Text>().text = "You can only purchase\n1 Money Tower";
        }
        else
        {
            if (Stats.Money >= SupportTower2Cost)
            {
                TowersSpawned.Add(Instantiate(SupportTower2, TowerSpawnLocation.position, TowerSpawnLocation.rotation, Towers));
                GameObject Temp = Instantiate(SupportTower2Grid, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), TowerMenuContents);

                int Cost = SupportTower2Cost;
                Stats.Money -= SupportTower2Cost;
                GameObject UpgradePrefab = SupportTower2Upgraded;
                string UpDamage = SupportTower2UpDamage;
                string UpAttackRate = SupportTower2UpAttackRate;

                GameObject TempSell = Temp.transform.Find("TowerContents").transform.Find("Sell").gameObject;
                TempSell.GetComponent<Button>().onClick.AddListener(delegate { Sell(TempSell, Cost); });

                GameObject TempUpgrade = Temp.transform.Find("TowerContents").transform.Find("Upgrade").gameObject;
                TempUpgrade.GetComponent<Button>().onClick.AddListener(delegate { Upgrade(TempUpgrade, Cost, UpgradePrefab, UpDamage, UpAttackRate); });
            }
            else
            {
                if (PopUpLocation.childCount > 0)
                {
                    Destroy(PopUpLocation.GetChild(0).gameObject);
                }
                NotEnoughMoney();
            }
        }
    }

    public void OpenTowerMenu()
    {
        TowerMenu.SetActive(true);
        IsTowerMenuOpened = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        MouseLook.PauseCameraMovement = true;
    }

    public void CloseTowerMenu()
    {
        TowerMenu.SetActive(false);
        IsTowerMenuOpened = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MouseLook.PauseCameraMovement = false;
    }

    public void Sell(GameObject x, int Cost)
    {
        Transform TowerGrid = x.transform.parent.parent;

        Destroy(TowersSpawned[TowerGrid.GetSiblingIndex()]);
        TowersSpawned.RemoveAt(TowerGrid.GetSiblingIndex());
        Destroy(TowerGrid.gameObject);

        Stats.Money += (Cost / 2);
    }

    public void Upgrade(GameObject x, int Cost, GameObject Upgrade, string UpDamage, string UpAttackRate)
    {
        if (Stats.Money >= (Cost * 3 / 2))
        {
            //Get old tower position
            Transform TowerPosition = TowersSpawned[x.transform.parent.parent.GetSiblingIndex()].transform;
            //Destroy the old tower
            Destroy(TowersSpawned[x.transform.parent.parent.GetSiblingIndex()]);
            //Create a new tower and replace old tower's position in the list with it
            TowersSpawned[x.transform.parent.parent.GetSiblingIndex()] = Instantiate(Upgrade, TowerPosition.position + new Vector3(0, 1, 0), TowerPosition.rotation, Towers);
            //Disable the upgrade button since we already upgraded the tower
            x.GetComponent<Button>().enabled = false;
            x.SetActive(false);
            //Increase Sell value of upgraded tower
            x.transform.parent.transform.Find("Sell").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            x.transform.parent.transform.Find("Sell").gameObject.GetComponent<Button>().onClick.AddListener(delegate { Sell(x, (Cost * 3 / 2)); });
            //Update the stats on the grid
            x.transform.parent.transform.Find("TowerName").GetComponent<Text>().text += " (upgraded)";
            x.transform.parent.transform.Find("TowerName").GetComponent<Text>().color = new Color(0f, 0.82f, 0.4f);
            x.transform.parent.transform.Find("Damage").GetChild(0).gameObject.GetComponent<Text>().text = UpDamage;
            x.transform.parent.transform.Find("AttackRate").GetChild(0).gameObject.GetComponent<Text>().text = UpAttackRate;
            x.transform.parent.transform.Find("Cost").GetChild(0).gameObject.GetComponent<Text>().text = (Cost * 3 / 2 / 2).ToString();
            x.transform.parent.transform.Find("UpgradeCost").gameObject.SetActive(false);

            Stats.Money -= (Cost * 3 / 2);
        }
        else
        {
            if (PopUpLocation.childCount > 0)
            {
                Destroy(PopUpLocation.GetChild(0).gameObject);
            }
            NotEnoughMoney();
        }
    }
}