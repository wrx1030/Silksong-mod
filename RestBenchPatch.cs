using DeployBench;
using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace PluginTutorial
{
    [HarmonyPatch(typeof(RestBench), "OnTriggerEnter2D", new Type[]{typeof(Collider2D)})]
    public class RestBenchPatch
    {
        [HarmonyPrefix]
        private static void Prefix(RestBench __instance, Collider2D otherObject)
        {
            if (otherObject.GetComponent<Deploybench>() == null)
            {
                var heroController = otherObject.GetComponentInParent<HeroController>();
                if (heroController == null)
                {
                    return;
                }

                // 添加 Deploybench，但暂时禁用，防止 Start() 立即执行
                var deploy = otherObject.gameObject.AddComponent<Deploybench>();
                deploy.enabled = false;
                Debug.Log($"[RestBenchPatch] 已向 {otherObject.name} 添加 Deploybench (暂未启用)");

                GameObject benchClone = UnityEngine.Object.Instantiate(__instance.gameObject);
                Debug.Log($"[RestBenchPatch] 已克隆 bench 对象: {benchClone.name}");

                var comps = benchClone.GetComponents<Deploybench>();
                if (comps.Length > 0)
                {
                    foreach (var comp in comps)
                    {
                        UnityEngine.Object.DestroyImmediate(comp, true);
                    }
                    Debug.Log($"[RestBenchPatch] 已删除 benchClone 上的 Deploybench 组件（共 {comps.Length} 个）");
                }

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

                benchClone.transform.SetParent(otherObject.gameObject.transform, false);
                benchClone.SetActive(false);
                Debug.Log($"[RestBenchPatch] 已将 benchClone 设为 {otherObject.name} 的子物体并隐藏");

                deploy.enabled = true;
                Debug.Log($"[RestBenchPatch] 已重新启用 {otherObject.name} 的 Deploybench 组件");
            }
        }
        
    }
}
