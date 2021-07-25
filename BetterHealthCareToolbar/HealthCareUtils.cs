using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;


namespace BetterHealthCareToolbar
{
	enum HealthCareCategory
	{
		HealthCare,
		DeathCare,
		ChildCare,
		ElderCare,
		RecreationalCare
	}

	static class HealthCareUtils
	{
		public static Texture2D[] newTextures = new Texture2D[10];

		public static bool IsDefaultHealthCareCategory(string cat)
		{
			switch (cat)
			{
				case "HealthcareDefault":
					return true;
				default:
					return false;
			};
		}

		public static UITextureAtlas GetAtlas(string name)
        {
            UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < atlases.Length; i++)
            {
                if (atlases[i].name == name)
                    return atlases[i];
            }
            return UIView.GetAView().defaultAtlas;
        }
		public static void SetToolbarTabSprite(ref UIButton button, HealthCareCategory cat)
		{
			var SpriteNames = new string[] {
				"HealthCare",
				"DeathCare",
				"ChildCare",
				"ElderCare",
				"RecreationalCare",
				"SubBarButtonBase",
				"SubBarButtonBaseDisabled",
				"SubBarButtonBaseFocused",
				"SubBarButtonBaseHovered",
				"SubBarButtonBasePressed"
			};
			button.atlas = TextureUtils.CreateTextureAtlas("HealthCareAtlas.png", "HealthCareAtlas", SpriteNames);
			switch (cat)
			{
				case HealthCareCategory.HealthCare:
					button.normalFgSprite = button.pressedFgSprite = button.disabledFgSprite = button.focusedFgSprite = button.hoveredFgSprite = "HealthCare";
					break;
				case HealthCareCategory.DeathCare:
					button.normalFgSprite = button.pressedFgSprite = button.disabledFgSprite = button.focusedFgSprite = button.hoveredFgSprite = "DeathCare";
					break;
				case HealthCareCategory.ChildCare:
					button.normalFgSprite = button.pressedFgSprite = button.disabledFgSprite = button.focusedFgSprite = button.hoveredFgSprite = "ChildCare";
					break;
				case HealthCareCategory.ElderCare:
					button.normalFgSprite = button.pressedFgSprite = button.disabledFgSprite = button.focusedFgSprite = button.hoveredFgSprite = "ElderCare";
					break;
				case HealthCareCategory.RecreationalCare:
					button.normalFgSprite = button.pressedFgSprite = button.disabledFgSprite = button.focusedFgSprite = button.hoveredFgSprite = "RecreationalCare";
					break;
				default:
					break;
			}
			button.normalBgSprite = "SubBarButtonBase";
			button.pressedBgSprite = "SubBarButtonBaseDisabled";
			button.disabledBgSprite = "SubBarButtonBaseFocused";
			button.focusedBgSprite = "SubBarButtonBaseHovered";
			button.hoveredBgSprite = "SubBarButtonBasePressed";
		}

		public static string GetTooltip(HealthCareCategory cat)
		{
			switch (cat)
			{
				case HealthCareCategory.HealthCare:
					return "Hospitals, clinics and basically all doctor services";
				case HealthCareCategory.DeathCare:
					return "Cemeteries, crematoriums and everything releated to the after life";
				case HealthCareCategory.ChildCare:
					return "Orphanage, group home, child support center, taking care of the city children";
				case HealthCareCategory.ElderCare:
					return "Nursing homes, Assisted living, taking care of the city elders";
				case HealthCareCategory.RecreationalCare:
					return "Pools, gyms, saunas, community buildings for the neighborhood";
				default:
					break;
			}
			return "";
		}

		public static GeneratedGroupPanel.GroupInfo CreateGroup(HealthCareCategory healthCareType)
		{
			string identifier = Mod.Identifier;
			int num = (int)healthCareType;
			return new GeneratedGroupPanel.GroupInfo(identifier + num.ToString(), (int)healthCareType);
		}

		public static List<HealthCareCategory> GetHealthCareCategories(BuildingInfo info)
		{
			var cats = new List<HealthCareCategory>();

            if (info.m_buildingAI is HospitalAI || (info.m_buildingAI is HelicopterDepotAI helicopterDepotAI && helicopterDepotAI.m_info.m_class.m_service == ItemClass.Service.HealthCare))
            {
				cats.Add(HealthCareCategory.HealthCare);
				return cats;
            }
			if(info.m_buildingAI is CemeteryAI)
            {
				cats.Add(HealthCareCategory.DeathCare);
				return cats;
            }
			if(info.m_buildingAI is ChildcareAI)
            {
				cats.Add(HealthCareCategory.ChildCare);
				return cats;
            }
			if(info.m_buildingAI is EldercareAI)
            {
				cats.Add(HealthCareCategory.ElderCare);
				return cats;
            }
			if(info.m_buildingAI is SaunaAI)
            {
				cats.Add(HealthCareCategory.RecreationalCare);
				return cats;
            }
			return cats;
		}
	}
}


