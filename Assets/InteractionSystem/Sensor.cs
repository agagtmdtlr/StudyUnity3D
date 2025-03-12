using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] float range = 1.0f;
    [SerializeField] LayerMask whatIsInteractor;
    public Vector3 Position => transform.position;

    private void Awake()
    {
        if (!TryGetComponent(out SphereCollider circle))
        {
            circle =  gameObject.AddComponent<SphereCollider>();
        }

        circle.isTrigger = true;
        circle.radius = range;
        circle.includeLayers = whatIsInteractor;
        circle.excludeLayers = ~whatIsInteractor.value;
    }


    public void Interact(Interactor interactor)
    {
        Debug.Log($"Interact form {interactor.gameObject.name}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
