import { Link } from "react-router-dom";

import ToolMenu from "../toolmenu/ToolMenu";
import AccountTools from "../toolmenu/AccountTools";

function UnauthorizedHeader() {
    return (
        <ToolMenu>
            <AccountTools>
                <Link to="/register">Зарегистрироваться</Link>
                <Link to="/login">Войти</Link>
            </AccountTools>
        </ToolMenu>
    );
}

export default UnauthorizedHeader;