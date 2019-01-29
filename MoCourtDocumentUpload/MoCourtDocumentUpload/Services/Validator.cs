using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MoCourtDocumentUpload.Services
{
    public class Validator
    {
        private string _nsNiemCore;
        private string _nsMoEcf;

        public Validator(string nsNiemCore = null, string nsMoEcf = null)
        {
            _nsNiemCore = nsNiemCore ?? "http://niem.gov/niem/niem-core/2.0";
            _nsMoEcf = nsMoEcf ?? "http://www.courts.mo.gov/exchanges/MoEcfExchange/1.0";
        }

        public bool ValidateDocument(string document)
        {
            var valid = false;
            try
            {
                var xml = BuildXmlDocument();

                xml.LoadXml(document);

                var eventHandler = new ValidationEventHandler(ValidationEventHandler);

                xml.Validate(eventHandler);
                valid = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: ", ex);
            }

            return valid;
        }

        private XmlDocument BuildXmlDocument()
        {
            var xml = new XmlDocument();

            xml.Schemas.Add(_nsMoEcf, "MoEcfExchange.xsd");
            xml.Schemas.Add(_nsNiemCore, "niem-core.xsd");
            xml.Schemas.Add("http://niem.gov/niem/proxy/xsd/2.0", "xsd.xsd");
            xml.Schemas.Add("http://niem.gov/niem/structures/2.0", "structures.xsd");
            xml.Schemas.Add("http://niem.gov/niem/domains/jxdm/4.1", "jxdm.xsd");
            xml.Schemas.Add("http://www.courts.mo.gov/exchanges/MoEcfExchangeExtensions/1.0", "MoEcfExchangeExtensions.xsd");

            xml.Schemas.Add("http://niem.gov/niem/appinfo/2.0", "appinfo20.xsd");
            xml.Schemas.Add("http://niem.gov/niem/appinfo/2.1", "appinfo21.xsd");
            xml.Schemas.Add("http://niem.gov/niem/fbi/2.0", "fbi.xsd");
            xml.Schemas.Add("http://niem.gov/niem/iso_3166/2.0", "iso_3166.xsd");
            xml.Schemas.Add("http://niem.gov/niem/iso_4217/2.0", "iso_4217.xsd");
            xml.Schemas.Add("http://www.courts.mo.gov/exchanges/ExchangeDocument/1.0", "MoExchangeDocument.xsd");
            xml.Schemas.Add("http://niem.gov/niem/usps_states/2.0", "usps_states.xsd");

            return xml;
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}
