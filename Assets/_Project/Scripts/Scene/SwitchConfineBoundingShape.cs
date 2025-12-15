using Unity.Cinemachine;
using UnityEngine;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    private void Start()
    {
        SwitchBoundingShape();
    }

    /// <summary>
    /// Switch the collider that Cinemachine uses to define the edges of the screen.
    /// </summary>
    private void SwitchBoundingShape()
    {
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.BOUNDS_CONFINER).GetComponent<PolygonCollider2D>();
        CinemachineConfiner2D cinemachineConfiner2D = GetComponent<CinemachineConfiner2D>();

        cinemachineConfiner2D.BoundingShape2D = polygonCollider2D;
        cinemachineConfiner2D.InvalidateBoundingShapeCache();
    }
}