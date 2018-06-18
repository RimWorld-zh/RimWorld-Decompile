using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200062A RID: 1578
	public abstract class WorldObjectComp
	{
		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06002024 RID: 8228 RVA: 0x001114A4 File Offset: 0x0010F8A4
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x001114C4 File Offset: 0x0010F8C4
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x001114F4 File Offset: 0x0010F8F4
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x001114FE File Offset: 0x0010F8FE
		public virtual void CompTick()
		{
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x00111504 File Offset: 0x0010F904
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x00111528 File Offset: 0x0010F928
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x0011154C File Offset: 0x0010F94C
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			yield break;
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00111570 File Offset: 0x0010F970
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00111594 File Offset: 0x0010F994
		public virtual IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			yield break;
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x001115B8 File Offset: 0x0010F9B8
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x001115D0 File Offset: 0x0010F9D0
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x001115E6 File Offset: 0x0010F9E6
		public virtual void PostPostRemove()
		{
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x001115E9 File Offset: 0x0010F9E9
		public virtual void PostMyMapRemoved()
		{
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x001115EC File Offset: 0x0010F9EC
		public virtual void PostMapGenerate()
		{
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x001115EF File Offset: 0x0010F9EF
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x001115F2 File Offset: 0x0010F9F2
		public virtual void PostExposeData()
		{
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x001115F8 File Offset: 0x0010F9F8
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

		// Token: 0x04001280 RID: 4736
		public WorldObject parent;

		// Token: 0x04001281 RID: 4737
		public WorldObjectCompProperties props;
	}
}
