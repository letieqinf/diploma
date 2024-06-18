import { useEffect, useRef, useState } from "react";
import IPracticeProfile from "../../../interfaces/IPracticeProfile";
import "./Contract.css";
import IStudent from "../../../interfaces/IStudent";
import IOrganization from "../../../interfaces/IOrganization";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
import IContract from "../../../interfaces/IContract";
import CustomInput from "../CustomInput";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faChevronDown, faChevronUp, faDownload, faEdit, faRemove, faSearch } from "@fortawesome/free-solid-svg-icons";
import CustomCheckbox from "../CustomCheckbox";
import VerticalTabs from "../VerticalTabs";
import IPracticeDate from "../../../interfaces/IPracticeDate";

interface IContractList {
    role: string,
    onEditClick?: (profile: IPracticeProfile) => void
}

export const ContractList: React.FunctionComponent<IContractList> = ( props ) => {
    const { role, onEditClick } = props;

    const axiosPrivate = useAxiosPrivate();

    const onLoad = useRef(false);

    const [profiles, setProfiles] = useState<IPracticeProfile[]>();

    const [input, setInput] = useState<string>(``);
    const [counter, setCounter] = useState<{ approved: number, notApproved: number, notSent: number }>();

    const handleInputChange = (event: React.SyntheticEvent) => {
        const target = event.target as HTMLInputElement;
        setInput(target.value);
    }

    useEffect(() => {
        const getProfiles = async () => {
            try {
                const response = await axiosPrivate.get(`api/practice-profiles?role=${ role }`);
                setProfiles(response.data);
            } catch (error) {
                console.error(error);
            }
        }

        if (onLoad.current) {
            getProfiles();
        }

        onLoad.current = true;
    }, []);

    useEffect(() => {
        if (profiles) {
            const approved = document.getElementById(`approved-${ role }`);
            const notApproved = document.getElementById(`not-approved-${ role }`);
            const notSent = document.getElementById(`not-sent-${ role }`);

            if (approved && notApproved && (role !== `teacher` || notSent)) {
                setCounter({
                    approved: approved.children.length,
                    notApproved: notApproved.children.length,
                    notSent: notSent ? notSent.children.length : 0
                });
            }
        }
    }, [profiles, input]);

    return (
        <div className={ `contract-list` }>
            <div className={ `contracts-lookup-container` }>
                <CustomInput
                    label={ <FontAwesomeIcon icon={ faSearch } /> }
                    collection={ `contracts-lookup` }
                    name={ `contractsInput` }
                    className={ `contracts-lookup` }
                    value={ input }
                    onChange={ (event: React.SyntheticEvent) => handleInputChange(event) }
                />
            </div>
            <div>
                <VerticalTabs
                    titles={ 
                        role === `teacher` ? 
                        [
                            <>Согласованы <span className={ `tab-amount` }>{ counter?.approved }</span></>, 
                            <>Не согласованы <span className={ `tab-amount` }>{ counter?.notApproved }</span></>,
                            <>Не отправлены <span className={ `tab-amount` }>{ counter?.notSent }</span></> 
                        ] :
                        [
                            <>Согласованы <span className={ `tab-amount` }>{ counter?.approved }</span></>,
                            <>Не согласованы <span className={ `tab-amount` }>{ counter?.notApproved }</span></>
                        ] }
                    collection={ `all-contracts` }
                    className={ `all-contracts` }
                >
                    <div>
                        {
                            profiles ?
                            <ul id={ `approved-${ role }` } className={ `contract-profile-container` }>
                                {
                                    profiles.map((profile, index) => {
                                        return (
                                            profile.status === 2 && <ContractCard profile={ profile } role={ role } filter={ input } key={ index } />
                                        );
                                    })
                                }
                            </ul> : <></>
                        }
                    </div>
                    <div>
                        {
                            profiles ?
                            <ul id={ `not-approved-${ role }` } className={ `contract-profile-container` }>
                                {
                                    profiles.map((profile, index) => {
                                        return (
                                            profile.status === 1 && <ContractCard profile={ profile } role={ role } filter={ input } onEditClick={ onEditClick } key={ index } />
                                        );
                                    })
                                }
                            </ul> : <></>
                        }
                    </div>
                    {
                        role === `teacher` ?
                        <div>
                            {
                                profiles ?
                                <ul id={ `not-sent-${ role }` } className={ `contract-profile-container` }>
                                    {
                                        profiles.map((profile, index) => {
                                            return (
                                                profile.status === 0 && <ContractCard profile={ profile } role={ role } filter={ input } key={ index } />
                                            );
                                        })
                                    }
                                </ul> : <></>
                            }
                        </div>  : <></>
                    }
                </VerticalTabs>
            </div>
        </div>
    );
}

interface IContractCard {
    profile: IPracticeProfile,
    role: string,
    filter: string,
    onEditClick?: (profile: IPracticeProfile) => void
}

