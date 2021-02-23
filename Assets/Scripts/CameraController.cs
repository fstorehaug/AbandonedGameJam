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

	private void Start()
	{
		zoomLerpFactor = Mathf.InverseLerp(cameraMinHeight, cameraMaxHeight, cameraBaseHeight);
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

		xDelta *= Time.deltaTime;

		float yDelta = 0f;
		if (mousePosition.y <= screenEdgeScrollBorderPx)
			yDelta = -Mathf.Lerp(cameraScrollSpeed, 0f, Mathf.Clamp01((float)mousePosition.y / screenEdgeScrollBorderPx));
		else if (mousePosition.y > Screen.height - screenEdgeScrollBorderPx)
			yDelta = Mathf.Lerp(0f, cameraScrollSpeed, Mathf.Clamp01((float)(mousePosition.y - (Screen.height - screenEdgeScrollBorderPx)) / screenEdgeScrollBorderPx));

		yDelta *= Time.deltaTime;

		Vector3 pos = transform.position;
		pos += xDelta * Vector3.right + yDelta * Vector3.forward;
		pos.y = Mathf.Lerp(cameraMinHeight, cameraMaxHeight, zoomLerpFactor);
		transform.position = pos;
	}
}
