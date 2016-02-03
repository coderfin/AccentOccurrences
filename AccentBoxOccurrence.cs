using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace AccentOccurrences
{
	internal class AccentBoxOccurrences : TextMarkerTag, IGlyphTag
	{
		public AccentBoxOccurrences() : base(Constants.BOX_FORMAT_NAME) { }
	}
}