import { useEffect, useRef, useState } from "react";
import IApplication from "../../interfaces/IApplication";
import IPracticeDate from "../../interfaces/IPracticeDate";
import IOrganization from "../../interfaces/IOrganization";
import applicationStatuses from "../../../const/ApplicationStatuses";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import RoleContent from "../../../components/RoleContent";

import "./ApplicationCard.css";

interface IApplicationCard {
    className?: string,
    date: IPracticeDate,
    application: IApplication
}

export const ApplicationCard: React.FunctionComponent<IApplicationCard> = (props) => {
    const axiosPrivate = useAxiosPrivate();
    const onLoad = useRef(false);
    const { className, date, application } = props;

    const [organization, setOrganization] = useState<IOrganization>();

    useEffect(() => {
        const getOrganization = async () => {
            try {
                const response = await axiosPrivate.get(`api/organizations/${application.organizationId}`)
                setOrganization(response.data);
            } catch (error) {
                console.error(error);
            }
        }

        onLoad.current && getOrganization();

        onLoad.current = true;
    }, []);

    return (
        <div className={ `application-card-container ${ className }` }>
            <div className="application-info-container content-part">
                <div className="application-info">
                    <div className="application-info-title title">
                        <h2>{ date.type }</h2>
                        <p>{ date.kind }</p>
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
                <RoleContent roles={ [`student`] }>
                    <div className="info-container">
                        <div className="title title-frame">
                            <h3>Статус заявки</h3>
                        </div>
                        <div className="content">
                            <p>{ applicationStatuses[application.status as 0 | 1 | 2] }</p>
                            <div className="progress-bar">
                                {
                                    Object.keys(applicationStatuses).map((key, index) => (
                                        <div key={index} className={"progress-bar-item" + (Number(key) <= application.status ? " active" : "")}></div>
                                    ))
                                }
                            </div>
                        </div>
                    </div>
                </RoleContent>
            </div>
        </div>
    );
}