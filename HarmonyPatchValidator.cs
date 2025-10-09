using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

// Token: 0x02000002 RID: 2
public static class HarmonyPatchValidator
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void Validate()
	{
		HarmonyPatchValidator.ValidateMethod(typeof(GameManager), "SetDeathRespawnSimple", new Type[0]);
		HarmonyPatchValidator.ValidateMethod(typeof(PlayerData), "SetBenchRespawn", new Type[]
		{
			typeof(RespawnMarker),
			typeof(string),
			typeof(int)
		});
		HarmonyPatchValidator.ValidateMethod(typeof(PlayerData), "SetBenchRespawn", new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(bool)
		});
		HarmonyPatchValidator.ValidateMethod(typeof(PlayerData), "SetBenchRespawn", new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(int),
			typeof(bool)
		});
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002148 File Offset: 0x00000348
	private static void ValidateMethod(Type type, string methodName, Type[] parameterTypes)
	{
		try
		{
			MethodInfo methodInfo = AccessTools.Method(type, methodName, parameterTypes, null);
			if (methodInfo == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[HarmonyValidator] ❌ 找不到方法：",
					type.FullName,
					".",
					methodName,
					"(",
					string.Join(", ", new string[]
					{
						parameterTypes.ToString()
					}),
					")"
				}));
			}
			else
			{
				Debug.Log(string.Format("[HarmonyValidator] ✅ 找到方法：{0}", methodInfo));
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}
}
