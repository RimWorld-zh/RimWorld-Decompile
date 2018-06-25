using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B62 RID: 2914
	public class SkillGain
	{
		// Token: 0x04002A81 RID: 10881
		public SkillDef skill;

		// Token: 0x04002A82 RID: 10882
		public int xp;

		// Token: 0x06003F9A RID: 16282 RVA: 0x0021892B File Offset: 0x00216D2B
		public SkillGain()
		{
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x00218934 File Offset: 0x00216D34
		public SkillGain(SkillDef skill, int xp)
		{
			this.skill = skill;
			this.xp = xp;
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x0021894C File Offset: 0x00216D4C
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
