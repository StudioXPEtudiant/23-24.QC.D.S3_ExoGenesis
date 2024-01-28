using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StudioXP.Scripts.Utils
{

    public class RollActiveChild : MonoBehaviour
    {

        private int currentChild = 0;

        void Start()
        {
            DisableAllChild();
            transform.GetChild(currentChild).gameObject.SetActive(true);
        }

        public void DisableAllChild()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void EnableNextChild()
        {
            DisableAllChild();
            currentChild++;
            if (currentChild >= transform.childCount)
                currentChild = 0;
            transform.GetChild(currentChild).gameObject.SetActive(true);
        }

        public void EnablePrevChild()
        {
            DisableAllChild();
            currentChild--;
            if (currentChild < 0)
                currentChild = transform.childCount-1;
            transform.GetChild(currentChild).gameObject.SetActive(true);
        }



    }
}
