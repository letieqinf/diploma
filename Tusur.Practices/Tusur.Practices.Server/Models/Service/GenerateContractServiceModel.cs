namespace Tusur.Practices.Server.Models.Service
{
    public class GenerateContractServiceModel
    {
        public struct StudentInformation
        {
            public string Year { get; set; }
            public string GroupName { get; set; }
            public string StudentName { get; set; }
            public string PracticeKind { get; set; }
            public string PracticeType { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string StudyFieldCode { get; set; }
            public string StudyFieldName { get; set; }
            public string SpecialtyName { get; set; }
        }

        public string SupervisorName { get; set; }
        public string SignatoryName { get; set; }
        public string ProxyStartDate { get; set; }
        public string ProxyNumber { get; set; }

        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
    }
}
