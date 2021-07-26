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
			var SpriteNames = new string[] {
				"HealthCareBase",
				"HealthCareDisabled",
				"HealthCareFocused",
				"HealthCareHovered",
				"HealthCarePressed",
				"DeathCareBase",
				"DeathCareDisabled",
				"DeathCareFocused",
				"DeathCareHovered",
				"DeathCarePressed",
				"ChildCareBase",
				"ChildCareDisabled",
				"ChildCareFocused",
				"ChildCareHovered",
				"ChildCarePressed",
				"ElderCareBase",
				"ElderCareDisabled",
				"ElderCareFocused",
				"ElderCareHovered",
				"ElderCarePressed",
				"RecreationalCareBase",
				"RecreationalCareDisabled",
				"RecreationalCareFocused",
				"RecreationalCareHovered",
				"RecreationalCarePressed",
				"SubBarButtonBase",
				"SubBarButtonBaseDisabled",
				"SubBarButtonBaseFocused",
				"SubBarButtonBaseHovered",
				"SubBarButtonBasePressed"
			};
			var path = @"E:\Github\BetterHealthCareToolbar\BetterHealthCareToolbar\Utils\Atlas\HealthCareAtlas.png";
			if(TextureUtils.GetAtlas("HealthCareAtlas") == null)
            {
				TextureUtils.InitialiseAtlas(path, "HealthCareAtlas");
				for(int i = 0; i < 25; i++)
				{
					TextureUtils.AddSpriteToAtlas(new Rect(32 * i, 0, 32, 22), SpriteNames[i], "HealthCareAtlas");
				}

				for(int i = 25; i < SpriteNames.Length; i++)
				{
					TextureUtils.AddSpriteToAtlas(new Rect(58 * i - 130, 0, 58, 22), SpriteNames[i], "HealthCareAtlas");
				}
            }
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
					button.atlas = TextureUtils.GetAtlas("HealthCareAtlas");
					button.normalBgSprite = "SubBarButtonBase";
					button.pressedBgSprite = "SubBarButtonBasePressed";
					button.disabledBgSprite = "SubBarButtonBaseDisabled";
					button.focusedBgSprite = "SubBarButtonBaseFocused";
					button.hoveredBgSprite = "SubBarButtonBaseHovered";
					HealthCareUtils.SetToolbarTabSprite(ref button, cat);
				}
			}
		}
	}
}
