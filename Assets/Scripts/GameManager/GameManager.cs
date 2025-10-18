using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentEnergy;
    [SerializeField] private int energyThreshold = 20;
    [SerializeField] private GameObject boss;
    private bool bossCalled = false;
    [SerializeField] private GameObject WhaleSpawn;

    [SerializeField] private Image energyBar;
    [SerializeField] private GameObject gameUi;

    [SerializeField] private AudioManager audioManager;
    void Start()
    {
        updateEnerbar();
        currentEnergy = 0;
        boss.SetActive(false);
        audioManager.PlayDefaultAudio();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddEnergy()
    {
        if(bossCalled) return;

        currentEnergy += 1; 
        updateEnerbar();
        audioManager.PlayEnergyPickUp();
        if(currentEnergy == energyThreshold)
        {
            CallBoss();
        }
    }
    private void CallBoss()
    {
        bossCalled = true;
        boss.SetActive(true);
        WhaleSpawn.SetActive(false);
        gameUi.SetActive(false);
        audioManager.PlayBossAudio();
    }
    private void updateEnerbar()
    {
        if(energyBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / (float)energyThreshold);
            energyBar.fillAmount = fillAmount;
        }
    }
}
