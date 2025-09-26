using System.Linq;
using Unity.AI.Navigation.Editor;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;

public class JammoAnimations : MonoBehaviour
{
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int Blend = Animator.StringToHash("Blend");
    
    [Header("State")]
    [SerializeField] private PlayerIndexValue playerIndex;
    [SerializeField] private IdleAnimationValue idleAnimation;
    [SerializeField] private EyeStateValue eyeState;

    [Header("Renderers")]
    [SerializeField] private Renderer[] bodyRenderers;
    [SerializeField] private Renderer eyesRenderer;

    [Header("Textures and colors")]
    [SerializeField] private Texture2D[] bodyTextures;
    [SerializeField, ColorUsage(true, true)] private Color[] eyeColors;
    [SerializeField] private Vector2[] eyesTextureOffset = { new(0, 0), new(.33f, 0), new(.66f, 0), new(.33f, .66f) };

    [Header("Animations")]
    [SerializeField, Range(0, 1)] private float walkThreshold = 0.1f;
    [SerializeField, Range(0, 1)] private float walkStartDamping = 0.3f;
    [SerializeField, Range(0, 1)] private float walkStopDamping = 0.15f;

    private Animator animator;
    private CharacterController characterController;
    private NavMeshAgent navMeshAgent;

    public PlayerIndexValue PlayerIndex
    {
        get => playerIndex;
        set
        {
            playerIndex = value;
            ConfigureVisual();
        }
    }

    public IdleAnimationValue IdleAnimation
    {
        get => idleAnimation;
        set
        {
            idleAnimation = value;
            ConfigureIdleAnimation();
        }
    }

    public EyeStateValue EyeState
    {
        get => eyeState;
        set
        {
            eyeState = value;
            ConfigureVisual();
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        ConfigureVisual();
        ConfigureIdleAnimation();

        // Prevent update if no CharacterController or NavMeshAgent is found.
        if (!characterController && !navMeshAgent) enabled = false;
    }

    private void Update()
    {
        // Get velocity from either the CharacterController or the NavMeshAgent.
        var velocity = Vector3.zero;
        if (characterController) velocity = characterController.velocity;
        else if (navMeshAgent) velocity = navMeshAgent.velocity;
        
        // Update animator values from velocity.
        var normalizedSpeed = Mathf.Clamp01(velocity.sqrMagnitude);
        if (normalizedSpeed > walkThreshold)
            animator.SetFloat(Blend, normalizedSpeed, walkStartDamping, Time.deltaTime);
        else if (normalizedSpeed < walkThreshold)
            animator.SetFloat(Blend, normalizedSpeed, walkStopDamping, Time.deltaTime);
    }

    private void ConfigureVisual()
    {
        // Visual values.
        var visualIndex = (int)playerIndex;
        var bodyTexture = bodyTextures[visualIndex];
        var eyeColor = eyeColors[visualIndex];
        var eyeTextureOffset = eyesTextureOffset[(int)eyeState];

        // Update body renderers
        for (var i = 0; i < bodyRenderers.Length; i++)
        {
            bodyRenderers[i].material.mainTexture = bodyTexture;
        }

        // Update eyes renderer.
        eyesRenderer.material.SetColor(EmissionColor, eyeColor);
        eyesRenderer.material.mainTextureOffset = eyeTextureOffset;
    }

    private void ConfigureIdleAnimation()
    {
        animator.SetTrigger(idleAnimation.ToString());
    }

#if UNITY_EDITOR
    [ContextMenu("Find Renderers")]
    private void GetRenderers()
    {
        Undo.RecordObject(this, $"Finding renderers in {gameObject.name}");
        eyesRenderer = transform.Find("HeadEyes").GetComponent<Renderer>();
        bodyRenderers = GetComponentsInChildren<Renderer>().Where(x => x != eyesRenderer).ToArray();
    }

    private void OnValidate()
    {
        if (!EditorApplication.isPlaying) return;
        if (animator is null) return;
        
        ConfigureVisual();
        ConfigureIdleAnimation();
    }
#endif

    public enum PlayerIndexValue
    {
        Player1 = 0,
        Player2 = 1,
        Player3 = 2,
        Player4 = 3,
    }

    public enum IdleAnimationValue
    {
        Normal = 0,
        Happy = 1,
        Angry = 2,
        Scared = 3
    }

    public enum EyeStateValue
    {
        Normal = 0,
        Happy = 1,
        Angry = 2,
        Scared = 3
    }
}