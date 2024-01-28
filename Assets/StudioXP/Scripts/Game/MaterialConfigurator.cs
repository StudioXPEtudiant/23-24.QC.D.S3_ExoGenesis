using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MaterialConfigurator : MonoBehaviour
{

    [SerializeField] private bool RandomizeOnStart = true;
    [SerializeField] [MinValue(0)] private int DefaultIndex = 0;
    [SerializeField] private Material[] MaterialsList;

    // Start is called before the first frame update
    void Start()
    {
        if(RandomizeOnStart)
        {
            Randomize();
        }
        else
        {
            ChangeMaterial(DefaultIndex);
        }

    }

    public void Randomize()
    {
        if (RandomizeOnStart && MaterialsList.Length > 0)
        {
            ChangeMaterial(Random.Range(0, MaterialsList.Length));
        }
    }

    public void ChangeMaterial(int index)
    {
        if (MaterialsList.Length > 0 && index< MaterialsList.Length)
        {
            GetComponent<Renderer>().material = MaterialsList[index];
        }
    }

   
}
