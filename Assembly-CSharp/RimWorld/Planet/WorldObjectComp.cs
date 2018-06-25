using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000628 RID: 1576
	public abstract class WorldObjectComp
	{
		// Token: 0x0400127A RID: 4730
		public WorldObject parent;

		// Token: 0x0400127B RID: 4731
		public WorldObjectCompProperties props;

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x00111648 File Offset: 0x0010FA48
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x00111668 File Offset: 0x0010FA68
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x00111698 File Offset: 0x0010FA98
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x001116A2 File Offset: 0x0010FAA2
		public virtual void CompTick()
		{
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x001116A8 File Offset: 0x0010FAA8
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x001116CC File Offset: 0x0010FACC
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x001116F0 File Offset: 0x0010FAF0
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			yield break;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x00111714 File Offset: 0x0010FB14
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x00111738 File Offset: 0x0010FB38
		public virtual IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			yield break;
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0011175C File Offset: 0x0010FB5C
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x00111774 File Offset: 0x0010FB74
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0011178A File Offset: 0x0010FB8A
		public virtual void PostPostRemove()
		{
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0011178D File Offset: 0x0010FB8D
		public virtual void PostMyMapRemoved()
		{
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x00111790 File Offset: 0x0010FB90
		public virtual void PostMapGenerate()
		{
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00111793 File Offset: 0x0010FB93
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00111796 File Offset: 0x0010FB96
		public virtual void PostExposeData()
		{
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x0011179C File Offset: 0x0010FB9C
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
