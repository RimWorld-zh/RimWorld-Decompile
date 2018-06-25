using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Building_TrapRearmable : Building_Trap
	{
		private bool autoRearm = false;

		private bool armedInt = true;

		private Graphic graphicUnarmedInt;

		private static readonly FloatRange TrapDamageFactor = new FloatRange(0.7f, 1.3f);

		private static readonly IntRange DamageCount = new IntRange(1, 2);

		public Building_TrapRearmable()
		{
		}

		public override bool Armed
		{
			get
			{
				return this.armedInt;
			}
		}

		public override Graphic Graphic
		{
			get
			{
				Graphic graphic;
				if (this.armedInt)
				{
					graphic = base.Graphic;
				}
				else
				{
					if (this.graphicUnarmedInt == null)
					{
						this.graphicUnarmedInt = this.def.building.trapUnarmedGraphicData.GraphicColoredFor(this);
					}
					graphic = this.graphicUnarmedInt;
				}
				return graphic;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.armedInt, "armed", false, false);
			Scribe_Values.Look<bool>(ref this.autoRearm, "autoRearm", false, false);
		}

		protected override void SpringSub(Pawn p)
		{
			this.armedInt = false;
			if (p != null)
			{
				this.DamagePawn(p);
			}
			if (this.autoRearm)
			{
				base.Map.designationManager.AddDesignation(new Designation(this, DesignationDefOf.RearmTrap));
			}
		}

		public void Rearm()
		{
			this.armedInt = true;
			SoundDefOf.TrapArm.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
		}

		private void DamagePawn(Pawn p)
		{
			BodyPartHeight height = (Rand.Value >= 0.666f) ? BodyPartHeight.Middle : BodyPartHeight.Top;
			int num = Mathf.RoundToInt(this.GetStatValue(StatDefOf.TrapMeleeDamage, true) * Building_TrapRearmable.TrapDamageFactor.RandomInRange);
			int randomInRange = Building_TrapRearmable.DamageCount.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				if (num <= 0)
				{
					break;
				}
				int num2 = Mathf.Max(1, Mathf.RoundToInt(Rand.Value * (float)num));
				num -= num2;
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Stab, (float)num2, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
				dinfo.SetBodyRegion(height, BodyPartDepth.Outside);
				p.TakeDamage(dinfo);
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			yield return new Command_Toggle
			{
				defaultLabel = "CommandAutoRearm".Translate(),
				defaultDesc = "CommandAutoRearmDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc3,
				icon = TexCommand.RearmTrap,
				isActive = (() => this.autoRearm),
				toggleAction = delegate()
				{
					this.autoRearm = !this.autoRearm;
				}
			};
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Building_TrapRearmable()
		{
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy0()
		{
			return base.GetGizmos();
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Command_Toggle <com>__0;

			internal Building_TrapRearmable $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
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
					enumerator = base.<GetGizmos>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					this.$PC = -1;
					return false;
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
						g = enumerator.Current;
						this.$current = g;
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
				Command_Toggle com = new Command_Toggle();
				com.defaultLabel = "CommandAutoRearm".Translate();
				com.defaultDesc = "CommandAutoRearmDesc".Translate();
				com.hotKey = KeyBindingDefOf.Misc3;
				com.icon = TexCommand.RearmTrap;
				com.isActive = (() => this.autoRearm);
				com.toggleAction = delegate()
				{
					this.autoRearm = !this.autoRearm;
				};
				this.$current = com;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Building_TrapRearmable.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new Building_TrapRearmable.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			internal bool <>m__0()
			{
				return this.autoRearm;
			}

			internal void <>m__1()
			{
				this.autoRearm = !this.autoRearm;
			}
		}
	}
}
