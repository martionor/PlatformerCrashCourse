using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;

    public Canvas gameCanvas;

    public void Awake()
    {
        gameCanvas = FindAnyObjectByType<Canvas>();

        
    }

    private void OnEnable()
    {
        CharacterEvents.characterDamaged+=(CharacterTookDamage);

        CharacterEvents.characterHealed+=(CharacterHealed);
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -=(CharacterTookDamage);

        CharacterEvents.characterHealed -=(CharacterHealed);
    }

    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        //Create text at character hit
        Vector3 spamPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spamPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();
    }

    public void CharacterHealed(GameObject character, int healthRestored) 
    {
        //TODO Health character
        Vector3 spamPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPrefab, spamPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = healthRestored.ToString();
    }
    
}
