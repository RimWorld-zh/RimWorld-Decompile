using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class NeedDef : Def
	{
		public Type needClass;

		public Intelligence minIntelligence;

		public bool colonistAndPrisonersOnly;

		public bool colonistsOnly;

		public bool onlyIfCausedByHediff;

		public bool neverOnPrisoner;

		public bool showOnNeedList = true;

		public float baseLevel = 0.5f;

		public bool major;

		public int listPriority;

		[NoTranslate]
		public string tutorHighlightTag;

		public bool showForCaravanMembers;

		public bool scaleBar;

		public float fallPerDay = 0.5f;

		public float seekerRisePerHour;

		public float seekerFallPerHour;

		public bool freezeWhileSleeping;

		public bool freezeInMentalState;

		public NeedDef()
		{
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in base.ConfigErrors())
			{
				yield return e;
			}
			if (this.description.NullOrEmpty() && this.showOnNeedList)
			{
				yield return "no description";
			}
			if (this.needClass == null)
			{
				yield return "needClass is null";
			}
			if (this.needClass == typeof(Need_Seeker) && (this.seekerRisePerHour == 0f || this.seekerFallPerHour == 0f))
			{
				yield return "seeker rise/fall rates not set";
			}
			yield break;
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

			internal string <e>__1;

			internal NeedDef $this;

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
					goto IL_100;
				case 3u:
					goto IL_12F;
				case 4u:
					goto IL_192;
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
						e = enumerator.Current;
						this.$current = e;
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
				if (this.description.NullOrEmpty() && this.showOnNeedList)
				{
					this.$current = "no description";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_100:
				if (this.needClass == null)
				{
					this.$current = "needClass is null";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_12F:
				if (this.needClass == typeof(Need_Seeker) && (this.seekerRisePerHour == 0f || this.seekerFallPerHour == 0f))
				{
					this.$current = "seeker rise/fall rates not set";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_192:
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
				NeedDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new NeedDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