const ContractCard: React.FunctionComponent<IContractCard> = ( props ) => {
    const { profile, role, filter, onEditClick } = props;

    const axiosPrivate = useAxiosPrivate();

    const [students, setStudents] = useState<IStudentInformation[]>();
    const [organization, setOrganization] = useState<IOrganization>();
    const [dates, setDates] = useState<IPracticeDate[]>();

    const onLoad = useRef(false);

    useEffect(() => {
        const getStudents = async (): Promise<IStudentInformation[]> => {
            let result: IStudentInformation[] = [];

            for (let student of profile.studentDates) {
                try {
                    const response = await axiosPrivate.get(`api/groups/students/${ student.studentId }/full-information`);
                    result = [...result, response.data];
                } catch (error) {
                    console.error(error);
                }
            }

            return result;
        }

        const getContract = async (): Promise<IContract | undefined> => {
            let result: IContract | undefined = undefined;

            try {
                const response = await axiosPrivate.get(`api/practice-profiles/contracts/${ profile.contractId }?role=${ role }`);
                result = response.data;
            } catch (error) {
                console.error(error);
            }

            return result;
        }

        const getOrganization = async (contract: IContract) => {
            try {
                const response = await axiosPrivate.get(`api/organizations/${ contract.organizationId }`);
                setOrganization(response.data);
            } catch (error) {
                console.error(error);
            }
        }

        const getDates = async (): Promise<IPracticeDate[]> => {
            let result: IPracticeDate[] = [];

            for (const student of profile.studentDates) {
                try {
                    const response = await axiosPrivate.get(`api/practices/dates/${ student.practiceDateId }`);
                    result = [...result, response.data];
                } catch (error) {
                    console.error(error);
                }
            }

            return result;
        }

        if (onLoad.current) {
            getStudents().then(value => setStudents(value));
            getContract().then(value => value && getOrganization(value));
            getDates().then(value => setDates(value));
        }

        onLoad.current = true;
    }, []);

    const handleRemove = (profile: IPracticeProfile) => {
        const removeProfile = async () => {
            try {
                await axiosPrivate.put(`api/practice-profiles`, {
                    contractId: profile.contractId,
                    studentDates: profile.studentDates
                })
                window.location.reload();
            } catch (error) {
                console.error(error);
            }
        }

        removeProfile();
    }

    const handleSubmit = (profile: IPracticeProfile) => {
        const submitProfile = async () => {
            try {
                await axiosPrivate.patch(`api/practice-profiles?role=${ role }`, {
                    contractId: profile.contractId,
                    studentDates: profile.studentDates
                })
                window.location.reload();
            } catch (error) {
                console.error(error);
            }
        }

        submitProfile();
    }

    const handleDownload = (profile: IPracticeProfile) => {
        const downloadContract = async () => {
            try {
                const response = await axiosPrivate.get(`api/documents/contracts/${ profile.contractId }/download`, {
                    responseType: `blob`
                });
                var file = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' });
                var fileURL = URL.createObjectURL(file); 
                window.open(fileURL);
            } catch (error) {
                console.error(error);
            }
        }

        downloadContract();
    }

    return (
        <>
            {
                organization ?
                organization.organizationName.toUpperCase().includes(filter.toUpperCase()) ?
                <li className={ `contract-container` }>
                    <div className={ `contract-header` }>
                        <div className={ `header-text organization` }>
                            <p>{ organization.organizationName }</p>
                            <p>{ organization.organizationAddress }</p>
                        </div>
                        <div className={ `header-buttons` }>
                            { 
                                role === `teacher` ?
                                <>
                                    { profile.status === 2 && <FontAwesomeIcon icon={ faDownload } onClick={ () => handleDownload(profile) } /> }
                                    { profile.status === 0 && <FontAwesomeIcon icon={ faCheck } onClick={ () => handleSubmit(profile) } /> }
                                    { profile.status < 2 && <FontAwesomeIcon icon={ faRemove } onClick={ () => handleRemove(profile) } /> }
                                </> :
                                role === `secretary`
                                ?
                                <>
                                    { profile.status === 2 && <FontAwesomeIcon icon={ faDownload } onClick={ () => handleDownload(profile) } /> }
                                </> :
                                <>
                                    { profile.status === 2 && <FontAwesomeIcon icon={ faDownload } onClick={ () => handleDownload(profile) } /> }
                                    { profile.status === 1 && <FontAwesomeIcon icon={ faEdit } onClick={ () => onEditClick && onEditClick(profile) } /> }
                                    { profile.status === 1 && <FontAwesomeIcon icon={ faCheck } onClick={ () => handleSubmit(profile) } /> }
                                </>
                            }
                        </div>
                    </div>
                    <div className={ `contract-students-container` }>
                        {
                            students ?
                            <ul className={ `contract-students` }>
                                {
                                    students.map((student, index) => {
                                        return (
                                            <li className={ `student-in-contract` } key={ index }>
                                                <p className={ `student-info` }>{ student.lastName } { student.name } { student.patronymic } ({ student.departmentAbbr }, гр. { student.groupName })</p>
                                                { dates && <p className={ `practice-info` }>{ dates[index].kind } - { dates[index].type }</p> }
                                            </li>
                                        );
                                    })
                                }
                            </ul> :
                            <p>Отсутствуют</p>
                        }
                    </div>
                </li> : <></>
                : <></>
            }
        </>
    );
}