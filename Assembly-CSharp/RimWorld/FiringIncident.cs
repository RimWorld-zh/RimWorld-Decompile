using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000317 RID: 791
	public class FiringIncident : IExposable
	{
		// Token: 0x04000898 RID: 2200
		public IncidentDef def;

		// Token: 0x04000899 RID: 2201
		public IncidentParms parms = new IncidentParms();

		// Token: 0x0400089A RID: 2202
		public StorytellerComp source;

		// Token: 0x06000D62 RID: 3426 RVA: 0x000734DF File Offset: 0x000718DF
		public FiringIncident()
		{
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x000734F3 File Offset: 0x000718F3
		public FiringIncident(IncidentDef def, StorytellerComp source, IncidentParms parms = null)
		{
			this.def = def;
			if (parms != null)
			{
				this.parms = parms;
			}
			this.source = source;
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00073522 File Offset: 0x00071922
		public void ExposeData()
		{
			Scribe_Defs.Look<IncidentDef>(ref this.def, "def");
			Scribe_Deep.Look<IncidentParms>(ref this.parms, "parms", new object[0]);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0007354C File Offset: 0x0007194C
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
