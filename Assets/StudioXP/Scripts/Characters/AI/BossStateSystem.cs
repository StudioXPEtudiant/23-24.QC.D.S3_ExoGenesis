using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StudioXP.Scripts.Characters.AI
{
    /// <summary>
    /// Système d'états de contrôle du boss.
    ///
    /// Il y a 8 type d'états possibles. <see cref="BossState"/>
    /// Chaque type d'état peut active un état aléatoire dans une liste.
    /// Neutral : Le boss n'a pas encore été activé.
    /// Neutral Hurt : Le boss a été attaqué en mode Neutral
    /// Awaken : Le boss a été réveillé. (Est activé après l'état Neutral Hurt)
    /// Awaken Hurt : Le boss a été blessé en mode Idle, Walk ou Attack
    /// Idle : Le joueur est plus loin que la distance d'Idle
    /// Walk : Le boss n'est pas en train d'attaquer
    /// Attack : Le boss effectue une attaque si le joueur est à l'intérieur de la distance d'attaque.
    ///          Par défaut le boss est en mode Walk et attaquera avec une probabilité défini par la chance d'attaque.
    /// Die : Le boss meurt
    /// </summary>
    public class BossStateSystem : MonoBehaviour
    {
        [SerializeField] private float attackDistance = 10;
        [SerializeField] private float idleDistance = 30;
        [SerializeField] private float attackChance = 0.8f;

        [SerializeField] private Animator animator;

        [SerializeField] private List<BossState> neutralHurtStates;
        [SerializeField] private List<BossState> awakenHurtStates;
        [SerializeField] private List<BossState> neutralStates;
        [SerializeField] private List<BossState> awakenStates;
        [SerializeField] private List<BossState> idleStates;
        [SerializeField] private List<BossState> walkStates;
        [SerializeField] private List<BossState> attackStates;
        [SerializeField] private List<BossState> dieStates;
        
        
        private Dictionary<List<BossState>, List<BossState>> _statePouches;
        private GameObject _player;
        private BossState _currentBossState;
        private int _currentHitPoints;
        private bool _activateTimer;
        private float _currentTimer;
        private bool _isAwaken;
        private bool _isDead;
        private bool _isAttacking;
        private static readonly int Loop = Animator.StringToHash("loop");

        /// <summary>
        /// Défini l'état à NeutralHurt ou AwakenHurt
        /// </summary>
        public void Hurt()
        {
            SetRandomState(_isAwaken ? awakenHurtStates : neutralHurtStates);
        }

        /// <summary>
        /// Défini l'état à Awaken si le boss n'est pas déjà réveillé
        /// </summary>
        public void SetAwaken()
        {
            if (_isAwaken) return;
            
            SetRandomState(awakenStates);
            _isAwaken = true;
        }

        /// <summary>
        /// Défini l'état à Die si le boss n'est pas déja mort
        /// </summary>
        public void Kill()
        {
            if (_isDead) return;

            SetRandomState(dieStates);
            _isDead = true;
        }

        /// <summary>
        /// Défini l'état suivant selon l'état courant
        /// </summary>
        private void SetNextState()
        {
            if (!_isAwaken)
            {
                SetRandomState(neutralStates);
            }
            else if (_isDead)
            {
                _currentBossState.Deactivate();
                Destroy(gameObject);
            }
            else
            {
                Vector3 playerPosition = _player.transform.position;
                playerPosition.z = 0;
                Vector3 thisPosition = transform.position;
                thisPosition.z = 0;
            
                float playerDistance = (playerPosition - thisPosition).magnitude;
                if (playerDistance <= attackDistance)
                {
                    if (!_isAttacking && attackChance > Random.value)
                    {
                        _isAttacking = true;
                        SetRandomState(attackStates);
                    }
                    else
                    {
                        _isAttacking = false;
                        SetRandomState(walkStates);
                    }
                        
                }
                else if (playerDistance >= idleDistance)
                {
                    _isAttacking = false;
                    SetRandomState(idleStates);
                }
                else
                {
                    _isAttacking = false;
                    SetRandomState(walkStates);
                }
            }
        }

        /// <summary>
        /// Défini un état au hasard dans la liste donné en paramètre
        /// </summary>
        /// <param name="states"></param>
        private void SetRandomState(List<BossState> states)
        {
            if (states.Count == 0) return;

            var pouch = _statePouches[states];
            SetState(pouch[Random.Range(0, pouch.Count)]);
        }

        /// <summary>
        /// Défini l'état du boss à l'état passé en paramètre
        /// </summary>
        /// <param name="bossState"></param>
        private void SetState(BossState bossState)
        {
            if(_currentBossState != null)
                _currentBossState.Deactivate();

            _currentBossState = bossState;

            if (_currentBossState == null) return;
            
            _activateTimer = _currentBossState.IsLooping;
            _currentTimer = _currentBossState.Duration;
                
            SetAnimation(bossState.Clip);
            animator.SetBool(Loop, _currentBossState.IsLooping);
            animator.Play("Animation", 0, 0.0f);
                
            _currentBossState.Activate();
        }

        /// <summary>
        /// Change l'animation clip du boss pour celle passé en paramètre
        /// </summary>
        /// <param name="clip"></param>
        private void SetAnimation(AnimationClip clip)
        {
            if (!animator || !clip) return;

            var aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>> {
                new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[0], clip)
            };
            aoc.ApplyOverrides(anims);
            animator.runtimeAnimatorController = aoc;
        }

        /// <summary>
        /// Remplis une poche de pige avec les états passés en paramètre.
        /// Le paramètre chance de chaque état est utilisé pour décider la quantité à mettre dans la poche de pige.
        /// Un état avec une valeur chance de 10 aura 10 copies d'elle même placé dans la poche de pige.
        /// </summary>
        /// <param name="states"></param>
        private void FillUpPouch(List<BossState> states)
        {
            var pouch = new List<BossState>();
            _statePouches[states] = pouch;
            foreach (var state in states)
            {
                for(int i = 0; i < state.Chances; i++)
                    pouch.Add(state);
            }
        }
        
        private void Awake()
        {
            _statePouches = new Dictionary<List<BossState>, List<BossState>>();
            FillUpPouch(neutralHurtStates);
            FillUpPouch(awakenHurtStates);
            FillUpPouch(neutralStates);
            FillUpPouch(awakenStates);
            FillUpPouch(idleStates);
            FillUpPouch(walkStates);
            FillUpPouch(attackStates);
            FillUpPouch(dieStates);
            
            _player = GameObject.FindWithTag("Player");
            SetRandomState(neutralStates);
        }
        
        private void Update()
        {
            if (_activateTimer)
            {
                _currentTimer -= Time.deltaTime;
                if (_currentTimer <= 0)
                {
                    SetNextState();
                }
            } 
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Nothing"))
            {
                SetNextState();
            }
        }
    }
}
