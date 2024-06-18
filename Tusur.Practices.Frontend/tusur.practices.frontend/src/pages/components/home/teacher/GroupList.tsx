import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import IGroup from "../../../interfaces/IGroup";
import CustomInput from "../../layout/CustomInput";
import { Organizer, OrganizerItem } from "../../layout/organizer/Organizer";

import "./GroupList.css";
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";
import { useEffect, useRef, useState } from "react";
import { GroupCard } from "./GroupCard";
import PracticeList from "../PracticeList";
import IPracticeDate from "../../../interfaces/IPracticeDate";
import ISupervisor from "../../../interfaces/ISupervisor";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
import IStudent from "../../../interfaces/IStudent";
import IApplication from "../../../interfaces/IApplication";
import { ApplicationFilter } from "./ApplicationFilter";
import { ApplicationCard } from "../ApplicationCard";
import ModalWindow from "../../layout/ModalWindow";
import { ApproveForm } from "./ApproveForm";

interface IGroupList {
    supervisors: ISupervisor[] | undefined
}

export const GroupList: React.FunctionComponent<IGroupList> = ( props ) => {
    const onLoad = useRef(false);
    const axiosPrivate = useAxiosPrivate();

    const { supervisors } = props;

    const [groups, setGroups] = useState<IGroup[]>();
    const [dates, setDates] = useState<IPracticeDate[]>();
    const [students, setStudents] = useState<IStudent[]>();
    const [applications, setApplications] = useState<IApplication[]>();

    const [organizerValue, setOrganizerValue] = useState<number>(0);

    const [searchValue, setSearchValue] = useState<string>("");
    const [currentGroup, setCurrentGroup] = useState<number>();
    const [currentDate, setCurrentDate] = useState<number>();
    const [currentStudent, setCurrentStudent] = useState<number>();

    const [hideApproveMW, setHideApproveMW] = useState<boolean>(true);
    const [hideDisapproveMW, setHideDisapproveMW] = useState<boolean>(true);

    const handleSearchChange = (event: any) => {
        setSearchValue(event.currentTarget.value);
    }

    const handleCardClick = (group: IGroup) => {
        const target = groups?.indexOf(group);
        if (target !== -1)
            setCurrentGroup(target);
    }

    const handleDateClick = (date: IPracticeDate) => {
        const target = dates?.indexOf(date);
        if (target !== -1)
            setCurrentDate(target);
    }

    const handleStudentClick = (student: IStudent) => {
        const target = students?.indexOf(student);
        if (target !== -1)
            setCurrentStudent(target);
    }

    const handleOrganizerChange = (value: number) => {
        setOrganizerValue(value);

        if (currentStudent !== undefined) {
            setCurrentStudent(undefined);
        }
        else if (currentDate !== undefined){
            setCurrentDate(undefined);
            setApplications(undefined);
        }
        else if (currentGroup !== undefined) {
            setCurrentGroup(undefined);
            setStudents(undefined);
        }
    }

    const handleApproveMWClose = () => {
        setHideApproveMW(true);
    }

    useEffect(() => {
        const getGroups = async (): Promise<IGroup[]> => {
            const data: IGroup[] = [];

            if (supervisors) {
                for (let supervisor of supervisors) {
                    try {
                        const response = await axiosPrivate.get(`api/groups/${supervisor.groupId}`);
                        if (data.some(group => group.name === response.data.name))
                            continue;

                        data.push(response.data);
                    } catch (error) {
                        console.log(error);
                    }
                }
            }            

            return data;
        }

        if (supervisors && onLoad.current == true) {
            getGroups()
                .then((value) => setGroups(value));
        }

        onLoad.current = true;
    }, [])

    useEffect(() => {
        const getDates = async (): Promise<IPracticeDate[]> => {
            const data: IPracticeDate[] = [];
            const supervisorsByGroup = supervisors?.filter(supervisor => groups?.some(group => group.id === supervisor.groupId));

            if (supervisorsByGroup) {
                for (let supervisor of supervisorsByGroup) {
                    try {
                        const response = await axiosPrivate.get(`api/practices/dates/${supervisor.practiceDateId}`);
                        data.push(response.data);
                    } catch (error) {
                        console.error(error);
                    }
                }
            }

            return data;
        }

        const getStudents = async () => {
            if (groups && currentGroup !== undefined) {
                try {
                    const response = await axiosPrivate.get(`api/groups/${ groups[currentGroup].id }/students`);
                    setStudents(response.data);
                } catch (error) {
                    console.error(error);
                }
            }
        }

        if (currentGroup !== undefined) {
            getDates()
                .then((value) => setDates(value));
            getStudents();
            setOrganizerValue(1);
        }
    }, [currentGroup])

    useEffect(() => {
        const getApplications = async () => {
            if ((dates && currentDate !== undefined) && (groups && currentGroup !== undefined)) {
                try {
                    const response = await axiosPrivate.get(`api/applications/filtered?practiceDateId=${ dates[currentDate].id }&groupId=${ groups[currentGroup].id }`);
                    setApplications(response.data);
                } catch (error) {
                    console.error(error);
                }
            }
        }

        if (currentDate !== undefined) {
            getApplications();
            setOrganizerValue(2);
        }
    }, [currentDate])

    useEffect(() => {
        if (currentStudent !== undefined) {
            setOrganizerValue(3);
        }
    }, [currentStudent]);

    return (
        groups ?
        <>
        <Organizer currentValue={ organizerValue } onChange={ (value: number) => handleOrganizerChange(value) } >
            <OrganizerItem value={ 0 }>
                <CustomInput
                    label={ <FontAwesomeIcon icon={ faMagnifyingGlass } /> }
                    collection={ `group-list-search` }
                    name={ `group-list-search` }
                    value={ searchValue }
                    onChange={ handleSearchChange }
                    className={ `group-list-search` }
                />
                <div className={ `group-list-container` }>
                    <ul className={ `group-list` }>
                        {
                            groups.map((group, index) => {
                                if (group.name.includes(searchValue)) {
                                    return (
                                        <li key={ index }>
                                            <GroupCard onClick={ () => handleCardClick(group) } group={ group } />
                                        </li>
                                    );
                                }
                            })
                        }
                    </ul>
                </div>
            </OrganizerItem>
            <OrganizerItem value={ 1 } parentValue={ 0 } title={ currentGroup !== undefined ? groups[currentGroup].name : '' }>
                {
                    dates ?
                    <PracticeList dates={ dates } onCardClick={ (date) => handleDateClick(date) } />
                    : <></>
                }
            </OrganizerItem>
            <OrganizerItem value={ 2 } parentValue={ 1 } title={ dates && currentDate !== undefined ? dates[currentDate].type : `` }>
                { 
                    applications && students && 
                    <ApplicationFilter applications={ applications } students={ students } onStudentClick={ (student) => handleStudentClick(student) } />
                }
            </OrganizerItem>
            <OrganizerItem 
                value={ 3 }
                parentValue={ 2 }
                title={ students && currentStudent !== undefined 
                        ? `${ students[currentStudent].lastName } ${ students[currentStudent].name } ${ students[currentStudent].patronymic }` 
                        : `` }
            >
                {
                    (() => {
                        if (dates && currentDate !== undefined && students && currentStudent !== undefined) {
                            const application = applications?.find(entity => entity.studentId == students![currentStudent!].id);
                            if (application)
                                return (
                                    <>
                                        <div className="teacher-application-card-container">
                                            <ApplicationCard application={ application } date={ dates![currentDate!] } className={ `teacher-application-card` } />
                                        </div>
                                        <div className={ `teacher-buttons-container` }>
                                            { 
                                                application.status === 1 && 
                                                (
                                                    <>
                                                        <button type={ "button" } className="button" onClick={ () => setHideApproveMW(false) }>Согласовать</button>
                                                        <button type={ "button" } className="button white">Вернуть на доработку</button>
                                                    </>
                                                )
                                            }
                                        </div>
                                    </>
                                );
                        }
                    })()
                }
            </OrganizerItem>
        </Organizer>
        <ModalWindow onClose={ handleApproveMWClose } disabled={ hideApproveMW }>
            {
                (() => {
                    if (dates && currentDate !== undefined && students && currentStudent !== undefined) {
                        const application = applications?.find(entity => entity.studentId == students![currentStudent!].id);
                        if (application)
                            return <ApproveForm application={ application }/>;
                    }
                })() 
            }
        </ModalWindow>
        </>
        :
        <p>Вы не были назначены в качестве руководителя ни для одной группы</p>
    );
}