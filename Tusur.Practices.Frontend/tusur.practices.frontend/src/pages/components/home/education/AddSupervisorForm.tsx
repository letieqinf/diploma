import { useState } from "react";
import IGroup from "../../../interfaces/IGroup";
import IPracticeDate from "../../../interfaces/IPracticeDate";
import ITeacher from "../../../interfaces/ITeacher";

import "./AddSupervisorForm.css";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";

interface IAddSupervisorForm {
    teachers: ITeacher[],
    group: IGroup,
    date: IPracticeDate
}

export const AddSupervisorForm: React.FunctionComponent<IAddSupervisorForm> = (props) => {
    const { teachers, group, date } = props;

    const axiosPrivate = useAxiosPrivate();

    const [teacherSelect, setTeacherSelect] = useState<ITeacher>();

    const handleSelectChange = (event: React.SyntheticEvent) => {
        let target = event.target;
        if (target instanceof HTMLSelectElement) {
            setTeacherSelect(teachers.find(entity => entity.teacherId === target.value));
        }
    }

    const handleSubmit = (teacher: ITeacher) => {
        const createSupervisor = async () => {
            try {
                await axiosPrivate.post(`api/supervisors`, {
                    teacherId: teacher.teacherId,
                    groupId: group.id,
                    practiceDateId: date.id
                });
                window.location.reload();
            } catch (error) {
                console.error(error);
            }
        }

        createSupervisor();
    }

    return (
        <form className={ `add-supervisor-form` }>
            <div className={ `teacher-selector` }>
                <div className={ `title title-frame blue` }>
                    <h3>Преподаватели</h3>
                </div>
                <div className={ `body` }>
                    <select onChange={ handleSelectChange } name={ `teacher-select` } id={ `teacher-select` } defaultValue={ `` }>
                        <option value={ `` } disabled>Выберите преподавателя</option>
                        {
                            teachers.map((teacher, index) => {
                                return (
                                    <option key={index} value={ teacher.teacherId }>
                                        { `${ teacher.lastName } ${ teacher.name } ${ teacher.patronymic }` }
                                    </option>
                                );
                            })
                        }
                    </select>
                </div>
            </div>
            <div>
                <button 
                    type={ `button` }
                    className={ `button ${ teacherSelect === undefined && `disabled` }` }
                    onClick={ () => teacherSelect && handleSubmit(teacherSelect) }
                >
                    Подтвердить
                </button>
            </div>
        </form>
    );
}