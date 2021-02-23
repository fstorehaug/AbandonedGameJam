using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float screenEdgeScrollBorderPx;
	public float cameraScrollSpeed;
	public float cameraBaseHeight;
	public float cameraMinHeight;
	public float cameraMaxHeight;
	public float cameraZoomSpeed;
	
	private float zoomLerpFactor;
	public PlayerControlls playerControlls;
	private Vector2 movementVector;

	private void Start()
	{
		zoomLerpFactor = Mathf.InverseLerp(cameraMinHeight, cameraMaxHeight, cameraBaseHeight);
		playerControlls = new PlayerControlls();
		playerControlls.Enable();

		playerControlls.KeyboardMouse.Move.performed += context => movementVector = context.ReadValue<Vector2>();
	}

	void Update()
	{
		float scrollDelta = 0f;// Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")) * cameraZoomSpeed * Time.deltaTime;
		zoomLerpFactor = Mathf.Clamp(0f, 1f, zoomLerpFactor + scrollDelta);

		var mousePosition = InteractionManager.instance.MousePosition;

		float xDelta = 0f;
		if (mousePosition.x <= screenEdgeScrollBorderPx)
			xDelta = -Mathf.Lerp(cameraScrollSpeed, 0f, (float)mousePosition.x / screenEdgeScrollBorderPx);
		else if (mousePosition.x > Screen.width - screenEdgeScrollBorderPx)
			xDelta = Mathf.Lerp(0f, cameraScrollSpeed, (float)(mousePosition.x - (Screen.width - screenEdgeScrollBorderPx)) / screenEdgeScrollBorderPx);

		if (movementVector.x != 0)
			xDelta += cameraScrollSpeed * movementVector.x;
		xDelta *= Time.deltaTime;

		float yDelta = 0f;
		if (mousePosition.y <= screenEdgeScrollBorderPx)
			yDelta = -Mathf.Lerp(cameraScrollSpeed, 0f, Mathf.Clamp01((float)mousePosition.y / screenEdgeScrollBorderPx));
		else if (mousePosition.y > Screen.height - screenEdgeScrollBorderPx)
			yDelta = Mathf.Lerp(0f, cameraScrollSpeed, Mathf.Clamp01((float)(mousePosition.y - (Screen.height - screenEdgeScrollBorderPx)) / screenEdgeScrollBorderPx));

		if (movementVector.y != 0)
			yDelta += cameraScrollSpeed * movementVector.y;

		yDelta *= Time.deltaTime;

		Vector3 pos = transform.position;
		pos += xDelta * Vector3.right + yDelta * Vector3.forward;
		pos.y = Mathf.Lerp(cameraMinHeight, cameraMaxHeight, zoomLerpFactor);
		transform.position = pos;
	}
}
