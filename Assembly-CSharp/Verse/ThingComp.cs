using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E09 RID: 3593
	public abstract class ThingComp
	{
		// Token: 0x04003555 RID: 13653
		public ThingWithComps parent;

		// Token: 0x04003556 RID: 13654
		public CompProperties props;

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x0600516A RID: 20842 RVA: 0x0009C240 File Offset: 0x0009A640
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x0009C260 File Offset: 0x0009A660
		public virtual void Initialize(CompProperties props)
		{
			this.props = props;
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x0009C26A File Offset: 0x0009A66A
		public virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x0009C26D File Offset: 0x0009A66D
		public virtual void PostExposeData()
		{
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x0009C270 File Offset: 0x0009A670
		public virtual void PostSpawnSetup(bool respawningAfterLoad)
		{
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x0009C273 File Offset: 0x0009A673
		public virtual void PostDeSpawn(Map map)
		{
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x0009C276 File Offset: 0x0009A676
		public virtual void PostDestroy(DestroyMode mode, Map previousMap)
		{
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x0009C279 File Offset: 0x0009A679
		public virtual void CompTick()
		{
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x0009C27C File Offset: 0x0009A67C
		public virtual void CompTickRare()
		{
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x0009C27F File Offset: 0x0009A67F
		public virtual void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x0009C285 File Offset: 0x0009A685
		public virtual void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x0009C288 File Offset: 0x0009A688
		public virtual void PostDraw()
		{
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x0009C28B File Offset: 0x0009A68B
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x0009C28E File Offset: 0x0009A68E
		public virtual void PostPrintOnto(SectionLayer layer)
		{
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x0009C291 File Offset: 0x0009A691
		public virtual void CompPrintForPowerGrid(SectionLayer layer)
		{
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x0009C294 File Offset: 0x0009A694
		public virtual void PreAbsorbStack(Thing otherStack, int count)
		{
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x0009C297 File Offset: 0x0009A697
		public virtual void PostSplitOff(Thing piece)
		{
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x0009C29C File Offset: 0x0009A69C
		public virtual string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x0009C2B4 File Offset: 0x0009A6B4
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield break;
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x0009C2D8 File Offset: 0x0009A6D8
		public virtual bool AllowStackWith(Thing other)
		{
			return true;
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x0009C2F0 File Offset: 0x0009A6F0
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x0009C308 File Offset: 0x0009A708
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x0009C320 File Offset: 0x0009A720
		public virtual IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x0009C343 File Offset: 0x0009A743
		public virtual void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x0009C346 File Offset: 0x0009A746
		public virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x0009C349 File Offset: 0x0009A749
		public virtual void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x0009C34C File Offset: 0x0009A74C
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x0009C350 File Offset: 0x0009A750
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
