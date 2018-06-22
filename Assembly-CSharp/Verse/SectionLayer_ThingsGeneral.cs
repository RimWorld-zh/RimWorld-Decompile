using System;

namespace Verse
{
	// Token: 0x02000C52 RID: 3154
	public class SectionLayer_ThingsGeneral : SectionLayer_Things
	{
		// Token: 0x06004578 RID: 17784 RVA: 0x0024BF6E File Offset: 0x0024A36E
		public SectionLayer_ThingsGeneral(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Things;
			this.requireAddToMapMesh = true;
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x0024BF88 File Offset: 0x0024A388
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
