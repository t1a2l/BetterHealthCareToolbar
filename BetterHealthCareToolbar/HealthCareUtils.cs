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
			var SpriteNames = new string[6];
			switch (cat)
			{
				case HealthCareCategory.HealthCare:
					string[] HealthCareSpriteNames =
                    {
						"HealthCare",
						"SubBarButtonBase",
						"SubBarButtonBasePressed",
						"SubBarButtonBaseDisabled",
						"SubBarButtonBaseFocused",
						"SubBarButtonBaseHovered"
                    };
					button.atlas = TextureUtils.CreateTextureAtlas("healthcare.png", "HealthCare", HealthCareSpriteNames);
					SpriteNames = HealthCareSpriteNames;
					break;
				case HealthCareCategory.DeathCare:
					string[] DeathCareSpriteNames =
                    {
						"DeathCare",
						"SubBarButtonBase",
						"SubBarButtonBasePressed",
						"SubBarButtonBaseDisabled",
						"SubBarButtonBaseFocused",
						"SubBarButtonBaseHovered"
                    };
					button.atlas = TextureUtils.CreateTextureAtlas("deathcare.png", "DeathCare", DeathCareSpriteNames);
					SpriteNames = DeathCareSpriteNames;
					break;
				case HealthCareCategory.ChildCare:
					string[] ChildCareSpriteNames =
                    {
						"ChildCare",
						"SubBarButtonBase",
						"SubBarButtonBasePressed",
						"SubBarButtonBaseDisabled",
						"SubBarButtonBaseFocused",
						"SubBarButtonBaseHovered"
                    };
					button.atlas = TextureUtils.CreateTextureAtlas("childcare.png", "ChildCare", ChildCareSpriteNames);
					SpriteNames = ChildCareSpriteNames;
					break;
				case HealthCareCategory.ElderCare:
					string[] ElderCareSpriteNames =
                    {
						"ElderCare",
						"SubBarButtonBase",
						"SubBarButtonBasePressed",
						"SubBarButtonBaseDisabled",
						"SubBarButtonBaseFocused",
						"SubBarButtonBaseHovered"
                    };
					button.atlas = TextureUtils.CreateTextureAtlas("eldercare.png", "ElderCare", ElderCareSpriteNames);
					SpriteNames = ElderCareSpriteNames;
					break;
				case HealthCareCategory.RecreationalCare:
					string[] RecreationalCareSpriteNames =
                    {
						"RecreationalCare",
						"SubBarButtonBase",
						"SubBarButtonBasePressed",
						"SubBarButtonBaseDisabled",
						"SubBarButtonBaseFocused",
						"SubBarButtonBaseHovered"
                    };
					button.atlas = TextureUtils.CreateTextureAtlas("recreationalcare.png", "RecreationalCare", RecreationalCareSpriteNames);
					SpriteNames = RecreationalCareSpriteNames;
					break;
				default:
					break;
			}
			button.normalFgSprite = button.pressedFgSprite = button.disabledFgSprite = button.focusedFgSprite = button.hoveredFgSprite = SpriteNames[0];
			button.normalBgSprite = SpriteNames[1];
			button.pressedBgSprite = SpriteNames[2];
			button.disabledBgSprite = SpriteNames[3];
			button.focusedBgSprite = SpriteNames[4];
			button.hoveredBgSprite = SpriteNames[5];
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
					return "Pools, gyms, sanuas, community buildings for the community";
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


