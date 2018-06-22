using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E01 RID: 3585
	public class CompEquippable : ThingComp, IVerbOwner
	{
		// Token: 0x06005143 RID: 20803 RVA: 0x0029BC92 File Offset: 0x0029A092
		public CompEquippable()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06005144 RID: 20804 RVA: 0x0029BCB0 File Offset: 0x0029A0B0
		private Pawn Holder
		{
			get
			{
				return this.PrimaryVerb.CasterPawn;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06005145 RID: 20805 RVA: 0x0029BCD0 File Offset: 0x0029A0D0
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06005146 RID: 20806 RVA: 0x0029BCF0 File Offset: 0x0029A0F0
		public Verb PrimaryVerb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06005147 RID: 20807 RVA: 0x0029BD10 File Offset: 0x0029A110
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06005148 RID: 20808 RVA: 0x0029BD2C File Offset: 0x0029A12C
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06005149 RID: 20809 RVA: 0x0029BD54 File Offset: 0x0029A154
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x0029BD7C File Offset: 0x0029A17C
		public IEnumerable<Command> GetVerbsCommands()
		{
			return this.verbTracker.GetVerbsCommands(KeyCode.None);
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x0029BDA0 File Offset: 0x0029A1A0
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Holder != null && this.Holder.equipment != null && this.Holder.equipment.Primary == this.parent)
			{
				this.Holder.equipment.Notify_PrimaryDestroyed();
			}
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x0029BDFC File Offset: 0x0029A1FC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x0029BE1F File Offset: 0x0029A21F
		public override void CompTick()
		{
			base.CompTick();
			this.verbTracker.VerbsTick();
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x0029BE34 File Offset: 0x0029A234
		public void Notify_EquipmentLost()
		{
			List<Verb> allVerbs = this.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				allVerbs[i].Notify_EquipmentLost();
			}
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x0029BE70 File Offset: 0x0029A270
		public string UniqueVerbOwnerID()
		{
			return this.parent.ThingID;
		}

		// Token: 0x0400354E RID: 13646
		public VerbTracker verbTracker = null;
	}
}
