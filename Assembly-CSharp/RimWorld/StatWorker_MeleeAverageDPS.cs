using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public class StatWorker_MeleeAverageDPS : StatWorker
	{
		[CompilerGenerated]
		private static Func<VerbUtility.VerbPropertiesWithSource, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<VerbUtility.VerbPropertiesWithSource, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<VerbUtility.VerbPropertiesWithSource, bool> <>f__am$cache2;

		public StatWorker_MeleeAverageDPS()
		{
		}

		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.IsWeapon && !thingDef.tools.NullOrEmpty<Tool>();
		}

		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return 0f;
			}
			Pawn attacker = StatWorker_MeleeAverageDPS.GetCurrentWeaponUser(req.Thing);
			float num = (from x in VerbUtility.GetAllVerbProperties(thingDef.Verbs, thingDef.tools)
			where x.verbProps.IsMeleeAttack
			select x).AverageWeighted((VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeSelectionWeight(x.tool, attacker, req.Thing, null, false), (VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeDamageAmount(x.tool, attacker, req.Thing, null));
			float num2 = (from x in VerbUtility.GetAllVerbProperties(thingDef.Verbs, thingDef.tools)
			where x.verbProps.IsMeleeAttack
			select x).AverageWeighted((VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeSelectionWeight(x.tool, attacker, req.Thing, null, false), (VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedCooldown(x.tool, attacker, req.Thing));
			return num / num2;
		}

		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return null;
			}
			Pawn currentWeaponUser = StatWorker_MeleeAverageDPS.GetCurrentWeaponUser(req.Thing);
			IEnumerable<VerbUtility.VerbPropertiesWithSource> enumerable = from x in VerbUtility.GetAllVerbProperties(thingDef.Verbs, thingDef.tools)
			where x.verbProps.IsMeleeAttack
			select x;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (VerbUtility.VerbPropertiesWithSource verbPropertiesWithSource in enumerable)
			{
				float num = verbPropertiesWithSource.verbProps.AdjustedMeleeDamageAmount(verbPropertiesWithSource.tool, currentWeaponUser, req.Thing, null);
				float num2 = verbPropertiesWithSource.verbProps.AdjustedCooldown(verbPropertiesWithSource.tool, currentWeaponUser, req.Thing);
				if (verbPropertiesWithSource.tool != null)
				{
					stringBuilder.AppendLine(string.Format("  {0}: {1} ({2})", "Tool".Translate(), verbPropertiesWithSource.tool.LabelCap, verbPropertiesWithSource.ToolCapacity.defName));
				}
				else
				{
					stringBuilder.AppendLine(string.Format("  {0}:", "StatsReport_NonToolAttack".Translate()));
				}
				stringBuilder.AppendLine(string.Format("    {0} {1}", num.ToString("F1"), "DamageLower".Translate()));
				stringBuilder.AppendLine(string.Format("    {0} {1}", num2.ToString("F2"), "SecondsPerAttackLower".Translate()));
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}

		public static Pawn GetCurrentWeaponUser(Thing weapon)
		{
			if (weapon == null)
			{
				return null;
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = weapon.ParentHolder as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				return pawn_EquipmentTracker.pawn;
			}
			Pawn_ApparelTracker pawn_ApparelTracker = weapon.ParentHolder as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				return pawn_ApparelTracker.pawn;
			}
			return null;
		}

		[CompilerGenerated]
		private static bool <GetValueUnfinalized>m__0(VerbUtility.VerbPropertiesWithSource x)
		{
			return x.verbProps.IsMeleeAttack;
		}

		[CompilerGenerated]
		private static bool <GetValueUnfinalized>m__1(VerbUtility.VerbPropertiesWithSource x)
		{
			return x.verbProps.IsMeleeAttack;
		}

		[CompilerGenerated]
		private static bool <GetExplanationUnfinalized>m__2(VerbUtility.VerbPropertiesWithSource x)
		{
			return x.verbProps.IsMeleeAttack;
		}

		[CompilerGenerated]
		private sealed class <GetValueUnfinalized>c__AnonStorey0
		{
			internal Pawn attacker;

			internal StatRequest req;

			public <GetValueUnfinalized>c__AnonStorey0()
			{
			}

			internal float <>m__0(VerbUtility.VerbPropertiesWithSource x)
			{
				return x.verbProps.AdjustedMeleeSelectionWeight(x.tool, this.attacker, this.req.Thing, null, false);
			}

			internal float <>m__1(VerbUtility.VerbPropertiesWithSource x)
			{
				return x.verbProps.AdjustedMeleeDamageAmount(x.tool, this.attacker, this.req.Thing, null);
			}

			internal float <>m__2(VerbUtility.VerbPropertiesWithSource x)
			{
				return x.verbProps.AdjustedMeleeSelectionWeight(x.tool, this.attacker, this.req.Thing, null, false);
			}

			internal float <>m__3(VerbUtility.VerbPropertiesWithSource x)
			{
				return x.verbProps.AdjustedCooldown(x.tool, this.attacker, this.req.Thing);
			}
		}
	}
}
