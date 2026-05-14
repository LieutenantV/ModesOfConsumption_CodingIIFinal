using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class FoodPickup : Interactable
{
    public override void Interact(Player player)
    {
        //Debug.Log("FoodPickup.Interact() called!");
        GainPoints();
        //Debug.Log("About to destroy: " + gameObject.name);
        Destroy(gameObject);
        //Debug.Log("Destroy() called");
    }

    public void GainPoints()
    {
        //Debug.Log("GainPoints() called");
        if (PointsManager.Instance == null)
        {
            //Debug.LogError("PointsManager.Instance is NULL!");
            return;
        }
        PointsManager.Instance.AddPoints(PointsManager.Instance.pointsPerFood);
        //Debug.Log("Points added: " + PointsManager.Instance.pointsPerFood);
    }
}