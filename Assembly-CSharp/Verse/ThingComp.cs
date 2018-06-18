using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E0A RID: 3594
	public abstract class ThingComp
	{
		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06005152 RID: 20818 RVA: 0x0009BF0C File Offset: 0x0009A30C
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x06005153 RID: 20819 RVA: 0x0009BF2C File Offset: 0x0009A32C
		public virtual void Initialize(CompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x0009BF36 File Offset: 0x0009A336
		public virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x0009BF39 File Offset: 0x0009A339
		public virtual void PostExposeData()
		{
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x0009BF3C File Offset: 0x0009A33C
		public virtual void PostSpawnSetup(bool respawningAfterLoad)
		{
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x0009BF3F File Offset: 0x0009A33F
		public virtual void PostDeSpawn(Map map)
		{
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x0009BF42 File Offset: 0x0009A342
		public virtual void PostDestroy(DestroyMode mode, Map previousMap)
		{
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x0009BF45 File Offset: 0x0009A345
		public virtual void CompTick()
		{
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x0009BF48 File Offset: 0x0009A348
		public virtual void CompTickRare()
		{
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x0009BF4B File Offset: 0x0009A34B
		public virtual void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x0009BF51 File Offset: 0x0009A351
		public virtual void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x0009BF54 File Offset: 0x0009A354
		public virtual void PostDraw()
		{
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x0009BF57 File Offset: 0x0009A357
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x0009BF5A File Offset: 0x0009A35A
		public virtual void PostPrintOnto(SectionLayer layer)
		{
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x0009BF5D File Offset: 0x0009A35D
		public virtual void CompPrintForPowerGrid(SectionLayer layer)
		{
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x0009BF60 File Offset: 0x0009A360
		public virtual void PreAbsorbStack(Thing otherStack, int count)
		{
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x0009BF63 File Offset: 0x0009A363
		public virtual void PostSplitOff(Thing piece)
		{
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x0009BF68 File Offset: 0x0009A368
		public virtual string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x0009BF80 File Offset: 0x0009A380
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield break;
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x0009BFA4 File Offset: 0x0009A3A4
		public virtual bool AllowStackWith(Thing other)
		{
			return true;
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x0009BFBC File Offset: 0x0009A3BC
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x0009BFD4 File Offset: 0x0009A3D4
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x0009BFEC File Offset: 0x0009A3EC
		public virtual IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x0009C00F File Offset: 0x0009A40F
		public virtual void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x0009C012 File Offset: 0x0009A412
		public virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x0009C015 File Offset: 0x0009A415
		public virtual void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x0009C018 File Offset: 0x0009A418
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x0009C01C File Offset: 0x0009A41C
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

		// Token: 0x0400354E RID: 13646
		public ThingWithComps parent;

		// Token: 0x0400354F RID: 13647
		public CompProperties props;
	}
}
