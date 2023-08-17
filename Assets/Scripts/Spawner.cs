using UnityEngine;

public class Spawner : MonoBehaviour
{
  public Transform spawnPointCard;
  public GameObject spawnCard;
  public Transform spawnPointEnemy;
  public GameObject spawnEnemy;
  private float newSpawnDuration = 1f;

#region Singleton

  public static Spawner Instance;

  private void Awake()
  {
    Instance = this;
  }

#endregion

  void SpawnNewCard()
  {
    Instantiate(spawnCard, spawnPointCard.position, Quaternion.identity);
  }
    
  public void NewSpawnCardRequest()
  {
    Invoke("SpawnNewCard", newSpawnDuration);
  }
  
  void SpawnNewEnemy()
  {
    Vector3 spawnPosition = spawnPointEnemy.position + new Vector3(Random.Range(-1f, 1f), 0, 0);
    Instantiate(spawnEnemy, spawnPosition, Quaternion.identity);
  }
  
  public void NewSpawnEnemyRequest()
  {
    Invoke("SpawnNewEnemy", newSpawnDuration);
  }
  
}