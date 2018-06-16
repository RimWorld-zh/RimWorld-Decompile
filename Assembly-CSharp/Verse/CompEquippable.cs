using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E05 RID: 3589
	public class CompEquippable : ThingComp, IVerbOwner
	{
		// Token: 0x06005131 RID: 20785 RVA: 0x0029A6D6 File Offset: 0x00298AD6
		public CompEquippable()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06005132 RID: 20786 RVA: 0x0029A6F4 File Offset: 0x00298AF4
		private Pawn Holder
		{
			get
			{
				return this.PrimaryVerb.CasterPawn;
			}
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06005133 RID: 20787 RVA: 0x0029A714 File Offset: 0x00298B14
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06005134 RID: 20788 RVA: 0x0029A734 File Offset: 0x00298B34
		public Verb PrimaryVerb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06005135 RID: 20789 RVA: 0x0029A754 File Offset: 0x00298B54
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06005136 RID: 20790 RVA: 0x0029A770 File Offset: 0x00298B70
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06005137 RID: 20791 RVA: 0x0029A798 File Offset: 0x00298B98
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x0029A7C0 File Offset: 0x00298BC0
		public IEnumerable<Command> GetVerbsCommands()
		{
			return this.verbTracker.GetVerbsCommands(KeyCode.None);
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x0029A7E4 File Offset: 0x00298BE4
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Holder != null && this.Holder.equipment != null && this.Holder.equipment.Primary == this.parent)
			{
				this.Holder.equipment.Notify_PrimaryDestroyed();
			}
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x0029A840 File Offset: 0x00298C40
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x0029A863 File Offset: 0x00298C63
		public override void CompTick()
		{
			base.CompTick();
			this.verbTracker.VerbsTick();
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x0029A878 File Offset: 0x00298C78
		public void Notify_EquipmentLost()
		{
			List<Verb> allVerbs = this.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				allVerbs[i].Notify_EquipmentLost();
			}
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x0029A8B4 File Offset: 0x00298CB4
		public string UniqueVerbOwnerID()
		{
			return this.parent.ThingID;
		}

		// Token: 0x04003549 RID: 13641
		public VerbTracker verbTracker = null;
	}
}
