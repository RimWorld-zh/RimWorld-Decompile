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

		public Intelligence minIntelligence = Intelligence.Animal;

		public bool colonistAndPrisonersOnly = false;

		public bool colonistsOnly = false;

		public bool onlyIfCausedByHediff = false;

		public bool neverOnPrisoner = false;

		public bool showOnNeedList = true;

		public float baseLevel = 0.5f;

		public bool major = false;

		public int listPriority = 0;

		[NoTranslate]
		public string tutorHighlightTag = null;

		public bool showForCaravanMembers = false;

		public bool scaleBar = false;

		public float fallPerDay = 0.5f;

		public float seekerRisePerHour = 0f;

		public float seekerFallPerHour = 0f;

		public bool freezeWhileSleeping = false;

		public bool freezeInMentalState = false;

		public NeedDef()
		{
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
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
			if (this.needClass == typeof(Need_Seeker))
			{
				if (this.seekerRisePerHour == 0f || this.seekerFallPerHour == 0f)
				{
					yield return "seeker rise/fall rates not set";
				}
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
					goto IL_104;
				case 3u:
					goto IL_133;
				case 4u:
					goto IL_197;
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
				IL_104:
				if (this.needClass == null)
				{
					this.$current = "needClass is null";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_133:
				if (this.needClass == typeof(Need_Seeker))
				{
					if (this.seekerRisePerHour == 0f || this.seekerFallPerHour == 0f)
					{
						this.$current = "seeker rise/fall rates not set";
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
				}
				IL_197:
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
