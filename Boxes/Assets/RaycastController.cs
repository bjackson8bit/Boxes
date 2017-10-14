using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class RaycastController : MonoBehaviour {
	protected const float skinWidth = 0.015f;

	[Header ("Raycast Parameters")]
	public float distBetweenHRays = 0.25f;
	public float distBetweenVRays = 0.25f;

	[Header ("Collision Masks")]
	public LayerMask obstacleCollisionMask; //collisions mask specific for obstacles
	public LayerMask oneWayCollisionMask; //collision mask specific for oneWayPlatforms

	internal int raycastHorizontalCount = 8;
	internal int raycastVerticalCount = 8;
	internal float horizontalRaySpacing;
	internal float verticalRaySpacing;
	internal RaycastOrigins raycastOrigins;
	internal BoxCollider collider; //collider of object extending this class
	const float distanceBetweenRays = 0.25f;

	public virtual void Awake() {
		collider = GetComponent<BoxCollider> ();
	}

	public virtual void Start () {
		CalculateRaySpacing ();
	}

	internal void UpdateRaycastOrigins () {
		Bounds bounds = GetBounds ();
		raycastOrigins.topLeft = new Vector3 (bounds.min.x, bounds.max.y, bounds.center.z);
		raycastOrigins.topRight = new Vector3 (bounds.max.x, bounds.max.y, bounds.center.z);	
		raycastOrigins.bottomLeft = new Vector3 (bounds.min.x, bounds.min.y, bounds.center.z);
		raycastOrigins.bottomRight = new Vector3 (bounds.max.x, bounds.min.y, bounds.center.z);
	}

	internal void CalculateRaySpacing() {
		Bounds bounds = GetBounds ();
		float boundsWidth = bounds.size.x;
		float boundsHeight = bounds.size.y;
		raycastHorizontalCount = Mathf.RoundToInt(boundsHeight / distBetweenHRays);
		raycastVerticalCount = Mathf.RoundToInt(boundsWidth / distBetweenVRays);
		horizontalRaySpacing = bounds.size.y / (raycastHorizontalCount - 1);
		verticalRaySpacing = bounds.size.x / (raycastVerticalCount - 1);	
	}

	Bounds GetBounds() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth* -2);
		return bounds;
	}

	internal struct RaycastOrigins {
		public Vector3 topRight, topLeft, bottomLeft, bottomRight;
	}
}
