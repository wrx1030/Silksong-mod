using BepInEx;
using HarmonyLib;

namespace PluginTutorial
{
	[BepInPlugin("rex.mod.DeployBench", "DeployBench", "0.9")]
	public class PluginTutorial : BaseUnityPlugin
	{
        private Harmony _harmony;

        private void Start()
		{
			this._harmony = new Harmony("rex.mod.DeployBench");
			this._harmony.PatchAll();
		}
	}
}
