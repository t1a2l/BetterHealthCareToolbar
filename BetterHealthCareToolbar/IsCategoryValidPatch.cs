﻿using HarmonyLib;
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
			if(ignore || !(__instance is HealthcarePanel) || !Mod.IsInGame() || !info)
			{
				return;
			}

			if (!HealthCareUtils.IsHealthCareCategory(info.category))
			{
				return;
			}

			var cat = HealthCareUtils.GetHealthCareCategory(info);
			if (!cat.HasValue)
			{
				return;
			}

			var group = HealthCareUtils.CreateGroup(cat.Value);
			__result = group.name == ___m_Category;
		}
	}
}