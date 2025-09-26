using System;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenCapture : MonoBehaviour
{
    [SerializeField] private string filePrefix = "Screenshot";

    public void Capture()
    {
        IEnumerator Routine()
        {
            yield return new WaitForEndOfFrame();

            var camera = Camera.main!;
            var oldCameraTexture = camera.targetTexture;
            var oldCameraClearFlags = camera.clearFlags;
            var oldRenderTexture = RenderTexture.active;

            var texture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            var renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);

            RenderTexture.active = renderTexture;
            camera.targetTexture = renderTexture;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.clear;
            camera.Render();
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            var byteArray = texture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + $"/{filePrefix}-{DateTime.Now:yyyy-MM-dd_hh-mm-ss}.png", byteArray);

            camera.clearFlags = oldCameraClearFlags;
            camera.targetTexture = oldCameraTexture;
            RenderTexture.active = oldRenderTexture;
            RenderTexture.ReleaseTemporary(renderTexture);
            Texture2D.DestroyImmediate(texture);
        }

        StartCoroutine(Routine());
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(ScreenCapture))]
public class ScreenCaptureEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        var captureButton = new Button();
        captureButton.text = "Capture";
        captureButton.clicked += () => ((ScreenCapture)target).Capture();
        root.Add(captureButton);

        return root;
    }
}

#endif