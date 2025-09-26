using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class JammoAI : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference mousePositionAction;

    [SerializeField] private GameObject destination;
    private NavMeshAgent agent;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = destination.transform.position;
    }
    // TODO : Compléter cette classe.
}