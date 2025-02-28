using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DuelManager : MonoBehaviour
{
    [Header("Настройки")]
    public List<GameObject> characterPrefabs;
    public Transform[] spawnPoints;
    public Slider[] healthSliders;
    public Text leftNameText, rightNameText;
    public Text leftStatusText, rightStatusText;
    public GameObject victoryPanel;
    public Text victoryText;
    public Button restartButton;

    private Character[] fighters = new Character[2];

    void Start()
    {
        restartButton.onClick.AddListener(RestartDuel);
        InitializeDuel();
    }

    void InitializeDuel()
    {
        victoryPanel.SetActive(false);
        leftStatusText.text = "";
        rightStatusText.text = "";

        List<int> availableIndices = new List<int> { 0, 1, 2 };
        int firstIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        availableIndices.Remove(firstIndex);
        int secondIndex = availableIndices[Random.Range(0, availableIndices.Count)];

        for (int i = 0; i < 2; i++)
        {
            int selectedIndex = (i == 0) ? firstIndex : secondIndex;
            GameObject selectedPrefab = characterPrefabs[selectedIndex];

            fighters[i] = Instantiate(
                selectedPrefab,
                spawnPoints[i].position,
                Quaternion.identity
            ).GetComponent<Character>();

            fighters[i].nameText = (i == 0) ? leftNameText : rightNameText;
            if (fighters[i].possibleNames.Length > 0)
            {
                fighters[i].nameText.text = fighters[i].possibleNames[Random.Range(0, fighters[i].possibleNames.Length)];
            }
            else
            {
                fighters[i].nameText.text = "cure your Dementia pls";
            }
            fighters[i].statusText = (i == 0) ? leftStatusText : rightStatusText;
            fighters[i].healthSlider = healthSliders[i];
            fighters[i].currentHealth = fighters[i].maxHealth;
            fighters[i].UpdateHealthUI();
            fighters[i].transform.localScale = new Vector3(1, 2, 1);
        }
    }

    void Update()
    {
        if (BothAlive())
        {
            ProcessAttacks();
        }
        else if (!victoryPanel.activeSelf)
        {
            ShowVictory();
        }
    }

    void ProcessAttacks()
    {
        for (int i = 0; i < 2; i++)
        {
            if (fighters[i].attackTimer <= 0)
            {
                Character target = fighters[1 - i];
                fighters[i].Attack(target);
            }
        }
    }

    bool BothAlive()
    {
        return fighters[0] != null && fighters[1] != null &&
               fighters[0].currentHealth > 0 && fighters[1].currentHealth > 0;
    }

    void ShowVictory()
    {
        victoryPanel.SetActive(true);
        Character winner = fighters[0].currentHealth > 0 ? fighters[0] : fighters[1];
        victoryText.text = winner.nameText.text + " wins!";
    }

    public void RestartDuel()
    {
        victoryPanel.SetActive(false);

        for (int i = 0; i < fighters.Length; i++)
        {
            if (fighters[i] != null)
            {
                fighters[i].ResetCharacter();
                Destroy(fighters[i].gameObject);
                fighters[i] = null;
            }
        }

        InitializeDuel();
    }
}