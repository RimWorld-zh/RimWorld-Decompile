using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class WorkTypeDefsUtility
	{
		public static IEnumerable<WorkTypeDef> WorkTypeDefsInPriorityOrder
		{
			get
			{
				return from wt in DefDatabase<WorkTypeDef>.AllDefs
				orderby wt.naturalPriority descending
				select wt;
			}
		}

		public static string LabelTranslated(this WorkTags tags)
		{
			string result;
			switch (tags)
			{
			case WorkTags.None:
			{
				result = "WorkTagNone".Translate();
				break;
			}
			case WorkTags.Intellectual:
			{
				result = "WorkTagIntellectual".Translate();
				break;
			}
			case WorkTags.ManualDumb:
			{
				result = "WorkTagManualDumb".Translate();
				break;
			}
			case WorkTags.ManualSkilled:
			{
				result = "WorkTagManualSkilled".Translate();
				break;
			}
			case WorkTags.Violent:
			{
				result = "WorkTagViolent".Translate();
				break;
			}
			case WorkTags.Caring:
			{
				result = "WorkTagCaring".Translate();
				break;
			}
			case WorkTags.Social:
			{
				result = "WorkTagSocial".Translate();
				break;
			}
			case WorkTags.Animals:
			{
				result = "WorkTagAnimals".Translate();
				break;
			}
			case WorkTags.Artistic:
			{
				result = "WorkTagArtistic".Translate();
				break;
			}
			case WorkTags.Crafting:
			{
				result = "WorkTagCrafting".Translate();
				break;
			}
			case WorkTags.Cooking:
			{
				result = "WorkTagCooking".Translate();
				break;
			}
			case WorkTags.Firefighting:
			{
				result = "WorkTagFirefighting".Translate();
				break;
			}
			case WorkTags.Cleaning:
			{
				result = "WorkTagCleaning".Translate();
				break;
			}
			case WorkTags.Hauling:
			{
				result = "WorkTagHauling".Translate();
				break;
			}
			case WorkTags.PlantWork:
			{
				result = "WorkTagPlantWork".Translate();
				break;
			}
			case WorkTags.Mining:
			{
				result = "WorkTagMining".Translate();
				break;
			}
			default:
			{
				Log.Error("Unknown or mixed worktags for naming: " + (int)tags);
				result = "Worktag";
				break;
			}
			}
			return result;
		}

		public static bool OverlapsWithOnAnyWorkType(this WorkTags a, WorkTags b)
		{
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < allDefsListForReading.Count)
				{
					WorkTypeDef workTypeDef = allDefsListForReading[num];
					if ((((workTypeDef.workTags & a) != 0) ? (workTypeDef.workTags & b) : WorkTags.None) != 0)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
