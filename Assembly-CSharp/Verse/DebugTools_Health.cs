using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class DebugTools_Health
	{
		[CompilerGenerated]
		private static Func<HediffDef, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<BodyPartRecord, string> <>f__am$cache1;

		public static List<DebugMenuOption> Options_RestorePart(Pawn p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (BodyPartRecord localPart2 in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.health.RestorePart(localPart, null, true);
				}));
			}
			return list;
		}

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

		private static List<DebugMenuOption> Options_Damage_BodyParts(Pawn p, DamageDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.TakeDamage(new DamageInfo(def, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
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
					p2.TakeDamage(new DamageInfo(def2, amount, 0f, -1f, null, localPart2, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}));
			}
			return list;
		}

		public static List<DebugMenuOption> Options_AddHediff()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (HediffDef localDef2 in from d in DefDatabase<HediffDef>.AllDefs
			orderby d.hediffClass.ToStringSafe<Type>()
			select d)
			{
				HediffDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn pawn = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).Where((Thing t) => t is Pawn).Cast<Pawn>().FirstOrDefault<Pawn>();
					if (pawn != null)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Hediff_BodyParts(pawn, localDef)));
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
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.health.AddHediff(def, null, null, null);
			}));
			foreach (BodyPartRecord localPart2 in from pa in p.RaceProps.body.AllParts
			orderby pa.Label
			select pa)
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.health.AddHediff(def, localPart, null, null);
				}));
			}
			return list;
		}

		public static List<DebugMenuOption> Options_RemoveHediff(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Hediff localH2 in pawn.health.hediffSet.hediffs)
			{
				Hediff localH = localH2;
				list.Add(new DebugMenuOption(localH.LabelCap + ((localH.Part == null) ? string.Empty : (" (" + localH.Part.def + ")")), DebugMenuOptionMode.Action, delegate()
				{
					pawn.health.RemoveHediff(localH);
				}));
			}
			return list;
		}

		[CompilerGenerated]
		private static string <Options_AddHediff>m__0(HediffDef d)
		{
			return d.hediffClass.ToStringSafe<Type>();
		}

		[CompilerGenerated]
		private static string <Options_Hediff_BodyParts>m__1(BodyPartRecord pa)
		{
			return pa.Label;
		}

		[CompilerGenerated]
		private sealed class <Options_RestorePart>c__AnonStorey0
		{
			internal Pawn p;

			public <Options_RestorePart>c__AnonStorey0()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <Options_RestorePart>c__AnonStorey1
		{
			internal BodyPartRecord localPart;

			internal DebugTools_Health.<Options_RestorePart>c__AnonStorey0 <>f__ref$0;

			public <Options_RestorePart>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.<>f__ref$0.p.health.RestorePart(this.localPart, null, true);
			}
		}

		[CompilerGenerated]
		private sealed class <Options_ApplyDamage>c__AnonStorey2
		{
			internal DamageDef localDef;

			public <Options_ApplyDamage>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Pawn pawn = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>().FirstOrDefault<Pawn>();
				if (pawn != null)
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Damage_BodyParts(pawn, this.localDef)));
				}
			}
		}

		[CompilerGenerated]
		private sealed class <Options_Damage_BodyParts>c__AnonStorey3
		{
			internal Pawn p;

			internal DamageDef def;

			public <Options_Damage_BodyParts>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				this.p.TakeDamage(new DamageInfo(this.def, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		[CompilerGenerated]
		private sealed class <Options_Damage_BodyParts>c__AnonStorey4
		{
			internal BodyPartRecord localPart;

			internal DebugTools_Health.<Options_Damage_BodyParts>c__AnonStorey3 <>f__ref$3;

			public <Options_Damage_BodyParts>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				Thing p = this.<>f__ref$3.p;
				DamageDef def = this.<>f__ref$3.def;
				float amount = 5f;
				BodyPartRecord hitPart = this.localPart;
				p.TakeDamage(new DamageInfo(def, amount, 0f, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		[CompilerGenerated]
		private sealed class <Options_AddHediff>c__AnonStorey5
		{
			internal HediffDef localDef;

			private static Func<Thing, bool> <>f__am$cache0;

			public <Options_AddHediff>c__AnonStorey5()
			{
			}

			internal void <>m__0()
			{
				Pawn pawn = (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Pawn
				select t).Cast<Pawn>().FirstOrDefault<Pawn>();
				if (pawn != null)
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Hediff_BodyParts(pawn, this.localDef)));
				}
			}

			private static bool <>m__1(Thing t)
			{
				return t is Pawn;
			}
		}

		[CompilerGenerated]
		private sealed class <Options_Hediff_BodyParts>c__AnonStorey6
		{
			internal Pawn p;

			internal HediffDef def;

			public <Options_Hediff_BodyParts>c__AnonStorey6()
			{
			}

			internal void <>m__0()
			{
				this.p.health.AddHediff(this.def, null, null, null);
			}
		}

		[CompilerGenerated]
		private sealed class <Options_Hediff_BodyParts>c__AnonStorey7
		{
			internal BodyPartRecord localPart;

			internal DebugTools_Health.<Options_Hediff_BodyParts>c__AnonStorey6 <>f__ref$6;

			public <Options_Hediff_BodyParts>c__AnonStorey7()
			{
			}

			internal void <>m__0()
			{
				this.<>f__ref$6.p.health.AddHediff(this.<>f__ref$6.def, this.localPart, null, null);
			}
		}

		[CompilerGenerated]
		private sealed class <Options_RemoveHediff>c__AnonStorey8
		{
			internal Pawn pawn;

			public <Options_RemoveHediff>c__AnonStorey8()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <Options_RemoveHediff>c__AnonStorey9
		{
			internal Hediff localH;

			internal DebugTools_Health.<Options_RemoveHediff>c__AnonStorey8 <>f__ref$8;

			public <Options_RemoveHediff>c__AnonStorey9()
			{
			}

			internal void <>m__0()
			{
				this.<>f__ref$8.pawn.health.RemoveHediff(this.localH);
			}
		}
	}
}
