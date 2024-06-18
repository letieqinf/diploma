interface IUser {
    id: string,
    name: string,
    lastName: string,
    patronymic: string | undefined,
    email: string
}

export default IUser;