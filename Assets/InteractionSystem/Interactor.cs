using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    List<Sensor> sensors;
    public void AddSensor(Sensor sensor)
    {
        sensors.Add(sensor);
    }
    public void RemoveSensor(Sensor sensor)
    {
        sensors.Remove(sensor);
    }

    private void Update()
    {
        /*Ray ray;
        Physics.Raycast()*/
    }

    public void Interact()
    {
        float minDistance = float.MaxValue;
        Sensor closestSensor = null;
        foreach (Sensor sensor in sensors)
        {
            float distance = Vector3.Distance(sensor.Position, transform.position);
            if (distance < minDistance)
            {
                closestSensor = sensor;
                minDistance = distance;
            }
        }

        if(closestSensor != null)
        {
            closestSensor.Interact(this);
        }
    }

}
