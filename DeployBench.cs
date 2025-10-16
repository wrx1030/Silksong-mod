using BepInEx;
using System;
using System.Collections;
using TMProOld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeployBench
{ 
    public class Deploybench : MonoBehaviour
    {
        private GameObject benchClone;
        private bool isDeploying = false;
        private KeyCode deployKey = KeyCode.B;

        private void FindBenchClone()
        {
            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeSelf && child.name.Contains("RestBench"))
                {
                    benchClone = child.gameObject;
                    Debug.Log($"[Deploybench] 找到 benchClone: {benchClone.name}");
                    return;
                }
            }

            Debug.LogWarning($"[Deploybench] 未找到 benchClone，父对象: {name}");
        }


        private void Update()
        {
            
            if (Input.GetKeyDown(deployKey))
            {
                if (benchClone == null)
                {
                    FindBenchClone();
                }

                if (benchClone == null)
                {
                    return; 
                }

                TryDeployBench();
            }
        }
        private void TryDeployBench()
        {
            if (isDeploying)
            {
                Debug.Log("[Deploybench] 正在部署中，请稍候...");
                return;
            }

            if (benchClone == null)
            {
                Debug.LogWarning("[Deploybench] benchClone 未找到，无法部署。");
                return;
            }

            if (benchClone.activeSelf)
            {
                Debug.Log("[Deploybench] benchClone 已经激活，无需重复部署。");
                return;
            }

            StartCoroutine(DeployRoutine());
        }

        private IEnumerator DeployRoutine()
        {
            isDeploying = true;

            // 对齐位置到父物体
            var parent = transform.parent;
            if (parent != null)
            {
                var pos = benchClone.transform.position;
                pos.x = parent.position.x;
                pos.y = parent.position.y;
                benchClone.transform.position = pos;
            }

            yield return null;

            if (benchClone != null)
            {
                benchClone.transform.SetParent(null);

                string currentSceneName = GameManager.instance.sceneName;
                if (!string.IsNullOrEmpty(currentSceneName))
                {
                    // 根据名称获取场景对象
                    Scene targetScene = SceneManager.GetSceneByName(currentSceneName);

                    // 验证场景是否有效（已加载且合法）
                    if (targetScene.IsValid())
                    {
                        // 检查长椅是否需要移动到新场景
                        if (benchClone.scene != targetScene)
                        {
                            // 将长椅移动到目标场景
                            SceneManager.MoveGameObjectToScene(benchClone, targetScene);
                            Debug.Log($"[Deploybench] 已将 {benchClone.name} 移动到正确的游戏场景: {targetScene.name}");

                            SceneTeleportMap.AddRespawnPoint(currentSceneName, benchClone.name);

                            Debug.Log($"[Deploybench] 已在 {currentSceneName} 保存重生点: {benchClone.name}");
                            UnityEngine.Object.DontDestroyOnLoad(benchClone);
                            // 激活 benchClone
                            benchClone.SetActive(true);
                            
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"[Deploybench] 无法根据名称 '{currentSceneName}' 找到已加载的场景。长椅可能位于错误的场景。");
                    }
                }
                else
                {
                    Debug.LogError("[Deploybench] 无法从 GameManager 获取当前场景名称！");
                }

                Debug.Log($"[Deploybench] 已部署 benchClone: {benchClone.name}");
            }
            else
            {
                // 如果 benchClone 已经被内部逻辑销毁了，在这里记录
                Debug.LogError("[Deploybench] benchClone 在激活后立即被销毁！");
            }

            // 添加冷却时间
            yield return new WaitForSeconds(0.5f);

            isDeploying = false;
        }
    }
}
