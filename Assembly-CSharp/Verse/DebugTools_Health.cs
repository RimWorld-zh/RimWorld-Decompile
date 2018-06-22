using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000E26 RID: 3622
	public static class DebugTools_Health
	{
		// Token: 0x06005510 RID: 21776 RVA: 0x002BAB20 File Offset: 0x002B8F20
		public static List<DebugMenuOption> Options_RestorePart(Pawn p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (BodyPartRecord localPart2 in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.health.RestorePart(localPart, null, true);
				}));
			}
			return list;
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x002BABF8 File Offset: 0x002B8FF8
		public static List<DebugMenuOption> Options_ApplyDamage()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (DamageDef localDef2 in DefDatabase<DamageDef>.AllDefs)
			{
				DamageDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn pawn = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>().FirstOrDefault<Pawn>();
					if (pawn != null)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Damage_BodyParts(pawn, localDef)));
					}
				}));
			}
			return list;
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x002BAC90 File Offset: 0x002B9090
		private static List<DebugMenuOption> Options_Damage_BodyParts(Pawn p, DamageDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.TakeDamage(new DamageInfo(def, 5f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}));
			foreach (BodyPartRecord localPart3 in p.RaceProps.body.AllParts)
			{
				BodyPartRecord localPart = localPart3;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					Thing p2 = p;
					DamageDef def2 = def;
					float amount = 5f;
					BodyPartRecord localPart2 = localPart;
					p2.TakeDamage(new DamageInfo(def2, amount, -1f, null, localPart2, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}));
			}
			return list;
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x002BAD8C File Offset: 0x002B918C
		public static List<DebugMenuOption> Options_AddHediff()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localDiffType2 in (from t in typeof(Hediff).AllSubclasses()
			where !t.IsAbstract
			select t).Concat(Gen.YieldSingle<Type>(typeof(Hediff))))
			{
				Type localDiffType = localDiffType2;
				if (localDiffType != typeof(Hediff_Injury))
				{
					list.Add(new DebugMenuOption(localDiffType.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_HediffsDefs(localDiffType)));
					}));
				}
			}
			return list;
		}

		// Token: 0x06005514 RID: 21780 RVA: 0x002BAE7C File Offset: 0x002B927C
		private static List<DebugMenuOption> Options_HediffsDefs(Type diffType)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (HediffDef localDef2 in from d in DefDatabase<HediffDef>.AllDefs
			where d.hediffClass == diffType
			select d)
			{
				HediffDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn pawn = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).Where((Thing t) => t is Pawn).Cast<Pawn>().FirstOrDefault<Pawn>();
					if (pawn != null)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Hediff_BodyParts(pawn, localDef)));
						DebugTools.curTool = null;
					}
				}));
			}
			return list;
		}

		// Token: 0x06005515 RID: 21781 RVA: 0x002BAF34 File Offset: 0x002B9334
		private static List<DebugMenuOption> Options_Hediff_BodyParts(Pawn p, HediffDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.health.AddHediff(def, null, null, null);
			}));
			foreach (BodyPartRecord localPart2 in p.RaceProps.body.AllParts)
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.health.AddHediff(def, localPart, null, null);
				}));
			}
			return list;
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x002BB030 File Offset: 0x002B9430
		public static List<DebugMenuOption> Options_RemoveHediff(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Hediff localH2 in pawn.health.hediffSet.hediffs)
			{
				Hediff localH = localH2;
				list.Add(new DebugMenuOption(localH.LabelCap + ((localH.Part == null) ? "" : (" (" + localH.Part.def + ")")), DebugMenuOptionMode.Action, delegate()
				{
					pawn.health.RemoveHediff(localH);
				}));
			}
			return list;
		}
	}
}
