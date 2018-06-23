using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D1C RID: 3356
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		// Token: 0x0400322C RID: 12844
		public VerbTracker verbTracker = null;

		// Token: 0x060049FA RID: 18938 RVA: 0x0026B64D File Offset: 0x00269A4D
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x060049FB RID: 18939 RVA: 0x0026B66C File Offset: 0x00269A6C
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x060049FC RID: 18940 RVA: 0x0026B68C File Offset: 0x00269A8C
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x060049FD RID: 18941 RVA: 0x0026B6A8 File Offset: 0x00269AA8
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x060049FE RID: 18942 RVA: 0x0026B6C8 File Offset: 0x00269AC8
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x0026B6E8 File Offset: 0x00269AE8
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

		// Token: 0x06004A00 RID: 18944 RVA: 0x0026B73A File Offset: 0x00269B3A
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x0026B750 File Offset: 0x00269B50
		public string UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID();
		}
	}
}
