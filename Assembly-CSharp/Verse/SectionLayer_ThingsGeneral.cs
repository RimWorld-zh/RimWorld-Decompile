using System;

namespace Verse
{
	// Token: 0x02000C55 RID: 3157
	public class SectionLayer_ThingsGeneral : SectionLayer_Things
	{
		// Token: 0x0600457B RID: 17787 RVA: 0x0024C32A File Offset: 0x0024A72A
		public SectionLayer_ThingsGeneral(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Things;
			this.requireAddToMapMesh = true;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x0024C344 File Offset: 0x0024A744
		protected override void TakePrintFrom(Thing t)
		{
			try
			{
				t.Print(this);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception printing ",
					t,
					" at ",
					t.Position,
					": ",
					ex.ToString()
				}), false);
			}
		}
	}
}
