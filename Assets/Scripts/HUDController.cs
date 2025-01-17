﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{

    [SerializeField] private int _maxHealth = 10;
    private int _currentHealth;
    [SerializeField] Slider _healthSlider;

    private int _crateMaxHealth = 10;
    private int _crateCurrentHealth;
    [SerializeField] Slider _crateHealthSlider;

    [SerializeField] Text _candyText;
    [SerializeField] Image _flashImage;

    [SerializeField] PlayerController _playerRef;
    [SerializeField] CandyCrate _crateRef;

    private int _candyAmount;

    // Set variables
    private void Awake()
    {
        //_playerRef = GameObject.Find("Player").GetComponent<PlayerController>();
        _maxHealth = _playerRef.MaxHealth;
        _currentHealth = _playerRef.CurrentHealth;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _currentHealth;

        _crateMaxHealth = _crateRef._maxHealth;
        _crateCurrentHealth = _crateRef._maxHealth;
        _crateHealthSlider.maxValue = _crateMaxHealth;
        _crateHealthSlider.value = _crateCurrentHealth;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            AddCandy(10);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            _playerRef.DecreaseHealth(5, 0);
        }
    }

    // Candy counter, will be used in HUD / Shop later
    public void AddCandy(int amount)
    {
        _candyAmount += amount;
        _candyText.text = _candyAmount.ToString();
    }

    // Event Subscribing
    private void OnEnable()
    {
        _playerRef.PlayerDamage += OnPlayerDamage;
        _crateRef.CrateDamage += OnCrateDamage;
    }

    private void OnDisable()
    {
        _playerRef.PlayerDamage -= OnPlayerDamage;
        _crateRef.CrateDamage -= OnCrateDamage;
    }

    // Event
    void OnPlayerDamage()
    {
        _currentHealth = _playerRef.CurrentHealth;
        _healthSlider.value = _currentHealth;
        StopAllCoroutines();
        StartCoroutine(ImageFlashCoroutine(.5f));
    }

    void OnCrateDamage()
    {
        _crateCurrentHealth = _crateRef._currentHealth;
        _crateHealthSlider.value = _crateCurrentHealth;
        StopAllCoroutines();
        StartCoroutine(SmallImageFlashCoroutine(.25f));
    }

    IEnumerator ImageFlashCoroutine(float fadeDuration)
    {
        _flashImage.enabled = true;
        Color initialColor = _flashImage.color;
        Color targetColor = new Color(0, 0, 0, 0);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            _flashImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        _flashImage.color = initialColor;
        _flashImage.enabled = false;
    }

    IEnumerator SmallImageFlashCoroutine(float fadeDuration)
    {
        _flashImage.enabled = true;
        Color initialColor = new Color(1, 0, 0, 0.5f);
        Color targetColor = new Color(0, 0, 0, 0);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            _flashImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        _flashImage.color = initialColor;
        _flashImage.enabled = false;
    }
}

