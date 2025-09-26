using UnityEngine;
using UnityEngine.InputSystem;

public class JammoPlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 0.1f;

    [Header("Inputs")]
    [SerializeField] private InputActionReference moveAction;
    
    // TODO : Compléter cette classe.
    private CharacterController characterController;

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
        var forward = cameraTransform.forward;
        var right = cameraTransform.right;
        
        // Retirer la rotation Y (garde la direction horizontale).
        forward.y = 0;
        right.y = 0;
        
        // Lire les entrées du joueur.
        var moveInput = moveAction.action.ReadValue<Vector2>();
        
        // Si le joueur veut pas bouger, ne pas faire bouger le joueur.
        if (moveInput == Vector2.zero)
        {
            characterController.Move(Vector3.zero);
        }
        else
        {
            // Y multiplie forward (avance/recule).
            // X multiplie right (gauche/droite).
            // Combinaison des deux fait le movement total.
            var moveDirection = forward * moveInput.y + right * moveInput.x;
            //Pas valide : var gravity = Physics.gravity * Time.deltaTime;
            characterController.Move(moveDirection * (speed * Time.deltaTime));
            
            // Rotate player using current direction.
            var lookRotation = Quaternion.LookRotation(moveDirection);
            // transform.rotation = lookRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
}