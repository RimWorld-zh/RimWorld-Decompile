using System;

namespace Verse
{
	// Token: 0x02000C56 RID: 3158
	public class SectionLayer_ThingsGeneral : SectionLayer_Things
	{
		// Token: 0x06004571 RID: 17777 RVA: 0x0024ABC6 File Offset: 0x00248FC6
		public SectionLayer_ThingsGeneral(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Things;
			this.requireAddToMapMesh = true;
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x0024ABE0 File Offset: 0x00248FE0
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
