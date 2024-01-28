using UnityEngine;

namespace StudioXP.Scripts.Objects
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private int quantity = 3;
        [SerializeField] private GameObject prefab;

        public void Spawn(GameObject prefab)
        {
            for (int i = 0; i < quantity; i++)
            {
                Instantiate(prefab, null).transform.localPosition = transform.position;
            }
        }

        public void Spawn()
        {
            for (int i = 0; i < quantity; i++)
            {
                Instantiate(prefab, null).transform.localPosition = new Vector3( transform.position.x, transform.position.y+1, transform.position.z);
            }
        }
    }
}
