using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StudioXP.Scripts.Components.Common
{
    public class Spawnable : MonoBehaviour
    {
        [Serializable]
        public enum SpawningType
        {
            Default,
            OneTime,
            OneTimeInteraction,
            Continuous
        }
        
        [LabelText("Type de spawn")]
        [SerializeField] private SpawningType spawningType;
        
        [LabelText("Nombre de spawns maximum")]
        [MinValue(1)]
        [SerializeField] private int maximumSpawn = 1;
        
        [PropertySpace]
        
        [LabelText("Distance de spawn")]
        [MinValue(0)]
        [SerializeField] private float spawnDistance = 50;
        
        [LabelText("Distance de despawn")]
        [MinValue(0)]
        [SerializeField] private float despawnDistance = 80;
        
        [PropertySpace]

        [LabelText("Délai entre les spawns")]
        [MinMaxSlider("minimumSpawnDelay", "maximumSpawnDelay", true)]
        [SerializeField] private Vector2 delayBetweenSpawn = Vector2.one;
        
        [LabelText("Premier spawn instantané")]
        [SerializeField] private bool firstSpawnIsInstant = true;

#pragma warning disable 0414
        [LabelText("Délai de spawn minimum")]
        [FoldoutGroup("Avancé", false)]
        [SerializeField] private float minimumSpawnDelay = 1;
        
        [LabelText("Délai de spawn maximun")]
        [FoldoutGroup("Avancé")]
        [SerializeField] private float maximumSpawnDelay = 50;
#pragma warning restore 0414

        private GameObject _player;
        private Spawnable _template;
        private bool _isSpawner;
        private bool _isInstance;
        private string _name;
        private int _interactionCount;
        private int _currentSpawns;
        private int _allTimeSpawns;
        private Spawnable _spawner;
        private bool _firstSpawned;
        private float _timeBeforeNextSpawn;

        public void Interact()
        {
            if (_isInstance)
                _spawner._interactionCount++;
        }

        public void SetActive(bool active)
        {
            _spawner.gameObject.SetActive(active);
        }

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");

            if (!_isSpawner && !_isInstance)
            {
                CreateSpawner();
            }
        }

        private void Update()
        {
            var camDistance = GetCameraDistance();
            var isAtDespawnDistance = camDistance >= despawnDistance;
            
            if (_isSpawner)
            {
                if (isAtDespawnDistance)
                {
                    if (_currentSpawns > 0) return;
                    
                    if (spawningType == SpawningType.OneTime && _allTimeSpawns >= maximumSpawn)
                    {
                        Destroy(gameObject);
                        return;
                    }

                    if (spawningType == SpawningType.OneTimeInteraction && _interactionCount >= maximumSpawn && _allTimeSpawns >= maximumSpawn)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    
                    _currentSpawns = 0;
                    _allTimeSpawns = 0;
                    _firstSpawned = false;
                    _timeBeforeNextSpawn = Random.Range(delayBetweenSpawn.x, delayBetweenSpawn.y);

                    return;
                }
                
                if (camDistance > spawnDistance) return;

                if ((spawningType != SpawningType.Continuous) && _allTimeSpawns >= maximumSpawn) return;
                
                _timeBeforeNextSpawn -= Time.deltaTime;
                if (_currentSpawns >= maximumSpawn) return;

                if (firstSpawnIsInstant && !_firstSpawned)
                {
                    _timeBeforeNextSpawn = 0;
                }

                if (_timeBeforeNextSpawn <= 0)
                {
                    Spawn();
                }

                return;
            }

            if (isAtDespawnDistance)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_isInstance)
            {
                _spawner._currentSpawns--;
            }
        }

        private void CreateSpawner()
        {
            var templateTransform = transform;
            
            var spawnerGo = new GameObject();
            spawnerGo.transform.parent = templateTransform.parent;
            spawnerGo.transform.localPosition = templateTransform.localPosition;
            spawnerGo.SetActive(false);
            var spawner = spawnerGo.AddComponent<Spawnable>();
            spawner.maximumSpawn = maximumSpawn;
            spawner.delayBetweenSpawn = delayBetweenSpawn;
            spawner.firstSpawnIsInstant = firstSpawnIsInstant;
            spawner._isSpawner = true;
            spawner._timeBeforeNextSpawn = Random.Range(delayBetweenSpawn.x, delayBetweenSpawn.y);
            spawner.despawnDistance = despawnDistance;
            spawner.spawningType = spawningType;

            var template = this;
            spawner._template = template;
            template.transform.parent = spawner.transform;
            _name = template.name;
            spawner._name = _name;
            spawner.name = $"{_name} (Spawner)";
            template.name = $"{_name} (Template)";
            template.gameObject.hideFlags = HideFlags.HideInHierarchy;
            _spawner = spawner;

            template.gameObject.SetActive(false);
            spawnerGo.SetActive(true);
        }

        private void Spawn()
        {
            _firstSpawned = true;
            _currentSpawns++;
            _allTimeSpawns++;
            var spawn = Instantiate(_template, transform);
            spawn.name = $"{_name} (Instance)";
            spawn._spawner = _template._spawner;
            spawn._isInstance = true;
            spawn.gameObject.SetActive(true);

            _timeBeforeNextSpawn = Random.Range(delayBetweenSpawn.x, delayBetweenSpawn.y);
        }

        private float GetCameraDistance()
        {
            var playerPos = _player.transform.position;
            var position = transform.position;

            var cameraDistance = (playerPos - position).magnitude;
            
#if DEBUG
            if (_isInstance)
                return cameraDistance;
            
            Color color;
            if(cameraDistance <= spawnDistance)
                color = Color.green;
            else if(cameraDistance >= despawnDistance)
                return cameraDistance;
            else
                color = Color.yellow;
            
            Debug.DrawLine(position, playerPos, color);
#endif
            
            return cameraDistance;
        }
    }
}
