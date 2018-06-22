using System;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F69 RID: 3945
	public class ShaderParameter
	{
		// Token: 0x06005F5F RID: 24415 RVA: 0x00309F44 File Offset: 0x00308344
		public void Apply(Material mat)
		{
			ShaderParameter.Type type = this.type;
			if (type != ShaderParameter.Type.Float)
			{
				if (type != ShaderParameter.Type.Vector)
				{
					if (type == ShaderParameter.Type.Texture)
					{
						if (this.valueTex == null)
						{
							Log.ErrorOnce(string.Format("Texture for {0} is not yet loaded; file may be invalid, or main thread may not have loaded it yet", this.name), 27929440, false);
						}
						mat.SetTexture(this.name, this.valueTex);
					}
				}
				else
				{
					mat.SetVector(this.name, this.value);
				}
			}
			else
			{
				mat.SetFloat(this.name, this.value.x);
			}
		}

		// Token: 0x06005F60 RID: 24416 RVA: 0x00309FE8 File Offset: 0x003083E8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ShaderParameter: " + xmlRoot.OuterXml, false);
			}
			else
			{
				this.name = xmlRoot.Name;
				string valstr = xmlRoot.FirstChild.Value;
				if (!valstr.NullOrEmpty() && valstr[0] == '(')
				{
					this.value = ParseHelper.FromStringVector4Adaptive(valstr);
					this.type = ShaderParameter.Type.Vector;
				}
				else if (!valstr.NullOrEmpty() && valstr[0] == '/')
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						this.valueTex = ContentFinder<Texture2D>.Get(valstr.TrimStart(new char[]
						{
							'/'
						}), true);
					});
					this.type = ShaderParameter.Type.Texture;
				}
				else
				{
					this.value = Vector4.one * (float)ParseHelper.FromString(valstr, typeof(float));
					this.type = ShaderParameter.Type.Float;
				}
			}
		}

		// Token: 0x04003EB2 RID: 16050
		[NoTranslate]
		private string name;

		// Token: 0x04003EB3 RID: 16051
		private Vector4 value;

		// Token: 0x04003EB4 RID: 16052
		private Texture2D valueTex;

		// Token: 0x04003EB5 RID: 16053
		private ShaderParameter.Type type;

		// Token: 0x02000F6A RID: 3946
		private enum Type
		{
			// Token: 0x04003EB7 RID: 16055
			Float,
			// Token: 0x04003EB8 RID: 16056
			Vector,
			// Token: 0x04003EB9 RID: 16057
			Matrix,
			// Token: 0x04003EBA RID: 16058
			Texture
		}
	}
}
