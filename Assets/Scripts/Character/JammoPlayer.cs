using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class JammoPlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 0.1f;

    [Header("Inputs")]
    [SerializeField] private InputActionReference moveAction;
    [FormerlySerializedAs("moveAction")] [SerializeField] private InputActionReference jumpAction;
    
    // TODO : Compléter cette classe.
    private CharacterController characterController;
    private float verticalVelocity;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Obetenir des vecteurs de direction relatifs à la caméra.
        var camera = Camera.main!;
        var cameraTransform = camera.transform;
        var up = cameraTransform.up;
        var forward = cameraTransform.forward;
        var right = cameraTransform.right;
        
        // Retirer la rotation Y (garde la direction horizontale).
        forward.y = 0;
        right.y = 0;
        
        // Lire les entrées du joueur.
        var moveInput = moveAction.action.ReadValue<Vector2>();
        var jumpInput = jumpAction.action.triggered;

        var horizontalMouvement = Vector3.zero;
        
        // Si le joueur veut pas bouger, ne pas faire bouger le joueur.
        if (moveInput != Vector2.zero)
        {
            // Y multiplie forward (avance/recule).
            // X multiplie right (gauche/droite).
            // Combinaison des deux fait le movement total.
            var moveDirection = forward * moveInput.y + right * moveInput.x;
            //Pas valide : var gravity = Physics.gravity * Time.deltaTime;
            horizontalMouvement = moveDirection * (speed * Time.deltaTime);
            // Rotate player using current direction.
            var lookRotation = Quaternion.LookRotation(moveDirection);
            // transform.rotation = lookRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        
        // Partie sur le saut.
        var gravity = Physics.gravity;
        var isGrounded = characterController.isGrounded;
        
        // La vélocité est de zéro si on touche le sol.
        if (isGrounded)
        {
            verticalVelocity = 0;
        }
        
        // Calculer la vélocité lorsque l'ont saute.
        //
        // Velolocite2 = 2 x Acceleration (Gravite) x Deplacement (Hauteur voulue)
        if (isGrounded && jumpInput)
        {
            verticalVelocity = Mathf.Sqrt(2 * -gravity.y * 3f);
        }
        
        // Appliquer la gravité.
        verticalVelocity += gravity.y * Time.deltaTime;
        
        // Calculer le mouvement vertical.
        var verticalMouvement = up * (verticalVelocity * Time.deltaTime);
        
        // Appliquer le mouvement.
        characterController.Move(horizontalMouvement + verticalMouvement);
    }
}