using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Hunt : Designator
	{
		private List<Pawn> justDesignated = new List<Pawn>();

		[CompilerGenerated]
		private static Func<Pawn, PawnKindDef> <>f__am$cache0;

		public Designator_Hunt()
		{
			this.defaultLabel = "DesignatorHunt".Translate();
			this.defaultDesc = "DesignatorHuntDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Hunt", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Hunt;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (!this.HuntablesInCell(c).Any<Pawn>())
			{
				result = "MessageMustDesignateHuntable".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.HuntablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			AcceptanceReport result;
			if (pawn != null && pawn.AnimalOrWildMan() && pawn.Faction == null && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null)
			{
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct<PawnKindDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kind = enumerator.Current;
					HuntUtility.ShowDesignationWarnings(this.justDesignated.First((Pawn x) => x.kindDef == kind));
				}
			}
			this.justDesignated.Clear();
		}

		private IEnumerable<Pawn> HuntablesInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return (Pawn)thingList[i];
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private static PawnKindDef <FinalizeDesignationSucceeded>m__0(Pawn p)
		{
			return p.kindDef;
		}

		[CompilerGenerated]
		private sealed class <FinalizeDesignationSucceeded>c__AnonStorey1
		{
			internal PawnKindDef kind;

			public <FinalizeDesignationSucceeded>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return x.kindDef == this.kind;
			}
		}

		[CompilerGenerated]
		private sealed class <HuntablesInCell>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IntVec3 c;

			internal List<Thing> <thingList>__0;

			internal int <i>__1;

			internal Designator_Hunt $this;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <HuntablesInCell>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (c.Fogged(base.Map))
					{
						return false;
					}
					thingList = c.GetThingList(base.Map);
					i = 0;
					break;
				case 1u:
					IL_C4:
					i++;
					break;
				default:
					return false;
				}
				if (i >= thingList.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (this.CanDesignateThing(thingList[i]).Accepted)
					{
						this.$current = (Pawn)thingList[i];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_C4;
				}
				return false;
			}

			Pawn IEnumerator<Pawn>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Pawn>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Designator_Hunt.<HuntablesInCell>c__Iterator0 <HuntablesInCell>c__Iterator = new Designator_Hunt.<HuntablesInCell>c__Iterator0();
				<HuntablesInCell>c__Iterator.$this = this;
				<HuntablesInCell>c__Iterator.c = c;
				return <HuntablesInCell>c__Iterator;
			}
		}
	}
}
