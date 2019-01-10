﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MoCourtDocumentUpload.Models
{
    public class BuildDocument
    {
        public string ReturnDocumentXML()
        {          
            var document = new BuildDocument().BuildMoEcfExchangeType(new RootObject());

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("mo-ecf", "http://www.courts.mo.gov/exchanges/MoEcfExchange/1.0");
            ns.Add("nc", "http://niem.gov/niem/niem-core/2.0");
            ns.Add("s", "http://niem.gov/niem/structures/2.0");
            ns.Add("j", "http://niem.gov/niem/domains/jxdm/4.1");
            ns.Add("mo-ecf-ext", "http://www.courts.mo.gov/exchanges/MoEcfExchangeExtensions/1.0");

            XmlSerializer xser = new XmlSerializer(typeof(MoEcfExchangeType));

            var xml = "";

            using (var sww = new StringWriterWithEncoding())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xser.Serialize(writer, document, ns);
                    xml = sww.ToString(); // Your XML
                }
            }
            return xml;
        }



        public MoEcfExchangeType BuildMoEcfExchangeType(RootObject root)
        {
            MoEcfExchangeType mo = new MoEcfExchangeType()
            {
                Person = BuildPersons(root.People),
                Organization = BuildOrganizations(root.Organizations),
                ContactInformation = BuildContactInformations(root.Contacts),
                Case = BuildCase(root.Case),
                CommentText = BuildTextType(root.CommentText),
                Fee = BuildFee(root.CaseFee),
                Identification = new List<IdentificationType>(){
                    new IdentificationType()
                    {
                        IdentificationID = BuildTextType("ATTYCASE0001"),
                        IdentificationCategory = IdentificationCategoryCodeSimpleType.ATTYREFNO,
                    }, new IdentificationType()
                    {
                        IdentificationID = BuildTextType("ecfUser001"),
                        IdentificationCategory = IdentificationCategoryCodeSimpleType.USERNAME,
                    },
                }.ToArray(),
                //PrimaryDocument = new List<DocumentType1>()
                //{
                //    BuildDocumentObject(new Document(){
                //        Category = "APTAC",
                //        Format ="application/pdf",
                //        ID ="1",
                //        Title = "PETITION 1",
                //        ByteArray = Document.Example1
                //    }),
                //      BuildDocumentObject(new Document(){
                //        Category = "APTAC",
                //        Format ="application/pdf",
                //        ID ="1",
                //        Title = "ATTACHMENT 2",
                //        ByteArray = Document.Example1
                //    }),
                //       BuildDocumentObject(new Document(){
                //        Category = "APTAC",
                //        Format ="application/pdf",
                //        ID ="1",
                //        Title = "PETITION 2",
                //        ByteArray = Document.Example1
                //    }),
                //        BuildDocumentObject(new Document(){
                //        Category = "APTAC",
                //        Format ="application/pdf",
                //        ID ="1",
                //        Title = "ATTACHMENT 1",
                //        ByteArray = Document.Example1
                //    }),

                //}.ToArray(),
                PersonContactInformationAssociation = BuildContactInformationAssociations(),
                EntityCaseAssociation = new List<EntityCaseAssociationType>()
                {
                    BuildEntityCaseAssociationType("c", "o1", null, "PET"),
                     BuildEntityCaseAssociationType("c",null, "p1", "PET"),
                      BuildEntityCaseAssociationType("c", "o2",null, "RES"),
                       BuildEntityCaseAssociationType("c",null, "p2", "RES")
                }.ToArray(),
                EntityDocumentAssociation = BuildEntityDocumentAssocation(),
                OrganizationContactInformationAssociation = new List<OrganizationContactInformationAssociationType>()
                {
                    BuildOrganizationContactInformationAssociation("o1", "ci3"),
                    BuildOrganizationContactInformationAssociation("o2", "ci4")
                }.ToArray()
            };
            return mo;
        }

        private static DocumentType1 BuildDocumentObject(Document document)
        {
            return new DocumentType1()
            {
                id = document.ID,
                DocumentBinary = new BinaryType()
                {
                    BinaryFormatID = BuildTextType(document.Format)
                },
                DocumentTitleText = BuildTextType(document.Title),
                DocumentCategoryText = BuildTextType(document.Category),
                Document = new DocumentType[1]
                                   {
                           new DocumentType()
                           {
                               DocumentBinary = new BinaryType()
                               {
                                   Item = new base64Binary(){
                                       Value = document.ByteArray
                                   },
                                   BinaryFormatID = BuildTextType(document.Format + "Z")
                               }
                           }
                                   }
            };
        }

        private static EntityDocumentAssociationType[] BuildEntityDocumentAssocation()
        {
            return new List<EntityDocumentAssociationType>(){
                BuildEntityAssociation("pd1", "p1", null),
                BuildEntityAssociation("pd1", null, "o1"),
                BuildEntityAssociation("pd2", "p1", null),
                 BuildEntityAssociation("pd2", null,"o1")
                }.ToArray();
        }

        private static EntityDocumentAssociationType BuildEntityAssociation(string documentRef, string personRef, string orgRef)
        {
            var docAssociation = new EntityDocumentAssociationType()
            {
                DocumentReference = BuildReference(documentRef)
            };
            if (!string.IsNullOrEmpty(orgRef))
            {
                docAssociation.EntityOrganizationReference = BuildReference(orgRef);
            }
            if (!string.IsNullOrEmpty(personRef))
            {
                docAssociation.EntityPersonReference = BuildReference(personRef);
            }
            return docAssociation;

        }
        private static OrganizationContactInformationAssociationType BuildOrganizationContactInformationAssociation(string organizationRef, string contactRef)
        {
            return new OrganizationContactInformationAssociationType()
            {
                OrganizationReference = BuildReference(organizationRef),
                ContactInformationReference = BuildReference(contactRef)
            };
        }
        private static EntityCaseAssociationType BuildEntityCaseAssociationType(string caseRef, string orgRef, string personRef, string associationDescription)
        {
            var caseAssociation = new EntityCaseAssociationType()
            {
                CaseReference = BuildReference(caseRef),
                AssociationDescriptionText = BuildTextType(associationDescription)
            };
            if (!string.IsNullOrEmpty(orgRef))
            {
                caseAssociation.EntityOrganizationReference = BuildReference(orgRef);
            }
            if (!string.IsNullOrEmpty(personRef))
            {
                caseAssociation.EntityPersonReference = BuildReference(personRef);
            }
            return caseAssociation;
        }
        private static ReferenceType BuildReference(string reference)
        {
            return new ReferenceType()
            {
                @ref = reference
            };
        }

        private static ObligationType1 BuildFee(Fee fee)
        {
            return new ObligationType1()
            {
                // TODO finish payment
                Item1 = new PaymentElectronicBankDraftType()
                {
                    AccountNumber = BuildTextType(fee.AccountNumber),
                    RoutingNumber = BuildTextType(fee.RoutingNumber),
                    BankAccountCategoryCode = new BankAccountCategoryCodeType()
                    {
                        Value = BankAccountCategoryCodeSimpleType.C
                    },
                    CheckCategoryCode = new CheckCategoryCodeType()
                    {
                        Value = CheckCategoryCodeSimpleType.P
                    }
                },
                Item = new AmountType()
                {
                    Value = decimal.Parse(fee.Amount)
                },
                //ObligationExemption = new List<ObligationExemptionType1>()
                //    {
                //        new ObligationExemptionType1()
                //        {
                //            ObligationExemptionCode = new ObligationExemptionCodeType()
                //            {
                //                Value = ObligationExemptionCodeSimpleType.IFP
                //            }
                //        }
                //    }.ToArray()
            };
        }

        private static CaseType1 BuildCase(CaseDetails courtCase)
        {
            return new CaseType1()
            {
                CaseAugmentation = new CaseAugmentationType()
                {
                    CaseCourt = new CourtType()
                    {
                        CourtDivisionText = BuildTextType(courtCase.Division),
                        //OrganizationName = BuildTextType("OrgName"),
                        //OrganizationOtherIdentification = BuildIdentificationTypes("")
                    },
                    //CaseOtherIdentification = BuildIdentificationType()
                },
                CaseCategoryText = BuildTextType(courtCase.Category),
                CaseTitleText = BuildTextType(courtCase.Title),
                id = courtCase.ID
            };
        }

        private static TextType BuildTextType(string value)
        {
            return new TextType() { Value = value };
        }

        private static PersonContactInformationAssociationType[] BuildContactInformationAssociations()
        {
            return new List<PersonContactInformationAssociationType>
                {
                    BuildContactInformationAssociation("p1", "ci1"),
                    BuildContactInformationAssociation("p2", "ci2")
                }.ToArray();
        }
        private static PersonContactInformationAssociationType BuildContactInformationAssociation(string personRef, string contactRef)
        {
            return new PersonContactInformationAssociationType()
            {
                ContactInformationReference = BuildReference(contactRef),
                PersonReference = BuildReference(personRef)
            };
        }
        private static ContactInformationType[] BuildContactInformations(List<Contact> contacts)
        {
            return contacts.Select(x => BuildContactInformation(x)).ToArray();
        }

        private static ContactInformationType BuildContactInformation(Contact contact)
        {
            var contactInfo = new ContactInformationType()
            {
                id = "ci" + contact.ID,
                ContactMailingAddress = new AddressType()
                {
                    Item = new StructuredAddressType()
                    {
                        Items = contact.StreetAdress.Select(x => new StreetType()
                        {
                            StreetFullText = BuildTextType(x)
                        }).ToArray(),
                        LocationCityName = new ProperNameTextType() { Value = contact.City },
                        LocationPostalCode = BuildTextType(contact.ZipCode),

                        Item1 = new CountryAlpha2CodeType()
                        {
                            Value = contact.Country
                        },

                    }
                },
                ContactTelephoneNumber = new TelephoneNumberType()
                {
                    Item = new NANPTelephoneNumberType()
                    {
                        TelephoneAreaCodeID = BuildTextType(contact.AreaCode),
                        TelephoneExchangeID = BuildTextType(contact.ExchangeID),
                        TelephoneLineID = BuildTextType(contact.LineID),
                    }
                }
            };
            if (!string.IsNullOrEmpty(contact.Email))
            {
                contactInfo.ContactEmailID = BuildTextType(contact.Email);
            }
            if (!string.IsNullOrEmpty(contact.State))
            {
                contactInfo.ContactMailingAddress.Item.LocationStateName = new ProperNameTextType() { Value = contact.State };
            }
            if (contact.UsState != null)
            {
                contactInfo.ContactMailingAddress.Item.LocationStateUSPostalServiceCode = new USStateCodeType()
                {
                    Value = contact.UsState.Value
                };

            }

            return contactInfo;
        }

        private static OrganizationType[] BuildOrganizations(List<Organization> organizations)
        {
            return organizations.Select(x => BuildOrganization(x)).ToArray();          
        }

        private static OrganizationType BuildOrganization(Organization organization)
        {
            return new OrganizationType()
            {
                id = "o" + organization.ID,
                OrganizationName = new TextType()
                {
                    Value = organization.Name
                },
                OrganizationOtherIdentification = BuildIdentificationTypes(organization.IdentificationID)
            };
        }

        private static IdentificationType[] BuildIdentificationTypes(string organizationID)
        {
            return new List<IdentificationType> {

                    new IdentificationType()
                    {
                        IdentificationCategory =  IdentificationCategoryCodeSimpleType.EIN,
                        IdentificationID = BuildTextType(organizationID)
                    }
                }.ToArray();
        }

        private static PersonType[] BuildPersons(List<Person> people)
        {
            return people.Select(x => BuildPerson(x)).ToArray();
        }

        private static PersonType BuildPerson(Person person)
        {
            var personType = new PersonType()
            {
                PersonBirthDate = new DateType()
                {
                    Item = new date()
                    {
                        Value = person.BirthDate
                    }
                },
                PersonName = new PersonNameType()
                {
                    PersonGivenName = new PersonNameTextType()
                    {
                        Value = person.GivenName
                    },
                    PersonMiddleName = new PersonNameTextType()
                    {
                        Value = person.MiddleName
                    },

                    PersonSurName = new PersonNameTextType()
                    {
                        Value = person.SurName
                    }
                },
                PersonSSNIdentification = new IdentificationType()
                {
                    IdentificationCategory = IdentificationCategoryCodeSimpleType.SSN,
                    IdentificationID = BuildTextType(person.SocialSecurityNumber)
                },
                PersonOtherIdentification = null,
                //PersonOtherIdentification = BuildIdentificationType(),
                Item = new SEXCodeType
                {
                    Value = SEXCodeSimpleType.M
                },
                id = "p" + person.ID

            };
            if (!string.IsNullOrEmpty(person.Suffix))
            {
                personType.PersonName.PersonNameSuffixText = BuildTextType(person.Suffix);
            }
            return personType;
        }

        private static IdentificationType BuildIdentificationType()
        {
            return new IdentificationType()
            {
                IdentificationCategory = IdentificationCategoryCodeSimpleType.USERNAME,
                IdentificationID = BuildTextType("testType")
            };
        }
      
    }

    public class Contact
    {
        public string Email { get; set; }
        public List<string> StreetAdress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public USStateCodeSimpleType? UsState { get; set; }
        public CountryAlpha2CodeSimpleType Country { get; set; }
        public string ZipCode { get; set; }

        public string AreaCode { get; set; }
        public string ExchangeID { get; set; }
        public string LineID { get; set; }
        public string ID { get; set; }
    }

    public class Person
    {
        public string ID { get; set; }
        public DateTime BirthDate { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public SEXCodeSimpleType SexCodeSimpleType { get; set; }
        public string Suffix { get; set; }
    }
    public class Organization
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IdentificationID { get; set; }
    }
    public class Document
    {
        public string ID { get; set; }
        public string Format { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public byte[] ByteArray { get; set; }
    }

    public class CaseDetails
    {
        public string Division { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string ID { get; set; }
    }
    public class RootObject
    {
        public List<Document> Documents { get; set; }
        public List<Organization> Organizations { get; set; }
        public List<Person> People { get; set; }
        public List<Contact> Contacts { get; set; }
        public string CommentText { get; set; }
        public CaseDetails Case { get; set; }
        public Fee CaseFee { get; set; }

    }

    public class Fee
    {
        public string Amount { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
    }

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