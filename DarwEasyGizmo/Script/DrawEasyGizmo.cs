using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DrawEasyGizmo : MonoBehaviour
{
    #region Variables
    //Mater Control Data
    [HideInInspector] public bool drawGizmos = true;
    [HideInInspector] public Color gizmoColor = Color.cyan;
    [HideInInspector] public float transparency = 0.5f;

    [HideInInspector] public Vector3 center = Vector3.zero;
    //--Cube--------------------------------------------
    [HideInInspector] public Vector3 cubeSize = Vector3.one;
    //--Sphere--------------------------------------------
    [HideInInspector] public float sphereRadius = 0.5f;
    //--Ray--------------------------------------------
    [HideInInspector] public float rayLength = 1f;
    //--Line--------------------------------------------
    [HideInInspector] public GameObject lineFrom;
    [HideInInspector] public GameObject lineTo;

    //Private Data
    [HideInInspector] public int gizmoTypeInt;
    [HideInInspector] public int gizmoShapeInt;
    [HideInInspector] public int rayDirectionInt;
    [HideInInspector] public List<Collider> colliderList = new List<Collider>();
    #endregion

    void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Gizmos.color = gizmoColor;
        gizmoColor.a = transparency;

        if (gizmoTypeInt == ((int)GizmosType.Collider))
            DrawColliderGizmo();

        else if (gizmoTypeInt == ((int)GizmosType.Custom))
            DrawCustomGizmo();
    }

    public void DrawColliderGizmo()
    {
        if (colliderList == null)
            colliderList = new List<Collider>();

        var colliders = colliderList;
        GetComponents(colliders);

        if (colliders == null)
            return;

        var scale = transform.lossyScale;
        var invScale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);


        foreach (var collider in colliders)
        {
            if (!collider.enabled)
                continue;

            switch (collider)
            {
                case BoxCollider _col:
                    Gizmos.DrawCube(_col.center, _col.size);
                    break;
                case SphereCollider _col:
                    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one * scale.x);
                    Gizmos.DrawSphere(_col.center, _col.radius);
                    break;
                case MeshCollider _col:
                    if (!_col.convex)
                        _col.convex = true;

                    Gizmos.DrawMesh(_col.sharedMesh);
                    break;
                default:

                    break;
            }
        }

        //colliders.Clear();
    }

    public void DrawCustomGizmo()
    {
        var finalPosition = transform.position + center;

        switch (gizmoShapeInt)
        {
            case (int)GizmoShape.Cube:
                Gizmos.DrawCube(finalPosition, cubeSize);
                break;
            case (int)GizmoShape.WireCube:
                Gizmos.DrawWireCube(finalPosition, cubeSize);
                break;
            case (int)GizmoShape.Sphere:
                Gizmos.DrawSphere(finalPosition, sphereRadius);
                break;
            case (int)GizmoShape.WireSphere:
                Gizmos.DrawWireSphere(finalPosition, sphereRadius);
                break;
            case (int)GizmoShape.Ray:
                Vector3 _direction = GetRayDirection();
                Gizmos.DrawRay(finalPosition, _direction * rayLength);
                break;
            case (int)GizmoShape.Line:
                if (lineFrom == null || lineTo == null) return;
                Gizmos.DrawLine(lineFrom.transform.position, lineTo.transform.position);
                break;
        }
    }

    public Vector3 GetRayDirection()
    {
        switch (rayDirectionInt)
        {
            case (int)RayDirection.forward:
                return Vector3.forward;
            case (int)RayDirection.backward:
                return Vector3.back;
            case (int)RayDirection.left:
                return Vector3.left;
            case (int)RayDirection.right:
                return Vector3.right;
            case (int)RayDirection.up:
                return Vector3.up;
            case (int)RayDirection.down:
                return Vector3.down;
            default:
                Debug.LogError("Wrong Ray Direction");
                break;
        }

        return Vector3.zero;

    }

    public void AddColliderComponent(AddCollider addCol)
    {
        switch(addCol)
        {
            case AddCollider.Box:
                this.gameObject.AddComponent<BoxCollider>();
                break;
            case AddCollider.Shere:
                this.gameObject.AddComponent<SphereCollider>();
                break;
            case AddCollider.Capsule:
                this.gameObject.AddComponent<CapsuleCollider>();
                break;
            case AddCollider.Mesh:
                this.gameObject.AddComponent<MeshCollider>();
                break;
        }
    }



}//class

#region Enums
public enum GizmosType { Collider, Custom }
public enum GizmoShape { Cube, WireCube, Sphere, WireSphere, Ray, Line }
public enum RayDirection { forward, backward, left, right, up, down }

public enum AddCollider { Box, Shere, Capsule, Mesh}

#endregion


