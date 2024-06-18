import IUser from "./IUser";

interface ITeacher extends IUser {
    teacherId: string,
    departmentId: string
}

export default ITeacher;