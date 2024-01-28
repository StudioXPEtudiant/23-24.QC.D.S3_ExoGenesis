using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioXP.Scripts.Objects
{
    public class RecipesCraftTriger : MonoBehaviour
    {

        public void caftAllValideRecipies()
        {
            foreach(Recipe recipe in GetComponentsInChildren<Recipe>())
            {
                recipe.Craft();
            }
        }

    }
}
