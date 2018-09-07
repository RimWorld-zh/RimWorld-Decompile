using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public static class VerbDefsHardcodedNative
	{
		public static IEnumerable<VerbProperties> AllVerbDefs()
		{
			VerbProperties d = new VerbProperties();
			d.verbClass = typeof(Verb_BeatFire);
			d.category = VerbCategory.BeatFire;
			d.range = 1.42f;
			d.noiseRadius = 3f;
			d.targetParams.canTargetFires = true;
			d.targetParams.canTargetPawns = false;
			d.targetParams.canTargetBuildings = false;
			d.targetParams.mapObjectTargetsMustBeAutoAttackable = false;
			d.warmupTime = 0f;
			d.defaultCooldownTime = 1.1f;
			d.soundCast = SoundDefOf.Interact_BeatFire;
			yield return d;
			d = new VerbProperties();
			d.verbClass = typeof(Verb_Ignite);
			d.category = VerbCategory.Ignite;
			d.range = 1.42f;
			d.noiseRadius = 3f;
			d.targetParams.onlyTargetFlammables = true;
			d.targetParams.canTargetBuildings = true;
			d.targetParams.canTargetPawns = false;
			d.targetParams.mapObjectTargetsMustBeAutoAttackable = false;
			d.warmupTime = 3f;
			d.defaultCooldownTime = 1.3f;
			d.soundCast = SoundDefOf.Interact_Ignite;
			yield return d;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <AllVerbDefs>c__Iterator0 : IEnumerable, IEnumerable<VerbProperties>, IEnumerator, IDisposable, IEnumerator<VerbProperties>
		{
			internal VerbProperties <d>__0;

			internal VerbProperties $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllVerbDefs>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					d = new VerbProperties();
					d.verbClass = typeof(Verb_BeatFire);
					d.category = VerbCategory.BeatFire;
					d.range = 1.42f;
					d.noiseRadius = 3f;
					d.targetParams.canTargetFires = true;
					d.targetParams.canTargetPawns = false;
					d.targetParams.canTargetBuildings = false;
					d.targetParams.mapObjectTargetsMustBeAutoAttackable = false;
					d.warmupTime = 0f;
					d.defaultCooldownTime = 1.1f;
					d.soundCast = SoundDefOf.Interact_BeatFire;
					this.$current = d;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					d = new VerbProperties();
					d.verbClass = typeof(Verb_Ignite);
					d.category = VerbCategory.Ignite;
					d.range = 1.42f;
					d.noiseRadius = 3f;
					d.targetParams.onlyTargetFlammables = true;
					d.targetParams.canTargetBuildings = true;
					d.targetParams.canTargetPawns = false;
					d.targetParams.mapObjectTargetsMustBeAutoAttackable = false;
					d.warmupTime = 3f;
					d.defaultCooldownTime = 1.3f;
					d.soundCast = SoundDefOf.Interact_Ignite;
					this.$current = d;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			VerbProperties IEnumerator<VerbProperties>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.VerbProperties>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<VerbProperties> IEnumerable<VerbProperties>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new VerbDefsHardcodedNative.<AllVerbDefs>c__Iterator0();
			}
		}
	}
}
