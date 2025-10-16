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
            
            if (benchClone != null)
            {
                string currentSceneName = GameManager.instance.sceneName;
                // 根据名称获取场景对象
                Scene targetScene = SceneManager.GetSceneByName(currentSceneName);

                // 验证场景是否有效（已加载且合法）
                if (targetScene.IsValid())
                {
                    // 检查长椅是否需要移动到新场景
                    if (benchClone.scene != targetScene)
                    {
                        benchClone.transform.SetParent(null);

                        // 将长椅移动到目标场景
                        SceneManager.MoveGameObjectToScene(benchClone, targetScene);
                        Debug.Log($"[Deploybench] 已将 {benchClone.name} 移动到正确的游戏场景: {targetScene.name}");


                        // 对齐位置到父物体
                        var heroPos = this.transform.position;
                        var pos = benchClone.transform.position;
                        pos.x = heroPos.x;
                        pos.y = heroPos.y - 1.0f;
                        benchClone.transform.position = pos;
                        Debug.Log($"[Deploybench] 已将 {benchClone.name} 对齐到位置: {pos.x},{pos.y}");

                        benchClone.SetActive(true);
                        Debug.Log("激活 benchClone");

                        SceneTeleportMap.AddRespawnPoint(currentSceneName, $"{benchClone.name}|{pos.x},{pos.y}");
                        Debug.Log($"[Deploybench] 已在 {currentSceneName} 保存重生点: {benchClone.name}|{pos.x},{pos.y}");
                        UnityEngine.Object.DontDestroyOnLoad(benchClone);
                    }
                }
                else
                {
                    Debug.LogWarning($"[Deploybench] 无法根据名称 '{currentSceneName}' 找到已加载的场景。长椅可能位于错误的场景。");
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
        }
    }
}
