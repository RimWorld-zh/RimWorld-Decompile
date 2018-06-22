using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B5F RID: 2911
	public class SkillGain
	{
		// Token: 0x06003F97 RID: 16279 RVA: 0x0021856F File Offset: 0x0021696F
		public SkillGain()
		{
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x00218578 File Offset: 0x00216978
		public SkillGain(SkillDef skill, int xp)
		{
			this.skill = skill;
			this.xp = xp;
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x00218590 File Offset: 0x00216990
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

		// Token: 0x04002A7A RID: 10874
		public SkillDef skill;

		// Token: 0x04002A7B RID: 10875
		public int xp;
	}
}
