//========================================================
// アスペクト比固定(拾い物)
//========================================================
using UnityEngine;
using System.Collections;

public class AspectCamera : MonoBehaviour
{
    private Color32 backgroundColor = Color.black;
    private static Camera _backgroundCamera; 

	void Awake ()
    {
        //--- プラットフォームにより処理するか変更
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            Destroy(this);
            return;
        }
         
    	if (_backgroundCamera != null) 
    		return; 
    
    
    	var backGroundCameraObject = new GameObject ("Background Color Camera"); 
    	_backgroundCamera = backGroundCameraObject.AddComponent<Camera> (); 
    	_backgroundCamera.depth = -99; 
    	_backgroundCamera.fieldOfView = 1; 
    	_backgroundCamera.farClipPlane = 1.1f; 
    	_backgroundCamera.nearClipPlane = 1;  
    	_backgroundCamera.cullingMask = 0; 
    	_backgroundCamera.depthTextureMode = DepthTextureMode.None; 
    	_backgroundCamera.backgroundColor = backgroundColor; 
    	_backgroundCamera.renderingPath = RenderingPath.VertexLit; 
    	_backgroundCamera.clearFlags = CameraClearFlags.SolidColor; 
    	_backgroundCamera.useOcclusionCulling = false; 
    	backGroundCameraObject.hideFlags = HideFlags.NotEditable; 


		Camera cam = gameObject.GetComponent<Camera>();
		float baseAspect = 1600f/900f;		// 画面比率
		float nowAspect = (float)Screen.height/(float)Screen.width;
		float changeAspect;
		
		if(baseAspect>nowAspect)
		{
			changeAspect = nowAspect/baseAspect;
			cam.rect=new Rect((1-changeAspect)*0.5f,0,changeAspect,1);
		}
		else
		{
			changeAspect = baseAspect/nowAspect;
			cam.rect=new Rect(0,(1-changeAspect)*0.5f,1,changeAspect);
		}

		Destroy(this);
	}
}
