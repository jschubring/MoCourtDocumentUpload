using System.IO;
using System.Text;

namespace MoCourtDocumentUpload.Extensions
{
    public sealed class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;

        public StringWriterWithEncoding()
        {
            this.encoding = Encoding.UTF8;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}
