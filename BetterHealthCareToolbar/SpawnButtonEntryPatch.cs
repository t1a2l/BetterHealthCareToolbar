using System;
using ColossalFramework.UI;
using HarmonyLib;
using UnityEngine;

namespace BetterHealthCareToolbar
{
    [HarmonyPatch(typeof(GeneratedGroupPanel), "PopulateGroups", new Type[]
	{
		typeof(GeneratedGroupPanel.GroupFilter),
		typeof(Comparison<GeneratedGroupPanel.GroupInfo>)
	})]
	internal class SpawnButtonEntryPatch
	{
		[HarmonyPostfix]
		public static void Postfix(GeneratedGroupPanel.GroupFilter filter, Comparison<GeneratedGroupPanel.GroupInfo> comparison, GeneratedGroupPanel __instance, UITabstrip ___m_Strip)
		{
			if (!(__instance is HealthcareGroupPanel) || !Mod.IsInGame())
			{
				// We only want the "HealthCare" main tab
				return;
			}
			string mainCategoryId = "MAIN_CATEGORY";
			foreach (UIComponent tab in ___m_Strip.tabs)
			{
				var button = tab as UIButton;
				if(!button)
                {
					// shouldn't happen?
					continue;
                }
				if (button.tooltip.Contains(Mod.Identifier))
				{
					string s = button.tooltip.Replace(mainCategoryId + "[" + Mod.Identifier, "");
					s = s.Replace("]:0", "");

                    bool result = int.TryParse(s, out int val);
                    if (!result)
					{
						Debug.Log(Mod.Identifier + "Unable to parse string: '" + button.tooltip + "'");
						return;
					}
					HealthCareCategory cat = (HealthCareCategory)val;
					if (!Enum.IsDefined(typeof(HealthCareCategory), cat))
					{
						Debug.Log(Mod.Identifier + "Unexpected HealthCareCategory value: '" + result + "'");
						return;
					}
					button.tooltip = HealthCareUtils.GetTooltip(cat);
					HealthCareUtils.SetToolbarTabSprite(__instance, ref button, cat);
				}
			}
		}
	}
}
