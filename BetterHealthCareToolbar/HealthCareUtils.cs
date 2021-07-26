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
			switch (cat)
			{
				case HealthCareCategory.HealthCare:
					button.normalFgSprite = "HealthCareBase";
					button.disabledFgSprite = "HealthCareDisabled";
					button.focusedFgSprite = "HealthCareFocused";
					button.hoveredFgSprite = "HealthCareHovered";
					button.pressedFgSprite ="HealthCarePressed";
					break;
				case HealthCareCategory.DeathCare:
					button.normalFgSprite = "DeathCareBase";
					button.disabledFgSprite = "DeathCareDisabled";
					button.focusedFgSprite = "DeathCareFocused";
					button.hoveredFgSprite = "DeathCareHovered";
					button.pressedFgSprite ="DeathCarePressed";
					break;
				case HealthCareCategory.ChildCare:
					button.normalFgSprite = "ChildCareBase";
					button.disabledFgSprite = "ChildCareDisabled";
					button.focusedFgSprite = "ChildCareFocused";
					button.hoveredFgSprite = "ChildCareHovered";
					button.pressedFgSprite ="ChildCarePressed";					
					break;
				case HealthCareCategory.ElderCare:
					button.normalFgSprite = "ElderCareBase";
					button.disabledFgSprite = "ElderCareDisabled";
					button.focusedFgSprite = "ElderCareFocused";
					button.hoveredFgSprite = "ElderCareHovered";
					button.pressedFgSprite ="ElderhCarePressed";					
					break;
				case HealthCareCategory.RecreationalCare:
					button.normalFgSprite = "RecreationalCareBase";
					button.disabledFgSprite = "RecreationalCareDisabled";
					button.focusedFgSprite = "RecreationalCareFocused";
					button.hoveredFgSprite = "RecreationalCareHovered";
					button.pressedFgSprite ="RecreationalCarePressed";
					break;
				default:
					break;
			}
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


