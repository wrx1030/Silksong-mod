using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace PluginTutorial
{
    [HarmonyPatch(typeof(RestBench), "OnTriggerExit2D", new Type[] { typeof(Collider2D) })]
    public class BenchDetecter
    {
        [HarmonyPrefix]
        private static void Prefix(RestBench __instance, Collider2D otherObject)
        {
            //Debug.Log("RestBench的物体名: " + __instance.gameObject.name);
            //Debug.Log("otherObject的物体名: " + otherObject.gameObject.name);

            //if (otherObject.gameObject.name == "Hero_Hornet(Clone)")
            //{
            //    GameObject go = otherObject.gameObject;
            //    MonoBehaviour[] monos = go.GetComponents<MonoBehaviour>();
            //    foreach (var mb in monos)
            //    {
            //        if (mb == null) continue;
            //        Type t = mb.GetType();
            //        // 查这个类型是否有方法 OnTriggerExit2D
            //        MethodInfo mi = t.GetMethod("OnTriggerExit2D",
            //            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            //            null,
            //            new Type[] { typeof(Collider2D) },
            //            null);
            //        if (mi != null)
            //        {
            //            Debug.Log("找到脚本组件： " + t.FullName + " 在其上定义了 OnTriggerExit2D");
            //        }
            //        else
            //        {
            //            Debug.Log("未找到脚本组件 OnTriggerExit2D");
            //        }
            //    }
            //}
        }
    }
}