using MoCourtDocumentUpload.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace MoCourtDocumentUpload.Models
{
    public class BuildDocument
    {
        public string ReturnDocumentXML(RootObject root)
        {
            var document = new BuildDocument().BuildMoEcfExchangeType(root);

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
                ContactInformation = BuildContactInformations(root),

                Case = BuildCase(root.Case),                
                Fee = BuildFee(root.CaseFee),
                Identification = BuildIdentification(root.Identifications),
                PrimaryDocument = BuildPrimaryDoc(root.DocumentGroups),

                EntityDocumentAssociation = BuildEntityDocumentAssocation(),
                PersonContactInformationAssociation = BuildContactInformationAssociations(root.People),
                OrganizationContactInformationAssociation = BuildOrganizationContact(root.Organizations),
                EntityCaseAssociation = BuildEntityCaseAssociation(root.Case.GetID(), root.Organizations, root.People),

                CommentText = BuildTextType(root.CommentText),

            };
            return mo;
        }

        private static EntityCaseAssociationType[] BuildEntityCaseAssociation(string caseID, List<Organization> orgs, List<Person> people)
        {
            var associations = new List<EntityCaseAssociationType>();
            associations.AddRange(orgs.Select(x => BuildEntityCaseAssociationType(caseID, x.GetID(), null, x.PartyType)));
            associations.AddRange(people.Select(x => BuildEntityCaseAssociationType(caseID, null, x.GetID(), x.PartyType)));
            return associations.ToArray();
        }

        private static OrganizationContactInformationAssociationType[] BuildOrganizationContact(List<Organization> organizations)
        {
            return organizations.Select(x => BuildOrganizationContactInformationAssociation(x.GetID(), x.Contact.GetID())).ToArray();
        }

        private static IdentificationType[] BuildIdentification(List<Identification> identifications)
        {
            return identifications.Select(BuildIdentificationType).ToArray();
        }

        private static DocumentType1[] BuildPrimaryDoc(List<DocumentGroup> documentGroups)
        {
            return documentGroups.Select(x => BuildDocumentObject(x.Documents)).ToArray();
        }

        private static DocumentType1 BuildDocumentObject(List<Document> documents)
        {
            var document = documents[0];
            var secondDoc = documents[1];
            return new DocumentType1()
            {
                id = document.GetID(),
                DocumentBinary = new BinaryType()
                {
                    Item = new base64Binary()
                    {
                        Value = document.ByteArray
                    }
                    ,
                    BinaryFormatID = BuildTextType(document.Format)
                },
                DocumentTitleText = BuildTextType(document.Title),
                DocumentCategoryText = BuildTextType(document.Category),

                Document = new DocumentType[1]
                                   {
                           new DocumentType()
                           {
                               DocumentTitleText = BuildTextType(secondDoc.Title),
                               DocumentBinary = new BinaryType()
                               {          Item = new base64Binary(){  Value =   secondDoc.ByteArray },
                                   BinaryFormatID = BuildTextType(secondDoc.Format)

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
                        CourtDivisionText = BuildTextType(courtCase.CourtLocation),
                        //OrganizationName = BuildTextType("OrgName"),
                        //OrganizationOtherIdentification = BuildIdentificationTypes("")
                    },
                },
                CaseCategoryText = BuildTextType(courtCase.Type),
                CaseTitleText = BuildTextType(courtCase.Style),
                id = courtCase.GetID(),
            };
        }

        private static TextType BuildTextType(string value)
        {
            return new TextType() { Value = value };
        }

        private static PersonContactInformationAssociationType[] BuildContactInformationAssociations(List<Person> people)
        {
            return people.Select(x => BuildContactInformationAssociation(x.GetID(), x.Contact.GetID())).ToArray();        
        }
        private static PersonContactInformationAssociationType BuildContactInformationAssociation(string personRef, string contactRef)
        {
            return new PersonContactInformationAssociationType()
            {
                ContactInformationReference = BuildReference(contactRef),
                PersonReference = BuildReference(personRef)
            };
        }
        private static ContactInformationType[] BuildContactInformations(RootObject rootObject)
        {
            var contacts = new List<Contact>();
            contacts.AddRange(rootObject.People.Select(x => x.Contact));
            contacts.AddRange(rootObject.Organizations.Select(x => x.Contact));

            return contacts.Select(x => BuildContactInformation(x)).ToArray();
        }

        private static ContactInformationType BuildContactInformation(Contact contact)
        {
            var contactInfo = new ContactInformationType()
            {
                id = contact.GetID(),             
                ContactMailingAddress = new AddressType()
                {
                    Item = new StructuredAddressType()
                    {
                        Items = contact.StreetAdress.Select(x => new StreetType()
                        {
                            StreetFullText = BuildTextType(x)
                        }).ToArray(),
                        LocationCityName = new ProperNameTextType() { Value = contact.City },                   
                        Item1 = new CountryAlpha2CodeType()
                        {
                            Value = contact.Country
                        },
                        LocationPostalCode = BuildTextType(contact.ZipCode)
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

            if (contact.UsState != null)
            {
                contactInfo.ContactMailingAddress.Item.LocationStateUSPostalServiceCode = new USStateCodeType()
                {
                    Value = contact.UsState.Value
                };
            }
            else if (!string.IsNullOrEmpty(contact.InternationalState))
            {
                contactInfo.ContactMailingAddress.Item.LocationStateName = new ProperNameTextType() { Value = contact.InternationalState };
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
                id = organization.GetID(),
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
            return people.Select(BuildPerson).ToArray();
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
                Item = new SEXCodeType
                {
                    Value = SEXCodeSimpleType.M
                },
                id = person.GetID()

            };
            if (!string.IsNullOrEmpty(person.Suffix))
            {
                personType.PersonName.PersonNameSuffixText = BuildTextType(person.Suffix);
            }
            return personType;
        }

        private static IdentificationType BuildIdentificationType(Identification identification)
        {
            return new IdentificationType()
            {
                IdentificationID = BuildTextType(identification.ID),
                IdentificationCategory = identification.Category
            };
        }
    }
}