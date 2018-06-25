using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020008F9 RID: 2297
	public static class BuildCopyCommandUtility
	{
		// Token: 0x06003532 RID: 13618 RVA: 0x001C7718 File Offset: 0x001C5B18
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
				command_Action.action = delegate()
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
				command_Action.iconAngle = des.iconAngle;
				command_Action.iconOffset = des.iconOffset;
				command_Action.order = 10f;
				if (stuff != null)
				{
					command_Action.defaultIconColor = stuff.stuffProps.color;
				}
				else
				{
					command_Action.defaultIconColor = buildable.uiIconColor;
				}
				command_Action.hotKey = KeyBindingDefOf.Misc11;
				result = command_Action;
			}
			return result;
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x001C7840 File Offset: 0x001C5C40
		public static Designator_Build FindAllowedDesignator(BuildableDef buildable, bool mustBeVisible = true)
		{
			List<DesignationCategoryDef> allDefsListForReading = DefDatabase<DesignationCategoryDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<Designator> allResolvedDesignators = allDefsListForReading[i].AllResolvedDesignators;
				for (int j = 0; j < allResolvedDesignators.Count; j++)
				{
					Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignatorRecursive(allResolvedDesignators[j], buildable, mustBeVisible);
					if (designator_Build != null)
					{
						return designator_Build;
					}
				}
			}
			return null;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x001C78C0 File Offset: 0x001C5CC0
		private static Designator_Build FindAllowedDesignatorRecursive(Designator designator, BuildableDef buildable, bool mustBeVisible)
		{
			Designator_Build result;
			if (!Current.Game.Rules.DesignatorAllowed(designator))
			{
				result = null;
			}
			else if (mustBeVisible && !designator.Visible)
			{
				result = null;
			}
			else
			{
				Designator_Build designator_Build = designator as Designator_Build;
				if (designator_Build != null && designator_Build.PlacingDef == buildable)
				{
					result = designator_Build;
				}
				else
				{
					Designator_Dropdown designator_Dropdown = designator as Designator_Dropdown;
					if (designator_Dropdown != null)
					{
						for (int i = 0; i < designator_Dropdown.Elements.Count; i++)
						{
							Designator_Build designator_Build2 = BuildCopyCommandUtility.FindAllowedDesignatorRecursive(designator_Dropdown.Elements[i], buildable, mustBeVisible);
							if (designator_Build2 != null)
							{
								return designator_Build2;
							}
						}
					}
					result = null;
				}
			}
			return result;
		}
	}
}
