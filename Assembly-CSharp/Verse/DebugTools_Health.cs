using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class DebugTools_Health
	{
		public static List<DebugMenuOption> Options_RestorePart(Pawn p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (BodyPartRecord notMissingPart in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined))
			{
				BodyPartRecord localPart = notMissingPart;
				list.Add(new DebugMenuOption(localPart.def.LabelCap, DebugMenuOptionMode.Action, delegate
				{
					p.health.RestorePart(localPart, null, true);
				}));
			}
			return list;
		}

		public static List<DebugMenuOption> Options_ApplyDamage()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (DamageDef allDef in DefDatabase<DamageDef>.AllDefs)
			{
				DamageDef localDef = allDef;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate
				{
					Pawn pawn = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>().FirstOrDefault();
					if (pawn != null)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Damage_BodyParts(pawn, localDef)));
					}
				}));
			}
			return list;
		}

		private static List<DebugMenuOption> Options_Damage_BodyParts(Pawn p, DamageDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate
			{
				p.TakeDamage(new DamageInfo(def, 5, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			}));
			foreach (BodyPartRecord allPart in p.RaceProps.body.AllParts)
			{
				BodyPartRecord localPart = allPart;
				list.Add(new DebugMenuOption(localPart.def.LabelCap, DebugMenuOptionMode.Action, delegate
				{
					Pawn pawn = p;
					DamageDef def2 = def;
					int amount = 5;
					BodyPartRecord hitPart = localPart;
					pawn.TakeDamage(new DamageInfo(def2, amount, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown));
				}));
			}
			return list;
		}

		public static List<DebugMenuOption> Options_AddHediff()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type item in (from t in typeof(Hediff).AllSubclasses()
			where !t.IsAbstract
			select t).Concat(Gen.YieldSingle(typeof(Hediff))))
			{
				Type localDiffType = item;
				if (localDiffType != typeof(Hediff_Injury))
				{
					list.Add(new DebugMenuOption(localDiffType.ToString(), DebugMenuOptionMode.Action, delegate
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_HediffsDefs(localDiffType)));
					}));
				}
			}
			return list;
		}

		private static List<DebugMenuOption> Options_HediffsDefs(Type diffType)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (HediffDef item in from d in DefDatabase<HediffDef>.AllDefs
			where d.hediffClass == diffType
			select d)
			{
				HediffDef localDef = item;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate
				{
					Pawn pawn = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).Where((Thing t) => t is Pawn).Cast<Pawn>()
						.FirstOrDefault();
					if (pawn != null)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Hediff_BodyParts(pawn, localDef)));
						DebugTools.curTool = null;
					}
				}));
			}
			return list;
		}

		private static List<DebugMenuOption> Options_Hediff_BodyParts(Pawn p, HediffDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate
			{
				p.health.AddHediff(def, null, null);
			}));
			foreach (BodyPartRecord allPart in p.RaceProps.body.AllParts)
			{
				BodyPartRecord localPart = allPart;
				list.Add(new DebugMenuOption(localPart.def.LabelCap, DebugMenuOptionMode.Action, delegate
				{
					p.health.AddHediff(def, localPart, null);
				}));
			}
			return list;
		}

		public static List<DebugMenuOption> Options_RemoveHediff(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				Hediff localH = hediff;
				list.Add(new DebugMenuOption(localH.LabelCap + ((localH.Part == null) ? string.Empty : (" (" + localH.Part.def + ")")), DebugMenuOptionMode.Action, delegate
				{
					pawn.health.RemoveHediff(localH);
				}));
			}
			return list;
		}
	}
}
