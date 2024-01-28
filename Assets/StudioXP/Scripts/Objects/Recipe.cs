using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StudioXP.Scripts.Objects
{
    public class Recipe : MonoBehaviour
    {
        [SerializeField] private List<RecipeDetail> recipes;


        // Start is called before the first frame update
        void Start()
        {
            refreshRecipe();
        }

        private RecipeDetail activ = null;
        

        public void Craft()
        {
            if (activ==null) return;
            int valideSlot = 0;
            foreach (RecipeDetailSlot rds in activ.SlotNeeds)
            {
                bool found = false;
                Storable resource = rds.slot.GetComponentInChildren<Storable>();
                if (resource != null)
                    foreach (String type in resource.StorageTypes)
                    {
                        found |= rds.valideType.Contains(type);
                    }

                if (found)
                    valideSlot++;
            }
            if (activ.SlotNeeds.Count == valideSlot)
            {
                foreach (RecipeDetailSlot rds in activ.SlotNeeds)
                {
                    rds.slot.ClearSlot();
                }

                var spawner = GetComponent<Spawner>();
                if (spawner)
                {
                    spawner.Spawn(activ.ToSpwan);
                }
            }         
        }

       public void refreshRecipe()
        {
            activ = null;

            foreach (RecipeDetail rd in recipes)
            {
                rd.Modele.SetActive(false);
                if (activ == null)
                {
                    int valideSlot = 0;
                    foreach (RecipeDetailSlot rds in rd.SlotNeeds)
                    {
                        bool found = false;
                        Storable resource = rds.slot.GetComponentInChildren<Storable>();
                        if(resource != null)
                            foreach (String type in resource.StorageTypes)
                            {
                                    found|=rds.valideType.Contains(type);
                            }


                        if (found)
                            valideSlot++;


                    }
                    if(rd.SlotNeeds.Count== valideSlot)
                    {
                        activ = rd;
                    }

                }
                

            }
            if(activ==null&& recipes.Count > 0)
            {
                activ = recipes[0];
                
            }
            activ.Modele.SetActive(true);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    [Serializable]
    public class RecipeDetail
    {

        public GameObject Modele;
        public List<RecipeDetailSlot> SlotNeeds;
        public GameObject ToSpwan;

    }

    [Serializable]
    public class RecipeDetailSlot
    {
        public StorageSlot slot;
        public List<String> valideType;
    }

}
