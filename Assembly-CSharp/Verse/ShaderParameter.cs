using System;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F6E RID: 3950
	public class ShaderParameter
	{
		// Token: 0x04003EBD RID: 16061
		[NoTranslate]
		private string name;

		// Token: 0x04003EBE RID: 16062
		private Vector4 value;

		// Token: 0x04003EBF RID: 16063
		private Texture2D valueTex;

		// Token: 0x04003EC0 RID: 16064
		private ShaderParameter.Type type;

		// Token: 0x06005F69 RID: 24425 RVA: 0x0030A808 File Offset: 0x00308C08
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

		// Token: 0x06005F6A RID: 24426 RVA: 0x0030A8AC File Offset: 0x00308CAC
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

		// Token: 0x02000F6F RID: 3951
		private enum Type
		{
			// Token: 0x04003EC2 RID: 16066
			Float,
			// Token: 0x04003EC3 RID: 16067
			Vector,
			// Token: 0x04003EC4 RID: 16068
			Matrix,
			// Token: 0x04003EC5 RID: 16069
			Texture
		}
	}
}
