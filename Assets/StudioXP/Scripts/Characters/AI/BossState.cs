using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace StudioXP.Scripts.Characters.AI
{
    /// <summary>
    /// État utilisé dans un système d'état.
    /// Les systèmes l'utilisant sont <see cref="BossStateSystem"/>.
    ///
    /// Un état peut être activé ou désactivé et lancera un évènement selon le cas.
    ///
    /// Lorsqu'un état est choisis, le système d'état pige dans une poche au hasard pour choisir l'état à activer.
    /// Chaque états ont un certain nombre de copies placés dans la poche. Le nombre de copie est défini par l'attribut chances.
    ///
    /// Un état contient de l'information sur l'animation à exécuté lorsqu'elle commence. Le clip, la duration et si
    /// l'animation joue en boucle y sont défini.
    /// </summary>
    [Serializable]
    public class BossState
    {
        [MinValue(1), MaxValue(100), SerializeField] private int chances = 1;
        [SerializeField] private AnimationClip clip;
        [SerializeField] private bool loop = true;
        [SerializeField, ShowIf("loop"), MinMaxSlider(0, 20)] private Vector2Int duration;
        [SerializeField] private UnityEvent activated;
        [SerializeField] private UnityEvent deactivated;

        private static int _id;

        public bool IsLooping => loop;

        public float Duration => Random.Range((float)duration.x, (float)duration.y);

        public AnimationClip Clip => clip;

        public int Chances => chances;

        /// <summary>
        /// Active l'état en appelant l'évènement activated.
        /// </summary>
        public void Activate()
        {
            activated.Invoke();
        }

        /// <summary>
        /// Désactive l'état en appelant l'évènement deactivated.
        /// </summary>
        public void Deactivate()
        {
            deactivated.Invoke();
        }
    }
}
