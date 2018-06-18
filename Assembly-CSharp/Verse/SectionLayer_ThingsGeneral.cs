using System;

namespace Verse
{
	// Token: 0x02000C55 RID: 3157
	public class SectionLayer_ThingsGeneral : SectionLayer_Things
	{
		// Token: 0x0600456F RID: 17775 RVA: 0x0024AB9E File Offset: 0x00248F9E
		public SectionLayer_ThingsGeneral(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Things;
			this.requireAddToMapMesh = true;
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x0024ABB8 File Offset: 0x00248FB8
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
