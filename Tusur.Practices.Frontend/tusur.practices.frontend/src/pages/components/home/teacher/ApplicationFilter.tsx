import { useEffect, useState } from "react";
import IApplication from "../../../interfaces/IApplication";
import IStudent from "../../../interfaces/IStudent";
import VerticalTabs from "../../layout/VerticalTabs";

import "./ApplicationFilter.css";

interface IApplicationFilter {
    students: IStudent[] | undefined,
    applications: IApplication[] | undefined,
    onStudentClick: (student: IStudent) => void
}

type FilteredStudents = {
    unapproved: IStudent[],
    approved: IStudent[],
    absent: IStudent[]
}

export const ApplicationFilter: React.FunctionComponent<IApplicationFilter> = (props) => {
    const { applications, students, onStudentClick } = props;

    const [filtered, setFiltered] = useState<FilteredStudents>();

    useEffect(() => {
        const filterStudents = () : FilteredStudents => {
            const result: FilteredStudents = {
                unapproved: [],
                approved: [],
                absent: []
            }
    
            if (!students)
                return result;
    
            for (let student of students) {
                const app = applications?.find(entity => entity.studentId === student.id);
    
                if (app) {
                    if (app.status === 1) {
                        result.unapproved.push(student);
                        continue;
                    }
    
                    result.approved.push(student);
                    console.log(app.status)
                    continue;
                }
    
                result.absent.push(student);
            }
    
            return result;
        }

        if (students && applications) {
            let result = filterStudents();
            setFiltered(result);
        }
    }, [students, applications])

    return (
        <div className="application-filter">
            <div className="title">
                <h3>Активные заявки</h3>
            </div>
            <VerticalTabs 
                titles={ 
                    [ <>Не согласованы <span className={ `tab-amount` }>{ filtered?.unapproved.length }</span></>, 
                    <>Согласованы <span className={ `tab-amount` }>{ filtered?.approved.length }</span></>, 
                    <>Отсутствуют <span className={ `tab-amount` }>{ filtered?.absent.length }</span></>] 
                }
                collection={ `application-types` }
                className={ `application-types` }
            >
                {
                    applications && students
                    ? 
                    (() => { 
                        return (
                            <>
                                <ul>
                                    { filtered?.unapproved.map((student, index) => {
                                        return <li key={ index } onClick={ () => onStudentClick(student) }>
                                            { student.lastName } { student.name } { student.patronymic }
                                        </li>;
                                    }) }
                                </ul>
                                <ul>
                                    { filtered?.approved.map((student, index) => {
                                        return <li key={ index } onClick={ () => onStudentClick(student) }>
                                            { student.lastName } { student.name } { student.patronymic }
                                        </li>;
                                    }) }
                                </ul>
                                <ul>
                                    { filtered?.absent.map((student, index) => {
                                        return <li key={ index }>{ student.lastName } { student.name } { student.patronymic }</li>;
                                    }) }
                                </ul>
                            </>
                        );
                    })()
                    : <></>
                }
            </VerticalTabs>
        </div>
    );
}