import { useState } from "react";
import ModalWindow from "../../layout/ModalWindow";
import VerticalTabs from "../../layout/VerticalTabs";
import { ContractList } from "../../layout/contract/Contract";
import { EditContractForm } from "./EditContractForm";
import "./Education.css";
import IPracticeProfile from "../../../interfaces/IPracticeProfile";
import { SupervisorsList } from "./SupervisorsList";
import ITeacher from "../../../interfaces/ITeacher";
import IGroup from "../../../interfaces/IGroup";
import IPracticeDate from "../../../interfaces/IPracticeDate";
import { AddSupervisorForm } from "./AddSupervisorForm";

type AddSupervisor = {
    teachers: ITeacher[],
    group: IGroup,
    date: IPracticeDate
}

export const Education: React.FunctionComponent = () => {
    const [profileToEdit, setProfileToEdit] = useState<IPracticeProfile>();
    const [addSupervisorModel, setAddSupervisorModel] = useState<AddSupervisor>();

    return (
        <div className={ `content-container education-content-container` }>
            <VerticalTabs
                titles={ [`Договоры`, `Руководители практик`] }
                collection={ `education-base` }
                className={ `content-container-blocks` }
            >
                <div>
                    <div className={ `content-header` }>
                        <h1 className={ `page-title` }>Все договоры</h1>
                    </div>
                    <div className={ `content-body` }>
                        <div className={ `contract-list-container` }>
                            <ContractList role={ `education` } onEditClick={ (profile) => setProfileToEdit(profile) } />
                        </div>
                    </div>
                </div>
                <div>
                    <div className={ `content-header` }>
                        <h1 className={ `page-title` }>Руководители практик</h1>
                    </div>
                    <div className={ `content-body` }>
                        <div className={ `supervisor-controller` }>
                            <SupervisorsList onCreateClick={ 
                                (teachers, group, date) => setAddSupervisorModel({
                                    teachers: teachers,
                                    group: group,
                                    date: date
                                })
                             } />
                        </div>
                    </div>
                </div>
            </VerticalTabs>
            <ModalWindow
                disabled={ profileToEdit === undefined }
                id={ `edit-contract-mw` }
                onClose={ () => setProfileToEdit(undefined) }
            >
                { profileToEdit && <EditContractForm profile={ profileToEdit } /> }
            </ModalWindow>
            <ModalWindow
                disabled={ addSupervisorModel === undefined }
                id={ `add-supervisor-mw` }
                onClose={ () => {
                    setAddSupervisorModel(undefined)
                } }
            >
                { addSupervisorModel && <AddSupervisorForm teachers={ addSupervisorModel.teachers } group={ addSupervisorModel.group } date={ addSupervisorModel.date } /> }
            </ModalWindow>
        </div>
    );
}