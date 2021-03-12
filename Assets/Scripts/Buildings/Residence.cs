using System.Collections.Generic;
using UnityEngine;

public class Residence : Building
{
    List<ServiceBuilding> nearbyServices = new List<ServiceBuilding>();

    public void AddServiceBuilding(ServiceBuilding serviceBuilding)
    {
        if (!nearbyServices.Contains(serviceBuilding))
        {
            Debug.Log("Added nearby service to residence");
            nearbyServices.Add(serviceBuilding);

            ChangeEffectivity(serviceBuilding.effectivityInfluence);
        }
    }
}
