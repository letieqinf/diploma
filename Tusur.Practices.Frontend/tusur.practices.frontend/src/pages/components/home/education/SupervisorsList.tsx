import { useEffect, useRef, useState } from "react";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
import "./SupervisorsList.css";
import ISupervisor from "../../../interfaces/ISupervisor";
import IPracticeDate from "../../../interfaces/IPracticeDate";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus, faRemove, faSearch } from "@fortawesome/free-solid-svg-icons";
import ITeacher from "../../../interfaces/ITeacher";
import IGroup from "../../../interfaces/IGroup";
import { Warning } from "../../layout/warning/Warning";
import CustomInput from "../../layout/CustomInput";

import dateFormat, { masks } from "dateformat";

interface ISupervisorsList {
    onCreateClick?: (teachers: ITeacher[], group: IGroup, date: IPracticeDate) => void
}

type SupervisorPractice = {
    groupId: string,
    teacherDate: {
        id: string,
        teacherId: string,
        practiceDateId: string
    }[]
}

type GroupPractice = {
    groupId: string,
    dates: IPracticeDate[]
}

export const SupervisorsList: React.FunctionComponent<ISupervisorsList> = (props) => {
    const { onCreateClick } = props;

    const axiosPrivate = useAxiosPrivate();
    const onLoad = useRef(false);

    const [supervisors, setSupervisors] = useState<SupervisorPractice[]>();
    const [dates, setDates] = useState<GroupPractice[]>();

    const [teachers, setTeachers] = useState<ITeacher[]>();
    const [groups, setGroups] = useState<IGroup[]>();

    const [input, setInput] = useState<string>(``);

    useEffect(() => {
        const getSupervisors = async (): Promise<ISupervisor[]> => {
            let result: ISupervisor[] = [];

            try {
                const response = await axiosPrivate.get(`api/supervisors?role=${ `education` }`);
                result = response.data;
            } catch (error) {
                console.error(error);
            }

            return result;
        }

        const getGroups = async () => {
            try {
                const response = await axiosPrivate.get(`api/groups`);
                setGroups(response.data);
            } catch (error) {
                console.error(error);
            }
        }

        const getTeachers = async () => {
            try {
                const response = await axiosPrivate.get(`api/faculties/departments/teachers`);
                setTeachers(response.data);
            } catch (error) {
                console.error(error);
            }
        }

        if (onLoad.current) {
            getSupervisors().then(value => {
                let result: SupervisorPractice[] = [];

                for (const element of value) {
                    let index = result.findIndex(entity => entity.groupId === element.groupId);
                    if (index === -1) {
                        result.push({
                            groupId: element.groupId,
                            teacherDate: [{
                                id: element.id,
                                teacherId: element.teacherId,
                                practiceDateId: element.practiceDateId
                            }]
                        })

                        continue;
                    }

                    result[index].teacherDate.push({
                        id: element.id,
                        teacherId: element.teacherId,
                        practiceDateId: element.practiceDateId
                    });
                }

                setSupervisors(result);
            });
            getTeachers();
            getGroups();
        }

        onLoad.current = true;
    }, [])

    useEffect(() => {
        const getDates = async (group: IGroup): Promise<IPracticeDate[] | undefined> => {
            let result: IPracticeDate[] | undefined = undefined;

            try {
                const response = await axiosPrivate.get(`api/practices/dates?role=${ `education` }&groupId=${ group.id }`);
                result = response.data;
            } catch (error) {
                console.error(error);
            }

            return result;
        }

        if (groups) {
            for (const group of groups) {
                getDates(group)
                    .then(value => {
                        if (value) {
                            if (dates) {
                                setDates([
                                    ...dates, {
                                        groupId: group.id,
                                        dates: value }
                                ]);
                            } else {
                                setDates([{
                                    groupId: group.id,
                                    dates: value }
                                ]);
                            }
                        }
                    });
            }
        }
    }, [groups]);


    return (
        <>
            <div className={ `buttons-container` }>
                <CustomInput
                    label={ <FontAwesomeIcon icon={ faSearch } className={ `svg` } /> }
                    collection={ `supervisor-list` }
                    className={ `supervisor-list-search` }
                    name={ `supervisorListSearch` }
                    value={ input }
                    onChange={ (event: React.SyntheticEvent) => {
                        const target = event.target as HTMLInputElement;
                        setInput(target.value);
                    } }
                />
            </div>
            <ul className={ `groups-container` }>
                {
                    (() => {
                        return dates && groups && teachers && supervisors && groups.map((group, index) => {
                            if (!group.name.toUpperCase().includes(input))
                                return;

                            let datesContainer = dates.find(entity => entity.groupId === group.id)?.dates;
                            let supervisorContrainer = supervisors.find(entity => entity.groupId === group.id)?.teacherDate;

                            return (
                                <GroupItem
                                    group={ group }
                                    supervisors={ supervisorContrainer }
                                    teachers={ teachers }
                                    dates={ datesContainer }
                                    key={ index }
                                    onCreateClick={ onCreateClick }
                                />
                            );
                        })
                    })()
                }
            </ul>
        </>
    );
}

