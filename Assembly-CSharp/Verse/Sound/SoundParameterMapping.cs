using System;

namespace Verse.Sound
{
	// Token: 0x02000B9E RID: 2974
	public class SoundParameterMapping
	{
		// Token: 0x04002B3F RID: 11071
		[Description("The independent parameter that the game will change to drive this relationship.\n\nOn the graph, this is the X axis.")]
		public SoundParamSource inParam = null;

		// Token: 0x04002B40 RID: 11072
		[Description("The dependent parameter that will respond to changes to the in-parameter.\n\nThis must match something the game can change about this sound.\n\nOn the graph, this is the y-axis.")]
		public SoundParamTarget outParam = null;

		// Token: 0x04002B41 RID: 11073
		[Description("Determines when sound parameters should be applies to samples.\n\nConstant means the parameters are updated every frame and can change continuously.\n\nOncePerSample means that the parameters are applied exactly once to each sample that plays.")]
		public SoundParamUpdateMode paramUpdateMode = SoundParamUpdateMode.Constant;

		// Token: 0x04002B42 RID: 11074
		[EditorHidden]
		public SimpleCurve curve;

		// Token: 0x06004064 RID: 16484 RVA: 0x0021D768 File Offset: 0x0021BB68
		public SoundParameterMapping()
		{
			this.curve = new SimpleCurve();
			this.curve.Add(new CurvePoint(0f, 0f), true);
			this.curve.Add(new CurvePoint(1f, 1f), true);
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x0021D7D4 File Offset: 0x0021BBD4
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			string title = ((this.inParam == null) ? "null" : this.inParam.Label) + " -> " + ((this.outParam == null) ? "null" : this.outParam.Label);
			if (widgetRow.ButtonText("Edit curve", "Edit the curve mapping the in parameter to the out parameter.", true, false))
			{
				Find.WindowStack.Add(new EditWindow_CurveEditor(this.curve, title));
			}
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0021D85C File Offset: 0x0021BC5C
		public void Apply(Sample samp)
		{
			if (this.inParam != null && this.outParam != null)
			{
				float num = this.inParam.ValueFor(samp);
				float value = this.curve.Evaluate(num);
				this.outParam.SetOn(samp, value);
				if (UnityData.isDebugBuild && this.curve.HasView)
				{
					this.curve.View.SetDebugInput(samp, num);
				}
			}
		}
	}
}
