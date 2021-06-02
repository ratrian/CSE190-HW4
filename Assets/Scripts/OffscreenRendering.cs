using System.Collections; using System.Collections.Generic;
using System.IO; using UnityEngine;

/// <summary>
/// Attach this script to an empty gameObject. Create a secondary camera
/// gameObject for offscreen rendering (not your main camera) and connect it
/// with this script. Offscreen camera should have a texture object attached to it.
/// OffscreenCamera texture object is used for rendering (please see camera properties).
/// </summary>
public class OffscreenRendering : MonoBehaviour {
	#region public members
	/// <summary>
	/// The desired number of screenshots per second.
	/// </summary>
	[Tooltip("Number of screenshots per second.")]
	public int ScreenshotsPerSecond = 1;
	/// <summary>
	/// Camera used to render screen to texture. Offscreen camera
	/// with desired target texture size should be attached here,
	/// not the main camera.
	/// </summary>
	[Tooltip("The camera that is used for off-screen rendering.")]
	public Camera OffscreenCameraLeft, OffscreenCameraRight;
	public GameObject LeftPlane, RightPlane, BottomPlane;
	public RenderTexture LeftPlaneLeftTexture, RightPlaneLeftTexture, BottomPlaneLeftTexture, LeftPlaneRightTexture,  RightPlaneRightTexture,  BottomPlaneRightTexture;
	#endregion
	/// <summary>
	/// Keep track of saved frames.
	/// counter is added as postifx to file names.
	/// </summary>
    private int FrameCounter = 0;
	
	// Use this for initialization
	void Start () {
		StartCoroutine("CaptureAndSaveFrames");
	}
	
