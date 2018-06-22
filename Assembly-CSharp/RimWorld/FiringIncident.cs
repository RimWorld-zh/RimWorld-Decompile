using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000315 RID: 789
	public class FiringIncident : IExposable
	{
		// Token: 0x06000D5F RID: 3423 RVA: 0x00073387 File Offset: 0x00071787
		public FiringIncident()
		{
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0007339B File Offset: 0x0007179B
		public FiringIncident(IncidentDef def, StorytellerComp source, IncidentParms parms = null)
		{
			this.def = def;
			if (parms != null)
			{
				this.parms = parms;
			}
			this.source = source;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x000733CA File Offset: 0x000717CA
		public void ExposeData()
		{
			Scribe_Defs.Look<IncidentDef>(ref this.def, "def");
			Scribe_Deep.Look<IncidentParms>(ref this.parms, "parms", new object[0]);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000733F4 File Offset: 0x000717F4
		public override string ToString()
		{
			string text = this.def.ToString();
			text = text.PadRight(17);
			string text2 = text;
			if (this.parms != null)
			{
				text2 = text2 + " " + this.parms.ToString();
			}
			if (this.source != null)
			{
				text2 = text2 + ", source=" + this.source;
			}
			return text2;
		}

		// Token: 0x04000895 RID: 2197
		public IncidentDef def;

		// Token: 0x04000896 RID: 2198
		public IncidentParms parms = new IncidentParms();

		// Token: 0x04000897 RID: 2199
		public StorytellerComp source;
	}
}
