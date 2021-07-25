using ICities;
using CitiesHarmony.API;
using UnityEngine.SceneManagement;

namespace BetterHealthCareToolbar
{
    public class  Mod : LoadingExtensionBase, IUserMod
    {
        string IUserMod.Name => "Better HealthCare Toolbar Mod";

        string IUserMod.Description => "Separate the HealthCare Toolbar into five categories - HealthCare, DeathCare, ChildCare, ElderCare and RecreationalCare";
        
        public void OnEnabled() {
             HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled() {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }

        public static bool IsMainMenu()
        {
            return SceneManager.GetActiveScene().name == "MainMenu";
        }

        public static bool IsInGame()
		{
			return SceneManager.GetActiveScene().name == "Game";
		}

        public static string Identifier = "HC.CT/";
    }

}
