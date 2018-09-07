using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
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

		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Hediff, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Hediff, BodyPartRecord> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<BodyPartRecord, float> <>f__am$cache3;

		public HediffSet(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

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

		public bool HasHead
		{
			get
			{
				bool? flag = this.cachedHasHead;
				if (flag == null)
				{
					this.cachedHasHead = new bool?(this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x.def == BodyPartDefOf.Head));
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

		public void AddDirect(Hediff hediff, DamageInfo? dinfo = null, DamageWorker.DamageResult damageResult = null)
		{
			if (hediff.def == null)
			{
				Log.Error("Tried to add health diff with null def. Canceling.", false);
				return;
			}
			if (hediff.Part != null && !this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff.Part))
			{
				Log.Error("Tried to add health diff to missing part " + hediff.Part, false);
				return;
			}
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
			if (!(hediff is Hediff_MissingPart) && hediff.Part != null && hediff.Part != this.pawn.RaceProps.body.corePart && this.GetPartHealth(hediff.Part) == 0f && hediff.Part != this.pawn.RaceProps.body.corePart)
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
			num = Mathf.Max(num, 0f);
			if (!part.def.destroyableByDamage)
			{
				num = Mathf.Max(num, 1f);
			}
			return (float)Mathf.RoundToInt(num);
		}

		public BodyPartRecord GetBrain()
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				if (bodyPartRecord.def.tags.Contains(BodyPartTagDefOf.ConsciousnessSource))
				{
					return bodyPartRecord;
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
				if (hediff_Injury != null && hediff_Injury.CanHealFromTending() && hediff_Injury.Severity > 0f)
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
			select x.Part).Distinct<BodyPartRecord>();
		}

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

		public List<Hediff_MissingPart> GetMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.CacheMissingPartsCommonAncestors();
			}
			return this.cachedMissingPartsCommonAncestors;
		}

		public IEnumerable<BodyPartRecord> GetNotMissingParts(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartTagDef tag = null, BodyPartRecord partParent = null)
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
								if (partParent == null || part.parent == partParent)
								{
									yield return part;
								}
							}
						}
					}
				}
			}
			yield break;
		}

		public BodyPartRecord GetRandomNotMissingPart(DamageDef damDef, BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartRecord partParent = null)
		{
			IEnumerable<BodyPartRecord> notMissingParts;
			if (this.GetNotMissingParts(height, depth, null, partParent).Any<BodyPartRecord>())
			{
				notMissingParts = this.GetNotMissingParts(height, depth, null, partParent);
			}
			else
			{
				if (!this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent).Any<BodyPartRecord>())
				{
					return null;
				}
				BodyPartHeight height2 = BodyPartHeight.Undefined;
				notMissingParts = this.GetNotMissingParts(height2, depth, null, partParent);
			}
			BodyPartRecord result;
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs * x.def.GetHitChanceFactorFor(damDef), out result))
			{
				return result;
			}
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs, out result))
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
			return this.HasDirectlyAddedPartFor(part) || (part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent));
		}

		public bool AncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent);
		}

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

		public bool HasImmunizableNotImmuneHediff()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].Visible && this.hediffs[i].def.PossibleToDevelopImmunityNaturally() && !this.hediffs[i].FullyImmune())
				{
					return true;
				}
			}
			return false;
		}

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

		private float CalculateBleedRate()
		{
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.health.Dead)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				num += this.hediffs[i].BleedRate;
			}
			return num / this.pawn.HealthScale;
		}

		private float CalculatePain()
		{
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.Dead)
			{
				return 0f;
			}
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

		public void Clear()
		{
			this.hediffs.Clear();
			this.DirtyCache();
		}

		[CompilerGenerated]
		private static bool <get_HasHead>m__0(BodyPartRecord x)
		{
			return x.def == BodyPartDefOf.Head;
		}

		[CompilerGenerated]
		private static bool <GetInjuredParts>m__1(Hediff x)
		{
			return x is Hediff_Injury;
		}

		[CompilerGenerated]
		private static BodyPartRecord <GetInjuredParts>m__2(Hediff x)
		{
			return x.Part;
		}

		[CompilerGenerated]
		private static float <GetRandomNotMissingPart>m__3(BodyPartRecord x)
		{
			return x.coverageAbs;
		}

		[CompilerGenerated]
		private sealed class <GetHediffs>c__Iterator0<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T> where T : Hediff
		{
			internal int <i>__1;

			internal T <t>__2;

			internal HediffSet $this;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetHediffs>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					IL_83:
					i++;
					break;
				default:
					return false;
				}
				if (i >= this.hediffs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					t = (this.hediffs[i] as T);
					if (t != null)
					{
						this.$current = t;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_83;
				}
				return false;
			}

			T IEnumerator<T>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HediffSet.<GetHediffs>c__Iterator0<T> <GetHediffs>c__Iterator = new HediffSet.<GetHediffs>c__Iterator0<T>();
				<GetHediffs>c__Iterator.$this = this;
				return <GetHediffs>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetHediffsVerbs>c__Iterator1 : IEnumerable, IEnumerable<Verb>, IEnumerator, IDisposable, IEnumerator<Verb>
		{
			internal int <i>__1;

			internal HediffComp_VerbGiver <vg>__2;

			internal List<Verb> <verbList>__3;

			internal int <j>__4;

			internal HediffSet $this;

			internal Verb $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetHediffsVerbs>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					goto IL_D8;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_B4:
				if (j < verbList.Count)
				{
					this.$current = verbList[j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_CA:
				i++;
				IL_D8:
				if (i >= this.hediffs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					vg = this.hediffs[i].TryGetComp<HediffComp_VerbGiver>();
					if (vg != null)
					{
						verbList = vg.VerbTracker.AllVerbs;
						j = 0;
						goto IL_B4;
					}
					goto IL_CA;
				}
				return false;
			}

			Verb IEnumerator<Verb>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Verb>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Verb> IEnumerable<Verb>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HediffSet.<GetHediffsVerbs>c__Iterator1 <GetHediffsVerbs>c__Iterator = new HediffSet.<GetHediffsVerbs>c__Iterator1();
				<GetHediffsVerbs>c__Iterator.$this = this;
				return <GetHediffsVerbs>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetHediffsTendable>c__Iterator2 : IEnumerable, IEnumerable<Hediff>, IEnumerator, IDisposable, IEnumerator<Hediff>
		{
			internal int <i>__1;

			internal HediffSet $this;

			internal Hediff $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetHediffsTendable>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					IL_7E:
					i++;
					break;
				default:
					return false;
				}
				if (i >= this.hediffs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (this.hediffs[i].TendableNow(false))
					{
						this.$current = this.hediffs[i];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_7E;
				}
				return false;
			}

			Hediff IEnumerator<Hediff>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Hediff>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Hediff> IEnumerable<Hediff>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HediffSet.<GetHediffsTendable>c__Iterator2 <GetHediffsTendable>c__Iterator = new HediffSet.<GetHediffsTendable>c__Iterator2();
				<GetHediffsTendable>c__Iterator.$this = this;
				return <GetHediffsTendable>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetInjuriesTendable>c__Iterator3 : IEnumerable, IEnumerable<Hediff_Injury>, IEnumerator, IDisposable, IEnumerator<Hediff_Injury>
		{
			internal int <i>__1;

			internal Hediff_Injury <inj>__2;

			internal HediffSet $this;

			internal Hediff_Injury $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetInjuriesTendable>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					IL_8A:
					i++;
					break;
				default:
					return false;
				}
				if (i >= this.hediffs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					inj = (this.hediffs[i] as Hediff_Injury);
					if (inj != null && inj.TendableNow(false))
					{
						this.$current = inj;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_8A;
				}
				return false;
			}

			Hediff_Injury IEnumerator<Hediff_Injury>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Hediff_Injury>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Hediff_Injury> IEnumerable<Hediff_Injury>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HediffSet.<GetInjuriesTendable>c__Iterator3 <GetInjuriesTendable>c__Iterator = new HediffSet.<GetInjuriesTendable>c__Iterator3();
				<GetInjuriesTendable>c__Iterator.$this = this;
				return <GetInjuriesTendable>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetNaturallyHealingInjuredParts>c__Iterator4 : IEnumerable, IEnumerable<BodyPartRecord>, IEnumerator, IDisposable, IEnumerator<BodyPartRecord>
		{
			internal IEnumerator<BodyPartRecord> $locvar0;

			internal BodyPartRecord <part>__1;

			internal int <i>__2;

			internal Hediff_Injury <injury>__3;

			internal HediffSet $this;

			internal BodyPartRecord $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetNaturallyHealingInjuredParts>c__Iterator4()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.GetInjuredParts().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						part = enumerator.Current;
						for (i = 0; i < this.hediffs.Count; i++)
						{
							injury = (this.hediffs[i] as Hediff_Injury);
							if (injury != null && this.hediffs[i].Part == part && injury.CanHealNaturally())
							{
								this.$current = part;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
						}
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			BodyPartRecord IEnumerator<BodyPartRecord>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.BodyPartRecord>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HediffSet.<GetNaturallyHealingInjuredParts>c__Iterator4 <GetNaturallyHealingInjuredParts>c__Iterator = new HediffSet.<GetNaturallyHealingInjuredParts>c__Iterator4();
				<GetNaturallyHealingInjuredParts>c__Iterator.$this = this;
				return <GetNaturallyHealingInjuredParts>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetNotMissingParts>c__Iterator5 : IEnumerable, IEnumerable<BodyPartRecord>, IEnumerator, IDisposable, IEnumerator<BodyPartRecord>
		{
			internal List<BodyPartRecord> <allPartsList>__0;

			internal int <i>__1;

			internal BodyPartRecord <part>__2;

			internal BodyPartHeight height;

			internal BodyPartDepth depth;

			internal BodyPartTagDef tag;

			internal BodyPartRecord partParent;

			internal HediffSet $this;

			internal BodyPartRecord $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetNotMissingParts>c__Iterator5()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					allPartsList = this.pawn.def.race.body.AllParts;
					i = 0;
					break;
				case 1u:
					IL_146:
					i++;
					break;
				default:
					return false;
				}
				if (i >= allPartsList.Count)
				{
					this.$PC = -1;
				}
				else
				{
					part = allPartsList[i];
					if (base.PartIsMissing(part))
					{
						goto IL_146;
					}
					if (height != BodyPartHeight.Undefined && part.height != height)
					{
						goto IL_146;
					}
					if (depth != BodyPartDepth.Undefined && part.depth != depth)
					{
						goto IL_146;
					}
					if (tag != null && !part.def.tags.Contains(tag))
					{
						goto IL_146;
					}
					if (partParent != null && part.parent != partParent)
					{
						goto IL_146;
					}
					this.$current = part;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				return false;
			}

			BodyPartRecord IEnumerator<BodyPartRecord>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.BodyPartRecord>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HediffSet.<GetNotMissingParts>c__Iterator5 <GetNotMissingParts>c__Iterator = new HediffSet.<GetNotMissingParts>c__Iterator5();
				<GetNotMissingParts>c__Iterator.$this = this;
				<GetNotMissingParts>c__Iterator.height = height;
				<GetNotMissingParts>c__Iterator.depth = depth;
				<GetNotMissingParts>c__Iterator.tag = tag;
				<GetNotMissingParts>c__Iterator.partParent = partParent;
				return <GetNotMissingParts>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetRandomNotMissingPart>c__AnonStorey7
		{
			internal DamageDef damDef;

			public <GetRandomNotMissingPart>c__AnonStorey7()
			{
			}

			internal float <>m__0(BodyPartRecord x)
			{
				return x.coverageAbs * x.def.GetHitChanceFactorFor(this.damDef);
			}
		}

		[CompilerGenerated]
		private sealed class <CacheMissingPartsCommonAncestors>c__AnonStorey8
		{
			internal BodyPartRecord node;

			public <CacheMissingPartsCommonAncestors>c__AnonStorey8()
			{
			}

			internal bool <>m__0(Hediff_MissingPart x)
			{
				return x.Part == this.node;
			}
		}

		[CompilerGenerated]
		private sealed class <GetTendableNonInjuryNonMissingPartHediffs>c__Iterator6 : IEnumerable, IEnumerable<Hediff>, IEnumerator, IDisposable, IEnumerator<Hediff>
		{
			internal int <i>__1;

			internal HediffSet $this;

			internal Hediff $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetTendableNonInjuryNonMissingPartHediffs>c__Iterator6()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					IL_CD:
					i++;
					break;
				default:
					return false;
				}
				if (i >= this.hediffs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (this.hediffs[i] is Hediff_Injury)
					{
						goto IL_CD;
					}
					if (this.hediffs[i] is Hediff_MissingPart)
					{
						goto IL_CD;
					}
					if (!this.hediffs[i].TendableNow(false))
					{
						goto IL_CD;
					}
					this.$current = this.hediffs[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				return false;
			}

			Hediff IEnumerator<Hediff>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Hediff>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Hediff> IEnumerable<Hediff>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HediffSet.<GetTendableNonInjuryNonMissingPartHediffs>c__Iterator6 <GetTendableNonInjuryNonMissingPartHediffs>c__Iterator = new HediffSet.<GetTendableNonInjuryNonMissingPartHediffs>c__Iterator6();
				<GetTendableNonInjuryNonMissingPartHediffs>c__Iterator.$this = this;
				return <GetTendableNonInjuryNonMissingPartHediffs>c__Iterator;
			}
		}
	}
}
