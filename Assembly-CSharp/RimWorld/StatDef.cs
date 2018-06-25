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
	public class StatDef : Def
	{
		public StatCategoryDef category = null;

		public Type workerClass = typeof(StatWorker);

		public float hideAtValue = -2.14748365E+09f;

		public bool alwaysHide;

		public bool showNonAbstract = true;

		public bool showIfUndefined = true;

		public bool showOnPawns = true;

		public bool showOnHumanlikes = true;

		public bool showOnNonWildManHumanlikes = true;

		public bool showOnAnimals = true;

		public bool showOnMechanoids = true;

		public bool showOnNonWorkTables = true;

		public bool neverDisabled = false;

		public int displayPriorityInCategory = 0;

		public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

		public ToStringStyle toStringStyle = ToStringStyle.Integer;

		public ToStringStyle? toStringStyleUnfinalized;

		[MustTranslate]
		public string formatString = null;

		public float defaultBaseValue = 1f;

		public List<SkillNeed> skillNeedOffsets = null;

		public float noSkillOffset = 0f;

		public List<PawnCapacityOffset> capacityOffsets = null;

		public List<StatDef> statFactors = null;

		public bool applyFactorsIfNegative = true;

		public List<SkillNeed> skillNeedFactors = null;

		public float noSkillFactor = 1f;

		public List<PawnCapacityFactor> capacityFactors = null;

		public SimpleCurve postProcessCurve = null;

		public float minValue = -9999999f;

		public float maxValue = 9999999f;

		public bool roundValue = false;

		public float roundToFiveOver = float.MaxValue;

		public bool minifiedThingInherits;

		public bool scenarioRandomizable = false;

		public List<StatPart> parts = null;

		[Unsaved]
		private StatWorker workerInt = null;

		[CompilerGenerated]
		private static Func<StatPart, float> <>f__am$cache0;

		public StatDef()
		{
		}

		public StatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					if (this.parts != null)
					{
						for (int i = 0; i < this.parts.Count; i++)
						{
							this.parts[i].parentStat = this;
						}
					}
					this.workerInt = (StatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.InitSetStat(this);
				}
				return this.workerInt;
			}
		}

		public ToStringStyle ToStringStyleUnfinalized
		{
			get
			{
				ToStringStyle? toStringStyle = this.toStringStyleUnfinalized;
				return (toStringStyle == null) ? this.toStringStyle : this.toStringStyleUnfinalized.Value;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.capacityFactors != null)
			{
				foreach (PawnCapacityFactor afac in this.capacityFactors)
				{
					if (afac.weight > 1f)
					{
						yield return this.defName + " has activity factor with weight > 1";
					}
				}
			}
			if (this.parts != null)
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					foreach (string err2 in this.parts[i].ConfigErrors())
					{
						yield return string.Concat(new string[]
						{
							this.defName,
							" has error in StatPart ",
							this.parts[i].ToString(),
							": ",
							err2
						});
					}
				}
			}
			yield break;
		}

		public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			return this.Worker.ValueToString(val, true, numberSense);
		}

		public static StatDef Named(string defName)
		{
			return DefDatabase<StatDef>.GetNamed(defName, true);
		}

		public override void PostLoad()
		{
			base.PostLoad();
			if (this.parts != null)
			{
				List<StatPart> partsCopy = this.parts.ToList<StatPart>();
				this.parts.SortBy((StatPart x) => -x.priority, (StatPart x) => partsCopy.IndexOf(x));
			}
		}

		public T GetStatPart<T>() where T : StatPart
		{
			return this.parts.OfType<T>().FirstOrDefault<T>();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private static float <PostLoad>m__0(StatPart x)
		{
			return -x.priority;
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <err>__1;

			internal List<PawnCapacityFactor>.Enumerator $locvar1;

			internal PawnCapacityFactor <afac>__2;

			internal int <i>__3;

			internal IEnumerator<string> $locvar2;

			internal string <err>__4;

			internal StatDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

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
					goto IL_E7;
				case 3u:
					Block_6:
					try
					{
						switch (num)
						{
						}
						if (enumerator3.MoveNext())
						{
							err2 = enumerator3.Current;
							this.$current = string.Concat(new string[]
							{
								this.defName,
								" has error in StatPart ",
								this.parts[i].ToString(),
								": ",
								err2
							});
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator3 != null)
							{
								enumerator3.Dispose();
							}
						}
					}
					i++;
					goto IL_294;
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
				if (this.capacityFactors == null)
				{
					goto IL_17D;
				}
				enumerator2 = this.capacityFactors.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_E7:
					switch (num)
					{
					case 2u:
						IL_150:
						break;
					}
					if (enumerator2.MoveNext())
					{
						afac = enumerator2.Current;
						if (afac.weight > 1f)
						{
							this.$current = this.defName + " has activity factor with weight > 1";
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
						goto IL_150;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				IL_17D:
				if (this.parts == null)
				{
					goto IL_2B0;
				}
				i = 0;
				IL_294:
				if (i < this.parts.Count)
				{
					enumerator3 = this.parts[i].ConfigErrors().GetEnumerator();
					num = 4294967293u;
					goto Block_6;
				}
				IL_2B0:
				this.$PC = -1;
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
				case 2u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					break;
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
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
				StatDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new StatDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <PostLoad>c__AnonStorey1
		{
			internal List<StatPart> partsCopy;

			public <PostLoad>c__AnonStorey1()
			{
			}

			internal int <>m__0(StatPart x)
			{
				return this.partsCopy.IndexOf(x);
			}
		}
	}
}
