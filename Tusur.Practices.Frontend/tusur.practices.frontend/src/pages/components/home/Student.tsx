import { useEffect, useRef, useState } from "react";

import "./Student.css";
import VerticalTabs from "../layout/VerticalTabs";
import PracticeList from "./PracticeList";
import IPracticeDate from "../../interfaces/IPracticeDate";
import ApplicationRoadmap from "./student/ApplicationRoadmap";
import ModalWindow from "../layout/ModalWindow";
import ApplyForm from "./student/ApplyForm";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";

function Student() {
    const axiousPrivate = useAxiosPrivate();
    const onLoad = useRef(false);

    const [dates, setDates] = useState<IPracticeDate[]>();

    useEffect(() => {
        const getPracticeDates = async () => {
            try {
                const response = await axiousPrivate.get('/api/practices/dates?role=student');
                let sorted: IPracticeDate[] = [...response.data].sort((a, b) => new Date(b.startsAt) - new Date(a.startsAt));
                setDates(() => sorted);
            } catch (error) {
                console.error(error);
            }
        };

        if (onLoad.current)
            getPracticeDates();

        onLoad.current = true;
    }, []);

    const [hideApplyMW, setHideApplyMW] = useState<boolean>(true);

    return (
        dates ?
        <div className="content-container student-content-container">
            <VerticalTabs titles={["Список практик", "Активные заявки"]} collection="student" className="content-container-blocks">
                <div>
                    <div className="content-header">
                        <h1 className="page-title">Мои практики</h1>
                    </div>
                    <div className="content-body">
                        <PracticeList dates={dates} />
                    </div>
                </div>
                <div>
                    <div className="content-header">
                        <h1 className="page-title">Мои активные заявки</h1>
                    </div>
                    <div className="content-body">
                        <ApplicationRoadmap practices={dates} />
                        <div className="buttons-container">
                            <button type="button" onClick={() => setHideApplyMW(false)}>Создать заявку</button>
                        </div>
                    </div>
                </div>
            </VerticalTabs>
            <ModalWindow id="apply-modal-window" disabled={hideApplyMW} onClose={() => setHideApplyMW(true)}>
                <ApplyForm dates={dates} />
            </ModalWindow>
        </div>
        : <></>
    );
}

export default Student;