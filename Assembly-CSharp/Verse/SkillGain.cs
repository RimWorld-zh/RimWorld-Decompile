using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B61 RID: 2913
	public class SkillGain
	{
		// Token: 0x04002A7A RID: 10874
		public SkillDef skill;

		// Token: 0x04002A7B RID: 10875
		public int xp;

		// Token: 0x06003F9A RID: 16282 RVA: 0x0021864B File Offset: 0x00216A4B
		public SkillGain()
		{
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x00218654 File Offset: 0x00216A54
		public SkillGain(SkillDef skill, int xp)
		{
			this.skill = skill;
			this.xp = xp;
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x0021866C File Offset: 0x00216A6C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured SkillGain: " + xmlRoot.OuterXml, false);
			}
			else
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name);
				this.xp = (int)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(int));
			}
		}
	}
}
