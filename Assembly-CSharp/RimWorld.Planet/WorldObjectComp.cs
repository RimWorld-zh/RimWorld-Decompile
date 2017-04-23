using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld.Planet
{
	public abstract class WorldObjectComp
	{
		public WorldObject parent;

		public WorldObjectCompProperties props;

		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		public virtual void CompTick()
		{
		}

		[DebuggerHidden]
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			WorldObjectComp.<GetGizmos>c__Iterator10C <GetGizmos>c__Iterator10C = new WorldObjectComp.<GetGizmos>c__Iterator10C();
			WorldObjectComp.<GetGizmos>c__Iterator10C expr_07 = <GetGizmos>c__Iterator10C;
			expr_07.$PC = -2;
			return expr_07;
		}

		[DebuggerHidden]
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			WorldObjectComp.<GetFloatMenuOptions>c__Iterator10D <GetFloatMenuOptions>c__Iterator10D = new WorldObjectComp.<GetFloatMenuOptions>c__Iterator10D();
			WorldObjectComp.<GetFloatMenuOptions>c__Iterator10D expr_07 = <GetFloatMenuOptions>c__Iterator10D;
			expr_07.$PC = -2;
			return expr_07;
		}

		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		public virtual void PostPostRemove()
		{
		}

		public virtual void PostExposeData()
		{
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				"(parent=",
				this.parent,
				" at=",
				(this.parent == null) ? -1 : this.parent.Tile,
				")"
			});
		}
	}
}
