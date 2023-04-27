using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth;

    [Header("To Attach")]
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private MeshRenderer _bodyMesh;
    [SerializeField] private MeshRenderer _noseMesh;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthText.text = _currentHealth.ToString();
    }

    public void TakeDamege(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = (int)Random.Range(30, 60);
            _bodyMesh.material.color = new Color(Random.Range(0f, 1f),
                              Random.Range(0f, 1f),
                              Random.Range(0f, 1f));

            _noseMesh.material.color = new Color(Random.Range(0f, 1f),
                  Random.Range(0f, 1f),
                  Random.Range(0f, 1f));
        }

        _healthText.text = _currentHealth.ToString();
    }
}
