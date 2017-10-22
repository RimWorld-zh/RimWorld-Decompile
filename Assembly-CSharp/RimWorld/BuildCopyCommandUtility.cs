using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public static class BuildCopyCommandUtility
	{
		public static Command BuildCopyCommand(BuildableDef buildable, ThingDef stuff)
		{
			Designator_Build des = BuildCopyCommandUtility.FindAllowedDesignator(buildable, true);
			Command result;
			if (des == null)
			{
				result = null;
			}
			else
			{
				Command_Action command_Action = new Command_Action();
				command_Action.action = (Action)delegate()
				{
					SoundDefOf.SelectDesignator.PlayOneShotOnCamera(null);
					des.SetStuffDef(stuff);
					Find.DesignatorManager.Select(des);
				};
				command_Action.defaultLabel = "CommandBuildCopy".Translate();
				command_Action.defaultDesc = "CommandBuildCopyDesc".Translate();
				command_Action.icon = des.icon;
				command_Action.iconProportions = des.iconProportions;
				command_Action.iconDrawScale = des.iconDrawScale;
				command_Action.iconTexCoords = des.iconTexCoords;
				if (stuff != null)
				{
					command_Action.defaultIconColor = stuff.stuffProps.color;
				}
				else
				{
					command_Action.defaultIconColor = buildable.IconDrawColor;
				}
				command_Action.hotKey = KeyBindingDefOf.Misc11;
				result = command_Action;
			}
			return result;
		}

		private static Designator_Build FindAllowedDesignator(BuildableDef buildable, bool mustBeVisible = true)
		{
			List<DesignationCategoryDef> allDefsListForReading = DefDatabase<DesignationCategoryDef>.AllDefsListForReading;
			GameRules rules = Current.Game.Rules;
			int num = 0;
			Designator_Build result;
			while (true)
			{
				Designator_Build designator_Build;
				if (num < allDefsListForReading.Count)
				{
					List<Designator> allResolvedDesignators = allDefsListForReading[num].AllResolvedDesignators;
					for (int i = 0; i < allResolvedDesignators.Count; i++)
					{
						if (rules.DesignatorAllowed(allResolvedDesignators[i]) && (!mustBeVisible || allResolvedDesignators[i].Visible))
						{
							designator_Build = (allResolvedDesignators[i] as Designator_Build);
							if (designator_Build != null && designator_Build.PlacingDef == buildable)
								goto IL_0088;
						}
					}
					num++;
					continue;
				}
				result = null;
				break;
				IL_0088:
				result = designator_Build;
				break;
			}
			return result;
		}
	}
}
