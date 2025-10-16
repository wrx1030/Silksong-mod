using DeployBench;
using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace PluginTutorial
{
    [HarmonyPatch(typeof(PlayMakerTriggerExit2D), "OnTriggerExit2D", new Type[]{typeof(Collider2D)})]
    public class RestBenchPatch
    {
        [HarmonyPrefix]
        private static void Prefix(PlayMakerTriggerExit2D __instance, Collider2D other)
        {
            
            if (__instance.GetComponent<Deploybench>() == null)
            {
                __instance.gameObject.AddComponent<Deploybench>();
                Debug.Log($"[RestBenchPatch] 已向 {__instance.name} 添加 Deploybench");
            }

            var benchcomp = other.GetComponentInParent<RestBench>();
            if (benchcomp == null)
            {
                return;
            }

            Transform parent = __instance.transform;
            foreach (Transform child in parent)
            {
                if (child.name.Contains("RestBench"))
                {
                    Debug.Log($"[RestBenchPatch] {parent.name} 已有克隆 {child.name}，跳过重复添加。");
                    return; // 直接退出，不再生成新的
                }
            }

            GameObject benchClone = UnityEngine.Object.Instantiate(other.gameObject);
            Debug.Log($"[RestBenchPatch] 已克隆 bench 对象: {benchClone.name}");

            Transform lightTransform = benchClone.transform.Find("Light");
            if (lightTransform != null)
            {
                GameObject lightObject = lightTransform.gameObject;

                // 获取所有iTween组件
                var itweenComponents = lightObject.GetComponents<MonoBehaviour>()
                    .Where(comp => comp != null && comp.GetType().Name == "iTween")
                    .ToList();

                Debug.Log($"[RestBenchPatch] 找到 {itweenComponents.Count} 个 iTween 组件");

                // 保留第一个激活的iTween，删除其余所有iTween
                bool hasKeptActiveOne = false;

                foreach (var comp in itweenComponents)
                {
                    if (comp.enabled && !hasKeptActiveOne)
                    {
                        // 保留第一个激活的组件
                        hasKeptActiveOne = true;
                        Debug.Log($"[RestBenchPatch] 保留激活的 iTween 组件");
                    }
                    else
                    {
                        // 删除其他所有组件（重复的激活组件 + 未激活组件）
                        UnityEngine.Object.Destroy(comp);
                        Debug.Log($"[RestBenchPatch] 删除多余的 iTween 组件: enabled={comp.enabled}");
                    }
                }
            }
            else
            {
                Debug.Log("未找到 iTween 组件");
            }

            benchClone.transform.SetParent(__instance.gameObject.transform, false);
            benchClone.SetActive(false);
            Debug.Log($"[RestBenchPatch] 已将 benchClone 设为 {__instance.name} 的子物体并隐藏");
            
        }
        
    }
}
