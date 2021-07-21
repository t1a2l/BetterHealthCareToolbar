using System.Collections.Generic;
using System.Reflection;
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
		public static void SetToolbarTabSprite(GeneratedGroupPanel __instance, ref UIButton button, HealthCareCategory cat)
		{
			const int SIZE = 31;
			UISprite buttonSprite = button.AddUIComponent<UISprite>();
            buttonSprite.size = new Vector2(24f, 24f);
            buttonSprite.relativePosition = new Vector2(4f, 5f);
			string[] spriteNames = new string[1];
			switch (cat)
			{
				case HealthCareCategory.HealthCare:
					buttonSprite = __instance.Find<UISprite>("HealthCareIcon");
					buttonSprite.spriteName = "healthcare";
					break;
				case HealthCareCategory.DeathCare:
					buttonSprite = __instance.Find<UISprite>("DeathCareIcon");
					buttonSprite.spriteName = "deathcare";
					break;
				case HealthCareCategory.ChildCare:
					spriteNames[0] = "childcare";
					buttonSprite.atlas = TextureUtils.CreateTextureAtlas("Icons/childcare.png", "ChildCare", SIZE, SIZE, spriteNames);
					break;
				case HealthCareCategory.ElderCare:
					spriteNames[0] = "eldercare";
					buttonSprite.atlas = TextureUtils.CreateTextureAtlas("Icons/eldercare.png", "ElderCare", SIZE, SIZE, spriteNames);
					break;
				case HealthCareCategory.RecreationalCare:
					spriteNames[0] = "recreationalcare";
					buttonSprite.atlas = TextureUtils.CreateTextureAtlas("Icons/recreationalcare.png", "RecreationalCare", SIZE, SIZE, spriteNames);
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


