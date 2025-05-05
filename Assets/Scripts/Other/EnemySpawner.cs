using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private GameObject _enemyPrefab;  // Префаб моба
    [SerializeField] private float _spawnInterval = 3f; // Интервал между спавном (в секундах)
    [SerializeField] private int _maxEnemies = 10;      // Макс. кол-во мобов на сцене
    [SerializeField] private bool _spawnInCircle = true; // Спавнить по кругу или в точках?
    [SerializeField] private float _spawnRadius = 5f;   // Радиус спавна (если выбран круг)

    [Header("Точки спавна (если не круг)")]
    [SerializeField] private Transform[] _spawnPoints;  // Ручные точки появления

    private int _currentEnemies = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true) // Бесконечный цикл спавна
        {
            if (_currentEnemies < _maxEnemies)
            {
                SpawnEnemy();
                _currentEnemies++;
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
        
        // Подписываемся на смерть моба
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.OnEnemyDeath += () => 
            {
                _currentEnemies--;
                Debug.Log("Враг умер! Осталось: " + _currentEnemies);
            };
        }
        else
        {
            Debug.LogError("У префаба врага нет EnemyAI!");
        }
    }

    private Vector3 GetSpawnPosition()
    {
        if (_spawnInCircle)
        {
            Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
            return transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
        }
        else
        {
            return _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
        }
    }
}