interface IGroupItem {
    group: IGroup,
    supervisors?: { id: string, teacherId: string, practiceDateId: string }[],
    teachers: ITeacher[],
    dates?: IPracticeDate[],
    onCreateClick?: (teachers: ITeacher[], group: IGroup, date: IPracticeDate) => void
}

const GroupItem: React.FunctionComponent<IGroupItem> = (props) => {
    const { group, supervisors, teachers, dates, onCreateClick } = props;

    const axiosPrivate = useAxiosPrivate();

    const handleDelete = (id: string) => {
        const deleteSupervisor = async () => {
            try {
                await axiosPrivate.delete(`api/supervisors/${ id }`);
                window.location.reload();
            } catch (error) {
                console.error(error);
            }
        }

        deleteSupervisor();
    }

    return (
        <li className={ `group-list-item` }>
            <div className={ `header` }>
                <p className={ `group-name` }>{ group.name }</p>
                <div className={ `group-info` }>
                    <p>{ group.studyFieldCode } { group.studyFieldName }</p>
                    <p>Кафедра { group.departmentAbbr }, { group.year }</p>
                </div>
            </div>
            <div className={ `body` }>
                <ul className={ `supervisor-list` }>
                    {
                        dates?.map((date, index) => {
                            let isHere = supervisors?.find(entity => entity.practiceDateId === date.id);
                            if (isHere) {
                                let teacher = teachers.find(entity => entity.teacherId == isHere.teacherId);
                                return (
                                    <li className={ `supervisor-list-item` } key={ index }>
                                        <div className={ `supervisor-list-item-info` }>
                                            <p>
                                                { date.kind } - { date.type } 
                                                <span> ({ dateFormat(date.startsAt, "yyyy.mm.dd") } - { dateFormat(date.endsAt, "yyyy.mm.dd") })</span>
                                            </p>
                                            <p>{ teacher?.lastName } { teacher?.name } { teacher?.patronymic }</p>
                                        </div>
                                        <div className={ `supervisor-list-item-action` } >
                                            <FontAwesomeIcon icon={ faRemove } onClick={ () => handleDelete(isHere.id) } />
                                        </div>
                                    </li>
                                );
                            } else {
                                return (
                                    <li className={ `supervisor-list-item red` } key={ index }>
                                        <div className={ `supervisor-list-item-info` }>
                                            <p>
                                                { date.kind } - { date.type }
                                                <span> ({ dateFormat(date.startsAt, "yyyy.mm.dd") } - { dateFormat(date.endsAt, "yyyy.mm.dd") })</span>
                                            </p>
                                            <Warning title={ `Не назначен` } />
                                        </div>
                                        <div className={ `supervisor-list-item-action` }>
                                            <FontAwesomeIcon
                                                icon={ faPlus }
                                                onClick={ () => {
                                                    return onCreateClick && onCreateClick(teachers.filter(entity => entity.departmentId === group.departmentId), group, date);
                                                } }
                                            />
                                        </div>
                                    </li>
                                );
                            }
                        })
                    }
                </ul>
            </div>
        </li>
    );
}