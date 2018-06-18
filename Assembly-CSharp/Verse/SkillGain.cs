using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B63 RID: 2915
	public class SkillGain
	{
		// Token: 0x06003F96 RID: 16278 RVA: 0x00217F07 File Offset: 0x00216307
		public SkillGain()
		{
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x00217F10 File Offset: 0x00216310
		public SkillGain(SkillDef skill, int xp)
		{
			this.skill = skill;
			this.xp = xp;
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x00217F28 File Offset: 0x00216328
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

		// Token: 0x04002A79 RID: 10873
		public SkillDef skill;

		// Token: 0x04002A7A RID: 10874
		public int xp;
	}
}
