using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class PhysicsController : MonoBehaviour {
	public LayerMask collisionMask;
	public LayerMask interactMask;

	const float skinWidth = .015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	float horizontalRaySpacing;
	float verticalRaySpacing;

	BoxCollider collider;
	RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;

	void Start() {
		collider = GetComponent<BoxCollider> ();
		CalculateRaySpacing ();
	}

	public void Move(Vector3 velocity) {
		UpdateRaycastOrigins ();
		collisions.Reset ();

		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}

		transform.Translate (velocity);
	}

	void HorizontalCollisions(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit hit;

			bool hitBool = Physics.Raycast (rayOrigin, Vector3.right * directionX, out hit, rayLength, collisionMask);

			RaycastHit interact;
			bool interactBool = Physics.Raycast (rayOrigin, Vector3.right * directionX, out interact, rayLength, interactMask);

			if (hitBool) {
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.collided = hit.collider.gameObject;
				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
			if (interactBool) {
				collisions.interacted = interact.collider.gameObject;
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit hit;

			bool hitBool = Physics.Raycast (rayOrigin, Vector3.up * directionY, out hit, rayLength, collisionMask);

			RaycastHit interact;
			bool interactBool = Physics.Raycast (rayOrigin, Vector3.up * directionY, out interact, rayLength, interactMask);


			if (hitBool) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.collided = hit.collider.gameObject;
				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
			if (interactBool) {
				collisions.interacted = interact.collider.gameObject;
			}
		}
	}

	void UpdateRaycastOrigins() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;
		public GameObject collided,interacted;
		public void Reset() {
			above = below = false;
			left = right = false;
			collided = null;
			interacted = null;
		}
	}
}