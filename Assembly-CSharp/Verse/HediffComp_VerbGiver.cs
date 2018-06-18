using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D1F RID: 3359
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		// Token: 0x060049E9 RID: 18921 RVA: 0x0026A219 File Offset: 0x00268619
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x060049EA RID: 18922 RVA: 0x0026A238 File Offset: 0x00268638
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x060049EB RID: 18923 RVA: 0x0026A258 File Offset: 0x00268658
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x060049EC RID: 18924 RVA: 0x0026A274 File Offset: 0x00268674
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x060049ED RID: 18925 RVA: 0x0026A294 File Offset: 0x00268694
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x0026A2B4 File Offset: 0x002686B4
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

		// Token: 0x060049EF RID: 18927 RVA: 0x0026A306 File Offset: 0x00268706
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.verbTracker.VerbsTick();
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x0026A31C File Offset: 0x0026871C
		public string UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID();
		}

		// Token: 0x04003221 RID: 12833
		public VerbTracker verbTracker = null;
	}
}
