using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KMUtils.Manager
{
    public class cCaptureManager : MonoBehaviour
    {
        public Camera targetCamera; // 캡처할 카메라 지정

        private string capturePath
        {
            get
            {
                return $"{Application.dataPath}\\Editor\\EditorCaptureData";
            }
        }
        private string currTimePath
        {
            get
            {
                CheckDirectory();
                return Path.Combine(capturePath, $"{DateTime.Now}.png".Replace(":", "-"));
            }
        }

        private (KeyCode, Action)[] keyAction;

        private void Awake()
        {
            keyAction = new (KeyCode, Action)[]
            {
                (KeyCode.Space, CaptureGameScene),
                (KeyCode.Return, CaptureTargetCam)
            };
        }

        private void Update()
        {
            foreach ((KeyCode key, Action action) in keyAction)
            {
                if (Input.GetKeyDown(key)) action();
            }
        }

        private void CheckDirectory()
        {
            if (Directory.Exists(capturePath)) return;
            Directory.CreateDirectory(capturePath);
        }

        private void CaptureGameScene()
        {
            string name = currTimePath;
            ScreenCapture.CaptureScreenshot(name);
            Debug.Log($"CaptureGameScene : {name}");
        }

        public void CaptureTargetCam()
        {// ui를 제외한 해당 카메라가 보는 화면을 캡쳐
            try
            {
                if (targetCamera == null)
                {
                    Debug.LogError("타겟 카메라가 지정되지 않았습니다.");
                    return;
                }

                int width = Screen.width;
                int height = Screen.height;

                // RenderTexture 생성
                RenderTexture rt = new RenderTexture(width, height, 24);
                targetCamera.targetTexture = rt;
                targetCamera.Render();

                // Texture2D에 렌더링된 화면 복사
                RenderTexture.active = rt;
                Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
                screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                screenShot.Apply();

                // RenderTexture 초기화
                targetCamera.targetTexture = null;
                RenderTexture.active = null;
                Destroy(rt);

                // PNG 파일로 저장
                byte[] bytes = screenShot.EncodeToPNG();
                File.WriteAllBytes(currTimePath, bytes);
                Debug.Log($"CaptureTargetCam : {currTimePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error : CaptureTargetCam : {e}\n{currTimePath}");
            }
        }
    }
}