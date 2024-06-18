namespace Tusur.Practices.Server.Models.Response
{
    public class GetGroupResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid ApprovedStudyPlanId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAbbr { get; set; }
        public string FacultyName { get; set; }
        public string FacultyAbbr { get; set; }
        public string StudyFormName { get; set; }
        public string DegreeName { get; set; }
        public ushort Year { get; set; }
        public string StudyFieldName { get; set; }
        public string StudyFieldCode { get; set; }
        public string SpecialtyName { get; set; }
    }
}
