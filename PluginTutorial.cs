using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace PluginTutorial
{
	// Token: 0x02000004 RID: 4
	[BepInPlugin("rex.mod.DeployBench", "DeployBench", "0.1")]
	public class PluginTutorial : BaseUnityPlugin
	{
		// Token: 0x0600000C RID: 12 RVA: 0x0000247F File Offset: 0x0000067F
		private void Start()
		{
			PluginTutorial.Log = base.Logger;
			HarmonyPatchValidator.Validate();
			this._harmony = new Harmony("rex.mod.DeployBench");
			this._harmony.PatchAll();
		}

		// Token: 0x0400000A RID: 10
		internal static ManualLogSource Log;

		// Token: 0x0400000B RID: 11
		private Harmony _harmony;

		// Token: 0x0400000C RID: 12
		private ConfigEntry<int> intConfig;

		// Token: 0x0400000D RID: 13
		private ConfigEntry<string> stringConfig;
	}
}
