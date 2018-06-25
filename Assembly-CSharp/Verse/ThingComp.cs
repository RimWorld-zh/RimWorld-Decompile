using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E0A RID: 3594
	public abstract class ThingComp
	{
		// Token: 0x0400355C RID: 13660
		public ThingWithComps parent;

		// Token: 0x0400355D RID: 13661
		public CompProperties props;

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x0600516A RID: 20842 RVA: 0x0009C250 File Offset: 0x0009A650
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x0009C270 File Offset: 0x0009A670
		public virtual void Initialize(CompProperties props)
		{
			this.props = props;
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x0009C27A File Offset: 0x0009A67A
		public virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x0009C27D File Offset: 0x0009A67D
		public virtual void PostExposeData()
		{
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x0009C280 File Offset: 0x0009A680
		public virtual void PostSpawnSetup(bool respawningAfterLoad)
		{
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x0009C283 File Offset: 0x0009A683
		public virtual void PostDeSpawn(Map map)
		{
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x0009C286 File Offset: 0x0009A686
		public virtual void PostDestroy(DestroyMode mode, Map previousMap)
		{
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x0009C289 File Offset: 0x0009A689
		public virtual void CompTick()
		{
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x0009C28C File Offset: 0x0009A68C
		public virtual void CompTickRare()
		{
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x0009C28F File Offset: 0x0009A68F
		public virtual void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x0009C295 File Offset: 0x0009A695
		public virtual void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x0009C298 File Offset: 0x0009A698
		public virtual void PostDraw()
		{
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x0009C29B File Offset: 0x0009A69B
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x0009C29E File Offset: 0x0009A69E
		public virtual void PostPrintOnto(SectionLayer layer)
		{
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x0009C2A1 File Offset: 0x0009A6A1
		public virtual void CompPrintForPowerGrid(SectionLayer layer)
		{
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x0009C2A4 File Offset: 0x0009A6A4
		public virtual void PreAbsorbStack(Thing otherStack, int count)
		{
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x0009C2A7 File Offset: 0x0009A6A7
		public virtual void PostSplitOff(Thing piece)
		{
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x0009C2AC File Offset: 0x0009A6AC
		public virtual string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x0009C2C4 File Offset: 0x0009A6C4
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield break;
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x0009C2E8 File Offset: 0x0009A6E8
		public virtual bool AllowStackWith(Thing other)
		{
			return true;
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x0009C300 File Offset: 0x0009A700
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x0009C318 File Offset: 0x0009A718
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x0009C330 File Offset: 0x0009A730
		public virtual IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x0009C353 File Offset: 0x0009A753
		public virtual void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x0009C356 File Offset: 0x0009A756
		public virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x0009C359 File Offset: 0x0009A759
		public virtual void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x0009C35C File Offset: 0x0009A75C
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x0009C360 File Offset: 0x0009A760
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				"(parent=",
				this.parent,
				" at=",
				(this.parent == null) ? IntVec3.Invalid : this.parent.Position,
				")"
			});
		}
	}
}
