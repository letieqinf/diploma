import { faUserCircle } from "@fortawesome/free-regular-svg-icons";
import { faAddressBook } from "@fortawesome/free-solid-svg-icons";
import { faPenRuler } from "@fortawesome/free-solid-svg-icons/faPenRuler";
import { faTasks } from "@fortawesome/free-solid-svg-icons";

import { Link } from "react-router-dom";
import Contents from "../components/layout/Contents";
import RoleContent from "../../components/RoleContent";

import "./Tools.css";

function Tools() {
    return (
        <article className="page-body tools-body">
            <h2 className="page-title">Панель управления</h2>
            <div className="page-content">
                <Contents title="Пользователи" icon={faUserCircle}>
                    <Link to="/tools/users">Все пользователи</Link>
                    <RoleContent roles={['education']}>
                        <Link to="/tools/users/roles">Управлять ролями</Link>
                    </RoleContent>
                </Contents>
                <Contents title="Учебный план" icon={faAddressBook}>
                    <Link to="/tools/groups">Все учебные планы</Link>
                    <Link to="/tools/faculties">Все факультеты</Link>
                    <Link to="/tools/departments">Все кафедры</Link>
                </Contents>
                <Contents title="Учебный процесс" icon={faPenRuler}>
                    <Link to="/tools/groups">Все группы</Link>
                    <RoleContent roles={['education']}>
                        <Link to="/tools/groups/manage">Управлять группами</Link>
                    </RoleContent>
                    <Link to="/tools/teachers">Все преподаватели</Link>
                    <Link to="/tools/supervisors">Все руководители</Link>
                </Contents>
                <Contents title="Практики" icon={faTasks}>
                    <Link to="/tools/applications">Все заявки</Link>
                    <Link to="/tools/practices">Все практики</Link>
                </Contents>
            </div>
        </article>
    );
}

export default Tools;