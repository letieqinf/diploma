import { useEffect, useRef, useState } from "react";
import IPracticeDate from "../../../interfaces/IPracticeDate";
import IApplication from "../../../interfaces/IApplication";

import "./ApplicationRoadmap.css";
import IOrganization from "../../../interfaces/IOrganization";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faChevronLeft, faChevronRight, faDownload, faRemove } from "@fortawesome/free-solid-svg-icons";
import applicationStatuses from "../../../../const/ApplicationStatuses";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";

function ApplicationRoadmap(props: { practices: IPracticeDate[] }) {
    const axiosPrivate = useAxiosPrivate();
    const onLoad = useRef(false);

    const [applications, setApplications] = useState<IApplication[]>();
    const [organization, setOrganization] = useState<IOrganization>();

    const [activePractice, setActivePractice] = useState<number>(0);
    const [active, setActive] = useState<number>(0);

    useEffect(() => {
        const getApplications = async () => {
            try {
                const response = await axiosPrivate.get('/api/applications?role=student');
                setApplications(response.data);
            } catch (error) {
                console.error(error);
            }
        }

        if (onLoad.current) {
            getApplications();
        }

        onLoad.current = true;
    }, [active])

    useEffect(() => {
        const getOrganization = async () => {
            try {
                const response = await axiosPrivate.get(`api/organizations/${applications![active].organizationId}`)
                setOrganization(response.data);
            } catch (error) {
                console.error(error);
            }
        }

        if (applications && applications.length) {
            let index = props.practices.findIndex(value => value.id == applications[active].practiceDateId);
            setActivePractice(index);
            getOrganization();
        }
    }, [applications]);

    const changeSwitchState = (state: boolean): void => {
        let switchers = document.querySelectorAll('.roadmap-container>.slides>.slide-switch');

        if (state) {
            switchers.forEach(switcher => switcher.classList.add('active'));
        } else {
            switchers.forEach(switcher => switcher.classList.remove('active'));
        }
    }

    const changeActive = (newActive: number) => {
        if (applications && applications.length) {
            if (newActive < 0)
                newActive = applications.length - 1;
            else if (newActive >= applications.length) {
                newActive = 0;
            }
        }

        setActive(() => newActive);
    }

    const handleSubmitClick = () => {
        const submitApplication = async () => {
            try {
                await axiosPrivate.patch(`api/applications/${ applications![active].id }/submit`);
            } catch (error) {
                console.error(error);
            }
        }

        submitApplication()
            .then(() => window.location.reload());
    }

    const handleDownload = () => {
        const retrieveApplication = async () => {
            try {
                const response = await axiosPrivate.get(`api/documents/applications/${ applications![active].id }/download`, {
                    responseType: `blob`
                });
                var file = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' });
                var fileURL = URL.createObjectURL(file); 
                window.open(fileURL);
            } catch (error) {
                console.error(error);
            }
        }

        retrieveApplication();
    }

    const handleRemove = () => {
        const removeApplication = async () => {
            try {
                await axiosPrivate.delete(`api/applications/${ applications![active].id }`);
                window.location.reload();
            } catch (error) {
                console.error(error);
            }
        }

        removeApplication();
    }

    return (
        applications && applications.length ?
        <div className="student-application-roadmap">
            <div className="roadmap-container" onMouseEnter={() => changeSwitchState(true)} onMouseLeave={() => changeSwitchState(false)}>
                <div className="slides">
                    <div className="slide-switch left">
                        <FontAwesomeIcon icon={faChevronLeft} onClick={() => changeActive(active - 1)} />
                    </div>
                    <div className="slide-content">
                        <div className="application-info-container content-part">
                            <div className="application-info">
                                <div className="application-info-title title">
                                    <h2>{ props.practices[activePractice].type }</h2>
                                    <p>{ props.practices[activePractice].kind }</p>
                                </div>
                                
                            </div>
                        </div>
                        <div className="service-info-container content-part">
                            <div className="info-container">
                                <div className="title title-frame">
                                    <h3>Профильная организация</h3>
                                </div>
                                <div className="content">
                                    {
                                        organization ?
                                        <>
                                            <p>{ organization.organizationName }</p>
                                            <p>{ organization.organizationAddress }</p>
                                        </>
                                        : 
                                        <p>Нет данных</p>
                                    }
                                </div>
                            </div>
                            <div className="info-container">
                                <div className="title title-frame">
                                    <h3>Статус заявки</h3>
                                </div>
                                <div className="content">
                                    <p>{ applicationStatuses[applications[active].status] }</p>
                                    <div className="progress-bar">
                                        {
                                            Object.keys(applicationStatuses).map((key, index) => (
                                                <div key={index} className={"progress-bar-item" + (Number(key) <= applications[active].status ? " active" : "")}></div>
                                            ))
                                        }
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="slide-switch right">
                        <FontAwesomeIcon icon={faChevronRight} onClick={() => changeActive(active + 1)} />
                    </div>
                </div>
                <div className="toggle-controller-container">
                    {
                        applications.map((_, index) => (
                            <div key={index} onClick={() => changeActive(index)} className={"toggle-controller" + (index === active ? " active" : "")}></div>
                        ))
                    }
                </div>
                <div className="application-functions">
                    {
                        applications[active].status === 0 ?
                        <FontAwesomeIcon icon={ faCheck } onClick={ handleSubmitClick } />
                        : <></>
                    }
                    {
                        applications[active].status === 2 ?
                        <FontAwesomeIcon icon={ faDownload } onClick={ handleDownload } />
                        : <></>
                    }
                    {
                        applications[active].status !== 2 ?
                        <FontAwesomeIcon icon={ faRemove } onClick={ handleRemove } />
                        : <></>
                    }
                </div>
            </div>
        </div>
        : <></>
    );
}

export default ApplicationRoadmap;