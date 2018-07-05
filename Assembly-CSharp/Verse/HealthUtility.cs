using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public static class HealthUtility
	{
		public static readonly Color GoodConditionColor = new Color(0.6f, 0.8f, 0.65f);

		public static readonly Color DarkRedColor = new Color(0.73f, 0.02f, 0.02f);

		public static readonly Color ImpairedColor = new Color(0.9f, 0.7f, 0f);

		public static readonly Color SlightlyImpairedColor = new Color(0.9f, 0.9f, 0f);

		private static List<Hediff> tmpHediffs = new List<Hediff>();

		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<BodyPartRecord, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Predicate<BodyPartTagDef> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<BodyPartRecord, float> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<BodyPartRecord, float> <>f__am$cache5;

		public static string GetGeneralConditionLabel(Pawn pawn, bool shortVersion = false)
		{
			string result;
			if (pawn.health.Dead)
			{
				result = "Dead".Translate();
			}
			else if (!pawn.health.capacities.CanBeAwake)
			{
				result = "Unconscious".Translate();
			}
			else if (pawn.health.InPainShock)
			{
				result = ((!shortVersion || !"PainShockShort".CanTranslate()) ? "PainShock".Translate() : "PainShockShort".Translate());
			}
			else if (pawn.Downed)
			{
				result = "Incapacitated".Translate();
			}
			else
			{
				bool flag = false;
				for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
				{
					Hediff_Injury hediff_Injury = pawn.health.hediffSet.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null)
					{
						if (!hediff_Injury.IsPermanent())
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					result = "Injured".Translate();
				}
				else if (pawn.health.hediffSet.PainTotal > 0.3f)
				{
					result = "InPain".Translate();
				}
				else
				{
					result = "Healthy".Translate();
				}
			}
			return result;
		}

		public static Pair<string, Color> GetPartConditionLabel(Pawn pawn, BodyPartRecord part)
		{
			float partHealth = pawn.health.hediffSet.GetPartHealth(part);
			float maxHealth = part.def.GetMaxHealth(pawn);
			float num = partHealth / maxHealth;
			Color second = Color.white;
			string first;
			if (partHealth <= 0f)
			{
				Hediff_MissingPart hediff_MissingPart = null;
				List<Hediff_MissingPart> missingPartsCommonAncestors = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int i = 0; i < missingPartsCommonAncestors.Count; i++)
				{
					if (missingPartsCommonAncestors[i].Part == part)
					{
						hediff_MissingPart = missingPartsCommonAncestors[i];
						break;
					}
				}
				if (hediff_MissingPart == null)
				{
					bool fresh = false;
					if (hediff_MissingPart != null && hediff_MissingPart.IsFreshNonSolidExtremity)
					{
						fresh = true;
					}
					bool solid = part.def.IsSolid(part, pawn.health.hediffSet.hediffs);
					first = HealthUtility.GetGeneralDestroyedPartLabel(part, fresh, solid);
					second = Color.gray;
				}
				else
				{
					first = hediff_MissingPart.LabelCap;
					second = hediff_MissingPart.LabelColor;
				}
			}
			else if (num < 0.4f)
			{
				first = "SeriouslyImpaired".Translate();
				second = HealthUtility.DarkRedColor;
			}
			else if (num < 0.7f)
			{
				first = "Impaired".Translate();
				second = HealthUtility.ImpairedColor;
			}
			else if (num < 0.999f)
			{
				first = "SlightlyImpaired".Translate();
				second = HealthUtility.SlightlyImpairedColor;
			}
			else
			{
				first = "GoodCondition".Translate();
				second = HealthUtility.GoodConditionColor;
			}
			return new Pair<string, Color>(first, second);
		}

		public static string GetGeneralDestroyedPartLabel(BodyPartRecord part, bool fresh, bool solid)
		{
			string result;
			if (part.parent == null)
			{
				result = "SeriouslyImpaired".Translate();
			}
			else if (part.depth == BodyPartDepth.Inside || fresh)
			{
				if (solid)
				{
					result = "ShatteredBodyPart".Translate();
				}
				else
				{
					result = "DestroyedBodyPart".Translate();
				}
			}
			else
			{
				result = "MissingBodyPart".Translate();
			}
			return result;
		}

		private static IEnumerable<BodyPartRecord> HittablePartsViolence(HediffSet bodyModel)
		{
			return from x in bodyModel.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
			where x.depth == BodyPartDepth.Outside || (x.depth == BodyPartDepth.Inside && x.def.IsSolid(x, bodyModel.hediffs))
			select x;
		}

		public static void GiveInjuriesOperationFailureMinor(Pawn p, BodyPartRecord part)
		{
			HealthUtility.GiveRandomSurgeryInjuries(p, 20, part);
		}

		public static void GiveInjuriesOperationFailureCatastrophic(Pawn p, BodyPartRecord part)
		{
			HealthUtility.GiveRandomSurgeryInjuries(p, 65, part);
		}

		public static void GiveInjuriesOperationFailureRidiculous(Pawn p)
		{
			HealthUtility.GiveRandomSurgeryInjuries(p, 65, null);
		}

		public static void HealNonPermanentInjuriesAndRestoreLegs(Pawn p)
		{
			if (!p.Dead)
			{
				HealthUtility.tmpHediffs.Clear();
				HealthUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
				for (int i = 0; i < HealthUtility.tmpHediffs.Count; i++)
				{
					Hediff_Injury hediff_Injury = HealthUtility.tmpHediffs[i] as Hediff_Injury;
					if (hediff_Injury != null && !hediff_Injury.IsPermanent())
					{
						p.health.RemoveHediff(hediff_Injury);
					}
					else
					{
						Hediff_MissingPart hediff_MissingPart = HealthUtility.tmpHediffs[i] as Hediff_MissingPart;
						if (hediff_MissingPart != null && hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) && (hediff_MissingPart.Part.parent == null || p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Contains(hediff_MissingPart.Part.parent)))
						{
							p.health.RestorePart(hediff_MissingPart.Part, null, true);
						}
					}
				}
				HealthUtility.tmpHediffs.Clear();
			}
		}

		private static void GiveRandomSurgeryInjuries(Pawn p, int totalDamage, BodyPartRecord operatedPart)
		{
			IEnumerable<BodyPartRecord> source;
			if (operatedPart == null)
			{
				source = from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
				where !x.def.conceptual
				select x;
			}
			else
			{
				source = from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
				where !x.def.conceptual
				select x into pa
				where pa == operatedPart || pa.parent == operatedPart || (operatedPart != null && operatedPart.parent == pa)
				select pa;
			}
			source = from x in source
			where HealthUtility.GetMinHealthOfPartsWeWantToAvoidDestroying(x, p) >= 2f
			select x;
			BodyPartRecord brain = p.health.hediffSet.GetBrain();
			if (brain != null)
			{
				float maxBrainHealth = brain.def.GetMaxHealth(p);
				source = from x in source
				where x != brain || p.health.hediffSet.GetPartHealth(x) >= maxBrainHealth * 0.5f + 1f
				select x;
			}
			while (totalDamage > 0 && source.Any<BodyPartRecord>())
			{
				BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);
				int num = Mathf.Max(3, GenMath.RoundRandom(partHealth * Rand.Range(0.5f, 1f)));
				float minHealthOfPartsWeWantToAvoidDestroying = HealthUtility.GetMinHealthOfPartsWeWantToAvoidDestroying(bodyPartRecord, p);
				if (minHealthOfPartsWeWantToAvoidDestroying - (float)num < 1f)
				{
					num = Mathf.RoundToInt(minHealthOfPartsWeWantToAvoidDestroying - 1f);
				}
				if (bodyPartRecord == brain && partHealth - (float)num < brain.def.GetMaxHealth(p) * 0.5f)
				{
					num = Mathf.Max(Mathf.RoundToInt(partHealth - brain.def.GetMaxHealth(p) * 0.5f), 1);
				}
				if (num <= 0)
				{
					break;
				}
				DamageDef damageDef = Rand.Element<DamageDef>(DamageDefOf.Cut, DamageDefOf.Scratch, DamageDefOf.Stab, DamageDefOf.Crush);
				Thing p2 = p;
				DamageDef def = damageDef;
				float amount = (float)num;
				BodyPartRecord hitPart = bodyPartRecord;
				p2.TakeDamage(new DamageInfo(def, amount, 0f, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				totalDamage -= num;
			}
		}

		private static float GetMinHealthOfPartsWeWantToAvoidDestroying(BodyPartRecord part, Pawn pawn)
		{
			float num = 999999f;
			while (part != null)
			{
				if (HealthUtility.ShouldRandomSurgeryInjuriesAvoidDestroying(part, pawn))
				{
					num = Mathf.Min(num, pawn.health.hediffSet.GetPartHealth(part));
				}
				part = part.parent;
			}
			return num;
		}

		private static bool ShouldRandomSurgeryInjuriesAvoidDestroying(BodyPartRecord part, Pawn pawn)
		{
			bool result;
			if (part == pawn.RaceProps.body.corePart)
			{
				result = true;
			}
			else if (part.def.tags.Any((BodyPartTagDef x) => x.vital))
			{
				result = true;
			}
			else
			{
				for (int i = 0; i < part.parts.Count; i++)
				{
					if (HealthUtility.ShouldRandomSurgeryInjuriesAvoidDestroying(part.parts[i], pawn))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		public static void DamageUntilDowned(Pawn p, bool allowBleedingWounds = true)
		{
			if (!p.health.Downed)
			{
				HediffSet hediffSet = p.health.hediffSet;
				p.health.forceIncap = true;
				IEnumerable<BodyPartRecord> source = from x in HealthUtility.HittablePartsViolence(hediffSet)
				where !p.health.hediffSet.hediffs.Any((Hediff y) => y.Part == x && y.CurStage != null && y.CurStage.partEfficiencyOffset < 0f)
				select x;
				int num = 0;
				while (num < 300 && !p.Downed && source.Any<BodyPartRecord>())
				{
					num++;
					BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
					int num2 = Mathf.RoundToInt(hediffSet.GetPartHealth(bodyPartRecord)) - 3;
					if (num2 >= 8)
					{
						DamageDef damageDef;
						if (bodyPartRecord.depth == BodyPartDepth.Outside)
						{
							if (!allowBleedingWounds && bodyPartRecord.def.bleedRate > 0f)
							{
								damageDef = DamageDefOf.Blunt;
							}
							else
							{
								damageDef = HealthUtility.RandomViolenceDamageType();
							}
						}
						else
						{
							damageDef = DamageDefOf.Blunt;
						}
						int num3 = Rand.RangeInclusive(Mathf.RoundToInt((float)num2 * 0.65f), num2);
						HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
						if (!p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num3))
						{
							DamageDef def = damageDef;
							float amount = (float)num3;
							float armorPenetration = 999f;
							BodyPartRecord hitPart = bodyPartRecord;
							DamageInfo dinfo = new DamageInfo(def, amount, armorPenetration, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
							dinfo.SetAllowDamagePropagation(false);
							p.TakeDamage(dinfo);
						}
					}
				}
				if (p.Dead)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(p + " died during GiveInjuriesToForceDowned");
					for (int i = 0; i < p.health.hediffSet.hediffs.Count; i++)
					{
						stringBuilder.AppendLine("   -" + p.health.hediffSet.hediffs[i].ToString());
					}
					Log.Error(stringBuilder.ToString(), false);
				}
				p.health.forceIncap = false;
			}
		}

		public static void DamageUntilDead(Pawn p)
		{
			HediffSet hediffSet = p.health.hediffSet;
			int num = 0;
			while (!p.Dead && num < 200 && HealthUtility.HittablePartsViolence(hediffSet).Any<BodyPartRecord>())
			{
				num++;
				BodyPartRecord bodyPartRecord = HealthUtility.HittablePartsViolence(hediffSet).RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				int num2 = Rand.RangeInclusive(8, 25);
				DamageDef damageDef;
				if (bodyPartRecord.depth == BodyPartDepth.Outside)
				{
					damageDef = HealthUtility.RandomViolenceDamageType();
				}
				else
				{
					damageDef = DamageDefOf.Blunt;
				}
				DamageDef def = damageDef;
				float amount = (float)num2;
				float armorPenetration = 999f;
				BodyPartRecord hitPart = bodyPartRecord;
				DamageInfo dinfo = new DamageInfo(def, amount, armorPenetration, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
				p.TakeDamage(dinfo);
			}
			if (!p.Dead)
			{
				Log.Error(p + " not killed during GiveInjuriesToKill", false);
			}
		}

		public static void DamageLegsUntilIncapableOfMoving(Pawn p, bool allowBleedingWounds = true)
		{
			int num = 0;
			p.health.forceIncap = true;
			while (p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && num < 300)
			{
				num++;
				IEnumerable<BodyPartRecord> source = from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
				where x.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) && p.health.hediffSet.GetPartHealth(x) >= 2f
				select x;
				if (!source.Any<BodyPartRecord>())
				{
					break;
				}
				BodyPartRecord bodyPartRecord = source.RandomElement<BodyPartRecord>();
				float maxHealth = bodyPartRecord.def.GetMaxHealth(p);
				float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);
				int min = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.12f), 1, (int)partHealth - 1);
				int max = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.27f), 1, (int)partHealth - 1);
				int num2 = Rand.RangeInclusive(min, max);
				DamageDef damageDef;
				if (!allowBleedingWounds && bodyPartRecord.def.bleedRate > 0f)
				{
					damageDef = DamageDefOf.Blunt;
				}
				else
				{
					damageDef = HealthUtility.RandomViolenceDamageType();
				}
				HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
				if (p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num2))
				{
					break;
				}
				DamageDef def = damageDef;
				float amount = (float)num2;
				float armorPenetration = 999f;
				BodyPartRecord hitPart = bodyPartRecord;
				DamageInfo dinfo = new DamageInfo(def, amount, armorPenetration, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
				dinfo.SetAllowDamagePropagation(false);
				p.TakeDamage(dinfo);
			}
			p.health.forceIncap = false;
		}

		public static DamageDef RandomViolenceDamageType()
		{
			DamageDef result;
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				result = DamageDefOf.Bullet;
				break;
			case 1:
				result = DamageDefOf.Blunt;
				break;
			case 2:
				result = DamageDefOf.Stab;
				break;
			case 3:
				result = DamageDefOf.Scratch;
				break;
			case 4:
				result = DamageDefOf.Cut;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		public static HediffDef GetHediffDefFromDamage(DamageDef dam, Pawn pawn, BodyPartRecord part)
		{
			HediffDef result = dam.hediff;
			if (part.def.IsSkinCovered(part, pawn.health.hediffSet) && dam.hediffSkin != null)
			{
				result = dam.hediffSkin;
			}
			if (part.def.IsSolid(part, pawn.health.hediffSet.hediffs) && dam.hediffSolid != null)
			{
				result = dam.hediffSolid;
			}
			return result;
		}

		public static bool TryAnesthetize(Pawn pawn)
		{
			bool result;
			if (!pawn.RaceProps.IsFlesh)
			{
				result = false;
			}
			else
			{
				pawn.health.forceIncap = true;
				pawn.health.AddHediff(HediffDefOf.Anesthetic, null, null, null);
				pawn.health.forceIncap = false;
				result = true;
			}
			return result;
		}

		public static void AdjustSeverity(Pawn pawn, HediffDef hdDef, float sevOffset)
		{
			if (sevOffset != 0f)
			{
				Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hdDef, false);
				if (hediff != null)
				{
					hediff.Severity += sevOffset;
				}
				else if (sevOffset > 0f)
				{
					hediff = HediffMaker.MakeHediff(hdDef, pawn, null);
					hediff.Severity = sevOffset;
					pawn.health.AddHediff(hediff, null, null, null);
				}
			}
		}

		public static BodyPartRemovalIntent PartRemovalIntent(Pawn pawn, BodyPartRecord part)
		{
			BodyPartRemovalIntent result;
			if (pawn.health.hediffSet.hediffs.Any((Hediff d) => d.Visible && d.Part == part && d.def.isBad))
			{
				result = BodyPartRemovalIntent.Amputate;
			}
			else
			{
				result = BodyPartRemovalIntent.Harvest;
			}
			return result;
		}

		public static int TicksUntilDeathDueToBloodLoss(Pawn pawn)
		{
			float bleedRateTotal = pawn.health.hediffSet.BleedRateTotal;
			int result;
			if (bleedRateTotal < 0.0001f)
			{
				result = int.MaxValue;
			}
			else
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
				float num = (firstHediffOfDef == null) ? 0f : firstHediffOfDef.Severity;
				result = (int)((1f - num) / bleedRateTotal * 60000f);
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static HealthUtility()
		{
		}

		[CompilerGenerated]
		private static bool <GiveRandomSurgeryInjuries>m__0(BodyPartRecord x)
		{
			return !x.def.conceptual;
		}

		[CompilerGenerated]
		private static bool <GiveRandomSurgeryInjuries>m__1(BodyPartRecord x)
		{
			return !x.def.conceptual;
		}

		[CompilerGenerated]
		private static float <GiveRandomSurgeryInjuries>m__2(BodyPartRecord x)
		{
			return x.coverageAbs;
		}

		[CompilerGenerated]
		private static bool <ShouldRandomSurgeryInjuriesAvoidDestroying>m__3(BodyPartTagDef x)
		{
			return x.vital;
		}

		[CompilerGenerated]
		private static float <DamageUntilDowned>m__4(BodyPartRecord x)
		{
			return x.coverageAbs;
		}

		[CompilerGenerated]
		private static float <DamageUntilDead>m__5(BodyPartRecord x)
		{
			return x.coverageAbs;
		}

		[CompilerGenerated]
		private sealed class <HittablePartsViolence>c__AnonStorey0
		{
			internal HediffSet bodyModel;

			public <HittablePartsViolence>c__AnonStorey0()
			{
			}

			internal bool <>m__0(BodyPartRecord x)
			{
				return x.depth == BodyPartDepth.Outside || (x.depth == BodyPartDepth.Inside && x.def.IsSolid(x, this.bodyModel.hediffs));
			}
		}

		[CompilerGenerated]
		private sealed class <GiveRandomSurgeryInjuries>c__AnonStorey1
		{
			internal BodyPartRecord operatedPart;

			internal Pawn p;

			internal BodyPartRecord brain;

			public <GiveRandomSurgeryInjuries>c__AnonStorey1()
			{
			}

			internal bool <>m__0(BodyPartRecord pa)
			{
				return pa == this.operatedPart || pa.parent == this.operatedPart || (this.operatedPart != null && this.operatedPart.parent == pa);
			}

			internal bool <>m__1(BodyPartRecord x)
			{
				return HealthUtility.GetMinHealthOfPartsWeWantToAvoidDestroying(x, this.p) >= 2f;
			}
		}

		[CompilerGenerated]
		private sealed class <GiveRandomSurgeryInjuries>c__AnonStorey2
		{
			internal float maxBrainHealth;

			internal HealthUtility.<GiveRandomSurgeryInjuries>c__AnonStorey1 <>f__ref$1;

			public <GiveRandomSurgeryInjuries>c__AnonStorey2()
			{
			}

			internal bool <>m__0(BodyPartRecord x)
			{
				return x != this.<>f__ref$1.brain || this.<>f__ref$1.p.health.hediffSet.GetPartHealth(x) >= this.maxBrainHealth * 0.5f + 1f;
			}
		}

		[CompilerGenerated]
		private sealed class <DamageUntilDowned>c__AnonStorey3
		{
			internal Pawn p;

			public <DamageUntilDowned>c__AnonStorey3()
			{
			}

			internal bool <>m__0(BodyPartRecord x)
			{
				return !this.p.health.hediffSet.hediffs.Any((Hediff y) => y.Part == x && y.CurStage != null && y.CurStage.partEfficiencyOffset < 0f);
			}

			private sealed class <DamageUntilDowned>c__AnonStorey4
			{
				internal BodyPartRecord x;

				internal HealthUtility.<DamageUntilDowned>c__AnonStorey3 <>f__ref$3;

				public <DamageUntilDowned>c__AnonStorey4()
				{
				}

				internal bool <>m__0(Hediff y)
				{
					return y.Part == this.x && y.CurStage != null && y.CurStage.partEfficiencyOffset < 0f;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DamageLegsUntilIncapableOfMoving>c__AnonStorey5
		{
			internal Pawn p;

			public <DamageLegsUntilIncapableOfMoving>c__AnonStorey5()
			{
			}

			internal bool <>m__0(BodyPartRecord x)
			{
				return x.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) && this.p.health.hediffSet.GetPartHealth(x) >= 2f;
			}
		}

		[CompilerGenerated]
		private sealed class <PartRemovalIntent>c__AnonStorey6
		{
			internal BodyPartRecord part;

			public <PartRemovalIntent>c__AnonStorey6()
			{
			}

			internal bool <>m__0(Hediff d)
			{
				return d.Visible && d.Part == this.part && d.def.isBad;
			}
		}
	}
}
