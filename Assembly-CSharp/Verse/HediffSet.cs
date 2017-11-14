using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class HediffSet : IExposable
	{
		public Pawn pawn;

		public List<Hediff> hediffs = new List<Hediff>();

		private List<Hediff_MissingPart> cachedMissingPartsCommonAncestors;

		private float cachedPain = -1f;

		private float cachedBleedRate = -1f;

		private bool? cachedHasHead;

		private Stack<BodyPartRecord> coveragePartsStack = new Stack<BodyPartRecord>();

		private HashSet<BodyPartRecord> coverageRejectedPartsSet = new HashSet<BodyPartRecord>();

		private Queue<BodyPartRecord> missingPartsCommonAncestorsQueue = new Queue<BodyPartRecord>();

		public float PainTotal
		{
			get
			{
				if (this.cachedPain < 0.0)
				{
					this.cachedPain = this.CalculatePain();
				}
				return this.cachedPain;
			}
		}

		public float BleedRateTotal
		{
			get
			{
				if (this.cachedBleedRate < 0.0)
				{
					this.cachedBleedRate = this.CalculateBleedRate();
				}
				return this.cachedBleedRate;
			}
		}

		public bool HasHead
		{
			get
			{
				bool? nullable = this.cachedHasHead;
				if (!nullable.HasValue)
				{
					this.cachedHasHead = this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).Any((BodyPartRecord x) => x.def == BodyPartDefOf.Head);
				}
				return this.cachedHasHead.Value;
			}
		}

		public float HungerRateFactor
		{
			get
			{
				float num = 1f;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.hungerRateFactor;
					}
				}
				for (int j = 0; j < this.hediffs.Count; j++)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.hungerRateFactorOffset;
					}
				}
				return Mathf.Max(num, 0f);
			}
		}

		public float RestFallFactor
		{
			get
			{
				float num = 1f;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.restFallFactor;
					}
				}
				for (int j = 0; j < this.hediffs.Count; j++)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.restFallFactorOffset;
					}
				}
				return Mathf.Max(num, 0f);
			}
		}

		public bool AnyHediffMakesSickThought
		{
			get
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					if (this.hediffs[i].def.makesSickThought && this.hediffs[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		public HediffSet(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Hediff>(ref this.hediffs, "hediffs", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					this.hediffs[i].pawn = this.pawn;
				}
				this.DirtyCache();
			}
		}

		public void AddDirect(Hediff hediff, DamageInfo? dinfo = default(DamageInfo?))
		{
			if (hediff.def == null)
			{
				Log.Error("Tried to add health diff with null def. Canceling.");
			}
			else if (hediff.Part != null && !this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).Contains(hediff.Part))
			{
				Log.Error("Tried to add health diff to missing part " + hediff.Part);
			}
			else
			{
				hediff.ageTicks = 0;
				hediff.pawn = this.pawn;
				bool flag = false;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					if (this.hediffs[i].TryMergeWith(hediff))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					this.hediffs.Add(hediff);
					hediff.PostAdd(dinfo);
					if (this.pawn.needs != null && this.pawn.needs.mood != null)
					{
						this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
					}
				}
				bool flag2 = hediff is Hediff_MissingPart;
				if (!(hediff is Hediff_MissingPart) && hediff.Part != null && hediff.Part != this.pawn.RaceProps.body.corePart && this.GetPartHealth(hediff.Part) == 0.0 && hediff.Part != this.pawn.RaceProps.body.corePart)
				{
					bool flag3 = this.HasDirectlyAddedPartFor(hediff.Part);
					Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
					hediff_MissingPart.IsFresh = !flag3;
					hediff_MissingPart.lastInjury = hediff.def;
					this.pawn.health.AddHediff(hediff_MissingPart, hediff.Part, dinfo);
					if (flag3)
					{
						if (dinfo.HasValue)
						{
							hediff_MissingPart.lastInjury = HealthUtility.GetHediffDefFromDamage(dinfo.Value.Def, this.pawn, hediff.Part);
						}
						else
						{
							hediff_MissingPart.lastInjury = null;
						}
					}
					flag2 = true;
				}
				this.DirtyCache();
				if (flag2 && this.pawn.apparel != null)
				{
					this.pawn.apparel.Notify_LostBodyPart();
				}
				if (hediff.def.causesNeed != null && !this.pawn.Dead)
				{
					this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
				}
			}
		}

		public void DirtyCache()
		{
			this.CacheMissingPartsCommonAncestors();
			this.cachedPain = -1f;
			this.cachedBleedRate = -1f;
			this.cachedHasHead = null;
			this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			this.pawn.health.summaryHealth.Notify_HealthChanged();
		}

		public IEnumerable<T> GetHediffs<T>() where T : Hediff
		{
			int i = 0;
			T t;
			while (true)
			{
				if (i < this.hediffs.Count)
				{
					t = (T)(this.hediffs[i] as T);
					if (t == null)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return t;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public Hediff GetFirstHediffOfDef(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return this.hediffs[i];
				}
			}
			return null;
		}

		public bool PartIsMissing(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_MissingPart)
				{
					return true;
				}
			}
			return false;
		}

		public float GetPartHealth(BodyPartRecord part)
		{
			if (part == null)
			{
				return 0f;
			}
			float num = part.def.GetMaxHealth(this.pawn);
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i] is Hediff_MissingPart && this.hediffs[i].Part == part)
				{
					return 0f;
				}
				if (this.hediffs[i].Part == part)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null)
					{
						num -= hediff_Injury.Severity;
					}
				}
			}
			if (num < 0.0)
			{
				num = 0f;
			}
			return (float)Mathf.RoundToInt(num);
		}

		public BodyPartRecord GetBrain()
		{
			foreach (BodyPartRecord notMissingPart in this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined))
			{
				if (notMissingPart.def.tags.Contains("ConsciousnessSource"))
				{
					return notMissingPart;
				}
			}
			return null;
		}

		public bool HasHediff(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		public bool HasHediff(HediffDef def, BodyPartRecord bodyPart, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && this.hediffs[i].Part == bodyPart && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<Verb> GetHediffsVerbs()
		{
			int j = 0;
			List<Verb> verbList;
			int i;
			while (true)
			{
				if (j < this.hediffs.Count)
				{
					HediffComp_VerbGiver vg = this.hediffs[j].TryGetComp<HediffComp_VerbGiver>();
					if (vg != null)
					{
						verbList = vg.VerbTracker.AllVerbs;
						i = 0;
						if (i < verbList.Count)
							break;
					}
					j++;
					continue;
				}
				yield break;
			}
			yield return verbList[i];
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public IEnumerable<Hediff> GetHediffsTendable()
		{
			int i = 0;
			while (true)
			{
				if (i < this.hediffs.Count)
				{
					if (!this.hediffs[i].TendableNow)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return this.hediffs[i];
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public bool HasTendableHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && this.hediffs[i].TendableNow)
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<Hediff_Injury> GetInjuriesTendable()
		{
			int i = 0;
			Hediff_Injury inj;
			while (true)
			{
				if (i < this.hediffs.Count)
				{
					inj = (this.hediffs[i] as Hediff_Injury);
					if (inj != null && inj.TendableNow)
						break;
					i++;
					continue;
				}
				yield break;
			}
			yield return inj;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public bool HasTendableInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.TendableNow)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasNaturallyHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealNaturally())
				{
					return true;
				}
			}
			return false;
		}

		public bool HasTendedAndHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealFromTending() && hediff_Injury.Severity > 0.0)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasTemperatureInjury(TemperatureInjuryStage minStage)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((this.hediffs[i].def == HediffDefOf.Hypothermia || this.hediffs[i].def == HediffDefOf.Heatstroke) && this.hediffs[i].CurStageIndex >= (int)minStage)
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<BodyPartRecord> GetInjuredParts()
		{
			return (from x in this.hediffs
			where x is Hediff_Injury
			select x.Part).Distinct();
		}

		public IEnumerable<BodyPartRecord> GetNaturallyHealingInjuredParts()
		{
			foreach (BodyPartRecord injuredPart in this.GetInjuredParts())
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					Hediff_Injury injury = this.hediffs[i] as Hediff_Injury;
					if (injury != null && this.hediffs[i].Part == injuredPart && injury.CanHealNaturally())
					{
						yield return injuredPart;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_0155:
			/*Error near IL_0156: Unexpected return in MoveNext()*/;
		}

		public List<Hediff_MissingPart> GetMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.CacheMissingPartsCommonAncestors();
			}
			return this.cachedMissingPartsCommonAncestors;
		}

		public IEnumerable<BodyPartRecord> GetNotMissingParts(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			List<BodyPartRecord> allPartsList = this.pawn.def.race.body.AllParts;
			int i = 0;
			BodyPartRecord part;
			while (true)
			{
				if (i < allPartsList.Count)
				{
					part = allPartsList[i];
					if (!this.PartIsMissing(part) && (height == BodyPartHeight.Undefined || part.height == height))
					{
						if (depth == BodyPartDepth.Undefined)
							break;
						if (part.depth == depth)
							break;
					}
					i++;
					continue;
				}
				yield break;
			}
			yield return part;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public BodyPartRecord GetRandomNotMissingPart(DamageDef damDef, BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			IEnumerable<BodyPartRecord> enumerable = null;
			if (this.GetNotMissingParts(height, depth).Any())
			{
				enumerable = this.GetNotMissingParts(height, depth);
				goto IL_0053;
			}
			if (this.GetNotMissingParts(BodyPartHeight.Undefined, depth).Any())
			{
				enumerable = this.GetNotMissingParts(BodyPartHeight.Undefined, depth);
				goto IL_0053;
			}
			return null;
			IL_0053:
			BodyPartRecord result = default(BodyPartRecord);
			if (enumerable.TryRandomElementByWeight<BodyPartRecord>((Func<BodyPartRecord, float>)((BodyPartRecord x) => x.coverageAbs * x.def.GetHitChanceFactorFor(damDef)), out result))
			{
				return result;
			}
			if (enumerable.TryRandomElementByWeight<BodyPartRecord>((Func<BodyPartRecord, float>)((BodyPartRecord x) => x.coverageAbs), out result))
			{
				return result;
			}
			throw new InvalidOperationException();
		}

		public float GetCoverageOfNotMissingNaturalParts(BodyPartRecord part)
		{
			if (this.PartIsMissing(part))
			{
				return 0f;
			}
			if (this.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				return 0f;
			}
			this.coverageRejectedPartsSet.Clear();
			List<Hediff_MissingPart> missingPartsCommonAncestors = this.GetMissingPartsCommonAncestors();
			for (int i = 0; i < missingPartsCommonAncestors.Count; i++)
			{
				this.coverageRejectedPartsSet.Add(missingPartsCommonAncestors[i].Part);
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j] is Hediff_AddedPart)
				{
					this.coverageRejectedPartsSet.Add(this.hediffs[j].Part);
				}
			}
			float num = 0f;
			this.coveragePartsStack.Clear();
			this.coveragePartsStack.Push(part);
			while (this.coveragePartsStack.Any())
			{
				BodyPartRecord bodyPartRecord = this.coveragePartsStack.Pop();
				num += bodyPartRecord.coverageAbs;
				for (int k = 0; k < bodyPartRecord.parts.Count; k++)
				{
					if (!this.coverageRejectedPartsSet.Contains(bodyPartRecord.parts[k]))
					{
						this.coveragePartsStack.Push(bodyPartRecord.parts[k]);
					}
				}
			}
			this.coveragePartsStack.Clear();
			this.coverageRejectedPartsSet.Clear();
			return num;
		}

		private void CacheMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.cachedMissingPartsCommonAncestors = new List<Hediff_MissingPart>();
			}
			else
			{
				this.cachedMissingPartsCommonAncestors.Clear();
			}
			this.missingPartsCommonAncestorsQueue.Clear();
			this.missingPartsCommonAncestorsQueue.Enqueue(this.pawn.def.race.body.corePart);
			while (this.missingPartsCommonAncestorsQueue.Count != 0)
			{
				BodyPartRecord node = this.missingPartsCommonAncestorsQueue.Dequeue();
				if (!this.PartOrAnyAncestorHasDirectlyAddedParts(node))
				{
					Hediff_MissingPart hediff_MissingPart = (from x in this.GetHediffs<Hediff_MissingPart>()
					where x.Part == node
					select x).FirstOrDefault();
					if (hediff_MissingPart != null)
					{
						this.cachedMissingPartsCommonAncestors.Add(hediff_MissingPart);
					}
					else
					{
						for (int i = 0; i < node.parts.Count; i++)
						{
							this.missingPartsCommonAncestorsQueue.Enqueue(node.parts[i]);
						}
					}
				}
			}
		}

		public bool HasDirectlyAddedPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_AddedPart)
				{
					return true;
				}
			}
			return false;
		}

		public bool PartOrAnyAncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			if (this.HasDirectlyAddedPartFor(part))
			{
				return true;
			}
			if (part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent))
			{
				return true;
			}
			return false;
		}

		public bool AncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			if (part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent))
			{
				return true;
			}
			return false;
		}

		public IEnumerable<Hediff> GetTendableNonInjuryNonMissingPartHediffs()
		{
			int i = 0;
			while (true)
			{
				if (i < this.hediffs.Count)
				{
					if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow)
						break;
					i++;
					continue;
				}
				yield break;
			}
			yield return this.hediffs[i];
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public bool HasTendableNonInjuryNonMissingPartHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && !(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasTendedImmunizableNotImmuneHediff()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].Visible && this.hediffs[i].IsTended() && this.hediffs[i].def.PossibleToDevelopImmunityNaturally() && !this.hediffs[i].FullyImmune())
				{
					return true;
				}
			}
			return false;
		}

		private float CalculateBleedRate()
		{
			if (this.pawn.RaceProps.IsFlesh && !this.pawn.health.Dead)
			{
				float num = 0f;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					num += this.hediffs[i].BleedRate;
				}
				return num / this.pawn.HealthScale;
			}
			return 0f;
		}

		private float CalculatePain()
		{
			if (this.pawn.RaceProps.IsFlesh && !this.pawn.Dead)
			{
				float num = 0f;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					num += this.hediffs[i].PainOffset;
				}
				float num2 = num / this.pawn.HealthScale;
				for (int j = 0; j < this.hediffs.Count; j++)
				{
					num2 *= this.hediffs[j].PainFactor;
				}
				return Mathf.Clamp(num2, 0f, 1f);
			}
			return 0f;
		}

		public void Clear()
		{
			this.hediffs.Clear();
			this.DirtyCache();
		}
	}
}
