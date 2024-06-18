import { useEffect, useRef, useState } from "react";
import "./Teacher.css";
import VerticalTabs from "../../layout/VerticalTabs";
import { GroupList } from "./GroupList";
import ISupervisor from "../../../interfaces/ISupervisor";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
import ModalWindow from "../../layout/ModalWindow";
import { CreateContractForm } from "./CreateContractForm";
import { ContractList } from "../../layout/contract/Contract";

export const Teacher: React.FunctionComponent = () => {
    const axiosPrivate = useAxiosPrivate();
    const onLoad = useRef(false);

    const [hideCreateContractMW, setHideCreateContractMW] = useState<boolean>(true);

    const [supervisors, setSupervisors] = useState<ISupervisor[]>();

    useEffect(() => {
        const getSupervisors = async () => {
            try {
                const response = await axiosPrivate.get(`api/supervisors?role=teacher`);
                setSupervisors(response.data);
            } catch (error) {
                console.error(error);
            }
        }

        if (onLoad.current) {
            getSupervisors();
        }

        onLoad.current = true;
    }, []);

    return (
        <div className={ `content-container teacher-content-container` }>
            <VerticalTabs titles={ ["Группы", "Договоры"] } collection={ `teacher` } className={ `content-container-blocks` }>
                <div>
                    <div className={ `content-header` }>
                        <h1 className={ `page-title` }>Мои группы</h1>
                    </div>
                    <div className={ `content-body` }>
                        {
                            supervisors && supervisors.length ?
                            <GroupList supervisors={ supervisors } />
                            : <p>Вы не были назначены в качестве руководителя ни для одной группы</p>
                        }
                    </div>
                </div>
                <div>
                    <div className={ `content-header` }>
                        <h1 className={ `page-title` }>Мои договоры</h1>
                    </div>
                    <div className={ `content-body` }>
                        {
                            supervisors && supervisors.length ?
                            <>
                                <div className={ `buttons-container` }>
                                    <button type={ `button` } onClick={ () => setHideCreateContractMW(false) }>Создать договор</button>
                                </div>
                                <div className={ `contract-list-container` }>
                                    <ContractList role={ `teacher` } />
                                </div>
                            </>
                            : <p>Вы не были назначены в качестве руководителя ни для одной группы</p>
                        }
                    </div>
                </div>
            </VerticalTabs>
            <ModalWindow id={ `create-contract-modal-window` } disabled={ hideCreateContractMW } onClose={ () => setHideCreateContractMW(true) }>
                { supervisors && supervisors.length && <CreateContractForm supervisors={ supervisors } /> }
            </ModalWindow>
        </div>
    );
}