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
				list.Add(new DebugMenuOption(localPart.def.LabelCap, DebugMenuOptionMode.Action, (Action)delegate()
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
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, (Action)delegate()
				{
					Pawn pawn = (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>().FirstOrDefault();
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
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, (Action)delegate()
			{
				p.TakeDamage(new DamageInfo(def, 5, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			}));
			foreach (BodyPartRecord allPart in p.RaceProps.body.AllParts)
			{
				BodyPartRecord localPart = allPart;
				list.Add(new DebugMenuOption(localPart.def.LabelCap, DebugMenuOptionMode.Action, (Action)delegate()
				{
					Pawn obj = p;
					DamageDef def2 = def;
					int amount = 5;
					BodyPartRecord hitPart = localPart;
					obj.TakeDamage(new DamageInfo(def2, amount, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown));
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
					list.Add(new DebugMenuOption(localDiffType.ToString(), DebugMenuOptionMode.Action, (Action)delegate
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
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, (Action)delegate()
				{
					Pawn pawn = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).Where((Func<Thing, bool>)((Thing t) => t is Pawn)).Cast<Pawn>().FirstOrDefault();
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
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, (Action)delegate()
			{
				p.health.AddHediff(def, null, default(DamageInfo?));
			}));
			foreach (BodyPartRecord allPart in p.RaceProps.body.AllParts)
			{
				BodyPartRecord localPart = allPart;
				list.Add(new DebugMenuOption(localPart.def.LabelCap, DebugMenuOptionMode.Action, (Action)delegate()
				{
					p.health.AddHediff(def, localPart, default(DamageInfo?));
				}));
			}
			return list;
		}
	}
}
