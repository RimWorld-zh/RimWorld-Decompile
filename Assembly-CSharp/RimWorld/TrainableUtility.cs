using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200053B RID: 1339
	public static class TrainableUtility
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x000D9774 File Offset: 0x000D7B74
		public static List<TrainableDef> TrainableDefsInListOrder
		{
			get
			{
				return TrainableUtility.defsInListOrder;
			}
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x000D9790 File Offset: 0x000D7B90
		public static void Reset()
		{
			TrainableUtility.defsInListOrder.Clear();
			TrainableUtility.defsInListOrder.AddRange(from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td);
			bool flag;
			do
			{
				flag = false;
				for (int i = 0; i < TrainableUtility.defsInListOrder.Count; i++)
				{
					TrainableDef trainableDef = TrainableUtility.defsInListOrder[i];
					if (trainableDef.prerequisites != null)
					{
						for (int j = 0; j < trainableDef.prerequisites.Count; j++)
						{
							if (trainableDef.indent <= trainableDef.prerequisites[j].indent)
							{
								trainableDef.indent = trainableDef.prerequisites[j].indent + 1;
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			while (flag);
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x000D9880 File Offset: 0x000D7C80
		public static string MasterString(Pawn pawn)
		{
			return (pawn.playerSettings.Master == null) ? ("(" + "NoneLower".Translate() + ")") : RelationsUtility.LabelWithBondInfo(pawn.playerSettings.Master, pawn);
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x000D98D4 File Offset: 0x000D7CD4
		public static int MinimumHandlingSkill(Pawn p)
		{
			return Mathf.RoundToInt(p.GetStatValue(StatDefOf.MinimumHandlingSkill, true));
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x000D98FC File Offset: 0x000D7CFC
		public static void MasterSelectButton(Rect rect, Pawn pawn, bool paintable)
		{
			Rect rect2 = rect;
			if (TrainableUtility.<>f__mg$cache0 == null)
			{
				TrainableUtility.<>f__mg$cache0 = new Func<Pawn, Pawn>(TrainableUtility.MasterSelectButton_GetMaster);
			}
			Func<Pawn, Pawn> getPayload = TrainableUtility.<>f__mg$cache0;
			if (TrainableUtility.<>f__mg$cache1 == null)
			{
				TrainableUtility.<>f__mg$cache1 = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Pawn>>>(TrainableUtility.MasterSelectButton_GenerateMenu);
			}
			Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Pawn>>> menuGenerator = TrainableUtility.<>f__mg$cache1;
			string buttonLabel = TrainableUtility.MasterString(pawn).Truncate(rect.width, null);
			string dragLabel = TrainableUtility.MasterString(pawn);
			Widgets.Dropdown<Pawn, Pawn>(rect2, pawn, getPayload, menuGenerator, buttonLabel, null, dragLabel, null, null, paintable);
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x000D9978 File Offset: 0x000D7D78
		private static Pawn MasterSelectButton_GetMaster(Pawn pet)
		{
			return pet.playerSettings.Master;
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x000D9998 File Offset: 0x000D7D98
		private static IEnumerable<Widgets.DropdownMenuElement<Pawn>> MasterSelectButton_GenerateMenu(Pawn p)
		{
			yield return new Widgets.DropdownMenuElement<Pawn>
			{
				option = new FloatMenuOption("(" + "NoneLower".Translate() + ")", delegate()
				{
					p.playerSettings.Master = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null),
				payload = null
			};
			using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMaps_FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn col = enumerator.Current;
					string lab = RelationsUtility.LabelWithBondInfo(col, p);
					Action action = null;
					int level = col.skills.GetSkill(SkillDefOf.Animals).Level;
					int minLevel = TrainableUtility.MinimumHandlingSkill(p);
					if (level < minLevel)
					{
						action = null;
						lab = lab + " (" + "SkillTooLow".Translate(new object[]
						{
							SkillDefOf.Animals.LabelCap,
							level,
							minLevel
						}) + ")";
					}
					else if (TrainableUtility.CanBeMaster(col, p, true))
					{
						action = delegate()
						{
							p.playerSettings.Master = col;
						};
					}
					yield return new Widgets.DropdownMenuElement<Pawn>
					{
						option = new FloatMenuOption(lab, action, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = col
					};
				}
			}
			yield break;
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x000D99C4 File Offset: 0x000D7DC4
		public static bool CanBeMaster(Pawn master, Pawn animal, bool checkSpawned = true)
		{
			bool result;
			if ((checkSpawned && !master.Spawned) || master.IsPrisoner)
			{
				result = false;
			}
			else
			{
				int level = master.skills.GetSkill(SkillDefOf.Animals).Level;
				int num = TrainableUtility.MinimumHandlingSkill(animal);
				result = (level >= num);
			}
			return result;
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x000D9A20 File Offset: 0x000D7E20
		public static string GetIconTooltipText(Pawn pawn)
		{
			string text = "";
			if (pawn.playerSettings.Master != null)
			{
				text += string.Format("{0}: {1}\n", "Master".Translate(), pawn.playerSettings.Master.LabelShort);
			}
			IEnumerable<Pawn> allColonistBondsFor = TrainableUtility.GetAllColonistBondsFor(pawn);
			if (allColonistBondsFor.Any<Pawn>())
			{
				text += string.Format("{0}: {1}\n", "BondedTo".Translate(), (from bond in allColonistBondsFor
				select bond.LabelShort).ToCommaList(true));
			}
			return text;
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x000D9AD0 File Offset: 0x000D7ED0
		public static IEnumerable<Pawn> GetAllColonistBondsFor(Pawn pet)
		{
			return from bond in pet.relations.DirectRelations
			where bond.def == PawnRelationDefOf.Bond && bond.otherPawn != null && bond.otherPawn.IsColonistPlayerControlled
			select bond.otherPawn;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x000D9B34 File Offset: 0x000D7F34
		public static int DegradationPeriodTicks(ThingDef def)
		{
			return Mathf.RoundToInt(TrainableUtility.DecayIntervalDaysFromWildnessCurve.Evaluate(def.race.wildness) * 60000f);
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x000D9B6C File Offset: 0x000D7F6C
		public static bool TamenessCanDecay(ThingDef def)
		{
			return def.race.wildness > 0.101f;
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x000D9B94 File Offset: 0x000D7F94
		public static string GetWildnessExplanation(ThingDef def)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("WildnessExplanation".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(string.Format("{0}: {1}", "TrainingDecayInterval".Translate(), TrainableUtility.DegradationPeriodTicks(def).ToStringTicksToDays("F1")));
			if (!TrainableUtility.TamenessCanDecay(def))
			{
				stringBuilder.AppendLine("TamenessWillNotDecay".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000EAD RID: 3757
		private static List<TrainableDef> defsInListOrder = new List<TrainableDef>();

		// Token: 0x04000EAE RID: 3758
		private static readonly SimpleCurve DecayIntervalDaysFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 15f),
				true
			},
			{
				new CurvePoint(1f, 5f),
				true
			}
		};

		// Token: 0x04000EB0 RID: 3760
		[CompilerGenerated]
		private static Func<Pawn, Pawn> <>f__mg$cache0;

		// Token: 0x04000EB1 RID: 3761
		[CompilerGenerated]
		private static Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Pawn>>> <>f__mg$cache1;
	}
}
