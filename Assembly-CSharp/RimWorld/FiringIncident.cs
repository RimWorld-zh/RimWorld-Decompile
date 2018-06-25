using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000317 RID: 791
	public class FiringIncident : IExposable
	{
		// Token: 0x04000895 RID: 2197
		public IncidentDef def;

		// Token: 0x04000896 RID: 2198
		public IncidentParms parms = new IncidentParms();

		// Token: 0x04000897 RID: 2199
		public StorytellerComp source;

		// Token: 0x06000D63 RID: 3427 RVA: 0x000734D7 File Offset: 0x000718D7
		public FiringIncident()
		{
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x000734EB File Offset: 0x000718EB
		public FiringIncident(IncidentDef def, StorytellerComp source, IncidentParms parms = null)
		{
			this.def = def;
			if (parms != null)
			{
				this.parms = parms;
			}
			this.source = source;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0007351A File Offset: 0x0007191A
		public void ExposeData()
		{
			Scribe_Defs.Look<IncidentDef>(ref this.def, "def");
			Scribe_Deep.Look<IncidentParms>(ref this.parms, "parms", new object[0]);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00073544 File Offset: 0x00071944
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
	}
}
