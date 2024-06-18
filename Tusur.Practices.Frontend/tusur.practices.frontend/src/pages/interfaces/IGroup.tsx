interface IGroup {
    id: string,
    departmentId: string,
    approvedStudyPlanId: string,
    name: string,
    departmentName: string,
    departmentAbbr: string,
    facultyName: string,
    facultyAbbr: string,
    studyFormName: string,
    degreeName: string,
    year: number,
    studyFieldName: string,
    studyFieldCode: string,
    specialtyName: string
}

export default IGroup;