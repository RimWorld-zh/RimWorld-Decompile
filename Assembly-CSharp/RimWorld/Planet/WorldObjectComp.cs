using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000628 RID: 1576
	public abstract class WorldObjectComp
	{
		// Token: 0x0400127E RID: 4734
		public WorldObject parent;

		// Token: 0x0400127F RID: 4735
		public WorldObjectCompProperties props;

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x001118B0 File Offset: 0x0010FCB0
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x001118D0 File Offset: 0x0010FCD0
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x00111900 File Offset: 0x0010FD00
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x0011190A File Offset: 0x0010FD0A
		public virtual void CompTick()
		{
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x00111910 File Offset: 0x0010FD10
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x00111934 File Offset: 0x0010FD34
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x00111958 File Offset: 0x0010FD58
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			yield break;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x0011197C File Offset: 0x0010FD7C
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x001119A0 File Offset: 0x0010FDA0
		public virtual IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			yield break;
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x001119C4 File Offset: 0x0010FDC4
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x001119DC File Offset: 0x0010FDDC
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x001119F2 File Offset: 0x0010FDF2
		public virtual void PostPostRemove()
		{
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x001119F5 File Offset: 0x0010FDF5
		public virtual void PostMyMapRemoved()
		{
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x001119F8 File Offset: 0x0010FDF8
		public virtual void PostMapGenerate()
		{
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x001119FB File Offset: 0x0010FDFB
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x001119FE File Offset: 0x0010FDFE
		public virtual void PostExposeData()
		{
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00111A04 File Offset: 0x0010FE04
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
