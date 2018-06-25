using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D1F RID: 3359
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		// Token: 0x04003233 RID: 12851
		public VerbTracker verbTracker = null;

		// Token: 0x060049FD RID: 18941 RVA: 0x0026BA09 File Offset: 0x00269E09
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x060049FE RID: 18942 RVA: 0x0026BA28 File Offset: 0x00269E28
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x060049FF RID: 18943 RVA: 0x0026BA48 File Offset: 0x00269E48
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06004A00 RID: 18944 RVA: 0x0026BA64 File Offset: 0x00269E64
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06004A01 RID: 18945 RVA: 0x0026BA84 File Offset: 0x00269E84
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x0026BAA4 File Offset: 0x00269EA4
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

		// Token: 0x06004A03 RID: 18947 RVA: 0x0026BAF6 File Offset: 0x00269EF6
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x0026BB0C File Offset: 0x00269F0C
		public string UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID();
		}
	}
}
