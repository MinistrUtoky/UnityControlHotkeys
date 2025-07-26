#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System.Diagnostics;
using UnityEngine;

namespace miscellaneousScripts
{
    internal class HotKeyManagementScript : MonoBehaviour
    {
        private void Start()
        {
            UnityEngine.Debug.developerConsoleVisible = false;
        }
        void Update()
        {
            ManageMainHotKeys();
        }

        private void ManageMainHotKeys()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                WindowKeeperScript.Instance.ToggleTopmost(Application.productName);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                UnityEngine.Debug.Log(
                    string.Format("Opening {0}", "/select," + (Application.streamingAssetsPath + "/config.txt").Replace("/", "\\"))
                    );
                Process.Start("explorer.exe", "/select," + (Application.streamingAssetsPath + "/config.txt").Replace("/", "\\"));
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Cursor.visible = !Cursor.visible;
            }
        }
    }
}
#endif