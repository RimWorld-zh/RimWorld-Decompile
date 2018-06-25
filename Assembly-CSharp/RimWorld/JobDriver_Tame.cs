using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Tame : JobDriver_InteractAnimal
	{
		public JobDriver_Tame()
		{
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in this.<MakeNewToils>__BaseCallProxy0())
			{
				yield return toil;
			}
			this.FailOn(() => base.Map.designationManager.DesignationOn(base.Animal, DesignationDefOf.Tame) == null && !base.OnLastToil);
			yield break;
		}

		protected override Toil FinalInteractToil()
		{
			return Toils_Interpersonal.TryRecruit(TargetIndex.A);
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

			internal Toil <toil>__1;

			internal JobDriver_Tame $this;

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
						toil = enumerator.Current;
						this.$current = toil;
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
				this.FailOn(() => base.Map.designationManager.DesignationOn(base.Animal, DesignationDefOf.Tame) == null && !base.OnLastToil);
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
				JobDriver_Tame.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Tame.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return base.Map.designationManager.DesignationOn(base.Animal, DesignationDefOf.Tame) == null && !base.OnLastToil;
			}
		}
	}
}
