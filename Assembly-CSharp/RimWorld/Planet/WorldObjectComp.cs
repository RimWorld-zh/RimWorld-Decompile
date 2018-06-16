using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200062A RID: 1578
	public abstract class WorldObjectComp
	{
		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x0011142C File Offset: 0x0010F82C
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x0011144C File Offset: 0x0010F84C
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x0011147C File Offset: 0x0010F87C
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x00111486 File Offset: 0x0010F886
		public virtual void CompTick()
		{
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0011148C File Offset: 0x0010F88C
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x001114B0 File Offset: 0x0010F8B0
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x001114D4 File Offset: 0x0010F8D4
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			yield break;
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x001114F8 File Offset: 0x0010F8F8
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x0011151C File Offset: 0x0010F91C
		public virtual IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			yield break;
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00111540 File Offset: 0x0010F940
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00111558 File Offset: 0x0010F958
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x0011156E File Offset: 0x0010F96E
		public virtual void PostPostRemove()
		{
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x00111571 File Offset: 0x0010F971
		public virtual void PostMyMapRemoved()
		{
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x00111574 File Offset: 0x0010F974
		public virtual void PostMapGenerate()
		{
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x00111577 File Offset: 0x0010F977
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x0011157A File Offset: 0x0010F97A
		public virtual void PostExposeData()
		{
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x00111580 File Offset: 0x0010F980
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
