using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIScript : MonoBehaviour
{
    public static int level = 0;
    public static bool nightMode = false;
    public static int vehicle = 0;
    public Sprite[] levelImages;
    public Sprite[] levelNightImages;
    public GameObject levelImage;
    public Sprite[] vehicleImages;
    public GameObject vehicleImage;
    public GameObject levelNext, levelPrev, vehicleNext, vehiclePrevious;
    public GameObject pausePanel;
    public Slider fuelBar;
    public Image fuelFill;
    public GameObject GameOverPanel;
    public TextMeshProUGUI scoreLabel, highScoreLabel;
    bool vehicleMode = false, levelMode = false;
    public TextMeshProUGUI airTimeLabel;
    public static int countryBest = 0, iceBest = 0, moonBest = 0, rockyBest = 0;
    float airTime = -1;
    bool saved;
    public GameObject Fog;
    public GameObject toggleBut;
    public Sprite sun, moon;
    public GameObject engine, suspension, tyre, engineButton, tyreButton, suspensionButton;
    public static int[] engineLevel, suspensionLevel, tyreLevel;
    public static bool[] vehicleBought,levelBought;
    public Animator tuneAnim;
    public TextMeshProUGUI diamondText, cointText;
    SaveData loadData;
    public Button vehicleBuyBut,playButton,tuneButton, levelBuyBut,racePlayButton;
    public static bool raceMode = false;
    public TextMeshProUGUI gameOverMessage;





    private void Start()
    {
        loadData = FileSystem.loadData();
        
        


        if (engine != null)
        {
            if (loadData != null)
            {
                engineLevel = loadData.engineLevel;
                suspensionLevel = loadData.suspensionLevel;
                tyreLevel = loadData.tyreLevel;
                vehicleBought = loadData.vehicleBought;
                levelBought = loadData.levelBought;
            }
            else
            {
                vehicleBought = new bool[3];
                vehicleBought[0] = true;
                vehicleBought[1] = false;
                vehicleBought[2] = false;
                levelBought = new bool[4];
                levelBought[0] = true;
                levelBought[1] = false;
                levelBought[2] = false;
                levelBought[3] = false;
                engineLevel = new int[3];
                suspensionLevel = new int[3];
                tyreLevel = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    engineLevel[i] = 0;
                    tyreLevel[i] = 0;
                    suspensionLevel[i] = 0;
                }
            }
        }
        if (engine != null)
        {
            engine.GetComponentInChildren<TextMeshProUGUI>().SetText((engineLevel[vehicle] + 1).ToString());
            suspension.GetComponentInChildren<TextMeshProUGUI>().SetText((suspensionLevel[vehicle] + 1).ToString());
            tyre.GetComponentInChildren<TextMeshProUGUI>().SetText((tyreLevel[vehicle] + 1).ToString());
        }
            saved = false;
        if (Fog != null)
        {
            Fog.SetActive(!nightMode);

        }
        
       
        if (loadData != null)
        {
            CollectibleControl.coinCount = loadData.coins;
            CollectibleControl.diamondCount = loadData.diamonds;
            countryBest = loadData.countryBest;
            iceBest = loadData.iceBest;
            moonBest = loadData.moonBest;
            rockyBest = loadData.rockyBest;
        }
        if (highScoreLabel != null)
        {

            switch (level)
            {
                case 0:
                    highScoreLabel.SetText("BEST:" + countryBest);
                    break;
                case 1:
                    highScoreLabel.SetText("BEST:" + iceBest);
                    break;
                case 2:
                    highScoreLabel.SetText("BEST:" + moonBest);
                    break;
                case 3:
                    highScoreLabel.SetText("BEST:" + rockyBest);
                    break;
            }
        }
        if (fuelBar != null)
        {
            fuelBar.maxValue = 100;
            fuelBar.minValue = 0;

        }
        if (levelImage != null)
        {
            toggleBut.SetActive(level==0||level==1);
            if (nightMode)
                toggleBut.GetComponent<Image>().sprite = moon;
            else
                toggleBut.GetComponent<Image>().sprite = sun;

           
            if(!nightMode)
            levelImage.GetComponent<Image>().sprite = levelImages[level];
            else
            {
                levelImage.GetComponent<Image>().sprite = levelNightImages[level];
            }
        }
        if (vehicleImage != null)
        {
            vehicleImage.GetComponent<Image>().sprite = vehicleImages[vehicle];
            
        }
        }
    private void FixedUpdate()
    {
        if (diamondText != null)
        {

            diamondText.SetText(CollectibleControl.diamondCount.ToString());
            cointText.SetText(CollectibleControl.coinCount.ToString());

        }
        if (fuelBar != null)
        {
            if (!CarControl.gameOver)
                scoreLabel.SetText("Score: " + (int)(CarControl.currentBaseScore + CarControl.bonusScore));
            if (CarControl.airTime < 0 && airTime > 40 * 0.7f)
            {
                airTimeLabel.GetComponent<Animator>().SetTrigger("AirTime");
                airTimeLabel.SetText("Air Time: " + (int)(airTime));
                GetComponent<AudioSource>().Play();
                CarControl.bonusScore += airTime;
            }


            if (CarControl.fuelAmt >= 0)
            {
                fuelBar.value = CarControl.fuelAmt;
                fuelFill.color = Color.Lerp(Color.red, Color.green, fuelBar.value / fuelBar.maxValue);



            }
            airTime = CarControl.airTime * 40;
        }
    }
    public void Play(bool _raceMode)
    {
        FileSystem.saveData();
        GetComponent<AudioSource>().Play();
        TerrainGenerator.amplitude = 2;
        TerrainGenerator.width = 0.2f;
        SceneManager.LoadScene(level + 1);
        raceMode = _raceMode;
    }
    
    public void levelChange(int change)
    {
        GetComponent<AudioSource>().Play();
        level = level + change;
        toggleBut.SetActive(level != 3 && level != 2);
    
        if (level == 3 || level == 2)
        {
            nightMode = false;
            
        }
        if (level >= levelImages.Length)
        {
            level = 0;
        }
        if (level < 0)
        {
            level = levelImages.Length - 1;
        }
        if (!nightMode)
        {
            levelImage.GetComponent<Image>().sprite = levelImages[level];
        }
        else
        {
            levelImage.GetComponent<Image>().sprite = levelNightImages[level];
        }
        }

    public void vehicleChange(int change)
    {
        GetComponent<AudioSource>().Play();
        vehicle = vehicle + change;
        if (vehicle >= vehicleImages.Length)
        {
            vehicle = 0;
        }
        if (vehicle < 0)
        {
            vehicle = vehicleImages.Length - 1;
        }
        
        vehicleImage.GetComponent<Image>().sprite = vehicleImages[vehicle];

    }
    public void levelBut()
    {
        tuneAnim.SetBool("modMode", false);
        GetComponent<AudioSource>().Play();
        if (vehicleMode)
        {
            vehicleMode = false;
            vehicleNext.GetComponent<Animator>().SetTrigger("ButTrigger");
            vehiclePrevious.GetComponent<Animator>().SetTrigger("ButTrigger");
        }
    
        if (!levelMode)
        {
            levelNext.GetComponent<Animator>().SetTrigger("ButTrigger");
            levelPrev.GetComponent<Animator>().SetTrigger("ButTrigger");
            
            levelMode = true;
        }

    }
    public void vehicleBut()
    {
        tuneAnim.SetBool("modMode", false);
        GetComponent<AudioSource>().Play();
        if (levelMode)
        {
            levelMode = false;
            levelNext.GetComponent<Animator>().SetTrigger("ButTrigger");
            levelPrev.GetComponent<Animator>().SetTrigger("ButTrigger");
        }
        
        if (!vehicleMode)
        {
            vehicleNext.GetComponent<Animator>().SetTrigger("ButTrigger");
            vehiclePrevious.GetComponent<Animator>().SetTrigger("ButTrigger");
            vehicleMode = true;
        }
    }
    public void pauseGame()
    {
        GetComponents<AudioSource>()[1].Play();
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().mute = true;
        FileSystem.saveData();
    }
    public void resume()
    {
        GetComponents<AudioSource>()[1].Play();
        GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().mute = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);

    }
    public void restart()
    {
        GetComponents<AudioSource>()[1].Play();
        CarControl.fuelAmt = 100;
        GameOverPanel.SetActive(false);
        CarControl.gameOver = false;
        GameOverPanel.GetComponent<Animator>().SetBool("gameover", false);
        CarControl.gameOver = false;
        TerrainGenerator.amplitude = 2;
        TerrainGenerator.width = 0.2f;
        CarControl.terrainPoint = Vector3.zero;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    
       

    }
    bool over = false;
    private void Update()
    {


        if (engine != null)
        {
            
            levelBuyBut.gameObject.SetActive(!levelBought[level]);
            vehicleBuyBut.gameObject.SetActive(!vehicleBought[vehicle]);
            cointText.color = Color.Lerp(cointText.color, Color.white, Time.deltaTime);
            diamondText.color = Color.Lerp(diamondText.color, Color.white, Time.deltaTime);
            playButton.interactable = vehicleBought[vehicle]&&levelBought[level];
            racePlayButton.interactable = vehicleBought[vehicle] && levelBought[level];
            tuneButton.interactable = vehicleBought[vehicle];
            engine.GetComponentInChildren<TextMeshProUGUI>().SetText((engineLevel[vehicle] + 1).ToString());
            suspension.GetComponentInChildren<TextMeshProUGUI>().SetText((suspensionLevel[vehicle] + 1).ToString());
            tyre.GetComponentInChildren<TextMeshProUGUI>().SetText((tyreLevel[vehicle] + 1).ToString());
            engineButton.GetComponentInChildren<TextMeshProUGUI>().SetText((4000*(engineLevel[vehicle]+1)).ToString());
            tyreButton.GetComponentInChildren<TextMeshProUGUI>().SetText((4000 * (tyreLevel[vehicle] + 1)).ToString());
            suspensionButton.GetComponentInChildren<TextMeshProUGUI>().SetText((4000 * (suspensionLevel[vehicle] + 1)).ToString());
            engineButton.GetComponent<Image>().color = new Color(255, 214, 0);
            suspensionButton.GetComponent<Image>().color = new Color(255, 214, 0);
            tyreButton.GetComponent<Image>().color = new Color(255, 214, 0);
            if (tyreLevel[vehicle] == 2)
            {
                tyreButton.GetComponentInChildren<TextMeshProUGUI>().SetText("");
                tyreButton.GetComponent<Image>().color = new Color(60, 60, 60);
            }
            if(engineLevel[vehicle] == 2)
            {
                engineButton.GetComponentInChildren<TextMeshProUGUI>().SetText("");
                engineButton.GetComponent<Image>().color = new Color(60, 60, 60);
            }
            if (suspensionLevel[vehicle] == 2)
            {
                suspensionButton.GetComponentInChildren<TextMeshProUGUI>().SetText("");
                suspensionButton.GetComponent<Image>().color = new Color(60, 60, 60);
            }
            
            
            vehicleBuyBut.GetComponentInChildren<TextMeshProUGUI>().SetText((2500 * vehicle).ToString());
            levelBuyBut.GetComponentInChildren<TextMeshProUGUI>().SetText((3500 * level).ToString());
            if (2500 * vehicle > CollectibleControl.diamondCount)
            {
                vehicleBuyBut.GetComponent<Image>().color = Color.red;
            }
            else
            {
                vehicleBuyBut.GetComponent<Image>().color = new Color(255, 214, 0);
            }
            if (4000 * (engineLevel[vehicle] + 1) <= CollectibleControl.coinCount)
            {
                engineButton.GetComponent<Image>().color = new Color(255, 214, 0);
            }
            else
            {
                engineButton.GetComponent<Image>().color = new Color(255, 0, 0);
            }
            if (4000 * (suspensionLevel[vehicle] + 1) <= CollectibleControl.coinCount)
            {
                suspensionButton.GetComponent<Image>().color = new Color(255, 214, 0);
            }
            else
            {
                suspensionButton.GetComponent<Image>().color = new Color(255, 0, 0);
            }
            if (4000 * (tyreLevel[vehicle] + 1) <= CollectibleControl.coinCount)
            {
                tyreButton.GetComponent<Image>().color = new Color(255, 214, 0);
            }
            else
            {
                tyreButton.GetComponent<Image>().color = new Color(255, 0, 0);
            }
            
        }
        if (CarControl.gameOver)
        {
         
            GameOverPanel.SetActive(true);
            GameOverPanel.GetComponent<Animator>().SetBool("gameover", true);
            if (!saved)
            {
                switch (level) {
                    case 0:
                    if (CarControl.currentBaseScore + CarControl.bonusScore > countryBest)
                        {
                            countryBest = (int)(CarControl.currentBaseScore + CarControl.bonusScore);
                            highScoreLabel.SetText("BEST: " + (int)(CarControl.currentBaseScore + CarControl.bonusScore));
                        }
                        break;
                    case 1:
                        if (CarControl.currentBaseScore + CarControl.bonusScore > iceBest)
                        {
                            
                            iceBest = (int)(CarControl.currentBaseScore + CarControl.bonusScore);
                            highScoreLabel.SetText("BEST: " + (int)(CarControl.currentBaseScore + CarControl.bonusScore));
                        }
                        break;
                    case 2:
                        if (CarControl.currentBaseScore + CarControl.bonusScore > moonBest)
                        {
                            moonBest = (int)(CarControl.currentBaseScore + CarControl.bonusScore);
                            highScoreLabel.SetText("BEST: " + (int)(CarControl.currentBaseScore + CarControl.bonusScore));
                        }
                        break;
                    case 3:
                        if (CarControl.currentBaseScore + CarControl.bonusScore > rockyBest)
                        {
                            rockyBest = (int)(CarControl.currentBaseScore + CarControl.bonusScore);
                            highScoreLabel.SetText("BEST: " + (int)(CarControl.currentBaseScore + CarControl.bonusScore));
                        }
                        break;


                }

       
                FileSystem.saveData();
                saved = true;
            }
            //Time.timeScale = 0;
          
        }
        if (raceMode)
        {
            over = true;
            if (FinishScript.win == 0 && BotController.botDead)
            {
                gameOverMessage.SetText("DRAW");
            }
            else if(FinishScript.win==1)
            {
                gameOverMessage.SetText("YOU WON");
            }
            else
            {
                gameOverMessage.SetText("YOU LOST");
            }
        }
    }
    public void exit()
    {

        CarControl.gameOver = false;
        CarControl.fuelAmt = 100;
        TerrainGenerator.amplitude = 2;
        TerrainGenerator.width = 0.2f;
        CarControl.terrainPoint = Vector3.zero;
        Time.timeScale = 1;
        GameOverPanel.GetComponent<Animator>().SetBool("gameover", false);

        SceneManager.LoadScene(0);
    }
    public void setMove(int mov)
    {
        CarControl.move = mov;
    }
    public void toggleDayNight()
    {
        GetComponent<AudioSource>().Play();
        nightMode = !nightMode;
        if (nightMode)
        {
            levelImage.GetComponent<Image>().sprite = levelNightImages[level];
            toggleBut.GetComponent<Image>().sprite = moon;
        }
        else
        {
            levelImage.GetComponent<Image>().sprite = levelImages[level];
            toggleBut.GetComponent<Image>().sprite = sun;
        }
    }

    public void engineMod()
    {
        if (engineLevel[vehicle] < 2)
        {
            if (CollectibleControl.coinCount > 4000 * (engineLevel[vehicle] + 1))
            {
                engineLevel[vehicle]++;
                CollectibleControl.coinCount -= 4000 * (engineLevel[vehicle]);
                GetComponents<AudioSource>()[1].Play();
            }
            else
            {
                GetComponents<AudioSource>()[0].Play();
                cointText.color = Color.red;
                
            }
            FileSystem.saveData();
        }
       
    }
    public void suspensionMod()
    {
        if (suspensionLevel[vehicle] < 2)
        {
            if (CollectibleControl.coinCount > 4000 * (suspensionLevel[vehicle] + 1))
            {
                suspensionLevel[vehicle]++;
                CollectibleControl.coinCount -= 4000 * (suspensionLevel[vehicle]);
                GetComponents<AudioSource>()[1].Play();

              
                FileSystem.saveData();
            }
            else
            {
                GetComponents<AudioSource>()[0].Play();
                cointText.color = Color.red;

            }
        }
       
    }
    public void tyreMod()
    {


        if (tyreLevel[vehicle] < 2)
        {
            if (CollectibleControl.coinCount > 4000 * (tyreLevel[vehicle] + 1))
            {
                tyreLevel[vehicle]++;
                CollectibleControl.coinCount -= 4000 * (tyreLevel[vehicle]);
                GetComponents<AudioSource>()[1].Play();

                FileSystem.saveData();
            }
            else
            {
                GetComponents<AudioSource>()[0].Play();
                cointText.color = Color.red;

            }
        }
       
    }
    public void perfomanceBut()
    {
        
        GetComponent<AudioSource>().Play();
        tuneAnim.SetBool("modMode", true);
        if (vehicleMode)
        {
            vehicleMode = false;
            vehicleNext.GetComponent<Animator>().SetTrigger("ButTrigger");
            vehiclePrevious.GetComponent<Animator>().SetTrigger("ButTrigger");
        }
        if (levelMode)
        {
            levelMode = false;
            levelNext.GetComponent<Animator>().SetTrigger("ButTrigger");
            levelPrev.GetComponent<Animator>().SetTrigger("ButTrigger");
        }
    }

    public void buyVehicle()
    {
        if (!vehicleBought[vehicle] && CollectibleControl.diamondCount >= vehicle * 2500)
        {
            CollectibleControl.diamondCount -= vehicle * 2500;
            vehicleBought[vehicle] = true;
            FileSystem.saveData(); 
            GetComponents<AudioSource>()[1].Play();
        }
        else
        {
            GetComponents<AudioSource>()[0].Play();
            diamondText.color = Color.red;
        }
    }
    public void buyLevel()
    {
        if (!levelBought[level] && CollectibleControl.coinCount >= level * 3500)
        {
            CollectibleControl.coinCount -= level * 3500;
            levelBought[level] = true;
            FileSystem.saveData();
            GetComponents<AudioSource>()[1].Play();
        }
        else
        {
            GetComponents<AudioSource>()[0].Play();
            cointText.color = Color.red;
        }
    }

    public void Close()
    {
        Application.Quit();
    }



    
  
}
