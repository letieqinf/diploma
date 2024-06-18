import IGroup from "../../../interfaces/IGroup";
import "./GroupCard.css";

interface IGroupCard {
    group: IGroup,
    onClick: React.MouseEventHandler
}

export const GroupCard: React.FunctionComponent<IGroupCard> = ( props ) => {
    const { group, onClick } = props;

    return (
        <div className="group-card-container card-container" onClick={ onClick }>
            <div className="group-card card">
                <div className="group-card-title card-title">
                    <h3>{ group.name }</h3>
                </div>
                <div className="group-card-info card-info">
                    <p className="study-field-code">{ group.studyFieldCode } { group.studyFieldName }</p>
                    <p className="study-field-name">Кафедра { group.departmentAbbr }, { group.year }</p>
                </div>
            </div>
        </div>
    );
}