using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E07 RID: 3591
	public abstract class ThingComp
	{
		// Token: 0x04003555 RID: 13653
		public ThingWithComps parent;

		// Token: 0x04003556 RID: 13654
		public CompProperties props;

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06005166 RID: 20838 RVA: 0x0009C0F0 File Offset: 0x0009A4F0
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x0009C110 File Offset: 0x0009A510
		public virtual void Initialize(CompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x0009C11A File Offset: 0x0009A51A
		public virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x0009C11D File Offset: 0x0009A51D
		public virtual void PostExposeData()
		{
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x0009C120 File Offset: 0x0009A520
		public virtual void PostSpawnSetup(bool respawningAfterLoad)
		{
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x0009C123 File Offset: 0x0009A523
		public virtual void PostDeSpawn(Map map)
		{
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x0009C126 File Offset: 0x0009A526
		public virtual void PostDestroy(DestroyMode mode, Map previousMap)
		{
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x0009C129 File Offset: 0x0009A529
		public virtual void CompTick()
		{
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x0009C12C File Offset: 0x0009A52C
		public virtual void CompTickRare()
		{
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x0009C12F File Offset: 0x0009A52F
		public virtual void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x0009C135 File Offset: 0x0009A535
		public virtual void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x0009C138 File Offset: 0x0009A538
		public virtual void PostDraw()
		{
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x0009C13B File Offset: 0x0009A53B
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x0009C13E File Offset: 0x0009A53E
		public virtual void PostPrintOnto(SectionLayer layer)
		{
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x0009C141 File Offset: 0x0009A541
		public virtual void CompPrintForPowerGrid(SectionLayer layer)
		{
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x0009C144 File Offset: 0x0009A544
		public virtual void PreAbsorbStack(Thing otherStack, int count)
		{
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x0009C147 File Offset: 0x0009A547
		public virtual void PostSplitOff(Thing piece)
		{
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x0009C14C File Offset: 0x0009A54C
		public virtual string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x0009C164 File Offset: 0x0009A564
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield break;
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x0009C188 File Offset: 0x0009A588
		public virtual bool AllowStackWith(Thing other)
		{
			return true;
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x0009C1A0 File Offset: 0x0009A5A0
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x0009C1B8 File Offset: 0x0009A5B8
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x0009C1D0 File Offset: 0x0009A5D0
		public virtual IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x0009C1F3 File Offset: 0x0009A5F3
		public virtual void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x0009C1F6 File Offset: 0x0009A5F6
		public virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x0009C1F9 File Offset: 0x0009A5F9
		public virtual void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x0009C1FC File Offset: 0x0009A5FC
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x0009C200 File Offset: 0x0009A600
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
