using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000576 RID: 1398
	public class WorldFeatureTextMesh_Legacy : WorldFeatureTextMesh
	{
		// Token: 0x06001A88 RID: 6792 RVA: 0x000E4C60 File Offset: 0x000E3060
		private static void TextScaleFactor_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x000E4C70 File Offset: 0x000E3070
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001A8A RID: 6794 RVA: 0x000E4C98 File Offset: 0x000E3098
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x000E4CC0 File Offset: 0x000E30C0
		// (set) Token: 0x06001A8C RID: 6796 RVA: 0x000E4CE0 File Offset: 0x000E30E0
		public override Color Color
		{
			get
			{
				return this.textMesh.color;
			}
			set
			{
				this.textMesh.color = value;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001A8D RID: 6797 RVA: 0x000E4CF0 File Offset: 0x000E30F0
		// (set) Token: 0x06001A8E RID: 6798 RVA: 0x000E4D10 File Offset: 0x000E3110
		public override string Text
		{
			get
			{
				return this.textMesh.text;
			}
			set
			{
				this.textMesh.text = value;
			}
		}

		// Token: 0x170003CB RID: 971
		// (set) Token: 0x06001A8F RID: 6799 RVA: 0x000E4D1F File Offset: 0x000E311F
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = Mathf.RoundToInt(value * WorldFeatureTextMesh_Legacy.TextScaleFactor);
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x000E4D3C File Offset: 0x000E313C
		// (set) Token: 0x06001A91 RID: 6801 RVA: 0x000E4D61 File Offset: 0x000E3161
		public override Quaternion Rotation
		{
			get
			{
				return this.textMesh.transform.rotation;
			}
			set
			{
				this.textMesh.transform.rotation = value;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x000E4D78 File Offset: 0x000E3178
		// (set) Token: 0x06001A93 RID: 6803 RVA: 0x000E4D9D File Offset: 0x000E319D
		public override Vector3 LocalPosition
		{
			get
			{
				return this.textMesh.transform.localPosition;
			}
			set
			{
				this.textMesh.transform.localPosition = value;
			}
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x000E4DB1 File Offset: 0x000E31B1
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x000E4DC5 File Offset: 0x000E31C5
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x000E4DD8 File Offset: 0x000E31D8
		public override void Init()
		{
			GameObject gameObject = new GameObject("World feature name (legacy)");
			gameObject.layer = WorldCameraManager.WorldLayer;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.AddComponent<TextMesh>();
			this.textMesh.color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.anchor = TextAnchor.MiddleCenter;
			this.textMesh.alignment = TextAlignment.Center;
			this.textMesh.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = WorldMaterials.FeatureNameRenderQueue;
			this.Color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000E4EA7 File Offset: 0x000E32A7
		public override void WrapAroundPlanetSurface()
		{
		}

		// Token: 0x04000F76 RID: 3958
		private TextMesh textMesh;

		// Token: 0x04000F77 RID: 3959
		private const float TextScale = 0.23f;

		// Token: 0x04000F78 RID: 3960
		private const int MinFontSize = 13;

		// Token: 0x04000F79 RID: 3961
		private const int MaxFontSize = 40;

		// Token: 0x04000F7A RID: 3962
		[TweakValue("Interface.World", 0f, 10f)]
		private static float TextScaleFactor = 7.5f;
	}
}
