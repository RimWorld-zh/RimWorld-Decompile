using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D20 RID: 3360
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		// Token: 0x060049EB RID: 18923 RVA: 0x0026A241 File Offset: 0x00268641
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x060049EC RID: 18924 RVA: 0x0026A260 File Offset: 0x00268660
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x060049ED RID: 18925 RVA: 0x0026A280 File Offset: 0x00268680
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x060049EE RID: 18926 RVA: 0x0026A29C File Offset: 0x0026869C
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x060049EF RID: 18927 RVA: 0x0026A2BC File Offset: 0x002686BC
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x0026A2DC File Offset: 0x002686DC
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
			}
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x0026A32E File Offset: 0x0026872E
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.verbTracker.VerbsTick();
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x0026A344 File Offset: 0x00268744
		public string UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID();
		}

		// Token: 0x04003223 RID: 12835
		public VerbTracker verbTracker = null;
	}
}
