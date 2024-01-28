using System.Collections.Generic;
using Sirenix.OdinInspector;
using StudioXP.Scripts.Events;
using UnityEngine;

namespace StudioXP.Scripts.Controllers
{
    public class InputController : MonoBehaviour
    {
        [LabelText("Contrôles")]
        [SerializeField] private List<InputEvent> inputs;

        void Update()
        {
            foreach(var input in inputs)
                input.Update();
        }
    }
}
