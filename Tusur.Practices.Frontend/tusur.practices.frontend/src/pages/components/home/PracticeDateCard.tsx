import { useEffect, useRef, useState } from "react";
import IPracticeDate from "../../interfaces/IPracticeDate";

import './PracticeDateCard.css';
import RoleContent from "../../../components/RoleContent";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import ISupervisor from "../../interfaces/ISupervisor";
import ITeacher from "../../interfaces/ITeacher";

import dateFormat, { masks } from "dateformat"

interface Supervisor {
    id: string,
    name: string,
    lastName: string,
    patronymic: string,
    email: string,
    isHead: boolean
}

interface IPracticeDateCard {
    practice: IPracticeDate,
    onClick?: React.MouseEventHandler
}

function PracticeDateCard(props: IPracticeDateCard) {
    const [supervisor, setSupervisor] = useState<ISupervisor>();
    const [teacher, setTeacher] = useState<ITeacher>();

    const onLoad = useRef(false);
    const axiosPrivate = useAxiosPrivate();

    useEffect(() => {
        const getSupervisor = async () => {
            try {
                const response = await axiosPrivate.get(`api/supervisors?role=${ `student` }&practiceDateId=${ props.practice.id }`);
                setSupervisor(response.data[0]);
            } catch (error) {
                console.error(error);
            }
        }

        if (onLoad.current) {
            getSupervisor();
        }

        onLoad.current = true;
    }, [])

    useEffect(() => {
        const getTeacher = async () => {
            if (supervisor) {
                try {
                    const response = await axiosPrivate.get(`api/faculties/departments/teachers/${ supervisor.teacherId }`);
                    setTeacher(response.data);
                } catch (error) {
                    console.error(error);
                }
            }
        }

        supervisor && getTeacher();
    }, [supervisor])

    return (
        <div className={ `practice-card-container card-container ${ props.onClick ? `clickable` : `` }` } onClick={ props.onClick }>
            <div className="practice-card card">
                <div className="practice-card-title card-title">
                    <h3>{ props.practice.type }</h3>
                    <p>{ props.practice.kind }</p>
                </div>
                <RoleContent roles={ [`student`] }>
                    <div className="practice-card-resp card-info">
                        <div className="resp-container">
                            <div className="resp-head-title">
                                <h4>Руководитель практики</h4>
                            </div>
                            <div className="resp-head-block">
                                { 
                                    teacher ?
                                    <p>{ teacher.lastName } { teacher.name } { teacher.patronymic }</p> :
                                    <p>Не назначен</p>
                                }
                            </div>
                        </div>
                        <div className={ `resp-dates` }>
                            <p>{ dateFormat(props.practice.startsAt, "yyyy.mm.dd") } - { dateFormat(props.practice.endsAt, "yyyy.mm.dd") }</p>
                        </div>
                    </div>
                </RoleContent>
            </div>
        </div>
    );
}

export default PracticeDateCard;