using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		public JobDriver_Kidnap()
		{
		}

		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.Item;
			}
		}

		public override string GetReport()
		{
			string result;
			if (this.Takee == null || this.pawn.HostileTo(this.Takee))
			{
				result = base.GetReport();
			}
			else
			{
				result = JobDefOf.Rescue.reportString.Replace("TargetA", this.Takee.LabelShort);
			}
			return result;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => this.Takee == null || (!this.Takee.Downed && this.Takee.Awake()));
			foreach (Toil t in this.<MakeNewToils>__BaseCallProxy0())
			{
				yield return t;
			}
			yield break;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Toil> <MakeNewToils>__BaseCallProxy0()
		{
			return base.MakeNewToils();
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal IEnumerator<Toil> $locvar0;

			internal Toil <t>__1;

			internal JobDriver_Kidnap $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
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
					this.FailOn(() => base.Takee == null || (!base.Takee.Downed && base.Takee.Awake()));
					enumerator = base.<MakeNewToils>__BaseCallProxy0().GetEnumerator();
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
					if (enumerator.MoveNext())
					{
						t = enumerator.Current;
						this.$current = t;
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
				this.$PC = -1;
				return false;
			}

			Toil IEnumerator<Toil>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_Kidnap.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Kidnap.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return base.Takee == null || (!base.Takee.Downed && base.Takee.Awake());
			}
		}
	}
}
