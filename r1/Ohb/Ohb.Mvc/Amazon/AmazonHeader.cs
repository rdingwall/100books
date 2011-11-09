using System.ServiceModel.Channels;
using System.Xml;

namespace Ohb.Mvc.Amazon
{
    // from http://flyingpies.wordpress.com/2009/08/01/17/
    public class AmazonHeader : MessageHeader
    {
        private readonly string name;
        private readonly string value;

        public AmazonHeader(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public override string Name { get { return name; } }
        public override string Namespace { get { return "http://security.amazonaws.com/doc/2007-01-01/"; } }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter xmlDictionaryWriter, MessageVersion messageVersion)
        {
            xmlDictionaryWriter.WriteString(value);
        }
    }
}