	/// <summary>
	/// Captures x frames per second.
	/// </summary>
	/// <returns>Enumerator object</returns>
	IEnumerator CaptureAndSaveFrames() {
	while (true) {
		yield return new WaitForEndOfFrame();

		// Remember currently active render texture.
		RenderTexture currentRT = RenderTexture.active;

		/* Left Plane */

		Vector3 pa;// = ;
		Vector3 pb;// = ;
		Vector3 pc;// = ;
		Vector3 pe;// = ;
		/*Vector3 va = pa - pe;
		Vector3 vb = pb - pe;
		Vector3 vc = pc - pe;
		Vector3 vr = (pb - pa).normalized;
		Vector3 vu = (pc - pa).normalized;
		Vector3 vn = Vector3.Cross(vr, vu).normalized;
		float d = -Vector3.Dot(vn, va);
		float n;// = ;
		float left = Vector3.Dot(vr, va) * n / d;
		float right = Vector3.Dot(vr, vb) * n / d;
		float bottom = Vector3.Dot(vu, va) * n / d;
		float top = Vector3.Dot(vu, vc) * n / d;
		Matrix4x4 p;// = Matrix4x4.Frustum(left, right, bottom, top, , );
		Matrix4x4 mT;
		mT.SetRow(0, new Vector4(-vr.x, -vr.y, -vr.z, 0.0f));
		mT.SetRow(1, new Vector4(-vu.x, -vu.y, -vu.z, 0.0f));
		mT.SetRow(2, new Vector4(-vn.x, -vn.y, -vn.z, 0.0f));
		mT.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		Matrix4x4 t;
		//t.SetRow(0, new Vector4(1.0f, 0.0f, 0.0f, ));
		//t.SetRow(1, new Vector4(0.0f, 1.0f, 0.0f, ));
		//t.SetRow(2, new Vector4(0.0f, 0.0f, 1.0f, ));
		//t.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		OffscreenCameraLeft.projectionMatrix = p * mT * t;
		GameObject.Find("lparent").transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.LeftEye);
		GameObject.Find("lparent").transform.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.LeftEye);*/

		/*
		OffscreenCameraRight.projectionMatrix = p * mT * t;
		GameObject.Find("rparent").transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightEye);
		GameObject.Find("rparent").transform.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightEye);*/

		OffscreenCameraLeft.targetTexture = LeftPlaneLeftTexture;
		// Set target texture for left camera as active render texture.
		RenderTexture.active = OffscreenCameraLeft.targetTexture;
		// Render to texture
		OffscreenCameraLeft.Render();
		// Read offscreen texture
		Texture2D offscreenTexture = new Texture2D(OffscreenCameraLeft.targetTexture.width, OffscreenCameraLeft.targetTexture.height, TextureFormat.RGB24, false); 			
		offscreenTexture.ReadPixels(new Rect(0, 0, OffscreenCameraLeft.targetTexture.width, OffscreenCameraLeft.targetTexture.height), 0, 0, false); 			
		offscreenTexture.Apply();
		// Delete texture.
		UnityEngine.Object.Destroy(offscreenTexture);

		OffscreenCameraRight.targetTexture = LeftPlaneRightTexture;
		// Set target texture for right camera as active render texture.
		RenderTexture.active = OffscreenCameraRight.targetTexture;
		// Render to texture
		OffscreenCameraRight.Render();
		// Read offscreen texture
		offscreenTexture = new Texture2D(OffscreenCameraRight.targetTexture.width, OffscreenCameraRight.targetTexture.height, TextureFormat.RGB24, false);
		offscreenTexture.ReadPixels(new Rect(0, 0, OffscreenCameraRight.targetTexture.width, OffscreenCameraRight.targetTexture.height), 0, 0, false);
		offscreenTexture.Apply();
		// Delete texture.
		UnityEngine.Object.Destroy(offscreenTexture);

		/* Right Plane */
		
		OffscreenCameraLeft.targetTexture = RightPlaneLeftTexture;
		// Set target texture for left camera as active render texture.
		RenderTexture.active = OffscreenCameraLeft.targetTexture;
		// Render to texture
		OffscreenCameraLeft.Render();
		// Read offscreen texture
		offscreenTexture = new Texture2D(OffscreenCameraLeft.targetTexture.width, OffscreenCameraLeft.targetTexture.height, TextureFormat.RGB24, false);
		offscreenTexture.ReadPixels(new Rect(0, 0, OffscreenCameraLeft.targetTexture.width, OffscreenCameraLeft.targetTexture.height), 0, 0, false);
		offscreenTexture.Apply();
		// Delete texture.
		UnityEngine.Object.Destroy(offscreenTexture);

		OffscreenCameraRight.targetTexture = RightPlaneRightTexture;
		// Set target texture for right camera as active render texture.
		RenderTexture.active = OffscreenCameraRight.targetTexture;
		// Render to texture
		OffscreenCameraRight.Render();
		// Read offscreen texture
		offscreenTexture = new Texture2D(OffscreenCameraRight.targetTexture.width, OffscreenCameraRight.targetTexture.height, TextureFormat.RGB24, false);
		offscreenTexture.ReadPixels(new Rect(0, 0, OffscreenCameraRight.targetTexture.width, OffscreenCameraRight.targetTexture.height), 0, 0, false);
		offscreenTexture.Apply();
		// Delete texture.
		UnityEngine.Object.Destroy(offscreenTexture);

		/* Bottom Plane */

		OffscreenCameraLeft.targetTexture = BottomPlaneLeftTexture;
		// Set target texture for left camera as active render texture.
		RenderTexture.active = OffscreenCameraLeft.targetTexture;
		// Render to texture
		OffscreenCameraLeft.Render();
		// Read offscreen texture
		offscreenTexture = new Texture2D(OffscreenCameraLeft.targetTexture.width, OffscreenCameraLeft.targetTexture.height, TextureFormat.RGB24, false);
		offscreenTexture.ReadPixels(new Rect(0, 0, OffscreenCameraLeft.targetTexture.width, OffscreenCameraLeft.targetTexture.height), 0, 0, false);
		offscreenTexture.Apply();
		// Delete texture.
		UnityEngine.Object.Destroy(offscreenTexture);

		OffscreenCameraRight.targetTexture = BottomPlaneRightTexture;
		// Set target texture for right camera as active render texture.
		RenderTexture.active = OffscreenCameraRight.targetTexture;
		// Render to texture
		OffscreenCameraRight.Render();
		// Read offscreen texture
		offscreenTexture = new Texture2D(OffscreenCameraRight.targetTexture.width, OffscreenCameraRight.targetTexture.height, TextureFormat.RGB24, false);
		offscreenTexture.ReadPixels(new Rect(0, 0, OffscreenCameraRight.targetTexture.width, OffscreenCameraRight.targetTexture.height), 0, 0, false);
		offscreenTexture.Apply();
		// Delete texture.
		UnityEngine.Object.Destroy(offscreenTexture);

		// Reset previous render texture.
		RenderTexture.active = currentRT;
		++FrameCounter;

		yield return new WaitForSeconds(1.0f / ScreenshotsPerSecond);
		}
	}
	
	/// <summary>
	/// Stop image capture.
	/// </summary>
	public void StopCapturing () {
		StopCoroutine("CaptureAndSaveFrames");    
		FrameCounter = 0;
	}
	
	/// <summary>
	/// Resume image capture.
	/// </summary>
	public void ResumeCapturing () {
		StartCoroutine("CaptureAndSaveFrames");
	}
}