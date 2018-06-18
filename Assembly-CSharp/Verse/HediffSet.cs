using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D3B RID: 3387
	public class HediffSet : IExposable
	{
		// Token: 0x06004A7D RID: 19069 RVA: 0x0026CE08 File Offset: 0x0026B208
		public HediffSet(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06004A7E RID: 19070 RVA: 0x0026CE6C File Offset: 0x0026B26C
		public float PainTotal
		{
			get
			{
				if (this.cachedPain < 0f)
				{
					this.cachedPain = this.CalculatePain();
				}
				return this.cachedPain;
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06004A7F RID: 19071 RVA: 0x0026CEA4 File Offset: 0x0026B2A4
		public float BleedRateTotal
		{
			get
			{
				if (this.cachedBleedRate < 0f)
				{
					this.cachedBleedRate = this.CalculateBleedRate();
				}
				return this.cachedBleedRate;
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06004A80 RID: 19072 RVA: 0x0026CEDC File Offset: 0x0026B2DC
		public bool HasHead
		{
			get
			{
				bool? flag = this.cachedHasHead;
				if (flag == null)
				{
					this.cachedHasHead = new bool?(this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Any((BodyPartRecord x) => x.def == BodyPartDefOf.Head));
				}
				return this.cachedHasHead.Value;
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06004A81 RID: 19073 RVA: 0x0026CF48 File Offset: 0x0026B348
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

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06004A82 RID: 19074 RVA: 0x0026CFF4 File Offset: 0x0026B3F4
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

		// Token: 0x06004A83 RID: 19075 RVA: 0x0026D0A0 File Offset: 0x0026B4A0
		public void ExposeData()
		{
			Scribe_Collections.Look<Hediff>(ref this.hediffs, "hediffs", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					this.hediffs[i].pawn = this.pawn;
				}
				for (int j = this.hediffs.Count - 1; j >= 0; j--)
				{
					BackCompatibility.HediffSetResolvingCrossRefs(this.hediffs[j], this.hediffs);
				}
				this.DirtyCache();
			}
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x0026D144 File Offset: 0x0026B544
		public void AddDirect(Hediff hediff, DamageInfo? dinfo = null, DamageWorker.DamageResult damageResult = null)
		{
			if (hediff.def == null)
			{
				Log.Error("Tried to add health diff with null def. Canceling.", false);
			}
			else if (hediff.Part != null && !this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Contains(hediff.Part))
			{
				Log.Error("Tried to add health diff to missing part " + hediff.Part, false);
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
				if (!(hediff is Hediff_MissingPart) && hediff.Part != null && hediff.Part != this.pawn.RaceProps.body.corePart && this.GetPartHealth(hediff.Part) == 0f)
				{
					if (hediff.Part != this.pawn.RaceProps.body.corePart)
					{
						bool flag3 = this.HasDirectlyAddedPartFor(hediff.Part);
						Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
						hediff_MissingPart.IsFresh = !flag3;
						hediff_MissingPart.lastInjury = hediff.def;
						this.pawn.health.AddHediff(hediff_MissingPart, hediff.Part, dinfo, null);
						if (damageResult != null)
						{
							damageResult.AddHediff(hediff_MissingPart);
						}
						if (flag3)
						{
							if (dinfo != null)
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

		// Token: 0x06004A85 RID: 19077 RVA: 0x0026D3E4 File Offset: 0x0026B7E4
		public void DirtyCache()
		{
			this.CacheMissingPartsCommonAncestors();
			this.cachedPain = -1f;
			this.cachedBleedRate = -1f;
			this.cachedHasHead = null;
			this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			this.pawn.health.summaryHealth.Notify_HealthChanged();
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x0026D448 File Offset: 0x0026B848
		public IEnumerable<T> GetHediffs<T>() where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null)
				{
					yield return t;
				}
			}
			yield break;
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x0026D474 File Offset: 0x0026B874
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

		// Token: 0x06004A88 RID: 19080 RVA: 0x0026D4F0 File Offset: 0x0026B8F0
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

		// Token: 0x06004A89 RID: 19081 RVA: 0x0026D558 File Offset: 0x0026B958
		public float GetPartHealth(BodyPartRecord part)
		{
			float result;
			if (part == null)
			{
				result = 0f;
			}
			else
			{
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
				if (num < 0f)
				{
					num = 0f;
				}
				result = (float)Mathf.RoundToInt(num);
			}
			return result;
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x0026D638 File Offset: 0x0026BA38
		public BodyPartRecord GetBrain()
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
			{
				if (bodyPartRecord.def.tags.Contains(BodyPartTagDefOf.ConsciousnessSource))
				{
					return bodyPartRecord;
				}
			}
			return null;
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x0026D6BC File Offset: 0x0026BABC
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

		// Token: 0x06004A8C RID: 19084 RVA: 0x0026D72C File Offset: 0x0026BB2C
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

		// Token: 0x06004A8D RID: 19085 RVA: 0x0026D7B4 File Offset: 0x0026BBB4
		public IEnumerable<Verb> GetHediffsVerbs()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				HediffComp_VerbGiver vg = this.hediffs[i].TryGetComp<HediffComp_VerbGiver>();
				if (vg != null)
				{
					List<Verb> verbList = vg.VerbTracker.AllVerbs;
					for (int j = 0; j < verbList.Count; j++)
					{
						yield return verbList[j];
					}
				}
			}
			yield break;
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x0026D7E0 File Offset: 0x0026BBE0
		public IEnumerable<Hediff> GetHediffsTendable()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].TendableNow(false))
				{
					yield return this.hediffs[i];
				}
			}
			yield break;
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x0026D80C File Offset: 0x0026BC0C
		public bool HasTendableHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!forAlert || this.hediffs[i].def.makesAlert)
				{
					if (this.hediffs[i].TendableNow(false))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x0026D888 File Offset: 0x0026BC88
		public IEnumerable<Hediff_Injury> GetInjuriesTendable()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury inj = this.hediffs[i] as Hediff_Injury;
				if (inj != null && inj.TendableNow(false))
				{
					yield return inj;
				}
			}
			yield break;
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x0026D8B4 File Offset: 0x0026BCB4
		public bool HasTendableInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x0026D914 File Offset: 0x0026BD14
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

		// Token: 0x06004A93 RID: 19091 RVA: 0x0026D974 File Offset: 0x0026BD74
		public bool HasTendedAndHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealFromTending() && hediff_Injury.Severity > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x0026D9E4 File Offset: 0x0026BDE4
		public bool HasTemperatureInjury(TemperatureInjuryStage minStage)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == HediffDefOf.Hypothermia || this.hediffs[i].def == HediffDefOf.Heatstroke)
				{
					if (this.hediffs[i].CurStageIndex >= (int)minStage)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x0026DA70 File Offset: 0x0026BE70
		public IEnumerable<BodyPartRecord> GetInjuredParts()
		{
			return (from x in this.hediffs
			where x is Hediff_Injury
			select x.Part).Distinct<BodyPartRecord>();
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x0026DAD4 File Offset: 0x0026BED4
		public IEnumerable<BodyPartRecord> GetNaturallyHealingInjuredParts()
		{
			foreach (BodyPartRecord part in this.GetInjuredParts())
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					Hediff_Injury injury = this.hediffs[i] as Hediff_Injury;
					if (injury != null && this.hediffs[i].Part == part && injury.CanHealNaturally())
					{
						yield return part;
						break;
					}
				}
			}
			yield break;
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x0026DB00 File Offset: 0x0026BF00
		public List<Hediff_MissingPart> GetMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.CacheMissingPartsCommonAncestors();
			}
			return this.cachedMissingPartsCommonAncestors;
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x0026DB2C File Offset: 0x0026BF2C
		public IEnumerable<BodyPartRecord> GetNotMissingParts(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartTagDef tag = null)
		{
			List<BodyPartRecord> allPartsList = this.pawn.def.race.body.AllParts;
			for (int i = 0; i < allPartsList.Count; i++)
			{
				BodyPartRecord part = allPartsList[i];
				if (!this.PartIsMissing(part))
				{
					if (height == BodyPartHeight.Undefined || part.height == height)
					{
						if (depth == BodyPartDepth.Undefined || part.depth == depth)
						{
							if (tag == null || part.def.tags.Contains(tag))
							{
								yield return part;
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x0026DB6C File Offset: 0x0026BF6C
		public BodyPartRecord GetRandomNotMissingPart(DamageDef damDef, BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			IEnumerable<BodyPartRecord> notMissingParts;
			if (this.GetNotMissingParts(height, depth, null).Any<BodyPartRecord>())
			{
				notMissingParts = this.GetNotMissingParts(height, depth, null);
			}
			else
			{
				if (!this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null).Any<BodyPartRecord>())
				{
					return null;
				}
				notMissingParts = this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null);
			}
			BodyPartRecord bodyPartRecord;
			BodyPartRecord result;
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs * x.def.GetHitChanceFactorFor(damDef), out bodyPartRecord))
			{
				result = bodyPartRecord;
			}
			else
			{
				if (!notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs, out bodyPartRecord))
				{
					throw new InvalidOperationException();
				}
				result = bodyPartRecord;
			}
			return result;
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x0026DC30 File Offset: 0x0026C030
		public float GetCoverageOfNotMissingNaturalParts(BodyPartRecord part)
		{
			float result;
			if (this.PartIsMissing(part))
			{
				result = 0f;
			}
			else if (this.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				result = 0f;
			}
			else
			{
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
				while (this.coveragePartsStack.Any<BodyPartRecord>())
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
				result = num;
			}
			return result;
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x0026DDC8 File Offset: 0x0026C1C8
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
					select x).FirstOrDefault<Hediff_MissingPart>();
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

		// Token: 0x06004A9C RID: 19100 RVA: 0x0026DEE8 File Offset: 0x0026C2E8
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

		// Token: 0x06004A9D RID: 19101 RVA: 0x0026DF50 File Offset: 0x0026C350
		public bool PartOrAnyAncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return this.HasDirectlyAddedPartFor(part) || (part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent));
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x0026DF9C File Offset: 0x0026C39C
		public bool AncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent);
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x0026DFD8 File Offset: 0x0026C3D8
		public IEnumerable<Hediff> GetTendableNonInjuryNonMissingPartHediffs()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!(this.hediffs[i] is Hediff_Injury))
				{
					if (!(this.hediffs[i] is Hediff_MissingPart))
					{
						if (this.hediffs[i].TendableNow(false))
						{
							yield return this.hediffs[i];
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x0026E004 File Offset: 0x0026C404
		public bool HasTendableNonInjuryNonMissingPartHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!forAlert || this.hediffs[i].def.makesAlert)
				{
					if (!(this.hediffs[i] is Hediff_Injury))
					{
						if (!(this.hediffs[i] is Hediff_MissingPart))
						{
							if (this.hediffs[i].TendableNow(false))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x0026E0B8 File Offset: 0x0026C4B8
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

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06004AA2 RID: 19106 RVA: 0x0026E184 File Offset: 0x0026C584
		public bool AnyHediffMakesSickThought
		{
			get
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					if (this.hediffs[i].def.makesSickThought)
					{
						if (this.hediffs[i].Visible)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x0026E1FC File Offset: 0x0026C5FC
		private float CalculateBleedRate()
		{
			float result;
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.health.Dead)
			{
				result = 0f;
			}
			else
			{
				float num = 0f;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					num += this.hediffs[i].BleedRate;
				}
				float num2 = num / this.pawn.HealthScale;
				result = num2;
			}
			return result;
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x0026E290 File Offset: 0x0026C690
		private float CalculatePain()
		{
			float result;
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.Dead)
			{
				result = 0f;
			}
			else
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
				result = Mathf.Clamp(num2, 0f, 1f);
			}
			return result;
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x0026E362 File Offset: 0x0026C762
		public void Clear()
		{
			this.hediffs.Clear();
			this.DirtyCache();
		}

		// Token: 0x04003250 RID: 12880
		public Pawn pawn;

		// Token: 0x04003251 RID: 12881
		public List<Hediff> hediffs = new List<Hediff>();

		// Token: 0x04003252 RID: 12882
		private List<Hediff_MissingPart> cachedMissingPartsCommonAncestors = null;

		// Token: 0x04003253 RID: 12883
		private float cachedPain = -1f;

		// Token: 0x04003254 RID: 12884
		private float cachedBleedRate = -1f;

		// Token: 0x04003255 RID: 12885
		private bool? cachedHasHead;

		// Token: 0x04003256 RID: 12886
		private Stack<BodyPartRecord> coveragePartsStack = new Stack<BodyPartRecord>();

		// Token: 0x04003257 RID: 12887
		private HashSet<BodyPartRecord> coverageRejectedPartsSet = new HashSet<BodyPartRecord>();

		// Token: 0x04003258 RID: 12888
		private Queue<BodyPartRecord> missingPartsCommonAncestorsQueue = new Queue<BodyPartRecord>();
	}
}
