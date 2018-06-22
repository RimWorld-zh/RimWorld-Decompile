using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000626 RID: 1574
	public abstract class WorldObjectComp
	{
		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x001114F8 File Offset: 0x0010F8F8
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x00111518 File Offset: 0x0010F918
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x00111548 File Offset: 0x0010F948
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x00111552 File Offset: 0x0010F952
		public virtual void CompTick()
		{
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x00111558 File Offset: 0x0010F958
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x0011157C File Offset: 0x0010F97C
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x001115A0 File Offset: 0x0010F9A0
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			yield break;
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x001115C4 File Offset: 0x0010F9C4
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x001115E8 File Offset: 0x0010F9E8
		public virtual IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			yield break;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0011160C File Offset: 0x0010FA0C
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x00111624 File Offset: 0x0010FA24
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x0011163A File Offset: 0x0010FA3A
		public virtual void PostPostRemove()
		{
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0011163D File Offset: 0x0010FA3D
		public virtual void PostMyMapRemoved()
		{
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x00111640 File Offset: 0x0010FA40
		public virtual void PostMapGenerate()
		{
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x00111643 File Offset: 0x0010FA43
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x00111646 File Offset: 0x0010FA46
		public virtual void PostExposeData()
		{
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0011164C File Offset: 0x0010FA4C
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

		// Token: 0x0400127A RID: 4730
		public WorldObject parent;

		// Token: 0x0400127B RID: 4731
		public WorldObjectCompProperties props;
	}
}
