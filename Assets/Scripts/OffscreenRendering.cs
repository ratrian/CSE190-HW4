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
	public Camera ControllerCameraLeft, ControllerCameraRight;
	public RenderTexture LeftPlaneLeftTexture, RightPlaneLeftTexture, BottomPlaneLeftTexture, LeftPlaneRightTexture, RightPlaneRightTexture, BottomPlaneRightTexture;
	Matrix4x4 pPrimeLeftPlaneLeft, pPrimeRightPlaneLeft, pPrimeBottomPlaneLeft, pPrimeLeftPlaneRight, pPrimeRightPlaneRight, pPrimeBottomPlaneRight;
	LineRenderer LeftPlaneLeftLine1, LeftPlaneLeftLine2, LeftPlaneLeftLine3, LeftPlaneLeftLine4, RightPlaneLeftLine1, RightPlaneLeftLine2, RightPlaneLeftLine3, RightPlaneLeftLine4, BottomPlaneLeftLine1, BottomPlaneLeftLine2, BottomPlaneLeftLine3, BottomPlaneLeftLine4, LeftPlaneRightLine1, LeftPlaneRightLine2, LeftPlaneRightLine3, LeftPlaneRightLine4, RightPlaneRightLine1, RightPlaneRightLine2, RightPlaneRightLine3, RightPlaneRightLine4, BottomPlaneRightLine1, BottomPlaneRightLine2, BottomPlaneRightLine3, BottomPlaneRightLine4;
	#endregion
	/// <summary>
	/// Keep track of saved frames.
	/// counter is added as postifx to file names.
	/// </summary>
    private int FrameCounter = 0;
	
	// Use this for initialization
	void Start () {
		LeftPlaneLeftLine1 = new GameObject("LeftPlaneLeftLine1").AddComponent<LineRenderer>();
		LeftPlaneLeftLine1.enabled = false;
		LeftPlaneLeftLine2 = new GameObject("LeftPlaneLeftLine2").AddComponent<LineRenderer>();
		LeftPlaneLeftLine2.enabled = false;
		LeftPlaneLeftLine3 = new GameObject("LeftPlaneLeftLine3").AddComponent<LineRenderer>();
		LeftPlaneLeftLine3.enabled = false;
		LeftPlaneLeftLine4 = new GameObject("LeftPlaneLeftLine4").AddComponent<LineRenderer>();
		LeftPlaneLeftLine4.enabled = false;
		RightPlaneLeftLine1 = new GameObject("RightPlaneLeftLine1").AddComponent<LineRenderer>();
		RightPlaneLeftLine1.enabled = false;
		RightPlaneLeftLine2 = new GameObject("RightPlaneLeftLine2").AddComponent<LineRenderer>();
		RightPlaneLeftLine2.enabled = false;
		RightPlaneLeftLine3 = new GameObject("RightPlaneLeftLine3").AddComponent<LineRenderer>();
		RightPlaneLeftLine3.enabled = false;
		RightPlaneLeftLine4 = new GameObject("RightPlaneLeftLine4").AddComponent<LineRenderer>();
		RightPlaneLeftLine4.enabled = false;
		BottomPlaneLeftLine1 = new GameObject("BottomPlaneLeftLine1").AddComponent<LineRenderer>();
		BottomPlaneLeftLine1.enabled = false;
		BottomPlaneLeftLine2 = new GameObject("BottomPlaneLeftLine2").AddComponent<LineRenderer>();
		BottomPlaneLeftLine2.enabled = false;
		BottomPlaneLeftLine3 = new GameObject("BottomPlaneLeftLine3").AddComponent<LineRenderer>();
		BottomPlaneLeftLine3.enabled = false;
		BottomPlaneLeftLine4 = new GameObject("BottomPlaneLeftLine4").AddComponent<LineRenderer>();
		BottomPlaneLeftLine4.enabled = false;
		LeftPlaneRightLine1 = new GameObject("LeftPlaneRightLine1").AddComponent<LineRenderer>();
		LeftPlaneRightLine1.enabled = false;
		LeftPlaneRightLine2 = new GameObject("LeftPlaneRightLine2").AddComponent<LineRenderer>();
		LeftPlaneRightLine2.enabled = false;
		LeftPlaneRightLine3 = new GameObject("LeftPlaneRightLine3").AddComponent<LineRenderer>();
		LeftPlaneRightLine3.enabled = false;
		LeftPlaneRightLine4 = new GameObject("LeftPlaneRightLine4").AddComponent<LineRenderer>();
		LeftPlaneRightLine4.enabled = false;
		RightPlaneRightLine1 = new GameObject("RightPlaneRightLine1").AddComponent<LineRenderer>();
		RightPlaneRightLine1.enabled = false;
		RightPlaneRightLine2 = new GameObject("RightPlaneRightLine2").AddComponent<LineRenderer>();
		RightPlaneRightLine2.enabled = false;
		RightPlaneRightLine3 = new GameObject("RightPlaneRightLine3").AddComponent<LineRenderer>();
		RightPlaneRightLine3.enabled = false;
		RightPlaneRightLine4 = new GameObject("RightPlaneRightLine4").AddComponent<LineRenderer>();
		RightPlaneRightLine4.enabled = false;
		BottomPlaneRightLine1 = new GameObject("BottomPlaneRightLine1").AddComponent<LineRenderer>();
		BottomPlaneRightLine1.enabled = false;
		BottomPlaneRightLine2 = new GameObject("BottomPlaneRightLine2").AddComponent<LineRenderer>();
		BottomPlaneRightLine2.enabled = false;
		BottomPlaneRightLine3 = new GameObject("BottomPlaneRightLine3").AddComponent<LineRenderer>();
		BottomPlaneRightLine3.enabled = false;
		BottomPlaneRightLine4 = new GameObject("BottomPlaneRightLine4").AddComponent<LineRenderer>();
		BottomPlaneRightLine4.enabled = false;

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

		Vector3 pa = new Vector3(-3.75f, -5.0f, -3.75f);
		Vector3 pb = new Vector3(-3.75f, -5.0f, 3.75f);
		Vector3 pc = new Vector3(-3.75f, 2.5f, -3.75f);
		Vector3 pd = new Vector3(-3.75f, 2.5f, 3.75f);
		Vector3 pe = GameObject.Find("LeftEyeAnchor").GetComponent<Camera>().transform.position;
		Vector3 va = pa - pe;
		Vector3 vb = pb - pe;
		Vector3 vc = pc - pe;
		Vector3 vr = (pb - pa).normalized;
		Vector3 vu = (pc - pa).normalized;
		Vector3 vn = Vector3.Cross(vr, vu).normalized;
		float d = -Vector3.Dot(vn, va);
		float left = Vector3.Dot(vr, va) * (0.3f) / d;
		float right = Vector3.Dot(vr, vb) * (0.3f) / d;
		float bottom = Vector3.Dot(vu, va) * (0.3f) / d;
		float top = Vector3.Dot(vu, vc) * (0.3f) / d;
		Matrix4x4 p = Matrix4x4.Frustum(left, right, bottom, top, 0.3f, 1000.0f);
		Matrix4x4 mT = new Matrix4x4();
		mT.SetRow(0, new Vector4(-vr.x, -vr.y, vr.z, 0.0f));
		mT.SetRow(1, new Vector4(-vu.x, -vu.y, vu.z, 0.0f));
		mT.SetRow(2, new Vector4(-vn.x, -vn.y, vn.z, 0.0f));
		mT.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		Matrix4x4 t = new Matrix4x4();
		t.SetRow(0, new Vector4(1.0f, 0.0f, 0.0f, -pe.x));
		t.SetRow(1, new Vector4(0.0f, 1.0f, 0.0f, -pe.y));
		t.SetRow(2, new Vector4(0.0f, 0.0f, 1.0f, -pe.z));
		t.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		if (!FreezeModeBehaviour.freeze)
		{
			GameObject.Find("lparent").transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.LeftEye);
			Vector3 temp = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightHand);
			temp.x -= 0.0325f;
			GameObject.Find("controllerlparent").transform.localPosition = temp;
			if (UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightHand).eulerAngles.y < 180.0f)
			{
				ControllerCameraLeft.cullingMask = (1 << 7) | (1 << 8);
			}
			else
            {
				ControllerCameraLeft.cullingMask = (1 << 7) | (1 << 9);
			}
			pPrimeLeftPlaneLeft = p * mT * t;
		}

		if (HeadInHandModeBehaviour.displayPyramids)
		{
			LeftPlaneLeftLine1.enabled = true;
			LeftPlaneLeftLine1.material.color = Color.green;
			LeftPlaneLeftLine1.startColor = Color.green;
			LeftPlaneLeftLine1.endColor = Color.green;
			LeftPlaneLeftLine1.startWidth = 0.01f;
			LeftPlaneLeftLine1.endWidth = 0.01f;
			LeftPlaneLeftLine1.positionCount = 2;
			LeftPlaneLeftLine1.useWorldSpace = true;
			LeftPlaneLeftLine1.SetPosition(0, ControllerCameraLeft.transform.position);
			LeftPlaneLeftLine1.SetPosition(1, pa);

			LeftPlaneLeftLine2.enabled = true;
			LeftPlaneLeftLine2.material.color = Color.green;
			LeftPlaneLeftLine2.startColor = Color.green;
			LeftPlaneLeftLine2.endColor = Color.green;
			LeftPlaneLeftLine2.startWidth = 0.01f;
			LeftPlaneLeftLine2.endWidth = 0.01f;
			LeftPlaneLeftLine2.positionCount = 2;
			LeftPlaneLeftLine2.useWorldSpace = true;
			LeftPlaneLeftLine2.SetPosition(0, ControllerCameraLeft.transform.position);
			LeftPlaneLeftLine2.SetPosition(1, pb);

			LeftPlaneLeftLine3.enabled = true;
			LeftPlaneLeftLine3.material.color = Color.green;
			LeftPlaneLeftLine3.startColor = Color.green;
			LeftPlaneLeftLine3.endColor = Color.green;
			LeftPlaneLeftLine3.startWidth = 0.01f;
			LeftPlaneLeftLine3.endWidth = 0.01f;
			LeftPlaneLeftLine3.positionCount = 2;
			LeftPlaneLeftLine3.useWorldSpace = true;
			LeftPlaneLeftLine3.SetPosition(0, ControllerCameraLeft.transform.position);
			LeftPlaneLeftLine3.SetPosition(1, pc);

			LeftPlaneLeftLine4.enabled = true;
			LeftPlaneLeftLine4.material.color = Color.green;
			LeftPlaneLeftLine4.startColor = Color.green;
			LeftPlaneLeftLine4.endColor = Color.green;
			LeftPlaneLeftLine4.startWidth = 0.01f;
			LeftPlaneLeftLine4.endWidth = 0.01f;
			LeftPlaneLeftLine4.positionCount = 2;
			LeftPlaneLeftLine4.useWorldSpace = true;
			LeftPlaneLeftLine4.SetPosition(0, ControllerCameraLeft.transform.position);
			LeftPlaneLeftLine4.SetPosition(1, pd);
		}
		else
		{
			LeftPlaneLeftLine1.enabled = false;
			LeftPlaneLeftLine2.enabled = false;
			LeftPlaneLeftLine3.enabled = false;
			LeftPlaneLeftLine4.enabled = false;
		}

		if ((FreezeModeBehaviour.fail) && (FreezeModeBehaviour.randomChoice == 0))
		{
			LeftPlaneLeftTexture.Release();
		}
		else
		{
			// Set target texture for left camera as active render texture.
			RenderTexture.active = LeftPlaneLeftTexture;
			if (HeadInHandModeBehaviour.controllerView)
			{
				OffscreenCameraLeft.enabled = false;
				ControllerCameraLeft.enabled = true;

				ControllerCameraLeft.projectionMatrix = pPrimeLeftPlaneLeft;
				ControllerCameraLeft.targetTexture = LeftPlaneLeftTexture;
				// Render to texture
				ControllerCameraLeft.Render();
			}
			else
			{
				OffscreenCameraLeft.enabled = true;
				ControllerCameraLeft.enabled = false;

				OffscreenCameraLeft.projectionMatrix = pPrimeLeftPlaneLeft;
				OffscreenCameraLeft.targetTexture = LeftPlaneLeftTexture;
				// Render to texture
				OffscreenCameraLeft.Render();
			}
		}

		pa = new Vector3(-3.75f, -5.0f, -3.75f);
		pb = new Vector3(-3.75f, -5.0f, 3.75f);
		pc = new Vector3(-3.75f, 2.5f, -3.75f);
		pd = new Vector3(-3.75f, 2.5f, 3.75f);
		pe = GameObject.Find("RightEyeAnchor").GetComponent<Camera>().transform.position;
		va = pa - pe;
		vb = pb - pe;
		vc = pc - pe;
		vr = (pb - pa).normalized;
		vu = (pc - pa).normalized;
		vn = Vector3.Cross(vr, vu).normalized;
		d = -Vector3.Dot(vn, va);
		left = Vector3.Dot(vr, va) * (0.3f) / d;
		right = Vector3.Dot(vr, vb) * (0.3f) / d;
		bottom = Vector3.Dot(vu, va) * (0.3f) / d;
		top = Vector3.Dot(vu, vc) * (0.3f) / d;
		p = Matrix4x4.Frustum(left, right, bottom, top, 0.3f, 1000.0f);
		mT = new Matrix4x4();
		mT.SetRow(0, new Vector4(-vr.x, -vr.y, vr.z, 0.0f));
		mT.SetRow(1, new Vector4(-vu.x, -vu.y, vu.z, 0.0f));
		mT.SetRow(2, new Vector4(-vn.x, -vn.y, vn.z, 0.0f));
		mT.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		t = new Matrix4x4();
		t.SetRow(0, new Vector4(1.0f, 0.0f, 0.0f, -pe.x));
		t.SetRow(1, new Vector4(0.0f, 1.0f, 0.0f, -pe.y));
		t.SetRow(2, new Vector4(0.0f, 0.0f, 1.0f, -pe.z));
		t.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		if (!FreezeModeBehaviour.freeze)
		{
			GameObject.Find("rparent").transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightEye);
			Vector3 temp = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightHand);
			temp.x += 0.0325f;
			GameObject.Find("controllerrparent").transform.localPosition = temp;
			if (UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightHand).eulerAngles.y < 180.0f)
			{
				ControllerCameraRight.cullingMask = (1 << 7) | (1 << 9);
			}
			else
			{
				ControllerCameraRight.cullingMask = (1 << 7) | (1 << 8);
			}
			pPrimeLeftPlaneRight = p * mT * t;
		}

		if (HeadInHandModeBehaviour.displayPyramids)
		{
			LeftPlaneRightLine1.enabled = true;
			LeftPlaneRightLine1.material.color = Color.red;
			LeftPlaneRightLine1.startColor = Color.red;
			LeftPlaneRightLine1.endColor = Color.red;
			LeftPlaneRightLine1.startWidth = 0.01f;
			LeftPlaneRightLine1.endWidth = 0.01f;
			LeftPlaneRightLine1.positionCount = 2;
			LeftPlaneRightLine1.useWorldSpace = true;
			LeftPlaneRightLine1.SetPosition(0, ControllerCameraRight.transform.position);
			LeftPlaneRightLine1.SetPosition(1, pa);

			LeftPlaneRightLine2.enabled = true;
			LeftPlaneRightLine2.material.color = Color.red;
			LeftPlaneRightLine2.startColor = Color.red;
			LeftPlaneRightLine2.endColor = Color.red;
			LeftPlaneRightLine2.startWidth = 0.01f;
			LeftPlaneRightLine2.endWidth = 0.01f;
			LeftPlaneRightLine2.positionCount = 2;
			LeftPlaneRightLine2.useWorldSpace = true;
			LeftPlaneRightLine2.SetPosition(0, ControllerCameraRight.transform.position);
			LeftPlaneRightLine2.SetPosition(1, pb);

			LeftPlaneRightLine3.enabled = true;
			LeftPlaneRightLine3.material.color = Color.red;
			LeftPlaneRightLine3.startColor = Color.red;
			LeftPlaneRightLine3.endColor = Color.red;
			LeftPlaneRightLine3.startWidth = 0.01f;
			LeftPlaneRightLine3.endWidth = 0.01f;
			LeftPlaneRightLine3.positionCount = 2;
			LeftPlaneRightLine3.useWorldSpace = true;
			LeftPlaneRightLine3.SetPosition(0, ControllerCameraRight.transform.position);
			LeftPlaneRightLine3.SetPosition(1, pc);

			LeftPlaneRightLine4.enabled = true;
			LeftPlaneRightLine4.material.color = Color.red;
			LeftPlaneRightLine4.startColor = Color.red;
			LeftPlaneRightLine4.endColor = Color.red;
			LeftPlaneRightLine4.startWidth = 0.01f;
			LeftPlaneRightLine4.endWidth = 0.01f;
			LeftPlaneRightLine4.positionCount = 2;
			LeftPlaneRightLine4.useWorldSpace = true;
			LeftPlaneRightLine4.SetPosition(0, ControllerCameraRight.transform.position);
			LeftPlaneRightLine4.SetPosition(1, pd);
		}
		else
		{
			LeftPlaneRightLine1.enabled = false;
			LeftPlaneRightLine2.enabled = false;
			LeftPlaneRightLine3.enabled = false;
			LeftPlaneRightLine4.enabled = false;
		}

		if ((FreezeModeBehaviour.fail) && (FreezeModeBehaviour.randomChoice == 1))
		{
			LeftPlaneRightTexture.Release();
		}
		else
		{
			// Set target texture for right camera as active render texture.
			RenderTexture.active = LeftPlaneRightTexture;
			if (HeadInHandModeBehaviour.controllerView)
			{
				OffscreenCameraRight.enabled = false;
				ControllerCameraRight.enabled = true;

				ControllerCameraRight.projectionMatrix = pPrimeLeftPlaneRight;
				ControllerCameraRight.targetTexture = LeftPlaneRightTexture;
				// Render to texture
				ControllerCameraRight.Render();
			}
			else
			{
				OffscreenCameraRight.enabled = true;
				ControllerCameraRight.enabled = false;

				OffscreenCameraRight.projectionMatrix = pPrimeLeftPlaneRight;
				OffscreenCameraRight.targetTexture = LeftPlaneRightTexture;
				// Render to texture
				OffscreenCameraRight.Render();
			}
		}

		/* Right Plane */

		pa = new Vector3(-3.75f, -5.0f, 3.75f);
		pb = new Vector3(3.75f, -5.0f, 3.75f);
		pc = new Vector3(-3.75f, 2.5f, 3.75f);
		pd = new Vector3(3.75f, 2.5f, 3.75f);
		pe = GameObject.Find("LeftEyeAnchor").GetComponent<Camera>().transform.position;
		va = pa - pe;
		vb = pb - pe;
		vc = pc - pe;
		vr = (pb - pa).normalized;
		vu = (pc - pa).normalized;
		vn = Vector3.Cross(vr, vu).normalized;
		d = -Vector3.Dot(vn, va);
		left = Vector3.Dot(vr, va) * (0.3f) / d;
		right = Vector3.Dot(vr, vb) * (0.3f) / d;
		bottom = Vector3.Dot(vu, va) * (0.3f) / d;
		top = Vector3.Dot(vu, vc) * (0.3f) / d;
		p = Matrix4x4.Frustum(left, right, bottom, top, 0.3f, 1000.0f);
		mT = new Matrix4x4();
		mT.SetRow(0, new Vector4(-vr.x, -vr.y, vr.z, 0.0f));
		mT.SetRow(1, new Vector4(-vu.x, -vu.y, vu.z, 0.0f));
		mT.SetRow(2, new Vector4(-vn.x, -vn.y, vn.z, 0.0f));
		mT.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		t = new Matrix4x4();
		t.SetRow(0, new Vector4(1.0f, 0.0f, 0.0f, -pe.x));
		t.SetRow(1, new Vector4(0.0f, 1.0f, 0.0f, -pe.y));
		t.SetRow(2, new Vector4(0.0f, 0.0f, 1.0f, -pe.z));
		t.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		if (!FreezeModeBehaviour.freeze)
		{
			GameObject.Find("lparent").transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.LeftEye);
			Vector3 temp = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightHand);
			temp.x -= 0.0325f;
			GameObject.Find("controllerlparent").transform.localPosition = temp;
			if (UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightHand).eulerAngles.y < 180.0f)
			{
				ControllerCameraLeft.cullingMask = (1 << 7) | (1 << 8);
			}
			else
			{
				ControllerCameraLeft.cullingMask = (1 << 7) | (1 << 9);
			}
			pPrimeRightPlaneLeft = p * mT * t;
		}

		if (HeadInHandModeBehaviour.displayPyramids)
		{
			RightPlaneLeftLine1.enabled = true;
			RightPlaneLeftLine1.material.color = Color.green;
			RightPlaneLeftLine1.startColor = Color.green;
			RightPlaneLeftLine1.endColor = Color.green;
			RightPlaneLeftLine1.startWidth = 0.01f;
			RightPlaneLeftLine1.endWidth = 0.01f;
			RightPlaneLeftLine1.positionCount = 2;
			RightPlaneLeftLine1.useWorldSpace = true;
			RightPlaneLeftLine1.SetPosition(0, ControllerCameraLeft.transform.position);
			RightPlaneLeftLine1.SetPosition(1, pa);

			RightPlaneLeftLine2.enabled = true;
			RightPlaneLeftLine2.material.color = Color.green;
			RightPlaneLeftLine2.startColor = Color.green;
			RightPlaneLeftLine2.endColor = Color.green;
			RightPlaneLeftLine2.startWidth = 0.01f;
			RightPlaneLeftLine2.endWidth = 0.01f;
			RightPlaneLeftLine2.positionCount = 2;
			RightPlaneLeftLine2.useWorldSpace = true;
			RightPlaneLeftLine2.SetPosition(0, ControllerCameraLeft.transform.position);
			RightPlaneLeftLine2.SetPosition(1, pb);

			RightPlaneLeftLine3.enabled = true;
			RightPlaneLeftLine3.material.color = Color.green;
			RightPlaneLeftLine3.startColor = Color.green;
			RightPlaneLeftLine3.endColor = Color.green;
			RightPlaneLeftLine3.startWidth = 0.01f;
			RightPlaneLeftLine3.endWidth = 0.01f;
			RightPlaneLeftLine3.positionCount = 2;
			RightPlaneLeftLine3.useWorldSpace = true;
			RightPlaneLeftLine3.SetPosition(0, ControllerCameraLeft.transform.position);
			RightPlaneLeftLine3.SetPosition(1, pc);

			RightPlaneLeftLine4.enabled = true;
			RightPlaneLeftLine4.material.color = Color.green;
			RightPlaneLeftLine4.startColor = Color.green;
			RightPlaneLeftLine4.endColor = Color.green;
			RightPlaneLeftLine4.startWidth = 0.01f;
			RightPlaneLeftLine4.endWidth = 0.01f;
			RightPlaneLeftLine4.positionCount = 2;
			RightPlaneLeftLine4.useWorldSpace = true;
			RightPlaneLeftLine4.SetPosition(0, ControllerCameraLeft.transform.position);
			RightPlaneLeftLine4.SetPosition(1, pd);
		}
		else
		{
			RightPlaneLeftLine1.enabled = false;
			RightPlaneLeftLine2.enabled = false;
			RightPlaneLeftLine3.enabled = false;
			RightPlaneLeftLine4.enabled = false;
		}

		if ((FreezeModeBehaviour.fail) && (FreezeModeBehaviour.randomChoice == 2))
		{
			RightPlaneLeftTexture.Release();
		}
		else
		{
			// Set target texture for left camera as active render texture.
			RenderTexture.active = RightPlaneLeftTexture;
			if (HeadInHandModeBehaviour.controllerView)
			{
				OffscreenCameraLeft.enabled = false;
				ControllerCameraLeft.enabled = true;

				ControllerCameraLeft.projectionMatrix = pPrimeRightPlaneLeft;
				ControllerCameraLeft.targetTexture = RightPlaneLeftTexture;
				// Render to texture
				ControllerCameraLeft.Render();
			}
			else
			{
				OffscreenCameraLeft.enabled = true;
				ControllerCameraLeft.enabled = false;

				OffscreenCameraLeft.projectionMatrix = pPrimeRightPlaneLeft;
				OffscreenCameraLeft.targetTexture = RightPlaneLeftTexture;
				// Render to texture
				OffscreenCameraLeft.Render();
			}
		}

		pa = new Vector3(-3.75f, -5.0f, 3.75f);
		pb = new Vector3(3.75f, -5.0f, 3.75f);
		pc = new Vector3(-3.75f, 2.5f, 3.75f);
		pd = new Vector3(3.75f, 2.5f, 3.75f);
		pe = GameObject.Find("RightEyeAnchor").GetComponent<Camera>().transform.position;
		va = pa - pe;
		vb = pb - pe;
		vc = pc - pe;
		vr = (pb - pa).normalized;
		vu = (pc - pa).normalized;
		vn = Vector3.Cross(vr, vu).normalized;
		d = -Vector3.Dot(vn, va);
		left = Vector3.Dot(vr, va) * (0.3f) / d;
		right = Vector3.Dot(vr, vb) * (0.3f) / d;
		bottom = Vector3.Dot(vu, va) * (0.3f) / d;
		top = Vector3.Dot(vu, vc) * (0.3f) / d;
		p = Matrix4x4.Frustum(left, right, bottom, top, 0.3f, 1000.0f);
		mT = new Matrix4x4();
		mT.SetRow(0, new Vector4(-vr.x, -vr.y, vr.z, 0.0f));
		mT.SetRow(1, new Vector4(-vu.x, -vu.y, vu.z, 0.0f));
		mT.SetRow(2, new Vector4(-vn.x, -vn.y, vn.z, 0.0f));
		mT.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		t = new Matrix4x4();
		t.SetRow(0, new Vector4(1.0f, 0.0f, 0.0f, -pe.x));
		t.SetRow(1, new Vector4(0.0f, 1.0f, 0.0f, -pe.y));
		t.SetRow(2, new Vector4(0.0f, 0.0f, 1.0f, -pe.z));
		t.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		if (!FreezeModeBehaviour.freeze)
		{
			GameObject.Find("rparent").transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightEye);
			Vector3 temp = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightHand);
			temp.x += 0.0325f;
			GameObject.Find("controllerrparent").transform.localPosition = temp;
			if (UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightHand).eulerAngles.y < 180.0f)
			{
				ControllerCameraRight.cullingMask = (1 << 7) | (1 << 9);
			}
			else
			{
				ControllerCameraRight.cullingMask = (1 << 7) | (1 << 8);
			}
			pPrimeRightPlaneRight = p * mT * t;
		}

		if (HeadInHandModeBehaviour.displayPyramids)
		{
			RightPlaneRightLine1.enabled = true;
			RightPlaneRightLine1.material.color = Color.red;
			RightPlaneRightLine1.startColor = Color.red;
			RightPlaneRightLine1.endColor = Color.red;
			RightPlaneRightLine1.startWidth = 0.01f;
			RightPlaneRightLine1.endWidth = 0.01f;
			RightPlaneRightLine1.positionCount = 2;
			RightPlaneRightLine1.useWorldSpace = true;
			RightPlaneRightLine1.SetPosition(0, ControllerCameraRight.transform.position);
			RightPlaneRightLine1.SetPosition(1, pa);

			RightPlaneRightLine2.enabled = true;
			RightPlaneRightLine2.material.color = Color.red;
			RightPlaneRightLine2.startColor = Color.red;
			RightPlaneRightLine2.endColor = Color.red;
			RightPlaneRightLine2.startWidth = 0.01f;
			RightPlaneRightLine2.endWidth = 0.01f;
			RightPlaneRightLine2.positionCount = 2;
			RightPlaneRightLine2.useWorldSpace = true;
			RightPlaneRightLine2.SetPosition(0, ControllerCameraRight.transform.position);
			RightPlaneRightLine2.SetPosition(1, pb);

			RightPlaneRightLine3.enabled = true;
			RightPlaneRightLine3.material.color = Color.red;
			RightPlaneRightLine3.startColor = Color.red;
			RightPlaneRightLine3.endColor = Color.red;
			RightPlaneRightLine3.startWidth = 0.01f;
			RightPlaneRightLine3.endWidth = 0.01f;
			RightPlaneRightLine3.positionCount = 2;
			RightPlaneRightLine3.useWorldSpace = true;
			RightPlaneRightLine3.SetPosition(0, ControllerCameraRight.transform.position);
			RightPlaneRightLine3.SetPosition(1, pc);

			RightPlaneRightLine4.enabled = true;
			RightPlaneRightLine4.material.color = Color.red;
			RightPlaneRightLine4.startColor = Color.red;
			RightPlaneRightLine4.endColor = Color.red;
			RightPlaneRightLine4.startWidth = 0.01f;
			RightPlaneRightLine4.endWidth = 0.01f;
			RightPlaneRightLine4.positionCount = 2;
			RightPlaneRightLine4.useWorldSpace = true;
			RightPlaneRightLine4.SetPosition(0, ControllerCameraRight.transform.position);
			RightPlaneRightLine4.SetPosition(1, pd);
		}
		else
		{
			RightPlaneRightLine1.enabled = false;
			RightPlaneRightLine2.enabled = false;
			RightPlaneRightLine3.enabled = false;
			RightPlaneRightLine4.enabled = false;
		}

		if ((FreezeModeBehaviour.fail) && (FreezeModeBehaviour.randomChoice == 3))
		{
			RightPlaneRightTexture.Release();
		}
		else
		{
			// Set target texture for right camera as active render texture.
			RenderTexture.active = RightPlaneRightTexture;
			if (HeadInHandModeBehaviour.controllerView)
			{
				OffscreenCameraRight.enabled = false;
				ControllerCameraRight.enabled = true;

				ControllerCameraRight.projectionMatrix = pPrimeRightPlaneRight;
				ControllerCameraRight.targetTexture = RightPlaneRightTexture;
				// Render to texture
				ControllerCameraRight.Render();
			}
			else
			{
				OffscreenCameraRight.enabled = true;
				ControllerCameraRight.enabled = false;

				OffscreenCameraRight.projectionMatrix = pPrimeRightPlaneRight;
				OffscreenCameraRight.targetTexture = RightPlaneRightTexture;
				// Render to texture
				OffscreenCameraRight.Render();
			}
		}

		/* Bottom Plane */

		pa = new Vector3(3.75f, -5.0f, -3.75f);
		pb = new Vector3(3.75f, -5.0f, 3.75f);
		pc = new Vector3(-3.75f, -5.0f, -3.75f);
		pd = new Vector3(-3.75f, -5.0f, 3.75f);
		pe = GameObject.Find("LeftEyeAnchor").GetComponent<Camera>().transform.position;
		va = pa - pe;
		vb = pb - pe;
		vc = pc - pe;
		vr = (pb - pa).normalized;
		vu = (pc - pa).normalized;
		vn = Vector3.Cross(vr, vu).normalized;
		d = -Vector3.Dot(vn, va);
		left = Vector3.Dot(vr, va) * (0.3f) / d;
		right = Vector3.Dot(vr, vb) * (0.3f) / d;
		bottom = Vector3.Dot(vu, va) * (0.3f) / d;
		top = Vector3.Dot(vu, vc) * (0.3f) / d;
		p = Matrix4x4.Frustum(left, right, bottom, top, 0.3f, 1000.0f);
		mT = new Matrix4x4();
		mT.SetRow(0, new Vector4(-vr.x, -vr.y, vr.z, 0.0f));
		mT.SetRow(1, new Vector4(-vu.x, -vu.y, vu.z, 0.0f));
		mT.SetRow(2, new Vector4(-vn.x, -vn.y, vn.z, 0.0f));
		mT.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		t = new Matrix4x4();
		t.SetRow(0, new Vector4(1.0f, 0.0f, 0.0f, -pe.x));
		t.SetRow(1, new Vector4(0.0f, 1.0f, 0.0f, -pe.y));
		t.SetRow(2, new Vector4(0.0f, 0.0f, 1.0f, -pe.z));
		t.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		if (!FreezeModeBehaviour.freeze)
		{
			GameObject.Find("lparent").transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.LeftEye);
			Vector3 temp = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightHand);
			temp.x -= 0.0325f;
			GameObject.Find("controllerlparent").transform.localPosition = temp;
			if (UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightHand).eulerAngles.y < 180.0f)
			{
				ControllerCameraLeft.cullingMask = (1 << 7) | (1 << 8);
			}
			else
			{
				ControllerCameraLeft.cullingMask = (1 << 7) | (1 << 9);
			}
			pPrimeBottomPlaneLeft = p * mT * t;
		}

		if (HeadInHandModeBehaviour.displayPyramids)
		{
			BottomPlaneLeftLine1.enabled = true;
			BottomPlaneLeftLine1.material.color = Color.green;
			BottomPlaneLeftLine1.startColor = Color.green;
			BottomPlaneLeftLine1.endColor = Color.green;
			BottomPlaneLeftLine1.startWidth = 0.01f;
			BottomPlaneLeftLine1.endWidth = 0.01f;
			BottomPlaneLeftLine1.positionCount = 2;
			BottomPlaneLeftLine1.useWorldSpace = true;
			BottomPlaneLeftLine1.SetPosition(0, ControllerCameraLeft.transform.position);
			BottomPlaneLeftLine1.SetPosition(1, pa);

			BottomPlaneLeftLine2.enabled = true;
			BottomPlaneLeftLine2.material.color = Color.green;
			BottomPlaneLeftLine2.startColor = Color.green;
			BottomPlaneLeftLine2.endColor = Color.green;
			BottomPlaneLeftLine2.startWidth = 0.01f;
			BottomPlaneLeftLine2.endWidth = 0.01f;
			BottomPlaneLeftLine2.positionCount = 2;
			BottomPlaneLeftLine2.useWorldSpace = true;
			BottomPlaneLeftLine2.SetPosition(0, ControllerCameraLeft.transform.position);
			BottomPlaneLeftLine2.SetPosition(1, pb);

			BottomPlaneLeftLine3.enabled = true;
			BottomPlaneLeftLine3.material.color = Color.green;
			BottomPlaneLeftLine3.startColor = Color.green;
			BottomPlaneLeftLine3.endColor = Color.green;
			BottomPlaneLeftLine3.startWidth = 0.01f;
			BottomPlaneLeftLine3.endWidth = 0.01f;
			BottomPlaneLeftLine3.positionCount = 2;
			BottomPlaneLeftLine3.useWorldSpace = true;
			BottomPlaneLeftLine3.SetPosition(0, ControllerCameraLeft.transform.position);
			BottomPlaneLeftLine3.SetPosition(1, pc);

			BottomPlaneLeftLine4.enabled = true;
			BottomPlaneLeftLine4.material.color = Color.green;
			BottomPlaneLeftLine4.startColor = Color.green;
			BottomPlaneLeftLine4.endColor = Color.green;
			BottomPlaneLeftLine4.startWidth = 0.01f;
			BottomPlaneLeftLine4.endWidth = 0.01f;
			BottomPlaneLeftLine4.positionCount = 2;
			BottomPlaneLeftLine4.useWorldSpace = true;
			BottomPlaneLeftLine4.SetPosition(0, ControllerCameraLeft.transform.position);
			BottomPlaneLeftLine4.SetPosition(1, pd);
		}
		else
		{
			BottomPlaneLeftLine1.enabled = false;
			BottomPlaneLeftLine2.enabled = false;
			BottomPlaneLeftLine3.enabled = false;
			BottomPlaneLeftLine4.enabled = false;
		}

		if ((FreezeModeBehaviour.fail) && (FreezeModeBehaviour.randomChoice == 4))
		{
			BottomPlaneLeftTexture.Release();
		}
		else
		{
			// Set target texture for left camera as active render texture.
			RenderTexture.active = BottomPlaneLeftTexture;
			if (HeadInHandModeBehaviour.controllerView)
			{
				OffscreenCameraLeft.enabled = false;
				ControllerCameraLeft.enabled = true;

				ControllerCameraLeft.projectionMatrix = pPrimeBottomPlaneLeft;
				ControllerCameraLeft.targetTexture = BottomPlaneLeftTexture;
				// Render to texture
				ControllerCameraLeft.Render();
			}
			else
			{
				OffscreenCameraLeft.enabled = true;
				ControllerCameraLeft.enabled = false;

				OffscreenCameraLeft.projectionMatrix = pPrimeBottomPlaneLeft;
				OffscreenCameraLeft.targetTexture = BottomPlaneLeftTexture;
				// Render to texture
				OffscreenCameraLeft.Render();
			}
		}

		pa = new Vector3(3.75f, -5.0f, -3.75f);
		pb = new Vector3(3.75f, -5.0f, 3.75f);
		pc = new Vector3(-3.75f, -5.0f, -3.75f);
		pd = new Vector3(-3.75f, -5.0f, 3.75f);
		pe = GameObject.Find("RightEyeAnchor").GetComponent<Camera>().transform.position;
		va = pa - pe;
		vb = pb - pe;
		vc = pc - pe;
		vr = (pb - pa).normalized;
		vu = (pc - pa).normalized;
		vn = Vector3.Cross(vr, vu).normalized;
		d = -Vector3.Dot(vn, va);
		left = Vector3.Dot(vr, va) * (0.3f) / d;
		right = Vector3.Dot(vr, vb) * (0.3f) / d;
		bottom = Vector3.Dot(vu, va) * (0.3f) / d;
		top = Vector3.Dot(vu, vc) * (0.3f) / d;
		p = Matrix4x4.Frustum(left, right, bottom, top, 0.3f, 1000.0f);
		mT = new Matrix4x4();
		mT.SetRow(0, new Vector4(-vr.x, -vr.y, vr.z, 0.0f));
		mT.SetRow(1, new Vector4(-vu.x, -vu.y, vu.z, 0.0f));
		mT.SetRow(2, new Vector4(-vn.x, -vn.y, vn.z, 0.0f));
		mT.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		t = new Matrix4x4();
		t.SetRow(0, new Vector4(1.0f, 0.0f, 0.0f, -pe.x));
		t.SetRow(1, new Vector4(0.0f, 1.0f, 0.0f, -pe.y));
		t.SetRow(2, new Vector4(0.0f, 0.0f, 1.0f, -pe.z));
		t.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
		if (!FreezeModeBehaviour.freeze)
		{
			GameObject.Find("rparent").transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightEye);
			Vector3 temp = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightHand);
			temp.x += 0.0325f;
			GameObject.Find("controllerrparent").transform.localPosition = temp;
			if (UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightHand).eulerAngles.y < 180.0f)
			{
				ControllerCameraRight.cullingMask = (1 << 7) | (1 << 9);
			}
			else
			{
				ControllerCameraRight.cullingMask = (1 << 7) | (1 << 8);
			}
			pPrimeBottomPlaneRight = p * mT * t;
		}

		if (HeadInHandModeBehaviour.displayPyramids)
		{
			BottomPlaneRightLine1.enabled = true;
			BottomPlaneRightLine1.material.color = Color.red;
			BottomPlaneRightLine1.startColor = Color.red;
			BottomPlaneRightLine1.endColor = Color.red;
			BottomPlaneRightLine1.startWidth = 0.01f;
			BottomPlaneRightLine1.endWidth = 0.01f;
			BottomPlaneRightLine1.positionCount = 2;
			BottomPlaneRightLine1.useWorldSpace = true;
			BottomPlaneRightLine1.SetPosition(0, ControllerCameraRight.transform.position);
			BottomPlaneRightLine1.SetPosition(1, pa);

			BottomPlaneRightLine2.enabled = true;
			BottomPlaneRightLine2.material.color = Color.red;
			BottomPlaneRightLine2.startColor = Color.red;
			BottomPlaneRightLine2.endColor = Color.red;
			BottomPlaneRightLine2.startWidth = 0.01f;
			BottomPlaneRightLine2.endWidth = 0.01f;
			BottomPlaneRightLine2.positionCount = 2;
			BottomPlaneRightLine2.useWorldSpace = true;
			BottomPlaneRightLine2.SetPosition(0, ControllerCameraRight.transform.position);
			BottomPlaneRightLine2.SetPosition(1, pb);

			BottomPlaneRightLine3.enabled = true;
			BottomPlaneRightLine3.material.color = Color.red;
			BottomPlaneRightLine3.startColor = Color.red;
			BottomPlaneRightLine3.endColor = Color.red;
			BottomPlaneRightLine3.startWidth = 0.01f;
			BottomPlaneRightLine3.endWidth = 0.01f;
			BottomPlaneRightLine3.positionCount = 2;
			BottomPlaneRightLine3.useWorldSpace = true;
			BottomPlaneRightLine3.SetPosition(0, ControllerCameraRight.transform.position);
			BottomPlaneRightLine3.SetPosition(1, pc);

			BottomPlaneRightLine4.enabled = true;
			BottomPlaneRightLine4.material.color = Color.red;
			BottomPlaneRightLine4.startColor = Color.red;
			BottomPlaneRightLine4.endColor = Color.red;
			BottomPlaneRightLine4.startWidth = 0.01f;
			BottomPlaneRightLine4.endWidth = 0.01f;
			BottomPlaneRightLine4.positionCount = 2;
			BottomPlaneRightLine4.useWorldSpace = true;
			BottomPlaneRightLine4.SetPosition(0, ControllerCameraRight.transform.position);
			BottomPlaneRightLine4.SetPosition(1, pd);
		}
		else
		{
			BottomPlaneRightLine1.enabled = false;
			BottomPlaneRightLine2.enabled = false;
			BottomPlaneRightLine3.enabled = false;
			BottomPlaneRightLine4.enabled = false;
		}

		if ((FreezeModeBehaviour.fail) && (FreezeModeBehaviour.randomChoice == 5))
		{
			BottomPlaneRightTexture.Release();
		}
		else
		{
			// Set target texture for right camera as active render texture.
			RenderTexture.active = BottomPlaneRightTexture;
			if (HeadInHandModeBehaviour.controllerView)
			{
				OffscreenCameraRight.enabled = false;
				ControllerCameraRight.enabled = true;

				ControllerCameraRight.projectionMatrix = pPrimeBottomPlaneRight;
				ControllerCameraRight.targetTexture = BottomPlaneRightTexture;
				// Render to texture
				ControllerCameraRight.Render();	
			}
			else
			{
				OffscreenCameraRight.enabled = true;
				ControllerCameraRight.enabled = false;

				OffscreenCameraRight.projectionMatrix = pPrimeBottomPlaneRight;
				OffscreenCameraRight.targetTexture = BottomPlaneRightTexture;
				// Render to texture
				OffscreenCameraRight.Render();
			}
		}

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