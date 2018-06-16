using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000576 RID: 1398
	public class WorldFeatureTextMesh_Legacy : WorldFeatureTextMesh
	{
		// Token: 0x06001A87 RID: 6791 RVA: 0x000E4C0C File Offset: 0x000E300C
		private static void TextScaleFactor_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001A88 RID: 6792 RVA: 0x000E4C1C File Offset: 0x000E301C
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x000E4C44 File Offset: 0x000E3044
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001A8A RID: 6794 RVA: 0x000E4C6C File Offset: 0x000E306C
		// (set) Token: 0x06001A8B RID: 6795 RVA: 0x000E4C8C File Offset: 0x000E308C
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
		// (get) Token: 0x06001A8C RID: 6796 RVA: 0x000E4C9C File Offset: 0x000E309C
		// (set) Token: 0x06001A8D RID: 6797 RVA: 0x000E4CBC File Offset: 0x000E30BC
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
		// (set) Token: 0x06001A8E RID: 6798 RVA: 0x000E4CCB File Offset: 0x000E30CB
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = Mathf.RoundToInt(value * WorldFeatureTextMesh_Legacy.TextScaleFactor);
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x000E4CE8 File Offset: 0x000E30E8
		// (set) Token: 0x06001A90 RID: 6800 RVA: 0x000E4D0D File Offset: 0x000E310D
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
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x000E4D24 File Offset: 0x000E3124
		// (set) Token: 0x06001A92 RID: 6802 RVA: 0x000E4D49 File Offset: 0x000E3149
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

		// Token: 0x06001A93 RID: 6803 RVA: 0x000E4D5D File Offset: 0x000E315D
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x000E4D71 File Offset: 0x000E3171
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x000E4D84 File Offset: 0x000E3184
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

		// Token: 0x06001A96 RID: 6806 RVA: 0x000E4E53 File Offset: 0x000E3253
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
