using UnityEngine;

namespace LFramework
{
    public static class UnityEngineCameraExtension
    {
        /// <summary>
        /// Camera.main.CaptureCamera(new Rect(0, 0, Screen.width, Screen.height))
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Texture2D CaptureCamera(this Camera camera, Rect rect)
        {
            var renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;

            var screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.Destroy(renderTexture);

            return screenShot;
        }

        public static Texture2D Capture(this RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;
            var rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            var screenShot = new Texture2D((int)renderTexture.width, (int)renderTexture.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();
            RenderTexture.active = null;
            return screenShot;
        }
    }
}