using ColossalFramework;
using HarmonyLib;
using System;
using System.Collections.Generic;
using static GeneratedGroupPanel;

namespace BetterHealthCareToolbar
{
	// This patch overrides the list of tabs that will be listed in the Roads toolbar (not their contents!).
	[HarmonyPatch(typeof(GeneratedGroupPanel), "CollectAssets")]
    class CollectAssetsPatch
    {
		[HarmonyPostfix]
        public static void Postfix(GroupFilter filter,
								   Comparison<GroupInfo > comparison,
								   ref PoolList<GroupInfo> __result,
								   GeneratedGroupPanel __instance)
        {
			if (!filter.IsFlagSet(GroupFilter.Building) ||
				__instance.service != ItemClass.Service.HealthCare ||
				!Mod.IsInGame() )
			{
				return;
			}

			__result.Clear();

			var healthCareCategoriesNeeded = new List<HealthCareCategory>();
			var toolManagerExists = Singleton<ToolManager>.exists;

			for (uint i = 0u; i < PrefabCollection<BuildingInfo>.LoadedCount(); ++i)
			{
				BuildingInfo info = PrefabCollection<BuildingInfo>.GetLoaded(i);
				if (info != null &&
					info.GetService() == ItemClass.Service.HealthCare &&
					(!toolManagerExists || info.m_availableIn.IsFlagSet(Singleton<ToolManager>.instance.m_properties.m_mode)) &&
					info.m_placementStyle == ItemClass.Placement.Manual)
				{
					if (!HealthCareUtils.IsHealthCareCategory(info.category))
                    {
						continue;
                    }
					var cats = HealthCareUtils.GetHealthCareCategories(info);

					foreach (var cat in cats)
					{
						if (!healthCareCategoriesNeeded.Contains(cat))
						{
							healthCareCategoriesNeeded.Add(cat);
						}
					}
				}
			}

			healthCareCategoriesNeeded.Sort();

			// Re-create tabs
			foreach (var cat in healthCareCategoriesNeeded)
            {
				__result.Add(HealthCareUtils.CreateGroup(cat));
            }

		}
    }
}