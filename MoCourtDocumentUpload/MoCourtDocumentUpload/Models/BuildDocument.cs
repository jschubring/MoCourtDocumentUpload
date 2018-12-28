using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MoCourtDocumentUpload.Models
{
    public class BuildDocument
    {
        public string ReturnDocumentXML()
        {
            var document = new BuildDocument().BuildMoEcfExchangeType();



            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("mo-ecf", "http://www.courts.mo.gov/exchanges/MoEcfExchange/1.0");
            ns.Add("nc", "http://niem.gov/niem/niem-core/2.0");
            ns.Add("s", "http://niem.gov/niem/structures/2.0");
            ns.Add("j", "http://niem.gov/niem/domains/jxdm/4.1");
            ns.Add("mo-ecf-ext", "http://www.courts.mo.gov/exchanges/MoEcfExchangeExtensions/1.0");

            XmlSerializer xser = new XmlSerializer(typeof(MoEcfExchangeType));

            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xser.Serialize(writer, document, ns);
                    xml = sww.ToString(); // Your XML
                }
            }
            return xml;
        }


        public MoEcfExchangeType BuildMoEcfExchangeType()
        {
            MoEcfExchangeType mo = new MoEcfExchangeType()
            {
                Person = BuildPersons(),
                Organization = BuildOrganizations(),
                //not done
                ContactInformation = BuildContactInformations(),
                Case = new CaseType1()
                {
                    CaseAugmentation = new CaseAugmentationType()
                    {
                        CaseCourt = new CourtType()
                        {
                            CourtDivisionText = BuildTextType("AD"),
                            OrganizationName = BuildTextType("OrgName"),
                            OrganizationOtherIdentification = BuildIdentificationTypes()
                        },
                        CaseOtherIdentification = BuildIdentificationType()
                    },
                    CaseCategoryText = BuildTextType("CP"),
                    CaseTitleText = BuildTextType("Ryan V Justin")
                },
                CommentText = BuildTextType("This is a comment"),
                Fee = new ObligationType1()
                {
                    // TODO finish payment
                    Item1 = { },
                    Item = new AmountType()
                    {
                        currencyCode = "",
                        currencyText = "",
                        Value = decimal.Parse("22.00")
                    },
                    ObligationExemption = new List<ObligationExemptionType1>()
                    {
                        new ObligationExemptionType1()
                        {
                            ObligationExemptionCode = new ObligationExemptionCodeType()
                            {
                                Value = ObligationExemptionCodeSimpleType.IFP
                            }
                        }
                    }.ToArray()
                },
                Identification = new List<IdentificationType>(){
                    new IdentificationType()
                    {
                        IdentificationID = BuildTextType("ecfUser001"),
                        IdentificationCategory = IdentificationCategoryCodeSimpleType.USERNAME,
                    }
                }.ToArray(),
                PrimaryDocument = new List<DocumentType1>()
                {
                
                }.ToArray(),
                PersonContactInformationAssociation = BuildContactInformationAssociations(),
                EntityCaseAssociation = null,
                EntityDocumentAssociation = null,
                OrganizationContactInformationAssociation = null
            };
            return mo;
        }

        private static TextType BuildTextType(string value)
        {
            return new TextType() { Value = value };
        }

        private static PersonContactInformationAssociationType[] BuildContactInformationAssociations()
        {
            return new List<PersonContactInformationAssociationType>
                {
                    BuildContactInformationAssociation()
                }.ToArray();
        }
        private static PersonContactInformationAssociationType BuildContactInformationAssociation()
        {
            return new PersonContactInformationAssociationType()
            {
                ContactInformationReference = new ReferenceType()
                {

                }
            };
        }
        private static ContactInformationType[] BuildContactInformations()
        {
            return new List<ContactInformationType>
                {
                    BuildContactInformation()
                }.ToArray();
        }

        private static ContactInformationType BuildContactInformation()
        {
            return new ContactInformationType()
            {

            };
        }

        private static OrganizationType[] BuildOrganizations()
        {
            return new List<OrganizationType>
                {
                    BuildOrganization()
                }.ToArray();
        }

        private static OrganizationType BuildOrganization()
        {
            return new OrganizationType()
            {
                OrganizationName = new TextType()
                {
                    Value = "Test Company"
                },
                OrganizationOtherIdentification = BuildIdentificationTypes()
            };
        }

        private static IdentificationType[] BuildIdentificationTypes()
        {
            return new List<IdentificationType> {

                    new IdentificationType()
                    {
                        IdentificationCategory =  IdentificationCategoryCodeSimpleType.EIN,
                        IdentificationID = new @string()
                        {
                            Value = "TESTID"
                        }
                    }
                }.ToArray();
        }

        private static PersonType[] BuildPersons()
        {
            return new List<PersonType>
                {
                    BuildPerson()
                }.ToArray();
        }

        private static PersonType BuildPerson()
        {
            return new PersonType()
            {
                PersonBirthDate = new DateType()
                {
                    Item = new date()
                    {
                        Value = DateTime.Parse("4/13/1991")
                    }
                },
                PersonName = new PersonNameType()
                {
                    PersonGivenName = new PersonNameTextType()
                    {
                        Value = "Ryan"
                    },
                    PersonMiddleName = new PersonNameTextType()
                    {
                        Value = "Richard"
                    },
                    PersonNameSuffixText = new PersonNameTextType()
                    {
                        Value = "Mr."
                    },
                    PersonSurName = new PersonNameTextType()
                    {
                        Value = "Schlueter"
                    }
                },
                PersonSSNIdentification = new IdentificationType()
                {
                    //IdentificationCategory = new IdentificationCategoryCodeType()
                    //{
                    //    Value = IdentificationCategoryCodeSimpleType.SSN
                    //},
                    IdentificationID = new @string()
                    {
                        Value = "99-999-9999"
                    }
                },
                PersonOtherIdentification = BuildIdentificationType(),
                Item = new SEXCodeType
                {
                    Value = SEXCodeSimpleType.M
                }
            };
        }

        private static IdentificationType BuildIdentificationType()
        {
            return new IdentificationType()
            {
                //IdentificationCategory = new IdentificationCategoryCodeType()
                //{
                //    Value = IdentificationCategoryCodeSimpleType.USERNAME
                //},
                IdentificationID = new @string()
                {
                    Value = "USERNAME"
                }
            };
        }
    }
}