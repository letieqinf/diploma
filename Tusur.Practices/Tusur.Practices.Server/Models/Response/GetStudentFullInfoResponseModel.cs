namespace Tusur.Practices.Server.Models.Response
{
    public class GetStudentFullInfoResponseModel
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
        public string GroupName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAbbr { get; set; }
        public string FacultyName { get; set; }
        public string FacultyAbbr { get; set; }
        public string SpecialtyName { get; set; }
        public string StudyFieldName { get; set; }
        public string StudyFieldCode { get; set; }
        public string DegreeName { get; set; }
        public string StudyFormName { get; set; }
    }
}
