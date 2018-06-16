using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000E2A RID: 3626
	public static class DebugTools_Health
	{
		// Token: 0x060054F6 RID: 21750 RVA: 0x002B8F68 File Offset: 0x002B7368
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

		// Token: 0x060054F7 RID: 21751 RVA: 0x002B9040 File Offset: 0x002B7440
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

		// Token: 0x060054F8 RID: 21752 RVA: 0x002B90D8 File Offset: 0x002B74D8
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

		// Token: 0x060054F9 RID: 21753 RVA: 0x002B91D4 File Offset: 0x002B75D4
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

		// Token: 0x060054FA RID: 21754 RVA: 0x002B92C4 File Offset: 0x002B76C4
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

		// Token: 0x060054FB RID: 21755 RVA: 0x002B937C File Offset: 0x002B777C
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

		// Token: 0x060054FC RID: 21756 RVA: 0x002B9478 File Offset: 0x002B7878
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
