using AccentOccurrences.Properties;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace AccentOccurrences
{
	[ClassificationType(ClassificationTypeNames = "EditorFormatDefinition/AccentGlyph")] // Constants.GLYPH_FORMAT_NAME
	[Export(typeof(EditorFormatDefinition))]
	[Name("EditorFormatDefinition/AccentGlyph")] // Constants.GLYPH_FORMAT_NAME
	[Order(After = Priority.High)] // Overrides any default styles for the text, this ensures that the styles listed below are applied
	[UserVisible(true)]
	internal class AccentGlyphFormatDefinition : ClassificationFormatDefinition
	{
		public AccentGlyphFormatDefinition()
		{
			this.DisplayName = Constants.GLYPH_DISPLAY_NAME;

			this.BackgroundBrush = new SolidColorBrush(Settings.Default.GlyphFill);
			this.ForegroundBrush = new SolidColorBrush(Settings.Default.GlyphBorder);
		}
	}
}