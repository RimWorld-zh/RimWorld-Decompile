using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class Trait : IExposable
	{
		public TraitDef def;

		private int degree;

		private bool scenForced;

		public Trait()
		{
		}

		public Trait(TraitDef def, int degree = 0, bool forced = false)
		{
			this.def = def;
			this.degree = degree;
			this.scenForced = forced;
		}

		public int Degree
		{
			get
			{
				return this.degree;
			}
		}

		public TraitDegreeData CurrentData
		{
			get
			{
				return this.def.DataAtDegree(this.degree);
			}
		}

		public string Label
		{
			get
			{
				return this.CurrentData.label;
			}
		}

		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		public bool ScenForced
		{
			get
			{
				return this.scenForced;
			}
		}

		public void ExposeData()
		{
			Scribe_Defs.Look<TraitDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
			Scribe_Values.Look<bool>(ref this.scenForced, "scenForced", false, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == null)
			{
				this.def = DefDatabase<TraitDef>.GetRandom();
				this.degree = PawnGenerator.RandomTraitDegree(this.def);
			}
		}

		public float OffsetOfStat(StatDef stat)
		{
			float num = 0f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statOffsets != null)
			{
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					if (currentData.statOffsets[i].stat == stat)
					{
						num += currentData.statOffsets[i].value;
					}
				}
			}
			return num;
		}

		public float MultiplierOfStat(StatDef stat)
		{
			float num = 1f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statFactors != null)
			{
				for (int i = 0; i < currentData.statFactors.Count; i++)
				{
					if (currentData.statFactors[i].stat == stat)
					{
						num *= currentData.statFactors[i].value;
					}
				}
			}
			return num;
		}

		public string TipString(Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TraitDegreeData currentData = this.CurrentData;
			stringBuilder.Append(currentData.description.AdjustedFor(pawn, "PAWN"));
			int count = this.CurrentData.skillGains.Count;
			if (count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
			}
			int num = 0;
			foreach (KeyValuePair<SkillDef, int> keyValuePair in this.CurrentData.skillGains)
			{
				if (keyValuePair.Value != 0)
				{
					string value = "    " + keyValuePair.Key.skillLabel.CapitalizeFirst() + ":   " + keyValuePair.Value.ToString("+##;-##");
					if (num < count - 1)
					{
						stringBuilder.AppendLine(value);
					}
					else
					{
						stringBuilder.Append(value);
					}
					num++;
				}
			}
			if (this.GetPermaThoughts().Any<ThoughtDef>())
			{
				stringBuilder.AppendLine();
				foreach (ThoughtDef thoughtDef in this.GetPermaThoughts())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("    " + "PermanentMoodEffect".Translate() + " " + thoughtDef.stages[0].baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset));
				}
			}
			if (currentData.statOffsets != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					StatModifier statModifier = currentData.statOffsets[i];
					string valueToStringAsOffset = statModifier.ValueToStringAsOffset;
					string value2 = "    " + statModifier.stat.LabelCap + " " + valueToStringAsOffset;
					if (i < currentData.statOffsets.Count - 1)
					{
						stringBuilder.AppendLine(value2);
					}
					else
					{
						stringBuilder.Append(value2);
					}
				}
			}
			if (currentData.statFactors != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				for (int j = 0; j < currentData.statFactors.Count; j++)
				{
					StatModifier statModifier2 = currentData.statFactors[j];
					string toStringAsFactor = statModifier2.ToStringAsFactor;
					string value3 = "    " + statModifier2.stat.LabelCap + " " + toStringAsFactor;
					if (j < currentData.statFactors.Count - 1)
					{
						stringBuilder.AppendLine(value3);
					}
					else
					{
						stringBuilder.Append(value3);
					}
				}
			}
			return stringBuilder.ToString();
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Trait(",
				this.def.ToString(),
				"-",
				this.degree,
				")"
			});
		}

		private IEnumerable<ThoughtDef> GetPermaThoughts()
		{
			TraitDegreeData degree = this.CurrentData;
			List<ThoughtDef> allThoughts = DefDatabase<ThoughtDef>.AllDefsListForReading;
			for (int i = 0; i < allThoughts.Count; i++)
			{
				if (allThoughts[i].IsSituational)
				{
					if (allThoughts[i].Worker is ThoughtWorker_AlwaysActive)
					{
						if (allThoughts[i].requiredTraits != null && allThoughts[i].requiredTraits.Contains(this.def))
						{
							if (!allThoughts[i].RequiresSpecificTraitsDegree || allThoughts[i].requiredTraitsDegree == degree.degree)
							{
								yield return allThoughts[i];
							}
						}
					}
				}
			}
			yield break;
		}

		private bool AllowsWorkType(WorkTypeDef workDef)
		{
			return (this.def.disabledWorkTags & workDef.workTags) == WorkTags.None;
		}

		public IEnumerable<WorkTypeDef> GetDisabledWorkTypes()
		{
			for (int i = 0; i < this.def.disabledWorkTypes.Count; i++)
			{
				yield return this.def.disabledWorkTypes[i];
			}
			List<WorkTypeDef> workTypeDefList = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int j = 0; j < workTypeDefList.Count; j++)
			{
				WorkTypeDef w = workTypeDefList[j];
				if (!this.AllowsWorkType(w))
				{
					yield return w;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GetPermaThoughts>c__Iterator0 : IEnumerable, IEnumerable<ThoughtDef>, IEnumerator, IDisposable, IEnumerator<ThoughtDef>
		{
			internal TraitDegreeData <degree>__0;

			internal List<ThoughtDef> <allThoughts>__0;

			internal int <i>__1;

			internal Trait $this;

			internal ThoughtDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetPermaThoughts>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					degree = base.CurrentData;
					allThoughts = DefDatabase<ThoughtDef>.AllDefsListForReading;
					i = 0;
					goto IL_15B;
				case 1u:
					break;
				default:
					return false;
				}
				IL_14D:
				i++;
				IL_15B:
				if (i >= allThoughts.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (!allThoughts[i].IsSituational)
					{
						goto IL_14D;
					}
					if (!(allThoughts[i].Worker is ThoughtWorker_AlwaysActive))
					{
						goto IL_14D;
					}
					if (allThoughts[i].requiredTraits == null || !allThoughts[i].requiredTraits.Contains(this.def))
					{
						goto IL_14D;
					}
					if (allThoughts[i].RequiresSpecificTraitsDegree && allThoughts[i].requiredTraitsDegree != degree.degree)
					{
						goto IL_14D;
					}
					this.$current = allThoughts[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				return false;
			}

			ThoughtDef IEnumerator<ThoughtDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.ThoughtDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThoughtDef> IEnumerable<ThoughtDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Trait.<GetPermaThoughts>c__Iterator0 <GetPermaThoughts>c__Iterator = new Trait.<GetPermaThoughts>c__Iterator0();
				<GetPermaThoughts>c__Iterator.$this = this;
				return <GetPermaThoughts>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetDisabledWorkTypes>c__Iterator1 : IEnumerable, IEnumerable<WorkTypeDef>, IEnumerator, IDisposable, IEnumerator<WorkTypeDef>
		{
			internal int <i>__1;

			internal List<WorkTypeDef> <workTypeDefList>__0;

			internal int <i>__2;

			internal WorkTypeDef <w>__3;

			internal Trait $this;

			internal WorkTypeDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetDisabledWorkTypes>c__Iterator1()
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
					i++;
					break;
				case 2u:
					IL_FC:
					j++;
					goto IL_10B;
				default:
					return false;
				}
				if (i < this.def.disabledWorkTypes.Count)
				{
					this.$current = this.def.disabledWorkTypes[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				workTypeDefList = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				j = 0;
				IL_10B:
				if (j >= workTypeDefList.Count)
				{
					this.$PC = -1;
				}
				else
				{
					w = workTypeDefList[j];
					if (!base.AllowsWorkType(w))
					{
						this.$current = w;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_FC;
				}
				return false;
			}

			WorkTypeDef IEnumerator<WorkTypeDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.WorkTypeDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<WorkTypeDef> IEnumerable<WorkTypeDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Trait.<GetDisabledWorkTypes>c__Iterator1 <GetDisabledWorkTypes>c__Iterator = new Trait.<GetDisabledWorkTypes>c__Iterator1();
				<GetDisabledWorkTypes>c__Iterator.$this = this;
				return <GetDisabledWorkTypes>c__Iterator;
			}
		}
	}
}
