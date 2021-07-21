using HarmonyLib;
using System;

namespace BetterHealthCareToolbar
{
	// This patch overrides the category(ies) that a health care building will be assigned to.
	[HarmonyPatch(typeof(GeneratedScrollPanel), "IsCategoryValid", new Type[] { typeof(BuildingInfo), typeof(bool) })]
	class IsCategoryValidPatch
	{
		[HarmonyPostfix]
		public static void Postfix(BuildingInfo info, bool ignore, GeneratedScrollPanel __instance, ref bool __result, ref string ___m_Category)
		{
			if(ignore || !(__instance is HealthcarePanel) || !Mod.IsInGame())
            {
				return;
            }

			var buildingInfo = info;

			if (!buildingInfo)
            {
				return;
            }

			if (!HealthCareUtils.IsDefaultHealthCareCategory(info.category))
			{
				return;
			}

			var cats = HealthCareUtils.GetHealthCareCategories(info);

			foreach (var cat in cats)
			{
				var group = HealthCareUtils.CreateGroup(cat);

				if (group.name == ___m_Category)
                {
					__result = true;
					return;
                }
			}

			__result = false;
		}
	}
}