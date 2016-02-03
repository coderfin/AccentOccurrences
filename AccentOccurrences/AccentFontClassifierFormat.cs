using AccentOccurrences;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace AccentFontClassifier
{
	[ClassificationType(ClassificationTypeNames = "EditorFormatDefinition/AccentFontClassifier")] // Constants.FONT_FORMAT_NAME
	[Export(typeof(EditorFormatDefinition))]
	[Name("EditorFormatDefinition/AccentFontClassifier")] // Constants.FONT_FORMAT_NAME
	[Order(After = Priority.High)] // Overrides any default styles for the text, this ensures that the styles listed below are applied
	[UserVisible(true)]
	internal sealed class AccentFontClassifierFormat : ClassificationFormatDefinition
	{
		public AccentFontClassifierFormat()
		{
			this.DisplayName = Constants.FONT_DISPLAY_NAME;

			this.ForegroundColor = Settings.Default.Font;
			this.BackgroundCustomizable = false;
			this.BackgroundOpacity = 0.0;
		}
	}
}