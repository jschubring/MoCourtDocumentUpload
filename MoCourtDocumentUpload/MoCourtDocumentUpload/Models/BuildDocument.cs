using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoCourtDocumentUpload.Models
{
    public class BuildDocument
    {
        MoEcfExchangeType mo = new MoEcfExchangeType()
        {
            Person = BuildPersons(),
            Organization = BuildOrganizations(),
            //not done
            ContactInformation = BuildContactInformations(),
            //not done
            PersonContactInformationAssociation = BuildContactInformationAssociations(),
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
            

        };

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
                    IdentificationCategory = new IdentificationCategoryCodeType()
                    {
                        Value = IdentificationCategoryCodeSimpleType.SSN
                    },
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
                IdentificationCategory = new IdentificationCategoryCodeType()
                {
                    Value = IdentificationCategoryCodeSimpleType.USERNAME
                },
                IdentificationID = new @string()
                {
                    Value = "USERNAME"
                }
            };
        }
    }
}
