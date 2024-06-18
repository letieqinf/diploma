interface IOrganization {
    id: string,
    inn: string | undefined,
    trrc: string | undefined,
    organizationName: string,
    organizationAddress: string,
    isApproved: boolean
}

export default IOrganization;