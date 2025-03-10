using System;
using TreeEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Planets
{
    public class PlanetControl : MonoBehaviour
    {
        [FormerlySerializedAs("Target")] [SerializeField] Transform target;
        [FormerlySerializedAs("Radius")] public  float radius;
        [FormerlySerializedAs("RevolveSpeed")] public  float revolveSpeed;
        [FormerlySerializedAs("OrbitSpeed")] public  float orbitSpeed =1f;
        private float _orbit;

        private void Awake()
        {
            radius = float.Parse(gameObject.name) + 1f;
            orbitSpeed = radius * 10f;
        }

        private void Update()
        {

            if (target != null)
            {
                _orbit += Time.deltaTime * orbitSpeed;
                var localPosFromTarget = Quaternion.Euler( Vector3.up * _orbit) * Vector3.forward * radius;
                transform.position = target.position + localPosFromTarget;
            }

            var euler = transform.localRotation.eulerAngles;
            euler.y += revolveSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(euler);
        }
    }
}
