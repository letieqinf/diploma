import { useEffect, useRef, useState } from "react";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faClose, faEdit, faRemove } from "@fortawesome/free-solid-svg-icons";

import { Selector, SelectorOption } from "../../layout/selector/Selector";
import { Warning } from "../../layout/warning/Warning";

import IApplication from "../../../interfaces/IApplication";
import IOrganization from "../../../interfaces/IOrganization";
import IPracticeDate from "../../../interfaces/IPracticeDate";
import IStudent from "../../../interfaces/IStudent";
import ISupervisor from "../../../interfaces/ISupervisor";

import "./CreateContractForm.css";

type StudentPractices = {
    student: IStudent,
    practices: IPracticeDate[]
}

interface ICreateContractForm {
    supervisors: ISupervisor[]
}

export const CreateContractForm: React.FunctionComponent<ICreateContractForm> = ( props ) => {
    const { supervisors } = props;

    const axiosPrivate = useAxiosPrivate();
    const onLoad = useRef(false);

    const [organizations, setOrganizations] = useState<IOrganization[]>();
    const [students, setStudents] = useState<StudentPractices[]>();

    const [showAddStudent, setShowAddStudent] = useState<boolean>(false);
    const [showAddOrganization, setShowAddOrganization] = useState<boolean>(true);

    const [organizationInput, setOrganizationInput] = useState<IOrganization>();
    const [studentInput, setStudentInput] = useState<IStudent>();
    const [dateInput, setDateInput] = useState<IPracticeDate>();

    const [selectedOrganization, setSelectedOrganization] = useState<IOrganization>();
    const [studentList, setStudentList] = useState<IStudent[]>([]);
    const [dateList, setDateList] = useState<IPracticeDate[]>([]);

    useEffect(() => {
        const getOrganizations = async () => {
            try {
                const response = await axiosPrivate.get(`api/organizations`);
                setOrganizations(response.data);
            } catch (error) {
                console.log(error);
            }
        }

        if (onLoad.current) {
            getOrganizations();
        }

        onLoad.current = true;
    }, []);

    useEffect(() => {
        const getApplications = async (): Promise<IApplication[]> => {
            let result: IApplication[] = [];
            
            for (const supervisor of supervisors) {
                try {
                    const response = await axiosPrivate.get(
                        `api/applications/filtered?practiceDateId=${ supervisor.practiceDateId }&groupId=${ supervisor.groupId }`
                    );
                    result = [...result, ...response.data].filter((value: IApplication) => value.organizationId === selectedOrganization?.id);
                } catch (error) {
                    console.error(error);
                }
            }

            return result;
        }

        const getStudents = async (applications: IApplication[]): Promise<IStudent[]> => {
            let result: IStudent[] = [];
            const filtered = applications.filter((value, index, array) => {
                return array.findIndex(entity => entity.studentId === value.studentId) === index
            });

            for (const application of filtered) {
                try {
                    if (application.status !== 2)
                        continue;

                    const response = await axiosPrivate.get(`api/groups/students/${ application.studentId }`);
                    result = [...result, response.data];
                } catch (error) {
                    console.error(error);
                }
            }

            return result;
        }

        const getDates = async (applications: IApplication[]): Promise<IPracticeDate[]> => {
            let result: IPracticeDate[] = [];
            const filtered = applications.filter((value, index, array) => {
                return array.findIndex(entity => entity.practiceDateId === value.practiceDateId) === index;
            });

            for (const application of filtered) {
                try {
                    const response = await axiosPrivate.get(`api/practices/dates/${ application.practiceDateId }`);
                    result = [...result, response.data];
                } catch (error) {
                    console.error(error);
                }  
            }
            
            return result;
        }

        const getStudentPractices = async (): Promise<StudentPractices[]> => {
            let studentPractices: StudentPractices[] = [];

            try {
                const applicationResult = await getApplications();
                const studentResult = await getStudents(applicationResult);
                const dateResult = await getDates(applicationResult);

                for (const application of applicationResult) {
                    const student = studentResult.find(value => value.id === application.studentId);
                    let studentIndex = studentPractices.findIndex(entity => entity.student.id === application.studentId);

                    if (student && studentIndex === -1) {
                        studentPractices.push({
                            student: student,
                            practices: []
                        })
                    }

                    studentIndex = studentPractices.findIndex(entity => entity.student.id === application.studentId);

                    if (studentIndex !== -1) {
                        const date = dateResult.find(value => value.id === application.practiceDateId);
                        if (date)
                            studentPractices[studentIndex].practices = [...studentPractices[studentIndex].practices, date];
                        continue;
                    }   
                }
            } catch (error) {
                console.log(error);
            }

            return studentPractices;
        }

        if (selectedOrganization) {
            getStudentPractices()
                .then(value => setStudents(value));
        }
    }, [selectedOrganization]);

    const handleCreate = (event: React.SyntheticEvent) => {
        const target = event.target as HTMLButtonElement;

        const create = async (studentDates: { studentId: string, practiceDateId: string }[]) => {
            try {
                await axiosPrivate.post(`api/practice-profiles/contracts/`, {
                    organizationId: selectedOrganization?.id,
                    studentDates: studentDates,
                    isDraft: target.value == `save`
                });
                window.location.reload();
            } catch (error) {
                console.error(error);
            }
        }

        let studentDates: { studentId: string, practiceDateId: string }[] = [];
        for (let i = 0; i < studentList.length; i++) {
            studentDates.push({
                studentId: studentList[i].id,
                practiceDateId: dateList[i].id
            });
        }

        create(studentDates);
    }

    return (
        <form className={ `create-contract` }>
            <div className={ `form-part` }>
                <div className={ `title title-frame blue` }>
                    <h3>Организация</h3>
                </div>
                {
                    showAddOrganization ?
                    <div className={ `list-selector content` }>
                        <Selector
                            label={ `Наименование` }
                            name={ `organizationName` }
                            collection={ `create-contract--org` }
                            className={ `organization-input` }
                            defaultValue={ organizationInput?.organizationName }
                            onChange={ () => setOrganizationInput(undefined) }
                        >
                            {
                                organizations ? organizations.map((org, index) => {
                                    return (
                                        <SelectorOption
                                            onMouseDown={ () => setOrganizationInput(org) }
                                            key={ index }
                                        >
                                            { org.organizationName }
                                        </SelectorOption>
                                    );
                                }) : <></>
                            }
                        </Selector>
                        <button
                            type={ `button` }
                            className={ `button${ !organizationInput ? ` disabled` : `` }` }
                            disabled={ !organizationInput }
                            onClick={ () => { 
                                setSelectedOrganization(organizationInput);
                                setOrganizationInput(undefined);
                                setShowAddOrganization(false);
                            } }
                        >
                            <FontAwesomeIcon icon={ faCheck } className={ `svg` } />
                        </button>
                    </div> : selectedOrganization ?
                    <div className={ `selected-item organization content` }>
                        <div className={ `selected-item-info organization` }>
                            <p>{ selectedOrganization.organizationName }</p>
                            <p>{ selectedOrganization.organizationAddress }</p>
                        </div>
                        <div className={ `selected-item-options organization` }>
                            <button
                                type="button"
                                className={ `button` }
                                onClick={ () => {
                                    setOrganizationInput(selectedOrganization);
                                    setStudentInput(undefined);
                                    setStudentList([]);
                                    setDateList([]);
                                    setDateInput(undefined);
                                    setSelectedOrganization(undefined);
                                    setShowAddOrganization(true);
                                    setShowAddStudent(false);
                                } }
                            >
                                <FontAwesomeIcon icon={ faEdit } />
                            </button>
                        </div>
                    </div> : <></>
                }
            </div>
            <div className={ `form-part contract-student-list-container` }>
                <div className={ `title title-frame blue` }>
                    <h3>Студенты</h3>
                </div>
                {
                    selectedOrganization ? studentList.length > 0 ?
                    <ul className={ `content student-list` }>
                        {
                            studentList.map((student, index) => {
                                return (
                                    <li key={ index } className={ `selected-item` }>
                                        <div className={ `selected-item-info` }>
                                            <p>{ `${ student.lastName } ${ student.name } ${ student.patronymic }` }</p>
                                            <p>{ dateList.length > 0 && `${ dateList[index].kind } ${ dateList[index].type }` }</p>
                                        </div>
                                        <div className={ `selected-item-options` }>
                                            <button
                                                type={ `button` }
                                                className={ `button red` }
                                                onClick={ () => setStudentList([...studentList].filter(entity => entity !== student)) }
                                            >
                                                <FontAwesomeIcon icon={ faRemove } />
                                            </button>
                                        </div>
                                    </li>
                                );
                            })
                        }
                    </ul> : <Warning title={ `Выберите хотя бы одного студента` } className={ `content` } />
                    : <Warning title={ `Выберите организацию` } className={ `content` } />
                }
                {
                    showAddStudent
                    ?
                    <div className={ `list-selector student-input-container content` }>
                        <div className={ `selectors` }>
                            <Selector
                                label={ `Имя студента` }
                                collection={ `create-contract-student` }
                                name={ `studentName` }
                                className={ `student-input` }
                                onChange={ () => setStudentInput(undefined) }
                            >
                                {
                                    students ? students.map((student, index) => {
                                        if (!studentList.includes(student.student)) {
                                            return (
                                                <SelectorOption
                                                    onMouseDown={ () => setStudentInput(student.student) }
                                                    key={ index }
                                                >
                                                    { `${ student.student.lastName } ${ student.student.name } ${ student.student.patronymic }` }
                                                </SelectorOption>
                                            );
                                        }

                                        return <></>;
                                    }) : <></>
                                }
                            </Selector>
                            <Selector
                                label={ `Практика` }
                                name={ `practiceName` }
                                collection={ `create-contract-student` }
                                className={ `student-input` }
                                onChange={ () => {} }
                                disabled={ !studentInput }
                            >
                                {
                                    studentInput &&
                                    (() => {
                                        const student = students?.find(student => student.student === studentInput);
                                        return ( 
                                            student ?
                                            student.practices.map((date, index) => {
                                                return (
                                                    <SelectorOption
                                                        onMouseDown={ () => setDateInput(date) }
                                                        key={ `${ index }` }
                                                    >
                                                        { `${ date.kind } ${ date.type }` }
                                                    </SelectorOption>
                                                );
                                            })
                                            : <></>
                                         );
                                    })()
                                }
                            </Selector>
                        </div>
                        <button
                            type={ `button` }
                            className={ `button  ${ !dateInput && `disabled` }` }
                            disabled={ !dateInput }
                            onClick={ () => { 
                                studentInput && setStudentList([...studentList, studentInput]);
                                dateInput && setDateList([...dateList, dateInput]);
                                setDateInput(undefined);
                                setShowAddStudent(false); 
                                setStudentInput(undefined);
                            } }
                        >
                            <FontAwesomeIcon icon={ faCheck } className={ `svg` } />
                        </button>
                        <button
                            type={ `button` }
                            className={ `button white` }
                            onClick={ () => { 
                                setShowAddStudent(false); 
                                setStudentInput(undefined);
                                setDateInput(undefined);
                            } }
                        >
                            <FontAwesomeIcon icon={ faClose } className={ `svg` } />
                        </button>
                    </div>
                    : selectedOrganization ?
                    <div className={ `buttons-container` }>
                        <button 
                            type={ `button` }
                            className={ `button` }
                            onClick={ () => setShowAddStudent(true) }
                        >
                            Добавить студента
                        </button>
                    </div> : <></>
                }
            </div>
            
            <div className={ `form-part buttons-container` }>
                <button
                    type={ `button` }
                    value={ `create` }
                    onClick={ handleCreate }
                    className={ `button ${ !dateList.length || !studentList.length || !selectedOrganization ? `disabled` : `` }` }
                    disabled={ !dateList.length || !studentList.length || !selectedOrganization }
                >
                    Отправить
                </button>
                <button 
                    type={ `button` }
                    value={ `save` }
                    onClick={ handleCreate }
                    className={ `button white ${ !dateList.length || !studentList.length || !selectedOrganization ? `disabled` : `` }` }
                    disabled={ !dateList.length || !studentList.length || !selectedOrganization }
                >
                    Сохранить
                </button>
            </div>
        </form>
    );
}