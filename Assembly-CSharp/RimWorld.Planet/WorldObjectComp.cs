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
			WorldObjectComp.<GetGizmos>c__Iterator10E <GetGizmos>c__Iterator10E = new WorldObjectComp.<GetGizmos>c__Iterator10E();
			WorldObjectComp.<GetGizmos>c__Iterator10E expr_07 = <GetGizmos>c__Iterator10E;
			expr_07.$PC = -2;
			return expr_07;
		}

		[DebuggerHidden]
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			WorldObjectComp.<GetFloatMenuOptions>c__Iterator10F <GetFloatMenuOptions>c__Iterator10F = new WorldObjectComp.<GetFloatMenuOptions>c__Iterator10F();
			WorldObjectComp.<GetFloatMenuOptions>c__Iterator10F expr_07 = <GetFloatMenuOptions>c__Iterator10F;
			expr_07.$PC = -2;
			return expr_07;
		}

		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		public virtual string GetDescriptionPart()
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
