using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class TraitDef : Def
	{
		public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

		public List<TraitDef> conflictingTraits = new List<TraitDef>();

		public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

		public WorkTags requiredWorkTags;

		public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

		public WorkTags disabledWorkTags;

		private float commonality = 1f;

		private float commonalityFemale = -1f;

		public bool allowOnHostileSpawn = true;

		public TraitDef()
		{
		}

		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		public TraitDegreeData DataAtDegree(int degree)
		{
			for (int i = 0; i < this.degreeDatas.Count; i++)
			{
				if (this.degreeDatas[i].degree == degree)
				{
					return this.degreeDatas[i];
				}
			}
			Log.Error(string.Concat(new object[]
			{
				this.defName,
				" found no data at degree ",
				degree,
				", returning first defined."
			}), false);
			return this.degreeDatas[0];
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.commonality < 0.001f && this.commonalityFemale < 0.001f)
			{
				yield return "TraitDef " + this.defName + " has 0 commonality.";
			}
			if (!this.degreeDatas.Any<TraitDegreeData>())
			{
				yield return this.defName + " has no degree datas.";
			}
			for (int i = 0; i < this.degreeDatas.Count; i++)
			{
				TraitDegreeData dd = this.degreeDatas[i];
				if ((from dd2 in this.degreeDatas
				where dd2.degree == dd.degree
				select dd2).Count<TraitDegreeData>() > 1)
				{
					yield return ">1 datas for degree " + dd.degree;
				}
			}
			yield break;
		}

		public bool ConflictsWith(Trait other)
		{
			if (other.def.conflictingTraits != null)
			{
				for (int i = 0; i < other.def.conflictingTraits.Count; i++)
				{
					if (other.def.conflictingTraits[i] == this)
					{
						return true;
					}
				}
			}
			return false;
		}

		public float GetGenderSpecificCommonality(Gender gender)
		{
			float result;
			if (gender == Gender.Female && this.commonalityFemale >= 0f)
			{
				result = this.commonalityFemale;
			}
			else
			{
				result = this.commonality;
			}
			return result;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <err>__1;

			internal int <i>__2;

			internal TraitDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			private TraitDef.<ConfigErrors>c__Iterator0.<ConfigErrors>c__AnonStorey1 $locvar1;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
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
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_11E;
				case 3u:
					goto IL_162;
				case 4u:
					IL_20C:
					i++;
					goto IL_21B;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						err = enumerator.Current;
						this.$current = err;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
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
				if (this.commonality < 0.001f && this.commonalityFemale < 0.001f)
				{
					this.$current = "TraitDef " + this.defName + " has 0 commonality.";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_11E:
				if (!this.degreeDatas.Any<TraitDegreeData>())
				{
					this.$current = this.defName + " has no degree datas.";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_162:
				i = 0;
				IL_21B:
				if (i >= this.degreeDatas.Count)
				{
					this.$PC = -1;
				}
				else
				{
					TraitDegreeData dd = this.degreeDatas[i];
					if ((from dd2 in this.degreeDatas
					where dd2.degree == dd.degree
					select dd2).Count<TraitDegreeData>() > 1)
					{
						this.$current = ">1 datas for degree " + dd.degree;
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
					goto IL_20C;
				}
				return false;
			}

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TraitDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new TraitDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}

			private sealed class <ConfigErrors>c__AnonStorey1
			{
				internal TraitDegreeData dd;

				internal TraitDef.<ConfigErrors>c__Iterator0 <>f__ref$0;

				public <ConfigErrors>c__AnonStorey1()
				{
				}

				internal bool <>m__0(TraitDegreeData dd2)
				{
					return dd2.degree == this.dd.degree;
				}
			}
		}
	}
}
