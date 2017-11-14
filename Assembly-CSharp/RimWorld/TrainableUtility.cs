using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class TrainableUtility
	{
		private static List<TrainableDef> defsInListOrder = new List<TrainableDef>();

		public static List<TrainableDef> TrainableDefsInListOrder
		{
			get
			{
				return TrainableUtility.defsInListOrder;
			}
		}

		public static void Reset()
		{
			TrainableUtility.defsInListOrder.Clear();
			TrainableUtility.defsInListOrder.AddRange(from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td);
			while (true)
			{
				bool flag = false;
				int num = 0;
				while (num < TrainableUtility.defsInListOrder.Count)
				{
					TrainableDef trainableDef = TrainableUtility.defsInListOrder[num];
					if (trainableDef.prerequisites != null)
					{
						int num2 = 0;
						while (num2 < trainableDef.prerequisites.Count)
						{
							if (trainableDef.indent > trainableDef.prerequisites[num2].indent)
							{
								num2++;
								continue;
							}
							trainableDef.indent = trainableDef.prerequisites[num2].indent + 1;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						num++;
						continue;
					}
					break;
				}
				if (!flag)
					break;
			}
		}

		public static string MasterString(Pawn pawn)
		{
			return (pawn.playerSettings.master == null) ? ("(" + "NoneLower".Translate() + ")") : RelationsUtility.LabelWithBondInfo(pawn.playerSettings.master, pawn);
		}

		public static void OpenMasterSelectMenu(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("(" + "NoneLower".Translate() + ")", delegate
			{
				p.playerSettings.master = null;
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			foreach (Pawn item in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				string text = RelationsUtility.LabelWithBondInfo(item, p);
				int level = item.skills.GetSkill(SkillDefOf.Animals).Level;
				int num = TrainableUtility.MinimumHandlingSkill(p);
				Action action;
				if (level >= num)
				{
					Pawn localCol = item;
					action = delegate
					{
						p.playerSettings.master = localCol;
					};
				}
				else
				{
					action = null;
					text = text + " (" + "SkillTooLow".Translate(SkillDefOf.Animals.LabelCap, level, num) + ")";
				}
				list.Add(new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static int MinimumHandlingSkill(Pawn p)
		{
			return Mathf.RoundToInt(p.GetStatValue(StatDefOf.MinimumHandlingSkill, true));
		}
	}
}
